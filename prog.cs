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
	bool	acquired = false;
	bool	powered = false;
	int		family = 0;
	int		device;
	byte	cBank = 0;
	bool	erased = false;
	byte	clockErase = 50;
	byte	clockProgram = 50;
	byte[]	sram = new byte[256];
	byte[]	serialNumberBytes = new byte[4];
	byte[]	protection = new byte[128];

    public byte[] deviceRevision = new byte[2];
	
	// Read into our sram[] from the PSoC's

	void
	progReadRam(int count, byte addr)
	{
		byte[]	dout = new byte[8];
		int		ra = addr;
		int		i;

		while(count > 0)
		{
			mrn(addr, dout);

			for(i = 0; i < 8; ) sram[ra++] = dout[i++];
			
			count -= 8;
		}
	}

	//	Read 64 bytes from flash and put into readBuffer at (bid * 64)

	bool
	progReadBlock(int bid)
	{
		int		i,j, ra;
		byte	id;
		byte	addr = Constants.BUFFER;
		byte[]	dout = new byte[8];

		if (CY8C27K)
			 id = (byte) (bid & 0x0FF);
		else id = (byte) (bid & 0x07F);

		_que(Constants.OP_READ_BLOCK, id, Constants.BUFFER, 0, 0, 0, 10);

		Thread.Sleep(100); // Should this be 25 ms ????

		if (0 != mr(0xF8))
		{
			message("!!! Flash is read protected");
			return false;
		}

		ra = bid * 64;

		// Get the 8x8 bytes out of SRAM

		for(i = 8; i-- > 0; )
		{
			mrn(addr, dout);

			for(j = 0; j < 8; ) readBuffer[ra++] = dout[j++];

			addr += 8;
		}
		return true;
	}

	bool
	aTest()
	{
		byte	fr;
		
		if (!DongleCheck()) return false;
		
		for(int trys = 2; trys > 0; --trys)
		{
			if (!acquired)
			{
				erased = false;
				progAcquire();

				if (acquired) 
				{
                    iows(0x1e0, 0x02);	// 12 MHz
                    iob0();
				
					calcClockRate(25);
					progCalibrate();
					
					return true;
				}
			}
			
			fr = ior(0x0FF);
			
			if (fr == 0x17 || fr == 0x13)
			{
				return acquired = true;
			}
		}
		
		return false;
	}

	// Read 8 bytes from Devices sram @ addr, put into buffer 'dout'

	void
	mrn(byte addr, byte [] dout)
	{
		if (acquired)
		{
			cmd[0] = Constants.TC_MR;
			cmd[1] = addr;
			cmd[2] = 8;

			USB_Command(10);
			GetInputReport(dout,8, status[2]);
		}
	}

	// Write the data from the programBuffer into SRAM then call flash write

	void
	progWriteBlock(byte bid, int pi)
	{
		byte	ra = Constants.BUFFER;
		byte[]	din = new byte[64];
		int		i;
		Int16	sum1, sum2 = 0;
		byte	b;
		
		if (!acquired) return;
		
		//	Fill PSoC's writeBuffer
		
		for(i = 0; i < 64; )  
		{
			b = fileBuffer1[pi++];
			din[i++] = b;
			sum2 += b;
		}
		
		SetOutputReport(din, 64);
		
		//	Transfer writeBuffer to DUT's RAM
		
		cmd[0] = Constants.TC_MW;
		cmd[1] = (byte) ra;
		cmd[2] = 64;

		USB_Command(10);	// Transfer takes 8 ms
		
		sum1 = (Int16) ( (status[2] << 8) | (status[3] & 0x0FF) );
		
		if (sidRevision >= 0x0207)
		{
			for(int trys = 4; trys > 0; --trys)
			{
				if (sum1 == sum2) break;
				
				if (Factory) message("USB CRC error");
				
				SetOutputReport(din, 64);
				Thread.Sleep(5);
				USB_Command(10);
				
				sum1 = (Int16) ( (status[2] << 8) | (status[3] & 0x0FF) );
			}
			if (sum1 != sum2) 
			{
				Output.Text += string.Format("Unable to send USB packet for Flash block={0}\r\n",bid);
				scroll();
				return;
			}
		}
			
		//	Program RAM -> Flash
		_que(Constants.OP_WRITE_BLOCK, bid, Constants.BUFFER, clockProgram, 0x56, 0, 20);
	}

	//void
	//mwn(byte addr, byte count, int pi)
	//{
	//    byte[]	din = new byte[8];
	//    int		i;

	//    for(i = 0; i < count; )  din[i++] = fileBuffer1[pi++];

	//    if (acquired)
	//    {
	//        SetOutputReport(din, count);

	//        cmd[0] = Constants.TC_MW;
	//        cmd[1] = (byte) addr;
	//        cmd[2] = count;

	//        USB_Command(2);
	//    }
	//}

	
	byte
	mr(byte addr)
	{
		if (acquired)
		{
			cmd[0] = Constants.TC_OP;
			cmd[1] = Constants.TCOP_MR;
			cmd[2] = addr;

			USB_Command(8);
			
			return status[2];
		}
		return 0;
	}

	byte
	ior(byte addr)
	{
		if (acquired)
		{
			cmd[0] = Constants.TC_OP;
			cmd[1] = Constants.TCOP_IOR;
			cmd[2] = addr;
			cmd[3] = 0;

			USB_Command(8);
			
			return status[2];
		}
		return 0;
	}

	void
	mw(byte addr, byte data)
	{
		if (acquired)
		{
			cmd[0] = Constants.TC_OP;
			cmd[1] = Constants.TCOP_MW;
			cmd[2] = addr;
			cmd[3] = data;

			USB_Command(1);
		}
	}

	void
	iow(byte addr, byte data)
	{
		if (acquired)
		{
			cmd[0] = Constants.TC_OP;
			cmd[1] = Constants.TCOP_IOW;
			cmd[2] = addr;
			cmd[3] = data;

			USB_Command(1);
		}
	}

	// Set the IO bank to 0 (main bank)
	
	void
	iob0()
	{
		byte fs;

		if (!acquired) return;

		fs = ior(0x0F7);				// Flag
		iow(0xF7, (byte)(fs & 0xEF));	// Clear IOB
	}
	
	void
	iows(Int16 address, byte data)
	{
		byte	fs;
		int		bank = address & 0x0100;

		if (!acquired) return;

		fs = ior(0x0F7);	// Flag

		if (0 != bank)
		{
			// Upper IO bank request

			if (0 != (fs & 0x10))
			{
				iow((byte) (address & 0x0FF), data);
			}
			else
			{
				iow(0xF7, (byte) (fs | 0x10));
				iow((byte) (address & 0x0FF), data);
			}
		}
		else
		{
			// Lower IO bank request

			if (0 != (fs & 0x10))
			{
				iow(0xF7, (byte) (fs & 0xEF));
				iow((byte) (address & 0x0FF), data);
			}
			else
			{
				iow((byte) (address & 0x0FF), data);
			}
		}
	}

	byte
	iorx(Int16 address)
	{
		byte	fs,rr = 0;
		int		bank = address & 0x0100;

		if (!acquired) return 0;

		fs = ior(0x0F7);	// Flag

		if (bank != 0)
		{
			// Upper IO bank request

			if (0 != (fs & 0x10))
			{
				rr = ior((byte) (address & 0x0FF));
			}
			else
			{
				iow(0xF7, (byte) (fs | 0x10));
				rr = ior((byte) (address & 0x0FF));
			}
		}
		else
		{
			// Lower IO bank request

			if (0 != (fs & 0x10))
			{
				iow(0xF7, (byte) (fs & 0xEF));
				rr = ior((byte) (address & 0x0FF));
			}
			else
			{
				rr = ior((byte) (address & 0x0FF));
			}
		}
		return rr;
	}

	int
	iord(Int16 addr, byte data)
	{
		byte	r;
		
		if (acquired)
		{
			r = ior((byte) addr);

			if (r != data)
			{
				/// message("IOR error, got %2.02X , expecting %2.02X",r, data);
				return 0;
			}
			return 1;
		}
		return 0;
	}


	// How long will the operation take

	int
	clock2delay(byte c)
	{
		return c / 2;
	}
	
	void
	_que(byte op, byte bid, byte addr, byte clock, byte pch, byte pcl, int duration)
	{
		int		cd = clock2delay(clock);
		
		if (duration < cd) duration = cd;
		
		cmd[0] = Constants.TC_QUE;
		cmd[1] = op;
		cmd[2] = bid;
		cmd[3] = addr;
		cmd[4] = clock;
		cmd[5] = pch;
		cmd[6] = pcl;

		if (acquired) 
		{
			USB_Command(duration);
		}
	}

	 void
	progCalibrate()
	{
		_que(Constants.OP_CALIBRATE,0,0, 0,0,0, 10);

		///iows(0x1E0,2);	// Set CPU to 12 MHz
		///iob0();
	}

	void
	bankSet(int bid)
	{
		byte	bank;

		if (bid == -1) cBank = 0x0FF;	// Invalid

		if (family == Constants.LITHIUM || family == Constants.HYDRA || family == Constants.RADON)
		{
			bank = (byte) (bid >> 7);

			if (bank != cBank)
			{
				iows(0x1FA,bank);
				iows(0x000,0);
				cBank = bank;
			}
		}
	}

	void
	progDump(byte [] mem, int addr, int count)
	{
		byte	data;
		int		j;

		while(0 != count--)
		{
			Output.Text += string.Format("{0:X4}: ", addr); 

			for(j = 0; (j < 16) ; ++j)
			{
				data = (byte) (mem[addr++] & 0x0FF);
				Output.Text += string.Format("{0:X2} ", data); 
			}
			crlf();
		}
		scroll();
	}


	void
	progFlashReadAll(int size)
	{
		int bid,bm;
		int	addr = 0;

		if (!aTest()) return;

		message("FlashReadAll");

		progressBar.Maximum = bm = (size + 63) / 64;
		progressBar.Value = 0;
		readBufferSize = 0;
		
		bankSet(-1);

		for(bid = 0; bid < bm; ++bid)
		{
			bankSet(bid);
			progReadBlock(bid);
			
			BufferCompare(addr);
			
			readBufferSize += 64;
			addr += 64;
			
			++progressBar.Value;
			Application.DoEvents();
		}
		scroll();
	}

	void
	BufferCompare(int addr)
	{
		bool	ok = true;
		
		for(int i = 0; i < 64; ++i)
		{
			if (readBuffer[addr+i] != fileBuffer1[addr+i]) 
				ok = false;
		}
		if (!ok) 
		{
			for(int i = 0; i < 8; ++i)
			{
				Output.Text += string.Format("{0:X4}: ", addr);
				
				for(int j = 0; j < 8; ++j)
				{
					Output.Text += string.Format(" {0:X2}", readBuffer[addr + j]);
				}
				Output.Text += " : ";
				for (int j = 0; j < 8; ++j)
				{
					Output.Text += string.Format(" {0:X2}", fileBuffer1[addr + j]);
				}
				Output.Text += "\r\n";
				
				addr += 8;
			}
		}
		else Output.Text += string.Format("{0:X4}: OK\r\n", addr);
	}
	
	//	There are 8196/64 blocks in Hydra (128)
	//	Read one block at 'addr'

	void
	progFlashRead(int addr, bool display)
	{
		int		bid;

		if (!aTest()) return;

		/// message("FlashRead->");


		bid = (addr / 64);

		bankSet(-1);
		bankSet(bid);
		
		progReadBlock(bid);
		
		if (display) progDump(readBuffer, addr, 4);
	}

	void
	calcClockRate(int t)
	{
		byte []	dout = new byte[8];
		int		s,i,m, eclk,pclk;

		iow(0xf7,00);	// clear IOX

		_que(Constants.OP_TABLE_READ, 3, Constants.BUFFER, 0, 0, 0, 10);	// table 3

		mrn((byte) 0x0F8, dout);

		if (t >= 0)
		{
			s = (dout[3] & 0x0FF);	// slope
			i = (dout[4] & 0x0FF);	// Intercept
			m = (dout[5] & 0x0FF);	// pMult
		}
		else
		{
			s = (dout[0] & 0x0FF);	// slope
			i = (dout[1] & 0x0FF);	// Intercept
			m = (dout[1] & 0x0FF);	// pMult
		}

		eclk = i - (t * s ) / 128;
		//pclk = (eclk * m ) / 64;
		pclk = 2 * eclk;
		
		if (eclk == 0 || pclk == 0)
		{
			// Defaults
			
			eclk = 5;
			pclk = 15;
			
			if (Factory) PopUp("Illegal clockRates, check PE data table", false);
		}
		clockErase = (byte) (eclk);
		clockProgram = (byte) (pclk);

		if (true) 
		{
			/*
			message("clockRates for 12MHz at %d C are %d %d, %2.2gms %2.2gms",
				t,eclk,pclk, 
				(3077 + 1300*eclk)/12000.0,(3077 + 1300*pclk)/12000.0); */
		}
	}

	void
	progProductEng()
	{
		int		cnt;
		byte	op;
		byte	ba;

		message("Product Engineering table ->");

		if (family != Constants.LITHIUM)
		{
			op = 0x10;

			iow(0xf7,00);	// clear IOX

			for(ba = 0, cnt = 2; 0 != cnt--; ba += 32)
			{
				_que(op, 0, Constants.BUFFER, 0, 0x0C0, (byte) (0x40+ba), 10);

				progReadRam(32, Constants.BUFFER);
				progDump(sram,0,2);
			}
		}
		else
		{
			op = 0x10;

			iow(0xf7,00);	// clear IOX

			for(ba = 0x20, cnt = 3; 0 < cnt--; ba += 32)
			{
				_que(op, 0, Constants.BUFFER, 0, 0x0C0, ba, 10);
				progReadRam(32, Constants.BUFFER);

				progDump(sram,0,2);
			}
		}
	}
	
	void
	progRunTest(byte v1,byte v2)
	{
		byte [] dout = new byte[8];
		byte [] din = new byte[8];
		int		j,addr;
		int		error = 0;
		
		for(addr = 0; addr < 256; )
		{
			for(j = 0; j < 8; ++j)
			{ 
				if ((j & 1) == 0) 
					 dout[j] = v1;
				else dout[j] = v2;
			}
			SetOutputReport(dout, 8);

			cmd[0] = Constants.TC_MW;
			cmd[1] = (byte) addr;
			cmd[2] = 8;

			USB_Command(2);
			
			mrn((byte) addr, din);
			
			for(j = 0; j < 8; ++j)
				if (dout[j] != din[j]) ++error;
			
			addr += 8;
		}
		
		if (error == 0) message("SRAM test passed");
		else message("SRAM test failed");
	}
	
	void
	progTestRAM()
	{
		progRunTest(0x00,0x00);
		progRunTest(0xFF,0x00);
		progRunTest(0xFF,0x00);
		progRunTest(0x55,0xAA);
	}
	
	// Erases then writes the single block (data from fileBuffer1)
	// Takes the Cal data form's entries and puts them temporarily into fileBuffer1
	
	public void
	progRewriteCal(int modType, byte [] calTable, int max)
	{
		int		i, pi = 0;
		byte	bid;
		byte[]	tbuf = new byte[64];
		
		switch(modType)
		{
		case 1:						// PC3
		case 2:						// SL1
		case 6:						// SL2
		case 0x0E:					// AP1
				pi = calAddress;	
				break;
		}
		
		if (pi == 0)
		{
			PopUp("Selected module does not support this operation!", true);
			return;
		}

		if (!aTest()) return;
		
		
		// Copy fileBuffer into temp buffer

		for(i = 0; i < 64; ++i) tbuf[i] = fileBuffer1[pi + i];
		
		// Copy cal table into program buffer
		
		for(i = 0; i < max; ++i) fileBuffer1[pi+i] = calTable[i];
		
		bid = (byte) (pi / 64);

		progCalibrate();
		bankSet(-1);
		bankSet(bid);

		_que(Constants.OP_ERASE_BLOCK, bid, 0, clockErase, 0x56, 0, 20);
		
		progWriteBlock(bid, pi);

		// Restore fileBuffer
		
		for (i = 0; i < max; ++i) fileBuffer1[pi + i] = tbuf[i];
	}
	
	// Erases then writes 1 blocks (data from fileBuffer1)
	// Takes the block where the serial number is !!!
	
	void
	progRewriteSerialNumber(int modType)
	{
		int		pi = 0;
		byte	bid;
		
		switch(modType)
		{
		case 1:
		case 2:
		case 3:
		case 4:
		case 6:
		case 7:	pi = revAddress;	
				break;
		}
		
		// Snap to beginning of 64 byte block
		
		pi &= 0x0FFC0;
		
		if (pi == 0)
		{
			PopUp("Selected module does not support this operation!", true);
			return;
		}
		
		bid = (byte) (pi / 64);
		
		if (!aTest()) return;

		progCalibrate();
		bankSet(-1);
		bankSet(bid);

		_que(Constants.OP_ERASE_BLOCK, bid, 0, clockErase, 0x56, 0, 20);
		progWriteBlock(bid, pi);
	}
	
	void 
	progEraseAll()
	{
		if (!aTest()) return;

		progCalibrate();

		_que(Constants.OP_ERASE_ALL,0,0, 16,0x56,0, 50);
		erased = true;

		message("Erase All complete");
	}

	// Read the Serial Number and cal table out of the DUT
	// They are in the last two blocks, which must be un-protected.
	
	bool
	progSerialNumber()
	{
		int			bid;
		int			addr;

		// Clear device data

		deviceRevision[0] = deviceRevision[1] = 1;

		for (int i = 0; i < 4; ) serialNumberBytes[i++] = 0;

		bankSet(-1);

		if (gc1)	// Global variable
		{
			bid = revAddress / 64;
			bankSet(bid);

			if (!progReadBlock(bid)) return false;

			deviceRevision[0] = readBuffer[revAddress+0];
			deviceRevision[1] = readBuffer[revAddress+1];

			ConvertSerialNumber2Bytes( serialNumber = 0x91000001 );

			Output.Text += string.Format("Device's code revision is {0:X2}{1:X2}\r\n",
			deviceRevision[0], deviceRevision[1]);
			scroll();

			return true;
		}

		if (gc2 || hk1) 
		{
			addr = 0x07C40;

			// Read user data and cal data

			if (MODE.SelectedIndex == MODE_FIRST_TIME)
			{
				addr = revAddress & 0x0FFFC;
				bid = addr / 64;
				bankSet(bid);
				
				if (!progReadBlock(bid)) return false;

                addr = 0x07C40; // Location of Product code
                bid = addr / 64;
                bankSet(bid);

                if (!progReadBlock(bid)) return false;
			}
			else
			{
				Output.Text += "Reading internal data tables\r\n";
				scroll();
				
				progressBar.Maximum = 15;
				progressBar.Value = 0;
				
				for(; addr < 0x8000; addr += 64)
				{
					bid = addr / 64;
					bankSet(bid);

					if (!progReadBlock(bid)) return false;
					
					++progressBar.Value;
				}
				progressBar.Value = 0;
			}
			
			deviceRevision[0] = readBuffer[revAddress+0];
			deviceRevision[1] = readBuffer[revAddress+1];
			
			Output.Text += string.Format("Device's code revision is {0:X2}.{1:X2}\r\n",
			deviceRevision[0], deviceRevision[1]);
			scroll();

			ConvertSerialNumber2Bytes( serialNumber = 0x0A000001 );

            if (hk1)
            {
                ConvertSerialNumber2Bytes(serialNumber = 0x0F000001);

                if (readBuffer[0x7C44] != moduleType)
                {
                    message("This unit is not a HK1!");
                }
            }
            else
            if (gc2)
            {
                ConvertSerialNumber2Bytes(serialNumber = 0x0A000001);

                if (readBuffer[0x7C44] != moduleType)
                {
                    message("This unit is not a RKL!");
                }
            }
			return true;
		}

		if (NET)	// Global variable
		{
			bid = revAddress / 64;
			bankSet(bid);
			
			if (!progReadBlock(bid))   return false;
			
			deviceRevision[0] = readBuffer[revAddress+0];
			deviceRevision[1] = readBuffer[revAddress+1];

			Output.Text += string.Format("Device's code revision is {0:X2}.{1:X2}\r\n",
			deviceRevision[0], deviceRevision[1]);
			scroll();

			addr = revAddress+2;

			for (int i = 0; i < 4; ) serialNumberBytes[i++] = readBuffer[addr++];

			Output.Text += string.Format("Device's serial number is {0:X2}{1:X2}{2:X2}{3:X2}\r\n",
			serialNumberBytes[0], serialNumberBytes[1], serialNumberBytes[2], serialNumberBytes[3]);

			scroll();

			serialNumber =	((uint) (serialNumberBytes[0] << 24)) |
							((uint) (serialNumberBytes[1] << 16)) |
							((uint) (serialNumberBytes[2] << 8)) |
							((uint) (serialNumberBytes[3] << 0));


			return (serialNumberBytes[0] == 0x05);
		}
	
	
		//if (sid)	// Global variable
		//{
		//    bid = revAddress / 64;
		//    bankSet(bid);

		//    if (!progReadBlock(bid)) return false;

		//    deviceRevision[0] = readBuffer[revAddress+0];
		//    deviceRevision[1] = readBuffer[revAddress+1];

		//    Output.Text += string.Format("Device's code revision is {0:X2}.{1:X2}\r\n",
		//    deviceRevision[0], deviceRevision[1]);
		//    scroll();
			
		//    ConvertSerialNumber2Bytes( serialNumber = 0x0b000002);

		//    return true;
		//}
		
		// Standard Modules

		bid = revAddress / 64;
			
		bankSet(bid);
		
		// Read the last two blocks into readBuffer
		
		if (!progReadBlock(bid))   return false;		// Load sn + rev into readBuffer
		if (!progReadBlock(bid+1)) return false;		// cal tables

		// Locate rev & serial number
		
		addr = revAddress;		

		for (int i = 0; i < 2;) deviceRevision[i++] = readBuffer[addr++];
		for (int i = 0; i < 4; ) serialNumberBytes[i++] = readBuffer[addr++];

		Output.Text += string.Format("Device's code revision is {0:X2}.{1:X2}\r\n", 
			deviceRevision[0], deviceRevision[1]);
	
		Output.Text += string.Format("Device's serial number is {0:X2}{1:X2}{2:X2}{3:X2}\r\n",
			serialNumberBytes[0],serialNumberBytes[1],serialNumberBytes[2],serialNumberBytes[3]);


		serialNumber =  ((uint)(serialNumberBytes[0] << 24)) |
						((uint)(serialNumberBytes[1] << 16)) |
						((uint)(serialNumberBytes[2] << 8)) |
						((uint)(serialNumberBytes[3] << 0));
						
		scroll();

		/// calibrationDump();

		switch (serialNumberBytes[0])
		{
		case 0x0B: if (productName == "30-0016") return true;
				break;
		case 0x0E: if (productName == "30-0035") return true;
				break;
        case 0x11: if (productName == "30-0039") return true;
                break;
		case 0x0D: if (productName == "30-0034") return true;
				break;
		case 1:	if (productName == "30-0014") return true;
				break;
		case 2:	if (productName == "30-0015") return true;
				break;
		case 3:	if (productName == "30-0018") return true;
				break;
		case 4:	if (productName == "30-0023") return true;
				break;
		case 6: if (productName == "30-0013") return true;
				break;
		case 7: if (productName == "30-0028") return true;
				break;
		case 8: if (productName == "30-0027") return true;
				break;
		case 9: if (productName == "30-0026") return true;
				break;
		}

		return false;	
	}

	// Currently only the SID uses PROTECTION info
	
	void
	progSetProtection()
	{
		byte[]	din = new byte[8];
		byte	addr = Constants.BUFFER;
		int		i,j,pi;

		// Write 8 x 8 bytes from protection buffer into DUTs BUFFER
		
		bankSet(0);
		
		for (pi = 0, j = 0; j < 8; ++j)
		{
			for (i = 0; i < 8; ) din[i++] = protection[pi++];
		
			SetOutputReport(din, 8);

			cmd[0] = Constants.TC_MW;
			cmd[1] = addr;
			cmd[2] = 8;

			USB_Command( 2);
			
			addr += 8;
		}
		
		_que(Constants.OP_PROTECT, 0, Constants.BUFFER, clockProgram, 0x56, 0, 40);
	}
	
	void 
	progProgram()
	{
		int		bid, bcnt;
		int		pi;

		if (!aTest()) return;
		
		// Device Serial number is valid, and its current revision info

		if (!fileCheck()) return;
		
		if (!erased)
		{
			progEraseAll();
		}

		message("Starting device programming");
		
		bcnt = (fileCount1 + 63) / 64;
		progressBar.Maximum = bcnt;
		progressBar.Value = 0;

		bankSet(-1);

		for(pi = 0, bid = 0; 0 < bcnt--; ++bid)
		{
			bankSet(bid);
			progWriteBlock((byte) (bid & 0x0FF), pi);
			pi += 64;
			progressBar.Value += 1;
			progressBar.Invalidate();
			progressBar.Update();
			Application.DoEvents();
		}
		
		if (sid) progSetProtection();
	}

	bool
	DeviceCheck()
	{
		if (gc1)
		{
			if (device != 0x011)
			{
				PopUp("The Module attached is not a RKE!",true);
				return false;
			}
			return true;
		}
		if (gc2)
		{
			if (device != 0x011)
			{
				PopUp("The Module attached is not a RKL!", true);
				return false;
			}
			return true;
		}
        if (hk1)
        {
            if (device != 0x011)
            {
                PopUp("The Module attached is not a HK1!", true);
                return false;
            }
            return true;
        }
		if (NET)
		{
			if (device != 0x011)
			{
				PopUp("The Module attached is not a NET!", true);
				return false;
			}
			return true;
		}
		if (sid)
		{
			if (device != 0x017)
			{
				PopUp("The Module attached is not a SID!",true);
				return false;
			}
			return true;
		}
		if (device != 0x012 && device != 0x014 && device != 0x015 && device != 0x010)
		{
			PopUp("The attached is not a ReefKeeper module!",true);
			
			return false;
		}
		return true;
	}

	void
	_setDevice(byte x, bool display)
	{
		device = 0;
		
		switch( x & 0x0FF)	// Family
		{
		case 0x010:	family = Constants.DIAMOND;
					if (display) message("Device is DIAMOND");
					device = 0x010;
					break;
		case 0x011:	family = Constants.HYDRA;
					if (display) message("Device is a control unit");
					device = 0x011;
					break;
		case 0x012:	family = Constants.LITHIUM;
					if (display) message("Device is a RK Module");
					device = 0x012;
					break;
		case 0x015:	family = Constants.NEUTRON;
					if (display) message("Device is NEUTRON");
					device = 0x015;
					break;
		case 0x017:	family = Constants.RADON;
					device = 0x17;
					if (display) message("Device is RADON");
					break;
		case 0x014: family = Constants.LITHIUM;
					if (display) message("Device is Proton");
					device = 0x014;
					break;
		default:	family = 0;
					if (display) message("Unknown IC type");
					break;
		}	
	}

	void
	progSiliconId(bool display)
	{
		byte[]	dout = new byte[8];
		byte	rev,x;
		char	r1,r2;

		_que(Constants.OP_TABLE_READ,0,0, 0,0,0, 10);

		rev = ior(0x0F0);	// Acc
		x = ior(0x0F3);	// Acc

		r1 = (char) ('A' + ((rev & 0x0F0) >> 4) - 1);
		r2 = (char) ('A' + ((rev & 0x00F) >> 0) - 1);

		if (display)
		{
			Output.Text += string.Format("Device Rev {0}{1}",r1,r2) + "\r\n";
			scroll();
		}
		mrn((byte) 0x0F8, dout);

		_setDevice(x, display);
		
		if (device ==  0x011) // Hydra
		{
			// Set Port3 DM210 to 011 (Rpu)
			iows(0x10C, 0xFF);	// DM0
			iows(0x10D, 0xFF);	// DM1
			iows(0x00F, 0x00);	// DM2
			iows(0x00C, 0x00);	// PRT3DR = 00
			
			// Set Port4 DM210 to 011 (Rpu)
			iows(0x110, 0xFF);
			iows(0x111, 0xFF);
			iows(0x013, 0x00);
			
			// R+ on Speaker
			iows(0x010, 0x02); // PRT4DR = BIT1 Speaker
				
			byte	pe = ior(0x010);
			
			if ((pe & 0x02) != 0) // P4[1]
			{
				// GC2
				
				if (gc1) PopUp("The attached is not a RKE Head unit!",true);
			}
			else
			{
				if (gc2 || hk1) PopUp("The attached is a RKE Head unit!", true);
			}
		}
		
	}

	//	The TC_POWER command 0/1 does not change the drive state of
	//	the SDA line. It ONLY turns on/off the power pin !!!
	
	public void
	progPower(bool on)
	{
		// First initilialize and turn off power

		cmd[0] = Constants.TC_POWER;
		cmd[1] = 0;
		cmd[2] = 0xFF;
		cmd[3] = 0xAA;
		cmd[4] = 0x55;
		
		USB_Command(2);

		acquired = false;
		
		Thread.Sleep(100);

		if (on)
		{
			cmd[0] = Constants.TC_POWER;
			cmd[1] = 1;
			USB_Command(2);
			powered = true;
			Power.Checked = true;
		}
	}

	public void
	progPowerOff()
	{
		cmd[0] = Constants.TC_POWER;
		cmd[1] = 0;
		USB_Command(2);
		
		powered = false;
		acquired = false;
		Power.Checked = false;

		cmd[0] = Constants.USBCMD_INIT;
		USB_Command(2);
	}

    //  !!! TC_ACQUIRE needs to have a timeout!
    //  Modify SID code and remove these comments

	void
	progAcquire()
	{
		int		tc;

		progressBar.Value = 0;
		progressBar.Maximum = 31;
		
		// Turn power Off AND setup for TC modes

		cmd[0] = Constants.TC_INIT;
		USB_Command(2);
		cmd[0] = Constants.TC_POWER;
		cmd[1] = 0;
		USB_Command(2);
		
		Thread.Sleep(400);

		for(tc = 3; 0 != tc--;)
		{
			cmd[0] = Constants.TC_ACQUIRE;	// does a TC_POWER(1) first
			USB_Command(50);

			if (status[2] == 0x017) 
			{ 
				acquired = true;
				
				break; 
			}

			cmd[0] = Constants.TC_POWER;
			cmd[1] = 0;
			USB_Command(2);

			for(int sc = 0; sc < 10; ++sc)
			{
				++progressBar.Value;
				Application.DoEvents();
				Thread.Sleep(100);
			}
		}
		progressBar.Value = 0;
		
		if (acquired)
		{
			powered = true;
			Power.Checked = true;

			progSiliconId(false);
		}
		else
		{
			powered = false;
			Power.Checked = false;
		}
	}

	int
	sizeTest(int bcnt)
	{
		int		size;
	
		if (family == Constants.HYDRA) size = 4 * 128;
		else
		if (family == Constants.DIAMOND) size = 256;
		else
		if (family == Constants.LITHIUM) size = 1 * 64;
		else
		if (family == Constants.RADON) size = 2 * 128;
		else
		if (family == Constants.NEUTRON) size = 1 * 128;
		else return 0;

		if (bcnt != 0 && bcnt != size)
		{
			Output.Text += string.Format("File size is too small, must be {0:D} bytes\r\n", size*64);
			return 0;
		}
		return size;
	}

	int
	progCheckSum(int count)
	{
		int		s1;
		int		bcnt,size;
		byte	b1,b2;
		byte	scnt,page;

		if (!aTest()) return 0;

		bcnt = size  = sizeTest(bcnt = (count + 63) / 64);
		
		if (size == 0) return 0; // Should not happen


		if (family == Constants.DIAMOND)
		{
			_que(Constants.OP_CHECKSUM, (byte) (bcnt & 0x0FF), 0, 0, 0, 0, 60);

			b1 = mr(0x0F8);
			b2 = mr(0x0F9);

			s1 = (b1 & 0x0FF) + (b2 & 0x0FF) * 256;
			
			return s1 & 0x0FFFF;
		}

		// Other PSoC families
		
		s1 = 0;
		page = 0;		
		// Get DUT's checksum

		while (bcnt > 0)
		{
			if (bcnt >= 128)
			{
				scnt = 128;
				bcnt -= 128;
			}
			else 
			{
				// Last pass we do all but last block

				scnt = (byte) (bcnt);
				bcnt = 0;
			}

			iows(0x1FA,page);
			iows(0x000,0);
			_que(Constants.OP_CHECKSUM,scnt,0, 0, 0,0, 50);

			b1 = mr(0x0F8);
			b2 = mr(0x0F9);

			s1 += (b1 & 0x0FF) + (b2 & 0x0FF) * 256;

			++page;
		}
		return s1 & 0x0FFFF;
	}
}
}
