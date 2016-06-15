using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using CyUSB;

namespace 
Programmer
{
partial class
DongleForm
{

	bool	weAreMaster = false;
//	bool	busInitialized = false;

	bool
	notMaster(bool display)
	{
		if (weAreMaster) return false;

		if (display) message("!!! The System Interface Device is not the master");

		return true;

	}

	byte
	page4k(byte page, short addr)
	{
		page &= 0x01;
		addr = (short) (0xF000 & addr);
		page = (byte)((page << 4) | ((addr >> 12) & 0x000F));

		return page;
	}

	short
	getFlashAddr()
	{
		return (short) (Convert.ToInt16(Address.Text, 16) & 0x0FFFF);
	}
	
	void
	donglePOR()
	{
		clearIt();

		cmd[0] = Constants.USBCMD_DONGLE_RESET;

		if (!USB_Command(10)) return;
	}

	void
	busPing()
	{
		if (!weAreMaster) return;

		clearIt();

		cmd[0] = Constants.USBCMD_DONGLE_PING;

		if (!USB_Command(10)) return;

		if (status[1] != 0)
		{
			StatusMessage("!!! Ping-> BusCommand ", status[1]);
		}
	}

	void
	doDUTReset()
	{
		byte[]	dout = new byte[8];

		if (!weAreMaster) return;

		clearIt();

		cmd[0] = Constants.USBCMD_DUT_RESET;
		cmd[1] = 0x7C;

		if (!USB_Command(10)) return;

		if (status[1] != 0)
		{
			StatusMessage("!!! Reset-> BusCommand ", status[1]);
		}
	}

	bool
	busRead4K(int page, int addr)
	{
		byte[] dout = new byte[8];
		int count,i;
		
		for(count = 4 * K; 0 < count; count -= 8)
		{
			clearIt();
			
			cmd[0] = Constants.USBCMD_LOADER_READ;
			cmd[1] = 0x7C;
			cmd[2] = (byte) page;
			cmd[3] = (byte) ((addr & 0x0FF00) >> 8);
			cmd[4] = (byte) (addr & 0x000FF);

			if (!USB_Command(5)) return false;
			
			if (status[1] != 0)
			{
				StatusMessage("!!! FlashRead-> BusCommand ", status[1]);
				return false;
			}

			GetInputReport(dout,8, status[2]);
			for(i = 0; i < 8; ) readBuffer[addr++] = dout[i++];

			progressBar.Value = progressBar.Value + 8;

			if ((count & 0x1FF) == 0)
			{
				Application.DoEvents();
				busDonglePing();
			}
		}
		return true;
	}
	
	
	
	bool
	SetAddress(int addr)
	{
		clearIt();
			
		cmd[0] = Constants.USBCMD_LOADER_ERASE;
		cmd[1] = 0x7C;
		cmd[2] = 0xFF;	// Ignore Erase, set address
		cmd[3] = (byte)((addr & 0x0FF00) >> 8);
		cmd[4] = (byte)(addr & 0x000FF);

		if (!USB_Command(5)) return false;

		if (status[1] != 0)
		{
			StatusMessage("!!! Set Address failure -> ", status[1]);
			return false;
		}
		return true;
	}		
			
	bool
	busErase(int page, int addr)
	{
		// Erase a 4K block

		clearIt();
		
		cmd[0] = Constants.USBCMD_LOADER_ERASE;
		cmd[1] = 0x7C;
		cmd[2] = (byte)page;
		cmd[3] = (byte)((addr & 0x0FF00) >> 8);
		cmd[4] = (byte)(addr & 0x000FF);

		if (!USB_Command(5)) return false;

		if (status[1] != 0)
		{
			StatusMessage("!!! FlashErase-> BusCommand ", status[1]);
			return false;
		}

		for (int i = 0; i < 5; ++i)
		{
			Application.DoEvents();
			Thread.Sleep(100);
		}
		
		return true;
	}
	
	bool 
	BlockInUse(int addr)
	{
		int		bi, cnt;
		
		for(cnt = 0; cnt < 64; ++cnt)
		{
			bi = addr / 64;
			
			if (blockSet1[bi] != 0) return true;
			
			addr += 64;
		}
		return false;
	}

	bool
	busWrite8(int addr)
	{
		byte[] dout = new byte[8];

		for (int i = 0; i < 8; ++i) dout[i] = fileBuffer1[addr + i];

		SetOutputReport(dout, 8);

		clearIt();

		cmd[0] = Constants.USBCMD_LOADER_WRITE;
		cmd[1] = 0x7C;

		if (!USB_Command(5)) return false;

		if (status[1] != 0)
		{
			StatusMessage("!!! FlashWrite failed ", status[1]);
			return false;
		}
		if (status[2] == 0)
		{
			message("!!! DUT did not accept write data ");
			return false;
		}
		return true;
	}
	
	bool
	busWrite4K(int page, int addr)
	{
		int i,k,j;
		int	bi;

		// If all 64 sections of the 4K block are not used, do NOT erase
		
		if (!BlockInUse(addr)) return true;
		
		busErase(page, addr);
		
		// Write Data
		
		for (k = 0; k < 64 ; ++k)
		{
			bi = k + (addr/64);
			
			if (blockSet1[bi] == 0) continue;
			
			i = addr + (k * 64);
			
			SetAddress(i);
			
			// Do 64 Bytes of writes
			
			for(j = 0; j < 8; ++j)
			{
				busWrite8(i);
				i += 8;
			}
			Thread.Sleep(5);	// Flash writes are < 5ms

			progressBar.Value += 8;

			Application.DoEvents();
			busDonglePing();
		}
		return true;
	}
	
	void
	busFlashWriteFile(int page)
	{
		int		addr;
		
		if (notMaster(true)) return;

		progressBar.Value = 0;
		progressBar.Maximum = fileCount1;
		
		for(addr = 0; addr < 64*K; )
		{
			if (!busWrite4K(0, addr)) break;
			
			addr += 4*K;
		}
		progressBar.Value = 0;
	}
	
	// Read out the 32K of control data
	
	void
	busFlashRead(int page)
	{
		if (notMaster(true)) return;

		readBufferSize = 0;

		progressBar.Maximum = 7 * 4 * K;
		progressBar.Value = 0;
		message("Starting the reading process");
		
		if (page == 0)
		{
			progressBar.Maximum = 7 * 4 * K;

			if (!busRead4K(0, 0x0000)) return;	//System Array
			if (!busRead4K(0, 0x8000)) return;	// Control
			if (!busRead4K(0, 0x9000)) return;	// Control
			if (!busRead4K(0, 0xA000)) return;	// Timers
			if (!busRead4K(0, 0xB000)) return;	// Timers
			if (!busRead4K(0, 0xC000)) return;	// Alarms
			if (!busRead4K(0, 0xD000)) return;	// Alarms
		}
		else
		{
			progressBar.Maximum = 16 * 4 * K;

			if (!busRead4K(1, 0x0000)) return;
			if (!busRead4K(1, 0x1000)) return;
			if (!busRead4K(1, 0x2000)) return;
			if (!busRead4K(1, 0x3000)) return;
			if (!busRead4K(1, 0x4000)) return;
			if (!busRead4K(1, 0x5000)) return;
			if (!busRead4K(1, 0x6000)) return;
			if (!busRead4K(1, 0x7000)) return;
			if (!busRead4K(1, 0x8000)) return;
			if (!busRead4K(1, 0x9000)) return;
			if (!busRead4K(1, 0xA000)) return;
			if (!busRead4K(1, 0xB000)) return;
			if (!busRead4K(1, 0xC000)) return;
			if (!busRead4K(1, 0xD000)) return;
			if (!busRead4K(1, 0xE000)) return;
			if (!busRead4K(1, 0xF000)) return;
		}

		readBufferSize = 64 * K;
			
		message("Control code reading finished");
		progressBar.Value = 0;
	}
	
	bool
	pingSuspended(int secs)
	{
		int		i = 0;
		
		progressBar.Maximum = secs;
		progressBar.Value = 0;

		do
		{
			if (i < 8 || i >= 12)
			{
				cmd[0] = Constants.USBCMD_COMMAND;
				cmd[1] = 0x7C;						// To a suspended Master
				cmd[2] = Constants.BUSCMD_NULL;		// BUS COMMAND is NULL

				USB_Command(20);

				if (0 != status[1]) 
				{
					StatusMessage("Suspend confirmation failed -> ", status[1]);
					progressBar.Value = 0;
					
					return false;
				}
			}
			progressBar.Value += 1;
			Application.DoEvents();
			
			Thread.Sleep(1000);
			
			--secs;
			++i;
		}
		while(secs > 0);
		
		progressBar.Value = 0;
			
		return true;
	}
	
	bool
	doMasterSuspend()
	{
		if (!DongleCheck()) return false;

		weAreMaster = false;

		clearIt();

		// The first byte is interpretted then stripped off
		cmd[0] = Constants.USBCMD_COMMAND;

		// The second byte is the hardware I2C address to use
		cmd[1] = 0x7F;			// To a real Master

		// the next 3 bytes are the command packet
		cmd[2] = 0x7C;			// From us
		cmd[3] = 0xA5;			// data validating FlashLoader suspend
		cmd[4] = 0x5A;

		for(int t = 10; t > 0; --t)
		{
			USB_Command(10);

			if (status[1] == 0) break;
			
			Thread.Sleep(100);
		}

		if (status[1] != 0)
		{
			StatusMessage("Suspend attempt failed -> ", status[1]);
			
			return false;
			/*
				// Is the GC1 already suspended?

				clearIt();

				// The first byte is interpretted then stripped off

				message("Controller was already suspended");
			*/
		}

		Thread.Sleep(500);
		
		if (!pingSuspended(1)) return false;

		message("Suspend complete");
		
		return true;
	}

	public bool
	doBecomeMaster(bool flag)
	{
		if (!DongleCheck()) return false;

		clearIt();

		cmd[0] = Constants.USBCMD_MASTER;

		if (flag) cmd[1] = 1;
		else cmd[1] = 0;

		USB_Command(5);

		if (status[1] != 0)
		{
			StatusMessage("Bus Master attempt failed ->", status[1]);
			return false;
		}
		if (flag)
		{
			weAreMaster = true;
			
			if (Factory) 
				message("The System Interface Device is now the Bus Master");
		}
		else
		{
			weAreMaster = false;
			
			if (Factory) 
				message("The System Interface Device is no longer the Bus Master");
		}
		
		return true;
	}
		

	//void
	//doBusInit()
	//{
	//    if (!busInitialized)
	//    {
	//        clearIt();

	//        cmd[0] = Constants.USBCMD_INIT;
	//        cmd[1] = 0;

	//        USB_Command(cmd,status, 10);

	//        if (status[1] != 0)
	//        {
	//            StatusMessage("INIT->", status[1]);
	//            return;
	//        }

	//        busInitialized = true;
	//    }
	//}
	
	void
	busPowerOff()
	{
		clearIt();

		cmd[0] = Constants.TC_INIT;
		USB_Command(2);
		powered = false;
		Power.Checked = false;
//		busInitialized = false;
		
		//// Now put I2C back online
		//cmd[0] = Constants.USBCMD_INIT;
		//USB_Command(2);
	}

	// Initialize the Dongle to use I2C on its bus

	public void
	busInit()
	{
		// Tell the Dongle to set its control lines
		// and Disable I2C.

		if (!DongleCheck()) return;
		
		clearIt();

		cmd[0] = Constants.USBCMD_INIT;
		USB_Command(2);

		if (status[1] != 0)
			StatusMessage("SID Initialization Failure!", status[1]);
		else message("SID initialized OK");
	}

	//void
	//busPower(bool on)
	//{
	//    if (powered) return;
		
	//    // Tell the Dongle to set its control lines
	//    // and disbale I2C.

	//    clearIt();

	//    cmd[0] = Constants.TC_INIT;
	//    USB_Command(2);

	//    if (on)
	//    {
	//        cmd[0] = Constants.TC_POWER;
	//        cmd[1] = 1;
	//        USB_Command(2);

	//        // Now put I2C back online and set controls lines PU

	//        cmd[0] = Constants.USBCMD_INIT;
	//        USB_Command(2);
			
	//        powered = true;
	//        Power.Checked = true;
	//    }
	//}

	public int
	SensorRead(int i)
	{
		byte [] buf = new byte[8];
		clearIt();

		cmd[0] = Constants.USBCMD_COMMAND;
		cmd[1] = 0x01;
		cmd[2] = Constants.BUSCMD_READ;
		cmd[3] = (byte)i;

		USB_Command(20);

		clearIt();
		cmd[0] = Constants.USBCMD_BUS_READ;
		cmd[1] = 0x01;
		cmd[2] = 0x02;	// Count

		USB_Command(2);				// Ask NET PSoC to read pic
		GetInputReport(buf, 8, status[2]);		// Get data from PSoC
		StatusMessage("Bus Read->", status[1]);
		
		return  ((buf[1] & 0x0FF) * 256) + (buf[1] & 0x0FF);
	}
	
	void
	doBusNull()
	{
		clearIt();

		cmd[0] = Constants.USBCMD_COMMAND;
		cmd[1] = 0x7C;
		cmd[2] = Constants.BUSCMD_NULL;		// BUS COMMAND is NULL

		USB_Command(20);

		StatusMessage("NULL->", status[1]);
	}
}
}
