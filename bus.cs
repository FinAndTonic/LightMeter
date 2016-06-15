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
  MainForm
  {
	public class Module
	{
		public int	majorRevision;
		public int	minorRevision;
		public string	name;
		public string	rev;
	};

	static	public	int		vold = -1;

	static public int		slaveCount = 0;
	static public byte		moduleId = 0;
	static public Module	[]	modules = new Module[64];
	static	int	K = 1024;

	byte[]	cmd = new byte[8];
	byte[]	status = new byte[8];
	byte[]	fileBuffer1 = new byte[64 * K];
	byte[]	fileBuffer2 = new byte[128 * K];
	byte[]	readBuffer = new byte[128 * K];
	byte[]	protectBuffer = new byte[512];
	int		readBufferSize;

	void
	clearIt()
	{
		int	i;

		for(i = 0; i < 8; ++i ) cmd[i] = 0;
	}

	void
	scroll()
	{
		Output.SelectionStart = Output.Text.Length;
		Output.ScrollToCaret();
		Output.Invalidate();
		Output.Update();
	}

	void
	crlf()
	{
		Output.Text += "\r\n";
	}

	// Dump msg and 8 bytes and CRLF

	void
	dump(int addr, byte  [] data, byte [] array)
	{
		int		i;
		string	err;

		Output.Text += string.Format("{0:X4}: ",addr & 0x0FFFF);
		err = "  ";

		for(i = 0; i<8; ++i)
		{
			if (array[addr+i] == data[i])
				 err += "-";
			else err += "X";
		}

		for(i = 0; i<8; ++i) Output.Text += string.Format("{0:X2} ",data[i]);

		Output.Text += err;
		crlf();
		scroll();
	}

	

	//// BUS:  USBCMD_FLASH, BUSCMD_FLASH_ERASE, page

	//void
	//doFlashErase()
	//{
		
	//}

	//void
	//doDeviceChecksum()
	//{
		
	//}

	//void
	//doFlashRead()
	//{
		
	//}

	void
	doProgram()
	{
		progProgram();
	}

	// For External Flash programming

	void
	flashProgram()
	{
		progressBar.Value = 1024;
		message("Flash programming finished!");
	}

	public void
	busDonglePing()
	{
		cmd[0] = Constants.USBCMD_DONGLE_PING;
		USB_Command(1);
		Thread.Sleep(50);
	}

	void
	busDongleReset()
	{
		DongleCheck();
		
		// Rev 2 needs app mode turned on
		
		if (sidRevision >= 0x0200)
		{
			clearIt();
			
			cmd[0] = Constants.USBCMD_INIT;
			cmd[1] = 0x00;
			
			USB_Command(1);

			// Turn on I2C lines 
			cmd[0] = Constants.USBCMD_STATE;
			cmd[1] = 0x00;		// I2C address
			cmd[2] = 0x01;		// I2C lines only

			USB_Command(1);

			cmd[0] = Constants.USBCMD_BLM_MODE;
			cmd[1] = 0x0FF;

			USB_Command(1);
			
			Thread.Sleep(100);
		}
		else
		{
			cmd[0] = Constants.USBCMD_DONGLE_RESET;
			USB_Command(1);
			
			Wait(4);
		}
	}

	public void
	ThreadSleep(int secs)
	{
		Thread.Sleep(100);
	}
	
	public void
	Wait(int secs)
	{
		int	count;

		secs *= 10;

		progressBar.Maximum = secs;
		progressBar.Value = 0;

		for(count = 0; count < secs; ++count)
		{
			Thread.Sleep(100);
			progressBar.Value += 1;
			Application.DoEvents();
		}
		progressBar.Value = 0;
	}
  }
}
