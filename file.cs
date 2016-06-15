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

//	GET_DATA:	Asks the Master to get from 1 -> 64 bytes from dongle.
//				The master does a read from dongle for full amount in one transfer
//	SEND_DATA:	Asks the master to send to dongle 1 -> 64 bytes.
//				
namespace 
Programmer
{
public
partial class
  MainForm
  {
	String	key = "If you can read this, you are nasty Fat Bastard";

	//bool	calDataValid = false;
	byte	majorRevision = 0;
	byte	minorRevision = 0;
	int		fileCount1 = 0;
	int		fileCount2 = 0;
	int		fileCheckSum1 = 0;
	int		fileCheckSum2 = 0;
	int		macAddress = 0x7000;	// last 2 bytes
	byte[]	blockSet1 = new byte[1 * K];
	byte[]	blockSet2 = new byte[2 * K];
	
	const int FUNCTION_PUMP	= 1;
	const int FUNCTION_CONTROL = 2;
	const int FUNCTION_MTIMER = 3;
	const int FUNCTION_STIMER = 4;
	const int FUNCTION_ADV_LIGHT = 5;
	const int FUNCTION_LUNAR = 6;
	const int FUNCTION_SWITCH = 7;
			
	
	//	Look in directory for largest revision file, and set revision data
	//	This routine is always called early. It is a seed for file selection.

	void
	fileRevisionDirScan()
	{
		string			dname = revisionDirectory + "\\" + productName;
		string			fname;
		byte			major,minor;
		DirectoryInfo	dir;
		
		dir = new DirectoryInfo(dname);

		majorRevision = 1;
		minorRevision = 1;
		
		// Look in Firmware directory and scan for revision files.
		// We are looking for names that match revision naming
		// conventions.
		
		if (!dir.Exists) return;
		
		foreach(FileInfo file in dir.GetFiles())
		{
			fname = file.Name;
			
			if (!fileStandardName(fname)) continue;

			try
			{
				major = (byte) Convert.ToByte(fname.Substring(6,2),16);
				minor = (byte) Convert.ToByte(fname.Substring(8,2),16);

				if (major > majorRevision) majorRevision = major;
				if (minor > minorRevision) minorRevision = minor;
			}
			catch (Exception e)
			{
				majorRevision = 1;
				minorRevision = 1;
			}
		}
	}
	
	// File names must conform to XXX-1-XXXX.hex or XXX-1-XXXXV.hex

	bool
	fileStandardName(String name)
	{
		string s;
		
		if (!name.Contains(".hex")) return false;
		
		return name.Contains(prefix);
		//return name.Substring(3,3) == "-1-";
	}
	
	bool
	fileFlashName(String name)
	{
		if (!fileStandardName(name)) return false;
		
		if (name.Contains("F.HEX") ||
			name.Contains("F.hex") ||
			name.Contains("f.hex")) return true;

		return false;
	}
	
	bool
	fileCheck()
	{
		if (productName == "")
		{
			message("No product selected!");
			return false;
		}

		if (fileCount1 != 0) return true;

		if (!fileLoadCode(true)) return false;

		return fileCount1 > 0;
	}

	void
	fileSetProtection()
	{
	    int	i;
		
		//	All blocks write-protected
		
	    for(i = 0; i < protection.Length; ) protection[i++] = 0xFF;

		// Different modules have some blocks made writable.
		
	    if (productName == "30-0011") protection[i = 63] = 0x00;
	    else
	    if (productName == "30-0012") protection[i = 127] = 0x00;
	    else
	    if (productName == "30-0014") protection[i = 15] = 0x0F;
	    else
	    if (productName == "30-0015") protection[i = 15] = 0x0F;
	    else
	    if (productName == "30-0016") protection[i = 31] = 0x0F;
	    else
	    if (productName == "30-0018") protection[i = 15] = 0x0F;
	    else
	    if (productName == "30-0023") protection[i = 15] = 0x0F;
	    else
	    if (productName == "30-0024") protection[i = 127] = 0x00;
	    else
	    if (productName == "30-0026") protection[i = 31] = 0x00;
	}
	
	void
	fileDetermineRevision(string fname)
	{
		int sl = fname.Length;

		majorRevision = 1;
		minorRevision = 1;

		if (fileStandardName(fname))
		{
			int	i = fname.IndexOf(prefix);
			fname = fname.Substring(i);
			
			majorRevision = (byte)Convert.ToByte(fname.Substring(6, 2), 16);
			minorRevision = (byte)Convert.ToByte(fname.Substring(8, 2), 16);
		}
	}
	

	bool
	fileLoadCode(bool codeFlag)
	{
		string			name;
		string			dir;
		DirectoryInfo	di;
		
		// Determine revisons codes
		
		fileRevisionDirScan();
		
		openFileDialog1.Filter = "HEX|*.hex|AllFiles|*.*";

		if (codeFlag)
		{
			dir = revisionDirectory + "\\" + productName;
		}
		else
		{
			dir = DirName();
		}
		
		di = new DirectoryInfo(dir); 

        if (!di.Exists)
        {
			Output.Text += string.Format("Firmware directory can't be found:\r\n    {0:s}\r\n",dir);
			scroll();
        }
		openFileDialog1.Title = string.Format("Locate the {0}'s firmware file",ShortName());
		
		openFileDialog1.FileName = String.Format("{0}-{1:X2}{2:X2}.hex", prefix, majorRevision, minorRevision);
		openFileDialog1.InitialDirectory = dir;
		

		if (openFileDialog1.ShowDialog() != DialogResult.OK) return false;
		
		name = openFileDialog1.FileName;
		
		if (!name.Contains(".hex")) return false;

		// Extract revision info based on name

		if (codeFlag) fileDetermineRevision(name);

		fileSetProtection();	// Set default protection bits
		
		// Load data and check embedded fileRevision info

		LoadHexFile(name, codeFlag);
		
		if (codeFlag)
		{
			fileDecrypt(fileBuffer1, fileCount1, true);
		
			// Normally we preserve cal, but the Factory can change serial number
		
			
		
			return fileExtractRevision();
		}
		else return true;
	}

	void
	fileBuildSerialNumber()
	{
		ConvertSerialNumber2Bytes(serialNumber = newSerialNumber);
	}
	
	bool
	fileNewSerialNumber()
	{
		string sname = "P:\\ReefKeeper3\\SerialNumbers\\" + productName;
		string line;
		string[] tokens;

		if (productName == "")
		{
			message("Select product from menu!");
			return false;
		}
		
		if (gc1 || gc2 || hk1) return true;
		
		if (!File.Exists(sname))
		{
			message("Serial Number database is not accessable!");
			return false;
		}

		StreamReader src = File.OpenText(sname);

		// {Hex date} string pairs on each line, date is ignored

		while ((line = src.ReadLine()) != null)
		{
            if (line.Length > 2)
            {
                tokens = line.Split(' ');
                newSerialNumber = (uint)Convert.ToInt32(tokens[0], 16);
            }
		}
		src.Close();

		Output.Text += string.Format("Last serial number in DB is {0:X8}\r\n", newSerialNumber);
		Output.Text += string.Format("New serial number will be {0:X8}\r\n", ++newSerialNumber);
		scroll();

		Number.Text = string.Format("{0:X8}", newSerialNumber);

		StreamWriter dst = File.AppendText(sname);

		dst.WriteLine("{0:X8} {1}/{2}/{3}",
			newSerialNumber, 
			DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));

		dst.Close();
		
		return true;
	}

	bool
	fileExtractRevision()
	{
		int addr = revAddress;
		
		majorRevision = fileBuffer1[addr+0];
		minorRevision = fileBuffer1[addr+1];

		Output.Text += string.Format("Loaded revision is {0:X2}.{1:X2}\r\n", majorRevision, minorRevision);
		scroll();
		
		if (gc1 || gc2 || hk1) return true;
		
		// The remaining types have the serial number after the rev
		
		if (fileBuffer1[revAddress+2] != moduleType)
		{
			PopUp("The file loaded is not appropriate for the selected product",true);
			return false;
		}
		
		return true;
	}
	
	// Called only if Factory is checked AND after the fileBuffer has been loaded and de-crypted
	// This routine forces the serial number only

	void
	fileSerializeBuffer()
	{	
		if (gc1 || gc2 || hk1) return;

		fileBuffer1[revAddress + 2] = serialNumberBytes[0];
		fileBuffer1[revAddress + 3] = serialNumberBytes[1];
		fileBuffer1[revAddress + 4] = serialNumberBytes[2];
		fileBuffer1[revAddress + 5] = serialNumberBytes[3];
	}
	
    // Read the HEX file that conatins the modules cal restore data

	public bool
	fileCalRead(string logName, byte [] buffer, int addr)
	{
		string			line;
		string []		tokens;
		short			value;
		int				i,count;

		StreamReader src = File.OpenText(logName);
			
		if (src == null)
		{
			PopUp("That file could not be read!", false);
			return false;
		}
		
		i = 0;
		count = 3 * fileCalCount();	// Offset, Target, Gain triples

		//	For GC2 get internal flash
		//	15 64 B blocks
		//	Cal info is in last block

		if (gc2 || hk1)
		{
			int b,k;

			for(b = 15; b > 0; --b)
			{
				for(k = 0; k < 4; ++k)
				{
					// get 16 bytes per line, ignore short lines

					while(null != (line = src.ReadLine()))
					{
						if (line.Length >= (3 * 16)) break;
					}

					fileGetHex(line, buffer, addr, 16);
					addr += 16;
				}
			}
		}
		else
		while( (count > 0) && ((line = src.ReadLine()) != null))
		{
			--count;

			tokens = line.Split('=');
			
			value = Convert.ToInt16(tokens[1]);
			
			if (tokens[0].Contains("Offset"))
			{
				i = Convert.ToInt32(tokens[0].Substring(7,1));
				
				putShort(buffer, value, addr + (i * 6),0);
			}
			else
			if (tokens[0].Contains("Target"))
			{
				i = Convert.ToInt32(tokens[0].Substring(7,1));

				putShort(buffer, value,addr + (i * 6),2);
			}
			else
			if (tokens[0].Contains("Gain"))
			{
				i = Convert.ToInt32(tokens[0].Substring(5,1));

				putShort(buffer, value,addr + (i * 6),4);
			}
		}

		src.Close();
		
		return true;
	}

    public Int16
    byte2int16(byte [] data, int i)
    {
        Int16   n;

        n = (Int16) (data[i] * 256 + data[i+1]);
        return n;
    }

    public bool
    fileGC2CalRead(string logName, Int16[] otg)
    {
        string      line;
        int         addr;
        byte []     tBuf = new byte[4 * 15 * 16];

        StreamReader src = File.OpenText(logName);

        if (src == null)
        {
            PopUp("That file could not be read!", false);
            return false;
        }
        //	Read file
        //	15 64 B blocks
        //	Cal info is in last block
        
        int b, k;
        addr = 0;

        for (b = 0; b < 15; ++b)
        {
            for (k = 0; k < 4; ++k)
            {
                // get 16 bytes per line, ignore short lines

                while (null != (line = src.ReadLine()))
                {
                    if (line.Length >= (3 * 16)) break;
                }

                fileGetHex(line, tBuf, addr, 16);
                addr += 16;
            }
        }

        addr = 14 * 4 * 16;  // point to last group of 4 lines

        otg[0] = byte2int16(tBuf, addr + 2);
        otg[1] = byte2int16(tBuf, addr + 4);
        otg[2] = byte2int16(tBuf, addr + 6);

        src.Close();

        return true;
    }
	
	//	!!! dbNewMAC is only called in Factory mode !!!
	//	The max.Text field is either used or modified depending on AutoIncrement
	//	The code buffer is modified
	
	bool
	dbNewMAC()
	{
		string	sname = "P:\\ReefKeeper3\\SerialNumbers\\" + productName + ".mac";
		string	line;
		string[]tokens;
		long	li;
		int		ma = 0x07010;

		if (productName == "")
		{
			message("Select product from menu!");
			return false;
		}

		if (AutoIncrement.Checked)
		{
			if (!File.Exists(sname))
			{
				message("MAC address database is not accessable!");
				return false;
			}

			StreamReader src = File.OpenText(sname);

			// {Hex date} string pairs on each line, date is ignored
			//	Get last record

			while ((line = src.ReadLine()) != null)
			{
				tokens = line.Split(' ');
				ma = Convert.ToInt32(tokens[0], 16);
			}
			src.Close();
			
			
			Output.Text += string.Format("Last MAC address in DB is {0:X4}\r\n", ma);
			Output.Text += string.Format("New MAC address will be {0:X4}\r\n", ++ma);
			scroll();
			mac.Text = string.Format("0050C2A0{0:X4}", ma);

			StreamWriter dst = File.AppendText(sname);

			dst.WriteLine("{0:X4} {1}/{2}/{3}",
				ma,
				DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"), DateTime.Now.ToString("dd"));

			dst.Close();
		}
		else
		{
			li = Convert.ToInt64(mac.Text, 16);
			ma = (int) (li & 0x0FFF);
		}

		macAddr[0] = 0x00;
		macAddr[1] = 0x50;
		macAddr[2] = 0xC2;
		macAddr[3] = 0xA0;
		macAddr[4] = (byte)(0x70 | ((ma >> 8) & 0x00F));
		macAddr[5] = (byte)(ma & 0x0FF);
		
		// Modifiy the program code buffer
		
		for(int i = 0; i < 6; ++i) fileBuffer2[0x1FFF0 + i] = macAddr[i];
		
		return true;
	}

	bool
	fileRestoreMAC()
	{
		string	name = LogName() + ".mac";
		int		addr;

		macAddr[0] = 0x00;
		macAddr[1] = 0x50;
		macAddr[2] = 0xC2;
		macAddr[3] = 0xA0;
		macAddr[4] = 0x7F;
		macAddr[5] = 0xFF;


		addr = fileMACRead(name,  0);

		if (addr == 0) return false;
	
		macAddr[4] = (byte) ((addr & 0x0FF00) >> 8);
		macAddr[5] = (byte) (addr & 0x000FF);

		copy8(fileBuffer2, 0x1FFF0, macAddr, 0);
		
		mac.Text = string.Format("0050C2A0{0:X4}", addr);
		
		return true;
	}

	//	MAC addresses are 00,50,C2,A0,70,00 -> 00,50,C2,A0,7F,FF
	//	Read from *.mac file

	int
	fileMACRead(string logName, int addr)
	{
		string line;
		short value;

		if (!File.Exists(logName))
		{
			PopUp("The MAC restore file does not exist!", false);
			return 0;
		}
		StreamReader src = File.OpenText(logName);

		if (src == null)
		{
			PopUp("The MAC restore file could not be read!", false);
			return 0;
		}

		value = 0;

		// Skip the first 4 bytes, 8 chars

		if ((line = src.ReadLine()) != null)
		{
			line = line.Substring(8,4);

			value = Convert.ToInt16(line, 16);
		}

		src.Close();

		return value;
	}
	
	void
	ConvertSerialNumber2Bytes(uint sn)
	{
		serialNumberBytes[0] = (byte)((sn & 0x0FF000000) >> 24);
		serialNumberBytes[1] = (byte)((sn & 0x000FF0000) >> 16);
		serialNumberBytes[2] = (byte)((sn & 0x00000FF00) >> 8);
		serialNumberBytes[3] = (byte)((sn & 0x0000000FF) >> 0);
	}

	public string
	DirName()
	{
		String s = 
			Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments);
		return s + "\\Digital Aquatics\\" + productName;
	}

	public string
	ShortName()
	{
		return prefix.Substring(0,3);
	}
	
	public string
	LogName()
	{
		string	mName = ModuleName();
		string	dir = DirName();
		string	name;

		if (gc2)
		{
			name = dir + "\\RKL-0A000001";
		}
        else
        if (hk1)
        {
            name = dir + "\\HK1-0F000001";
        }
		else
		{
			name = dir + string.Format("\\{0:s}-{1:X8}",mName,serialNumber);
		}
		return name;
	}


	// Cleans up the readBuffer with valid serial number and cal data
	// This is NOT used during factory mode.

	//	File format:
	//	CalInfo[]
	//
	//	The file's name ==> serial number, for GC2 it is 99000001

	bool
	fileLogValidate(bool check)
	{
		string		logName,dir;
		int			addr,i,n;
		byte[]		calBuffer = new byte[64];


		if (gc1)
		{
			badRead = false;
			
			return true;
		}

		if (sid)
		{
			badRead = false;
			
			return true;
		}

		dir = DirName();
		logName = LogName() + ".cal";
		
		// If the read SN is bad, ask for a file
		
		if (badRead || (serialNumberBytes[0] != moduleType))
		{
			if (check)
			{
				PopUp("Module's serial number is corrupted,\nplease select a backup file for recovery", true);
			}
			else 
			{
				PopUp("Module's serial number is corrupted",false);
				return false;
			}
			
			openFileDialog1.Filter = "CAL|*.cal";
			openFileDialog1.FileName = "";
			openFileDialog1.InitialDirectory = dir;

			openFileDialog1.Title = string.Format("Locate {0}'s calibration file",ShortName());
			
			if (openFileDialog1.ShowDialog() != DialogResult.OK) return false;

			logName = openFileDialog1.FileName;
			
			if (!File.Exists(logName))
			{
				PopUp("That calibration restore file does not exist!",true);
				return false;
			}
			
			int		sl = logName.Length;
			string	sn = logName.Substring(sl - 12,8);
			
			if (gc2) 
			{
				ConvertSerialNumber2Bytes( serialNumber = 0x0A000001 );
			}
            else
            if (hk1)
            {
                ConvertSerialNumber2Bytes(serialNumber = 0x0F000001);
            }
			else 
			{
				serialNumber = (uint) Convert.ToInt32(sn,16);

				newSerialNumber = serialNumber;

				Output.Text += string.Format("Recovered serial number is '{0:X8}'\r\n",serialNumber);
				scroll();
				
				ConvertSerialNumber2Bytes(serialNumber);

				//	For all Modules the serial number is stored 4 byte back from
				//	revision info

				addr = revAddress + 2;
				
				readBuffer[addr++] = serialNumberBytes[0];
				readBuffer[addr++] = serialNumberBytes[1];
				readBuffer[addr++] = serialNumberBytes[2];
				readBuffer[addr++] = serialNumberBytes[3];

				if (!NET)
				{
					addr = calAddress;
					
					// Clear cal table in readBuffer
			
					for(i = 0; i < 64; ) readBuffer[addr + i++] = 0;
				}
			}

			// Now read recovery file
			
			if (!fileCalRead(logName, readBuffer, calAddress)) return false;
			
			badRead = false;
			
			return true;
		}

		//////////////////////////////////////////////////
		// The device's internal info is OK , check it	//
		//////////////////////////////////////////////////

		// Determine the size of the cal table
		
		n = fileCalCount();

		// Read cal log into buffer and check against readBuffer

		if (File.Exists(logName)) 
		{
			bool	tf = true;
            Int16[] otg1 = new Int16[3];
            Int16[] otg2 = new Int16[3];

			if (gc2 || hk1)
			{
				if (!UserMode()) return true;

                // readBuffer has current values, now read restore file

                fileGC2CalRead(logName, otg1);

                otg2[0] = byte2int16(readBuffer, calAddress);
                otg2[1] = byte2int16(readBuffer, calAddress+2);
                otg2[2] = byte2int16(readBuffer, calAddress+4);

                if ((otg1[0] == otg2[0]) && (otg1[1] == otg2[1]) && (otg1[2] == otg2[2]))
                {
                    return true;
                }

				gc2Restore	form = new gc2Restore();
				DialogResult	dr;
				
				dr = form.ShowDialog();
				
				if (dr == DialogResult.Cancel) 
					return false;

				if (dr == DialogResult.OK)
				{
					// Create
					
					return fileCalWrite(logName);
				}

				if (dr == DialogResult.Yes)
				{
					// restore into readBuffer
					
					return fileCalRead(logName, readBuffer, 0x7C40);
				}
				return true;
			}


			// Restore op for the other modules

			// Clear the cal buffer

			for(i = 0; i < 64; ) calBuffer[i++] = 0;
				
			fileCalRead(logName, calBuffer, 0);

			// compare readBuffer to calBuffer
			
			for(i = 0; i < n * 6; ++i)
			{
				if (calBuffer[i] != readBuffer[calAddress + i]) {tf = false; break;}
			}
			
			if (tf)
			{
				if (!NET) message("Module's internal calibration data matches restore file");
				return true;
			}
			
			// Data does not match
			
			if (check)
			{
				CalForm	cal = new CalForm();
				DialogResult	dr;
				
				dr = cal.ShowDialog();
				
				if (dr == DialogResult.Cancel) return false;
				if (dr == DialogResult.OK)
				{
					// Module -> Restore, return success
					
					return fileCalWrite(logName);
				}
			}
			else
			{
				message("Module's internal calibration data does not match restore file");
				return true;
			}
			
			// dr == YES -> use restore file
			
			message("Copying the restore calibration data");
			
			// Copy file's data into read buffer
			
			for (i = 0; i < 64; ++i)
			{
				readBuffer[calAddress + i] = calBuffer[i];
			}
			return true;
		}
		
		// cal log does not exist, Create it!
		
		return fileCalWrite(logName);
	}

	uint
	fileFindSerialNumber()
	{
		string	dir, logName;
		uint	num;
		
		if (gc1)
		{
			PopUp("This operation is not applicable to a GC1!", false);
			return 0;
		}

		//if (sid)
		//{
		//    PopUp("This operation is not applicable to a SID!", false);
		//    return 0;
		//}

		dir = DirName();
		logName = LogName();

		openFileDialog1.Filter = "CAL|*.cal";
		openFileDialog1.FileName = "";
		openFileDialog1.InitialDirectory = dir;
		openFileDialog1.Title = "Locate a calibration file for serial number restoration";
		
		if (openFileDialog1.ShowDialog() != DialogResult.OK) return 0;

		logName = openFileDialog1.FileName;

		int sl = logName.Length;
		string sn = logName.Substring(sl - 12, 8);

		num = (uint) Convert.ToInt32(sn, 16);

		return num;
	}
	
	//bool
	//fileRestoreSerialNumber()
	//{
	//    string dir,logName;
	//    int		addr;
	//    byte[] calBuffer = new byte[64];

	//    if (gc1)
	//    {
	//        PopUp("This operation is not applicable to a GC1!",false);
	//        return false;
	//    }

	//    if (sid)
	//    {
	//        PopUp("This operation is not applicable to a SID!",false);
	//        return false;
	//    }

	//    dir = DirName();
	//    logName = LogName();

	//    openFileDialog1.Filter = "CAL|*.cal";
	//    openFileDialog1.FileName = "";
	//    openFileDialog1.InitialDirectory = dir;

	//    if (openFileDialog1.ShowDialog() != DialogResult.OK) return false;

	//    logName = openFileDialog1.FileName;

	//    if (!File.Exists(logName))
	//    {
	//        PopUp("That calibration restore file does not exist!",true);
	//        return false;
	//    }

	//    int sl = logName.Length;
	//    string sn = logName.Substring(sl - 12,8);

	//    newSerialNumber = serialNumber = (uint) Convert.ToInt32(sn,16);

	//    Output.Text += string.Format("Recovered serial number is '{0:X8}'\r\n",serialNumber);
	//    scroll();

	//    ConvertSerialNumber2Bytes(serialNumber);

	//    addr = revAddress+2;

	//    readBuffer[addr++] = serialNumberBytes[0];
	//    readBuffer[addr++] = serialNumberBytes[1];
	//    readBuffer[addr++] = serialNumberBytes[2];
	//    readBuffer[addr++] = serialNumberBytes[3];

	//    // Clear cal table in readBuffer

	//    addr = calAddress;
		
	//    for(int i = 0; i < 64; ++i) readBuffer[calAddress + i] = 0;

	//    // Now read this file

	//    if (!fileCalRead(logName,readBuffer,calAddress)) return false;

	//    badRead = false;

	//    return true;
	//}

	int
	fileCalCount()
	{
		if (productName.Equals("30-0012")) return 1;	// RKL
        if (productName.Equals("30-0036")) return 1;	// HK1
        if (productName.Equals("30-0037")) return 1;	// PK1
		if (productName.Equals("30-0014")) return 1;	// PC4
		if (productName.Equals("30-0015")) return 5;	// SL1
		if (productName.Equals("30-0013")) return 5;	// SL2
		if (productName.Equals("30-0035")) return 2;	// AP1
		if (productName.Equals("30-0039")) return 2;	// AP2

		// All others have NO cal info

		return 0;
	}

	// Copy 64 bytes from readBuffer into the cal restore file
	
	void
	BlockWrite64Bytes( StreamWriter	dst, int addr)
	{
		string line;
		int		j,k,n;

		for(k = 0; k < 4; ++k)
		{
			line = "";

			for(j = 0; j < 16; ++j)
			{
				n = readBuffer[addr + j];
				line += string.Format("{0:X2} ",n);
			}
			dst.Write(line + "\r\n");

			addr += 16;
		}
		dst.Write("\r\n");
	}
	
	public bool
	fileCalWrite(string logName)
	{
		int addr,i,n;
		byte[] calBuffer = new byte[64];
		short o,t,g;
		FileStream file;


		// Determine the size of the cal table

		n = fileCalCount();
			
		try
		{
			file = new FileStream(logName,FileMode.Create,FileAccess.Write);
		}
		catch (SystemException )
		{
			file = null;
		}
		
		if (file == null)
		{
			Output.Text += string.Format("Can't create file '{0:s}'\r\n",logName);
			scroll();
			
			PopUp("Failed in attempt to backup calibration data to file!",true);
			
			return false;
		}
		
		StreamWriter	dst = new StreamWriter(file);

		if (gc2 || hk1)
		{
			int		j;

			addr = 0x07C40;

			dst.Write("; Slaves\r\n");
			BlockWrite64Bytes(dst, addr); addr += 64;

			dst.Write("; Controls\r\n");
			
			for(j = 0 ; j < 4; ++j)
			{
				BlockWrite64Bytes(dst, addr); 
				addr += 64;
			}
			
			dst.Write("; Alarms\r\n");

			for (j = 0; j < 4; ++j)
			{
				BlockWrite64Bytes(dst, addr);
				addr += 64;
			}

			dst.Write("; Timers\r\n");

			for (j = 0; j < 5; ++j)
			{
				BlockWrite64Bytes(dst, addr);
				addr += 64;
			}

			dst.Write("; System\r\n");

			BlockWrite64Bytes(dst, addr);
		}
		else
		{
			string line;
			
			addr = calAddress;
			
			for (i = 0; i < n; ++i)
			{
				o = (short)((readBuffer[addr++] * 256) & 0x0FF00);
				o |= (short)(readBuffer[addr++] & 0x0FF);

				t = (short)((readBuffer[addr++] * 256) & 0x0FF00);
				t |= (short)(readBuffer[addr++] & 0x0FF);

				g = (short)((readBuffer[addr++] * 256) & 0x0FF00);
				g |= (short)(readBuffer[addr++] & 0x0FF);

				line = string.Format("Offset[{0}]= {1}\r\n",i,o);
				dst.Write(line);
				line = string.Format("Target[{0}]= {1}\r\n",i,t);
				dst.Write(line);
				line = string.Format("Gain[{0}]= {1}\r\n",i,g);
				dst.Write(line);
			}
		}
		dst.Close();
		
		return true;
	}

	bool
	fileMACWrite()
	{
		string fName,line;
		FileStream file;

		fName = LogName() + ".mac";

		try
		{
			file = new FileStream(fName, FileMode.Create, FileAccess.Write);
		}
		catch (SystemException)
		{
			file = null;
		}

		if (file == null)
		{
			Output.Text += string.Format("Can't create file '{0:s}'\r\n", fName);
			scroll();

			PopUp("Failed in attempt to backup MAC address data to file!", true);

			return false;
		}

		StreamWriter dst = new StreamWriter(file);

		line = string.Format("0050C2A0{0:X4}\r\n", macAddress);
		dst.Write(line);
		dst.Close();

		return true;
	}

	void
	putShort(byte [] mem, short v, int addr, int offset)
	{
		byte	b0,b1;
		
		b1 = (byte) (v & 0x0FF);
		b0 = (byte) ((v & 0x0FF00) >> 8);
		
		mem[addr + offset + 0] = b0;
		mem[addr + offset + 1] = b1;
	}

	void
	copy8(byte [] dst, int i, byte [] src, int j)
	{
		for(int c = 0; c < 8; ++c)
		{
			dst[i++] = src[j++];
		}
	}

	void
	BufferCS()
	{
		int	fs;
		
		fs = fileCheckSum(fileBuffer1, fileCount1);
		
		Output.Text += string.Format("CheckSum is {0:X4}", fs);
		
		crlf();
		scroll();

	}
	
	int
	fileCheckSum(byte[] array, int count)
	{
		int sum;

		sum = 0;

		for (int i = 0; 0 < count--; ) sum += array[i++];

		return sum & 0x0FFFF;
	}

	void
	fileBufferFill(byte [] array, byte b)
	{
		for(int i = 0; i < array.Length; ) array[i++] = b;
	}

	//	Called after the PSoC has been acquired and its internal cal tables
	//	were read. Copies from the readBuffer (PSoC) into master data buffer.
	//	Puts the serial number into the fileBuffer
	
	void
	filePreserveCal(bool FTM)
	{
		int		i, ra, count;
		byte	r1,r2;
		bool	sf = true;
		
		if (gc1 || sid) return;	// These do not have serial numbers OR cal
		
		r1 = r2 = 1;
		ra = calAddress;
		
		// Remaining devices are 4K in size
		// Copy readBuffer into fileBuffer
		
		if (NET)
		{
			 count = 0;
		}
		else
		if (gc2 || hk1)
		{
			ra = 0x7C40;
			sf = false;
			
			// Get File's rev data
			
			r1 = fileBuffer1[revAddress+0];
			r2 = fileBuffer1[revAddress+1];

			count = 64 * 15;
			
			if (FTM)
			{
				count = 64 * 1;
				ra = 0x7FC0;
			}
		}
		else 
		{
			count = 64;
		}

		if (sf)
		{
			for (i = 0; i < 4; ++i)
			{
				fileBuffer1[revAddress + i + 2] = serialNumberBytes[i];
			}
		}
		
		for (i = 0; i < count; ++i, ++ra)
		{
			fileBuffer1[ra] = readBuffer[ra];
		}
		
		if (gc2 || hk1)
		{
			// Restore file's rev data
			
			fileBuffer1[revAddress+0] = r1;
			fileBuffer1[revAddress+1] = r2;
		}
	}

	// Get data in the form of XX XX ... XX hex bytes

	void
	fileGetHex(string line, byte [] array, int addr, int count)
	{
		int		i,j,l;
		byte	b;

		if (line == null) return;

		l = line.Length;

		for(j = i = 0; i < count; ++i)
		{
			if (j < l)
				 b = (byte) Convert.ToInt16(line.Substring(j, 2), 16);
			else b = 0;

			array[addr + i] = b;
			j += 3;
		}
	}

	void
	LoadHexFile(string name, bool codeFlag)
	{
		int		sum;
		int		length,addr,a2;
		int		type,data,count;
		int		i,j,rn,bs;
		bool	psoc = true;
		bool	pic = false;
		bool	pFlag = false;
		bool	enabled = true;
		string	line;
		byte	major1,major2;
		byte	minor1,minor2;


		fileCheckSum1 = fileCheckSum2 = count = fileCount1 = fileCount2 = 0;

		// Clear block in use records
		for (i = 0; i < blockSet1.Length; ) blockSet1[i++] = 0;
		for (i = 0; i < blockSet2.Length; ) blockSet2[i++] = 0;

		// Initialize buffers to cleared state

		if (codeFlag)
		{
			fileBufferFill(fileBuffer2, 0xFF);
			fileBufferFill(fileBuffer1, 0x00);
		}
		major1 = major2 = majorRevision;
		minor1 = minor2 = minorRevision;

		if (!File.Exists(name))
		{
			Output.Text += string.Format("Specified file '{0}' does not exist!\r\n",name);
			scroll();
			return;
		}
		// Clear memory used for security/protect

		StreamReader	src = File.OpenText(name);

		rn = i = 0;
		a2 = 0;

		psoc = true;

		while((line = src.ReadLine()) != null)
		{
			++rn;

			if (line.Substring(0,1) == ";")
			{
				// Comment ;PSoC MMnn  or ;PIC MMnn

				if (line.Substring(1,4)== "PSoC")
				{
					major1 = (byte)Convert.ToByte(line.Substring(6, 2), 16);
					
					if (line.Substring(8,1) == ".")
						 minor1 = (byte)Convert.ToByte(line.Substring(9, 2), 16);
					else minor1 = (byte)Convert.ToByte(line.Substring(8, 2), 16);
					
					if (!psoc) fileCount2 = count;

					psoc = true;
					pic = false;
					pFlag = false;
					enabled = true;
					count = fileCount1 = 0;
				}
				else
				if (line.Substring(1,3)== "PIC")
				{
					major2 = (byte)Convert.ToByte(line.Substring(5, 2), 16);
					
					if (line.Substring(7, 1) == ".")
						 minor1 = (byte)Convert.ToByte(line.Substring(8, 2), 16);
					else minor1 = (byte)Convert.ToByte(line.Substring(7, 2), 16);
					
					if (psoc) fileCount1 = count;

					pic = true;
					psoc = false;
					pFlag = false;
					enabled = true;
					count = fileCount2 = 0;
				}
				else
				if ((line.Length >= 11) && line.Substring(1, 10) == "Protection")
				{
					if (psoc) fileCount1 = count;

					pic = false;
					psoc = false;
					pFlag = true;
					enabled = true;
					count = 0;
				}
				continue;
			}

			if (!enabled) continue;

			sum = 0;

			// Character 0 ':' is ignored 
			length = Convert.ToInt16(line.Substring(1, 2),16);

			if (length < 2) continue;

			sum += length;

			// Grab 4 characters
			addr = 0x0FFFF & Convert.ToInt16(line.Substring(3, 4), 16);

			i = addr + ((a2 << 16) & 0x0F0000);

			bs = i / 64;

			if (psoc)
				 blockSet1[bs] = 1;
			else
			if (pic) blockSet2[bs] = 1;

			sum += (addr >> 8) & 0x0FF;
			sum += (addr & 0x0FF);

			type = Convert.ToInt16(line.Substring(7, 2), 16);
			sum += type;
			j = 9;

			if (type == 0)
			{
				while(0 < length--)
				{
					data = Convert.ToInt16(line.Substring(j, 2), 16);

					if (psoc)
						 fileBuffer1[i++] = (byte) (data & 0x00FF);
					else
					if (pic)
						fileBuffer2[i++] = (byte)(data & 0x00FF);
					else
					if (pFlag)
						protection[i++] = (byte)(data & 0x00FF);

					j += 2;
					sum += data;
					++count;
				}

				if (psoc) fileCount1 = count;
				else fileCount2 = count;
			}
			else
			if (type == 4)
			{
				// Extended address, length must be == 2
				// Does not count as loaded data

				if (length != 2)
				{
					message("Hex file has illegal format");
					fileCount1 = fileCount2 = count = 0;
					break;
				}

				if (psoc)
				{
					a2 = 0; 
					enabled = false;
					continue;
				}

				// Grab 4 characters, extended address
				a2 = Convert.ToInt16(line.Substring(j, 4), 16);

				data = Convert.ToByte(line.Substring(j, 2), 16);
				sum += data;
				j += 2;
				data = Convert.ToByte(line.Substring(j, 2), 16);
				sum += data;
				j += 2;

				if (a2 != 0) a2 = 0x01;	// constrain to 128K total
			}

			data = Convert.ToByte(line.Substring(j, 2), 16);
			data *= -1;
			data &= 0x00FF;
			sum  &= 0x00FF;

			if (data != sum)
			{
				Output.Text += string.Format("CheckSum error in record #{0:d}\r\n",rn);
				fileCount1 = fileCount2 = 0;
				break;
			}
			
			if (psoc)
			{
				fileCheckSum1 += sum;
				fileCheckSum1 &= 0x0FFFF;
			}
			else
			if (pic)
			{
				fileCheckSum2 += sum;
				fileCheckSum2 &= 0x0FFFF;
			}
		}
		src.Close();

		Output.Text += string.Format("{0:d} bytes loaded\r\n",
			fileCount1 + fileCount2);

		if ((major1 != major2) || (minor1 != minor2) )
		{
			Output.Text += string.Format("The file has corrupted revision data within!\r\n");
			scroll();
		}

		majorRevision = major1;
		minorRevision = minor1;
		
		// PIC queer, B3 must be forced to 0 at 1FFF9!!!!

		data = fileBuffer2[0x1FFF9];
		fileBuffer2[0x1FFF9] = (byte) (data & 0xF7);
	}
	
	// Decrypt the fileBuffer1
	
	void
	fileDecrypt(byte[] src, int count, bool decrypt)
	{
		int		i, sl = key.Length;
		byte	k;
		int		checksum = 0;

		if (count == 0) return;

		if ((src == fileBuffer1) && (src[0] == 0x080)) decrypt = false;
		
		for (i = 0; i < count; ++i)
		{
			if (decrypt)
			{
				k = (byte) key[i % sl];

				src[i] ^= k;
			}
			checksum += src[i];
		}
		checksum &= 0x0FFFF;
		
		Output.Text += string.Format("Program buffer's checksum = {0:X4}",checksum) + "\r\n";
		scroll();;
	}

	void
	fileSaveReadBuffer(bool log)
	{
		string name;

		if (readBufferSize == 0) return;
		
		if (log)
			 saveFileDialog.Filter = "LOG|*.log";
		else saveFileDialog.Filter = "HEX|*.hex";

		saveFileDialog.InitialDirectory = DirName();
		saveFileDialog.FileName = "GC1-Flash";

		if (DialogResult.OK == saveFileDialog.ShowDialog())
		{
			name = saveFileDialog.FileName;

			WriteFirmwareFile(name, log);
		}
	}
	
	void
	fileWrite4K(int addr, StreamWriter dst)
	{
		string line;
		int i,j;
		byte b;
		int sum;
		
		for (i = 0; i < 4*K; )
		{
			line = string.Format(":40{0:X4}00",addr);
			
			sum = 64;
			sum += (addr >> 8) & 0x0FF;
			sum += (addr & 0x0FF);

			for (j = 64; j-- > 0; ++i)
			{
				b = readBuffer[addr++];
				line += string.Format("{0:X2}",b);

				sum += b;
			}
			b = (byte)(0x0FF & -sum);
			line += string.Format("{0:X2}\r\n",b);
			dst.Write(line);
		}
	}
	
	void
	WriteFirmwareFile(string name, bool log)
	{
		FileStream file = new FileStream(name,FileMode.Create,FileAccess.Write);
		StreamWriter dst = new StreamWriter(file);

		if (log)
		{
			fileWrite4K(0x0000, dst);
			fileWrite4K(0x1000, dst);
			fileWrite4K(0x2000, dst);
			fileWrite4K(0x3000, dst);
			fileWrite4K(0x4000, dst);
			fileWrite4K(0x5000, dst);
			fileWrite4K(0x6000, dst);
			fileWrite4K(0x7000, dst);
			fileWrite4K(0x8000, dst);
			fileWrite4K(0x9000, dst);
			fileWrite4K(0xA000, dst);
			fileWrite4K(0xB000, dst);
			fileWrite4K(0xC000, dst);
			fileWrite4K(0xD000, dst);
			fileWrite4K(0xE000, dst);
			fileWrite4K(0xF000, dst);
		}
		else
		{
			fileWrite4K(0x0000, dst);
			fileWrite4K(0x8000,dst);
			fileWrite4K(0x9000,dst);
			fileWrite4K(0xA000,dst);
			fileWrite4K(0xB000,dst);
			fileWrite4K(0xC000,dst);
			fileWrite4K(0xD000,dst);
		}
	

		dst.Write(":00000001FF\r\n");
		dst.Close();
	}
	
	int
	getLong(int addr)
	{
		int l = 0;

		for (int i = 0; i < 4; ++i)
		{
			l <<= 8;
			l += fileBuffer1[addr + i];
		}
		return l;
	}
	void
	Examine()
	{
		int		addr, k, type;
		int		sn;
		String	tName;

		// Slaves

		for (k = 1, addr = 0x100 + 4; k <= 63; ++k)
		{
			type = fileBuffer1[addr];
			
			sn = getLong(addr);
			addr += 4;

			if (sn > 0)
			{
				tName = ModuleName(type);
				Output.Text += string.Format("Module Id:{0}, {1}, Serial Number {2:X8}\r\n",  
					k, tName, sn);
			}
		}

		message("Control functions:");
		
		if (fileBuffer1[0x8000] == 0xFF) addr = 0x9000;
		else addr = 0x8000;
		
		addr += 4 * 16;	// Skip first 4 controls, 0...3
		
		for (k = 4; k < 256; ++k)
		{
			controlFunction(addr);
			addr += 16;
		}
		
		message("\r\nAlarms:");
		
		if (fileBuffer1[0xC000] == 0xFF) addr = 0xD000;
		else addr = 0xC000;

		addr += 16;	// Skip alarm 0
		
		for (k = 1; k <= 63; ++k)
		{
			alarmFunction(addr, k);
			addr += 16;
		}
		
		message("\r\nTimers:");

		if (fileBuffer1[0xA000] == 0xFF) addr = 0xB000;
		else addr = 0xA000;

		addr += 16;	// Skip Timer 0

		for (k = 1; k <= 65; ++k)
		{
			timerFunction(addr, k);
			addr += 16;
		}
	}

	bool
	timerFunction(int addr, int id)
	{
		byte dow = fileBuffer1[addr];

		if (dow == 0xFF) return false;

		if (id == 64)
		{
			Output.Text += string.Format(" WaveMaker A/B:\r\n");
		}
		else
		if (id == 65)
		{
			Output.Text += string.Format(" WaveMaker C/D:\r\n");
		}
		else
		{
			Output.Text += string.Format(" Timer:{0}\r\n", id);
		}		
		
		if ((dow & 0x80) > 0)
		{
			message("  Oscillate");
		}
		else
		{
			char d1,d2,d3,d4,d5,d6,d7;
			
			d1 = d2 = d3 = d4 = d5 = d6 = d7 = '-';

			if ((dow & 0x01) > 0) d1 = 'S';
			if ((dow & 0x02) > 0) d2 = 'M';
			if ((dow & 0x04) > 0) d3 = 'T';
			if ((dow & 0x08) > 0) d4 = 'W';
			if ((dow & 0x10) > 0) d5 = 'T';
			if ((dow & 0x20) > 0) d6 = 'F';
			if ((dow & 0x40) > 0) d7 = 'S';

			Output.Text += string.Format("  DOW = {0}{1}{2}{3}{4}{5}{6}\r\n", d1, d2, d3, d4, d5, d6, d7);
		}

		byte h = fileBuffer1[addr + 1];
		byte m = fileBuffer1[addr + 2];
		byte s = fileBuffer1[addr + 3];

		Output.Text += string.Format("  Start at {0:X2}:{1:X2}:{2:X2}\r\n", h, m, s);

		h = fileBuffer1[addr + 4];
		m = fileBuffer1[addr + 5];
		s = fileBuffer1[addr + 6];

		Output.Text += string.Format("  ON for {0:X2}:{1:X2}:{2:X2}\r\n", h, m, s);
		
		h = fileBuffer1[addr + 7];
		m = fileBuffer1[addr + 8];
		s = fileBuffer1[addr + 9];		

		Output.Text += string.Format("  Off for {0:X2}:{1:X2}:{2:X2}\r\n", h, m, s);

		if ((dow & 0x80) == 0)
		{
			byte repeat = fileBuffer1[addr + 10];
			
			if (repeat == 0)
				 Output.Text += string.Format("  Done once\r\n");
			else Output.Text += string.Format("  Repeated {0}\r\n", repeat);
		}
		message("");
		return true;
	}
	
	bool
	alarmFunction(int addr, int id)
	{
		byte m = fileBuffer1[addr+0];

		if (m == 0xFF) return false;
		
		byte	inv1 = fileBuffer1[addr + 1];
		byte	inv2 = fileBuffer1[addr + 6];
		byte	inv3 = fileBuffer1[addr + 11];
		char	a1,a2,a3;
		
		int		sp1 = codeInt16(addr + 4);
		int		sp2 = codeInt16(addr + 9);
		int		sp3 = codeInt16(addr + 14);
		
		a1 = a2 = a3 = '-';
		
		if ((m & 0x01) > 0) a1 = 'F';
		if ((m & 0x02) > 0) a2 = 'B';
		if ((m & 0x04) > 0) a3 = 'E';

		if (m == 0)
			 Output.Text += string.Format(" Alarm:{0} ---\r\n", id);
		else Output.Text += string.Format(" Alarm:{0} {1}{2}{3}\r\n", id, a1,a2,a3);  
		
		if (fileBuffer1[addr+2] > 0)	// Module ID
		{
			port(addr + 2);
			
			if (inv1 > 0)
			{
				 Output.Text += string.Format("  On when below:{0}\r\n", sp1);
			}
			else 
			{
				Output.Text += string.Format("  On when above:{0}\r\n", sp1);
			}
		}
		if (fileBuffer1[addr + 7] > 0)	// Module ID
		{
			port(addr + 7);

			if (inv2 > 0)
			{
				Output.Text += string.Format("  On when above:{0}\r\n", sp2);
			}
			else
			{
				Output.Text += string.Format("  On when below:{0}\r\n", sp2);
			}
		}
		if (fileBuffer1[addr + 12] > 0)	// Module ID
		{
			port(addr + 12);

			if (inv3 > 0)
			{
				Output.Text += string.Format("  On when above:{0}\r\n", sp3);
			}
			else
			{
				Output.Text += string.Format("  On when below:{0}\r\n", sp3);
			}
		}
		//Output.Text += string.Format("\r\n");
		return true;
	}
	
	bool
	controlFunction(int addr)
	{
		byte	f = fileBuffer1[addr];
		
		if (f == 0xFF) return false;
		
		switch(f)
		{
		case FUNCTION_PUMP:		pump(addr);	break;
		case FUNCTION_CONTROL:	control(addr); break;
		case FUNCTION_MTIMER:	mTimer(addr); break;
		case FUNCTION_STIMER:	sTimer(addr); break;
		case FUNCTION_ADV_LIGHT:advLight(addr); break;
		case FUNCTION_LUNAR:	lunar(addr); break;
		case FUNCTION_SWITCH:	control(addr); break;
		}
		
		return true;
	}

	void
	twoEvents(int addr)
	{
		byte	inv1 = fileBuffer1[addr+4];		// invert1
		byte	aId  = fileBuffer1[addr+5];		// Alarm
		byte	inv2 = fileBuffer1[addr+6];		// standby invert
		byte	ss = fileBuffer1[addr+7];		// standby
		String	state;
		
		if (inv1 == 0) state = "Off";
		else state = "On";
		
		if (aId > 0)
		{
			Output.Text += string.Format("  Alarm Id:{0} -> {1}\r\n", aId, state);
		}
		
		if (inv2 == 0) state = "Off";
		else state = "On";	
		
		if (0 != (ss & 0x01))
		{
			Output.Text += string.Format("  During Standby1 -> {0}\r\n", state);
		}
		
		if (0 != (ss & 0x02))
		{
			Output.Text += string.Format("  During Standby2 -> {0}\r\n", state);
		}
	}
	
	void
	port(int addr)
	{
		byte	mi = fileBuffer1[addr+0];	// Module
		byte	si = fileBuffer1[addr+1];	// Sensor
		
		Output.Text += string.Format("  Module Id:{0}, Sensor Id:{1}\r\n", mi, si);
	}
	
	void
	timer(String conj, byte inv, byte id)
	{
		if (id > 0)
		{
			if (inv > 0)
				 Output.Text += string.Format("  Off when Timer:{0} {1}\r\n", id,conj);
			else Output.Text += string.Format("  On when Timer:{0} {1}\r\n", id, conj);
		}
	}
	
	void
	delay(int addr)
	{
		byte	hh = fileBuffer1[addr+0];
		byte	mm = fileBuffer1[addr+1];
		byte	ss = fileBuffer1[addr+2];
		
		if ((hh + mm + ss) > 0)
		{
			Output.Text += string.Format("  Delay after standby by {0:X2}:{1:X2}:{2:X2}\r\n", hh,mm,ss);
		}
		
	}
	
	int
	codeInt16(int addr)
	{
		byte	msb = fileBuffer1[addr+0];
		byte	lsb = fileBuffer1[addr+1];
		
		return msb * 256 + lsb;
	}

	void
	WaveMaker(int addr)
	{
		byte m = fileBuffer1[addr];

		switch (m & 0x07)
		{
			case 0: message("  Wavemaker A"); break;
			case 1: message("  WaveMaker B"); break;
			case 2: message("  WaveMaker C"); break;
			case 3: message("  WaveMaker D"); break;
			default:message("  Sump/Skimmer"); break;
		}
	}
	
	void
	mode(int addr)
	{
		byte	m = fileBuffer1[addr];
		
		if ((m & 0x80) != 0)	// SureOn
		{
			message("  SureOn Enabled");
		}
		
		switch(m & 0x30)
		{
		case 0x00:	message("  Channel Off"); break;
		case 0x10:	message("  Channel On"); break;
		case 0x20:	message("  Channel Auto"); break;
		}
	}
	
	void
	timerStruct(int addr)
	{
		byte []	days = new byte[7];
		char	d1,d2,d3,d4,d5,d6,d7;
		
		byte	dow = fileBuffer1[addr+0];
		byte h1 = fileBuffer1[addr + 1];
		byte m1 = fileBuffer1[addr + 2];
		byte s1 = fileBuffer1[addr + 3];
		byte h0 = fileBuffer1[addr + 4];
		byte m0 = fileBuffer1[addr + 5];
		byte s0 = fileBuffer1[addr + 6];
		
		if ((dow & 0x80) > 0)
		{
			message("  Oscillate");
			return;
		}
		d1 = d2 = d3 = d4 = d5 = d6 = d7 =  '-';
		
		if ((dow & 0x01) > 0) d1 = 'S';
		if ((dow & 0x02) > 0) d2 = 'M';
		if ((dow & 0x04) > 0) d3 = 'T';
		if ((dow & 0x08) > 0) d4 = 'W';
		if ((dow & 0x10) > 0) d5 = 'T';
		if ((dow & 0x20) > 0) d6 = 'F';
		if ((dow & 0x40) > 0) d7 = 'S';
		
		Output.Text += string.Format("  DOW = {0}{1}{2}{3}{4}{5}{6}\r\n", d1,d2,d3,d4,d5,d6,d7);
		Output.Text += string.Format("  Start {0:X2}:{1:X2}:{2:X2}\r\n", h1,m1,s1);
		Output.Text += string.Format("  Stop  {0:X2}:{1:X2}:{2:X2}\r\n", h0, m0, s0);
	}
	
	void
	pump(int addr)
	{
		// mode
		// Port
		// two events
		// invert
		// timer
		// HHMMSS delay
		
		message(" Pump");
		mode(addr+1);
		WaveMaker(addr+1);
		port(addr+2);
		twoEvents(addr);
		timer("", 0, fileBuffer1[addr+9]);
		delay(addr+10);
	}
	
	void	
	control(int addr)
	{
		message(" Controller:");
		mode(addr+1);
		port(addr+2);
		twoEvents(addr);
		port(addr+8);
		
		int		sp = codeInt16(addr+11);
		int		hy = codeInt16(addr+13);

		Output.Text += string.Format("  Setpoint={0}\r\n", sp);
		Output.Text += string.Format("  Hysteresis={0}\r\n", hy);
		
		timer("", 0, fileBuffer1[addr+15]);
	}
	
	void
	mTimer(int addr)
	{
		message(" Multi-Timer");
		mode(addr+1);
		port(addr + 2);
		twoEvents(addr);
		timer("", fileBuffer1[addr+8], fileBuffer1[addr + 9]);
		timer("OR",fileBuffer1[addr + 10], fileBuffer1[addr + 11]);
		timer("OR",fileBuffer1[addr + 12], fileBuffer1[addr + 13]);
	}

	void
	sTimer(int addr)
	{
		message(" Light");
		mode(addr+1);
		port(addr + 2);
		twoEvents(addr);
		
		byte	inv = fileBuffer1[addr+8];
		
		if (inv != 0)
			 message("  Off when:");
		else message("  On when:");
		
		timerStruct(addr+9);
	}
	
	void
	advLight(int addr)
	{
		message(" Advanced Light");
		mode(addr+1);
		port(addr + 2);
		twoEvents(addr);
	}

	void
	lunar(int addr)
	{
		message(" Lunar Light");
		mode(addr+1);
		port(addr + 2);
		twoEvents(addr);

		byte	i = fileBuffer1[addr+11];
		
		if (i > 0)
		{
			Output.Text += string.Format("  Intensity {0}\r\n", i );
		}
	}
}


}
