/* 
 * Project:    Panel Tester
 * Company:    Dynon Avionics
 * Author:     Warren and Laurine
 * Created:    2012
 */


using System;
using System.Linq;
using System.Data;
using System.Text;
using System.Drawing;
using System.IO.Ports;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;

using SV_COM_425.Properties;
using System.Threading;
using System.IO;


namespace 
SV_COM_425
{
	public enum DataMode { Text, Hex }
	public enum LogMsgType { Incoming, Outgoing, Normal, Warning, Error };

 
	public partial class frmTerminal : Form
	{
	  public class 
	  Packet
	  {
			public int	start,end;
			
			public
			Packet()
			{
				start = end = -1;
			}
			
			public int
			Size()
			{
				int		s;
				
				s = this.end - this.start + 1;
				
				if (s < 0) s += 256;
				
				return s;
			}
	  }
	  

	  // The main control for communicating through the RS-232 port
	  private SerialPort comport = new SerialPort();


	  // Retrieve the current configuration
		public static Configuration userConfig;
	  //public static SerialConfigSection panelPortConfig;

	  public int		cmdCounter = 0;
	  public int		rxPacket = 0;
	  public bool		transmitState = false;
	  public int		seconds = 0;
	  public int		secondsStart = 0;
	  public int		timerStart;
	  public int		timerTest = 0;
	  public int		ack = 0;
	  
	  public string		SNBdirectory = null;
	  public string		pathDirectory = null;
	  public string		pathFile = null;
	  public string		pathBoard = null;
	  public string		pathDwnld = null;
	  public string		fileLine = null;
	  public byte []	fileBuffer = new byte[16 * 1024];
	  
	  public System.Text.ASCIIEncoding  encoding = new System.Text.ASCIIEncoding();

	  public bool       ON = true;
	  public bool       OFF = false;
	  public bool       measPowerInProgress = false;
	  public bool		transmitting = false;
	  public bool		TMAP = true;
	  public int		txCounter = 0;
	  public string		outputMessage = null;
	  public int		queWrite = 0;
	  public byte []	que = new byte[256];
	  public int		start = 0;
	  public Packet	[]	packetFIFO = new Packet[16];  
	  public int		packetRead = 0;
	  public int		packetWrite = 0; 
	  public bool		radioAttached = false;
	  public bool		radioIsAlive = false;
	  public int		modeCounter = 0;
	  public bool		binaryMode = false;
	  public bool		cmdAlwaysValid = false;
	  public bool		powerUp = false;
	  public byte []	number = new byte[2];
	  public byte []	pinData = new byte[11];
	  public byte []	calRXData = new byte[19]; // without typecode
	  public byte []    calTXData = new byte[19]; // without typecode
	  public bool		calRXDataIsValid = false;
	  public bool       calTXDataIsValid = false;
	  public int		fileCount = 0;
	  public int		psocAddress = 0;
	  public int		lastAddress = 0;
	  public int		oldScrollValue = 0;
	  public int		maxRSSIbar = 0;
	  public int		minRSSI = 0;
	  public bool		goDown = false;
	  public bool		goUp = false;
	  public bool		updateFrequencies = true;
	  
	  // Various colors for logging info
	  private Color[] LogMsgTypeColor = { Color.Blue, Color.Green, Color.Black, Color.Orange, Color.Red };

	  private Settings settings = Settings.Default;


	  public 
	  frmTerminal()
	  {
		  // Load user settings
		  settings.Reload();

		  // Build the form
		  InitializeComponent();

		  // Restore the users settings
		  InitializeControlValues();

		  // Enable/disable controls based on the current state
		  EnableControls();

		  // When data is recieved through the port, call this method
		  comport.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
	  }

	  /// <summary> Save the user's settings. </summary>
	  private void
	  SaveSettings()
	  {
		  settings.BaudRate = int.Parse(cmbBaudRate.Text);
		  settings.DataBits = int.Parse(cmbDataBits.Text);

		  settings.Parity = (Parity)Enum.Parse(typeof(Parity), cmbParity.Text);
		  settings.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cmbStopBits.Text);
		  settings.PortName = cmbPortName.Text;

		  settings.Save();
	  }

	  /// <summary> Populate the form's controls with default settings. </summary>
	  private void
	  InitializeControlValues()
	  {
		  cmbParity.Items.Clear(); cmbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
		  cmbStopBits.Items.Clear(); cmbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));

		  cmbStopBits.Text = settings.StopBits.ToString();
		  cmbDataBits.Text = "8"; settings.DataBits.ToString();
		  cmbParity.Text = "Odd"; //settings.Parity.ToString();
		  cmbBaudRate.Text = "38400";//settings.BaudRate.ToString();


		  RefreshComPortList();

		  // If it is still avalible, select the last com port used
		  if (cmbPortName.Items.Contains(settings.PortName)) cmbPortName.Text = settings.PortName;
		  else
			  if (cmbPortName.Items.Count > 0) cmbPortName.SelectedIndex = cmbPortName.Items.Count - 1;
			  else
			  {
				  MessageBox.Show(this, "There are no COM Ports detected on this computer.\nPlease install a COM Port and restart this app.", "No COM Ports Installed", MessageBoxButtons.OK, MessageBoxIcon.Error);
				  this.Close();
			  }
	  }

	  /// <summary> Enable/disable controls based on the app's current state. </summary>
	  private void
	  EnableControls()
	  {
		  // Enable/disable controls based on whether the port is open or not
		  //gbPortSettings.Enabled = !comport.IsOpen;

		  if (comport.IsOpen) btnOpenPort.Text = "&Close Port";
		  else btnOpenPort.Text = "&Open Port";
	  }


	  /// <summary> Log data to the terminal window. </summary>
	  /// <param name="msgtype"> The type of message to be written. </param>
	  /// <param name="msg"> The string containing the message to be shown. </param>
	  private void
	  Log(LogMsgType msgtype, string msg)
	  {
		  rtfTerminal.SelectedText = string.Empty;
		  rtfTerminal.SelectionFont = new Font(rtfTerminal.SelectionFont, FontStyle.Bold);
		  rtfTerminal.SelectionColor = LogMsgTypeColor[(int)msgtype];
		  rtfTerminal.SelectionLength = 0;
		  rtfTerminal.SelectionStart = rtfTerminal.Text.Length;

		  if (msg.Contains('\r'))
		  {
			  //msg += "\n";
		  }
		  else 
			msg += "\r\n";

		  rtfTerminal.AppendText(msg);
		  rtfTerminal.SelectionStart = rtfTerminal.Text.Length;
		  rtfTerminal.ScrollToCaret();
	  }

	  /// <summary> Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>
	  /// <param name="s"> The string containing the hex digits (with or without spaces). </param>
	  /// <returns> Returns an array of bytes. </returns>
	  private byte[]
	  HexStringToByteArray(string s)
	  {
		  s = s.Replace(" ", "");
		  byte[] buffer = new byte[s.Length / 2];

		  for (int i = 0; i < s.Length; i += 2)
			  buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);

		  return buffer;
	  }

	  /// <summary> Converts an array of bytes into a formatted string of hex digits (ex: E4 CA B2)</summary>
	  /// <param name="data"> The array of bytes to be translated into a string of hex digits. </param>
	  /// <returns> Returns a well formatted string of hex digits with spacing. </returns>
	  private string
	  ByteArrayToHexString(byte[] data)
	  {
		  StringBuilder sb = new StringBuilder(data.Length * 3);

		  foreach (byte b in data)
		  {
			  if (b != 0x0D)
				  sb.Append(Convert.ToString(b, 16).PadLeft(2, '0').PadRight(3, ' '));
			  else sb.Append('\r');
		  }
		  return sb.ToString().ToUpper();
	  }

	  
		private void
		frmTerminal_FormClosing(object sender, FormClosingEventArgs e)
		{
			// The form is closing, save the user's preferences
			SaveSettings();
		}


		private void 
		frmTerminal_Load(object sender, EventArgs e)
		{
			OpenThePort();
		}
		
	  private void
	  cmbBaudRate_Validating(object sender, CancelEventArgs e)
	  {
		  int x;
		  e.Cancel = !int.TryParse(cmbBaudRate.Text, out x);
	  }

	  private void
	  cmbDataBits_Validating(object sender, CancelEventArgs e)
	  {
		  int x;
		  e.Cancel = !int.TryParse(cmbDataBits.Text, out x);
	  }

	  

	  //
	  private void
	  btnOpenPort_Click(object sender, EventArgs e)
	  {
		  OpenThePort();
	  }

	public void
	OpenThePort()
	{
		bool error = false;

		// If the port is open, close it.
		if (comport.IsOpen) 
		{
			comport.Close();
			btnOpenPort.BackColor = Color.LightGray;
			//btnTest.BackColor = Color.Red;
		}
		else
		{
		  // Set the port's settings
		  
			comport.BaudRate = int.Parse("38400");
			comport.DataBits = int.Parse("8");
			comport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "1");
			comport.Parity = (Parity)Enum.Parse(typeof(Parity), "Odd");
			comport.PortName = cmbPortName.Text;

			//btnTest.BackColor = Color.Red;
			modeCounter = 0;
			
			try
			{
			  // Open the port
			  comport.Open();
			  
			  // Send message requesting cal data
			  
			  btnOpenPort.BackColor = Color.LightGreen;
			}
			catch (UnauthorizedAccessException) { error = true; }
			catch (IOException) { error = true; }
			catch (ArgumentException) { error = true; }

			if (error) MessageBox.Show(this, "Could not open the COM port.  Most likely it is already in use, has been removed, or is unavailable.", "COM Port Unavalible", MessageBoxButtons.OK, MessageBoxIcon.Stop);
		}

		// Change the state of the form's controls
		EnableControls();

		// Laurine
		// If the port closes, change the status of the radio to enable a verification once the port is open again
		if (!comport.IsOpen)
		{
		  radioAttached = false;
		}
	}

	public int
	PC()
	{
		int	pc = packetWrite - packetRead;
		
		if (pc < 0) pc += 16;
		
		return pc;
	}
	
	public Packet
	PreviousPacket()
	{
		int		i = packetWrite - 1;
			
		if (i < 0) i += 16;
			
		return packetFIFO[i];
	}
	
	public Packet
	QuePacket()
	{
		Packet	p = null;
		
		if (PC() > 16)
		{
			//error
			return null;
		}
		else
		{
			p = new Packet();
			packetFIFO[packetWrite] = p;
			packetWrite = (packetWrite + 1) & 0x0F;
		}
		return p;
	}
	
	public Packet
	PopPacket()
	{
		Packet	p = null;
		
		if (PC() <= 0) return null;
		
		p = packetFIFO[packetRead];
		
		if (p.end != -1)
		{
			packetRead = (1 + packetRead) & 0x0F;
		}
		else p = null;
		
		return p;
	}
		
		
		
// Que received bytes into large circular buffer
// Packets are deliniated by F0 ... F1 and are recorded
// into a FIFO.

	public void
	QueBinary(byte [] buffer)
	{
		Packet	p = null,prev;
		int		wi;
		
		if (buffer == null) return;
		
		prev = PreviousPacket();
		
		foreach(byte b in buffer)
		{
			que[wi = queWrite] = b;
			queWrite = (queWrite + 1) & 0x0FF;
			
			if (b == 0xF0)
			{
				p = QuePacket();
				p.start  = wi;
				
				continue;
			}
			
			if (b == 0xF1)
			{
				++rxPacket;
				
				if (p != null) {p.end = wi; prev = p = null; continue;}
				if (prev != null) {prev.end = wi; prev = null; continue;}
				
				// Error: an F1 without a starting F0
				
				Que("Improper framed TMAP packet received!");
			}
		}
	}
	
	//	When switching from TMAP to Ascii the packet que is cleared
	//	Ascii packets start at the end of the previous one.
	
	public void
	QueAscii(byte [] buffer)
	{
		Packet	p = null;
		int		wi;
		
		if (buffer == null) return;
		
		foreach(byte b in buffer)
		{
			que[wi = queWrite] = b;
			queWrite = (queWrite + 1) & 0x0FF;
			
			if (b == 0x0A)
			{
				p = QuePacket();
				p.end  = wi;
				p.start = start;
				
				start = wi + 1;
				start &= 0x0FF;
			}
		}
	}
	
	// Packets are bracketted by 0xF0 and 0xF1
	
	private void
	port_DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		int count = comport.BytesToRead;

		if (count == 0) return;

		// Create a byte array buffer to hold the incoming data
		byte[] buffer = new byte[count];

		// Read the data from the port and store it in our buffer
		comport.Read(buffer, 0, count);

		if (TMAP)
			 QueBinary(buffer);
		else QueAscii(buffer);
	}

	  private void
	  btnClear_Click(object sender, EventArgs e)
	  {
		  ClearTerminal();
		  cmdCounter = 0;
		  rxPacket = 0;
		  
		  byte []	data = new byte[19];
		  
		  for(int i = 0; i < 19; ) data[i++] = 0;
		  
		  ReadResponseRX(data);
		  ReadResponseTX(data);
	  }

	  private void
	  ClearTerminal()
	  {
		  rtfTerminal.Clear();
	  }

	  private void
	  tmrCheckComPorts_Tick(object sender, EventArgs e)
	  {
		  // checks to see if COM ports have been added or removed
		  // since it is quite common now with USB-to-Serial adapters
		  RefreshComPortList();
	  }

	  private void
	  RefreshComPortList()
	  {
		  // Determain if the list of com port names has changed since last checked
		  string selected = RefreshComPortList(cmbPortName.Items.Cast<string>(), cmbPortName.SelectedItem as string, comport.IsOpen);

		  // If there was an update, then update the control showing the user the list of port names
		  if (!String.IsNullOrEmpty(selected))
		  {
			  cmbPortName.Items.Clear();
			  cmbPortName.Items.AddRange(OrderedPortNames());
			  cmbPortName.SelectedItem = selected;
		  }
	  }

	  private string[]
	  OrderedPortNames()
	  {
		  // Just a placeholder for a successful parsing of a string to an integer
		  int num;

		  // Order the serial port names in numberic order (if possible)
		  return SerialPort.GetPortNames().OrderBy(a => a.Length > 3 && int.TryParse(a.Substring(3), out num) ? num : 0).ToArray();
	  }

	  private string
	  RefreshComPortList(IEnumerable<string> PreviousPortNames, string CurrentSelection, bool PortOpen)
	  {
		  // Create a new return report to populate
		  string selected = null;

		  // Retrieve the list of ports currently mounted by the operating system (sorted by name)
		  string[] ports = SerialPort.GetPortNames();

		  // First determain if there was a change (any additions or removals)
		  bool updated = PreviousPortNames.Except(ports).Count() > 0 || ports.Except(PreviousPortNames).Count() > 0;

		  // If there was a change, then select an appropriate default port
		  if (updated)
		  {
			  // Use the correctly ordered set of port names
			  ports = OrderedPortNames();

			  // Find newest port if one or more were added
			  string newest = SerialPort.GetPortNames().Except(PreviousPortNames).OrderBy(a => a).LastOrDefault();

			  // If the port was already open... (see logic notes and reasoning in Notes.txt)
			  if (PortOpen)
			  {
				  if (ports.Contains(CurrentSelection)) selected = CurrentSelection;
				  else if (!String.IsNullOrEmpty(newest)) selected = newest;
				  else selected = ports.LastOrDefault();
			  }
			  else
			  {
				  if (!String.IsNullOrEmpty(newest)) selected = newest;
				  else if (ports.Contains(CurrentSelection)) selected = CurrentSelection;
				  else selected = ports.LastOrDefault();
			  }
		  }

		  // If there was a change to the port list, return the recommended default selection
		  return selected;
	  }


	  public static byte[] 
	  ReverseBytes(byte[] inArray)
	  {
		  byte temp;
		  int highCtr = inArray.Length - 1;

		  for (int ctr = 0; ctr < inArray.Length / 2; ctr++)
		  {
			  temp = inArray[ctr];
			  inArray[ctr] = inArray[highCtr];
			  inArray[highCtr] = temp;
			  highCtr -= 1;
		  }
		  return inArray;
	  }

	  private void
	  SendGetVersion()
	  {
		  byte[] frame = new byte[6];

		  // Create a frame with content to send

		  frame[0] = 0x5B;
		  SendMsg(frame, 1);
	  }

	  private void
	  SendGetFrequencies()
	  {
		  byte[] frame = new byte[6];

		  // Create a frame with content to send

		  frame[0] = 0x57;
		  SendMsg(frame, 1);
		  
		  updateFrequencies = true;
	  }
	  
	  private void
	  SetInitialConfig()
	  {
		  PrimaryFreq.Value = Convert.ToDecimal(123.45);
		  SecondaryFreq.Value = Convert.ToDecimal(123.45);

		  Volume.Value = 64;
   
		  Sidetone.Value = 128;
		  MicGain.Value = 127;
		  chkDualWatch.Checked = false;
		  chkUnsquelch.Checked = false;
	  }

	private void 
	btnDwnld_Click(object sender, EventArgs e)
	{
		if (ChooseFile())
		{
			// fileBuffer has binary data
			
			radioIsAlive = false;
			
			TMAP = false;
			timer1.Stop();
			//btnTest.BackColor= Color.Red;
			
			if (comport.IsOpen) comport.BaseStream.Flush();
			
			for(int i = 0; i < que.Length; ) que[i++] = 0;
			queWrite = 0;
			
			for(int i = 0; i < packetFIFO.Length; ) packetFIFO[i++] = null;
			
			packetWrite = packetRead = 0;
			
			psocAddress = 0x0400;
			
			RequestDownload();
			
			timer1.Start();
		}
		else Log(LogMsgType.Error, "Unable to open HEX file");
	}

	public bool
	ChooseFile()
	{          
		string dir;
		DirectoryInfo di;

		dir = Application.StartupPath;

		di = new DirectoryInfo(dir);

		if (!di.Exists)
		{
			rtfTerminal.Text += string.Format("Firmware directory can't be found:\r\n    {0:s}\r\n", dir);
		}

		openFileDialog.FileName = "";
		openFileDialog.InitialDirectory = dir;

		if (openFileDialog.ShowDialog() != DialogResult.OK) return false;

		pathDwnld = openFileDialog.FileName;
		// tbDwnld.Text = pathDwnld.Substring(pathDwnld.LastIndexOf("\\") + 1);

		return LoadHexFile(pathDwnld);
	}

	public bool
	SaveHex(string fileName)
	{
		using (StreamWriter dst = new StreamWriter(fileName))
		{
			if (dst == null) return false;
			
			int		address = 0;
			string	line;
			
			for(address = 0x0400; address < 0x3F40; address += 64)
			{
				line = IntelRecord(address);
				dst.WriteLine(line);
			}
			dst.Close();
			
			return true;
		}
		return false;
	}
	
	public bool
	LoadHexFile(string fileName)
	{
		int		sum;
		int		length,addr,a2;
		int		type,data;
		int		i,j,rn;
		string	line;
		int     dataSum;
		bool	enabled = true;

		fileCount = 0;
		dataSum = 0;

		if (!File.Exists(fileName))
		{
			Log(LogMsgType.Error, "Unknown file!");
			
			return false;
		}
		
		StreamReader	src = File.OpenText(fileName);

		Log(LogMsgType.Normal, "Loading: " + fileName);
		
		rn = i = 0;
		a2 = 0;

		while((line = src.ReadLine()) != null)
		{
			++rn;

			if (line.Substring(0,1) == ";")
			{
				// Comment ;PSoC MMnn  or ;PIC MMnn

				continue;
			}

			if (!enabled) continue;
			
			sum = 0;

			// Character 0 ':' is ignored 
			length = Convert.ToInt16(line.Substring(1, 2), 16);

			if (length < 2) continue;

			sum += length;

			// Grab 4 characters
			addr = 0x0FFFF & Convert.ToInt16(line.Substring(3, 4), 16);


			i = addr + ((a2 << 16) & 0x0F0000);

			sum += (addr >> 8) & 0x0FF;
			sum += (addr & 0x0FF);

			type = Convert.ToInt16(line.Substring(7, 2), 16);
			sum += type;
			j = 9;

			if (type == 0)
			{
				while (0 < length--)
				{
					data = Convert.ToInt16(line.Substring(j, 2), 16);

					fileBuffer[i++] = (byte)(data & 0x00FF);

					j += 2;
					sum += data;
					dataSum += data;
					++fileCount;
				}
			}
			else
			if (type == 4)
			{
				// Extended address, length must be == 2
				// Does not count as loaded data

				if (length != 2)
				{
					Log(LogMsgType.Error, "Hex file has illegal format");
					fileCount = 0;
					break;
				}

				a2 = 0;
				enabled = false;
				continue;
			}

			data = Convert.ToByte(line.Substring(j, 2), 16);                            
						  
			data *= -1;
			data &= 0x00FF;
			sum  &= 0x00FF;

			if (data != sum)
			{
				Log(LogMsgType.Error, string.Format("CheckSum error in record #{0:d}\r\n", rn));
				fileCount = 0;
				return false;
			}               
		}

		src.Close();

		Log(LogMsgType.Normal, string.Format("{0:d} bytes loaded", fileCount));

	
		//// Calc checksum
		
		sum = 0;
		
		for(i = 0x400; i < 0x3f00; )
		{
		    sum += (fileBuffer[i] * 256) + fileBuffer[i+1];
		    i += 2;
		}
		
		sum &= 0x0FFFF;
		
		int	fileSum = 0;
		
		fileSum = 0x0FFFF & (fileBuffer[0x3F00] * 256 + fileBuffer[0x3F01]);
		
		if (fileSum != sum)
		{
			Log(LogMsgType.Normal, string.Format("File's checksum: {0:X4} != {1:X4} (calc)", 
				fileSum, sum));
			return false;
		}
		//fileBuffer[0x3F00] = (byte) ((sum >> 8) & 0x0FF);
		//fileBuffer[0x3F01] = (byte) (sum & 0x0FF);
		
		return true;
	}
	
	// Send TMAP command to enter bootloader
	
	private void
	RequestDownload()
	{
		byte[] frame = new byte[4];

		frame[0] = 0xF0;
		frame[1] = 0x5D;	// Upgrade cmd
		frame[2] = 0x12;	// Dynon magic number
		frame[3] = 0xF1;

		if (comport.IsOpen) 
		{
			comport.Write(frame, 0, 4);

			// Show the hex digits in the terminal window
			Log(LogMsgType.Outgoing, ByteArrayToHexString(frame));
		}
	}
	
	public string
	IntelRecord(int address)
	{
		string	line;
		byte	b;
		int		cs = 0;
		
		cs = 0x040;
		cs += (address >> 8) & 0x0FF;
		cs += address & 0x0FF;
		
		line = string.Format(":40{0:X4}00", address);
				
		for(int i = 0; i < 64; ++i) 
		{
			b = fileBuffer[address + i];
			cs += b;
			line += string.Format("{0:X2}", b);
		}
	
		cs *= -1;
		
		line += string.Format("{0:X2}", (byte) (cs & 0x0FF));

		return line;
	}
	
	public string
	NextFileLine()
	{
		string	line;
		byte [] data = new byte[64];
		
		if ((fileCount == 0) || (psocAddress >= 0x3F40)) return null;
		
		lastAddress = psocAddress;
		
		line = IntelRecord(psocAddress);
		
		psocAddress += 64;

		return line;
	}
	
	  private void
	  GetVersion()
	  {
		  byte[] frame = new byte[3];

		  frame[0] = 0xF0;
		  frame[1] = 0x5B;
		  frame[2] = 0xF1;

		  // Send the binary data out the port
		  comport.Write(frame, 0, 3);
		  ++cmdCounter;
		  
		  // Show the hex digits on in the terminal window
		  //Log(LogMsgType.Outgoing, ByteArrayToHexString(frame));
		  Que(ByteArrayToHexString(frame));
	  }

	  private void
	  btnGetVersion_Click(object sender, EventArgs e)
	  {
		  GetVersion();
	  }



	private void
	btnTimer_Click(object sender, EventArgs e)
	{
		// Turn on polling
		
		//cbPoll.Checked = true;
		modeCounter = 50000;
		
		timerTest = 120;		// 30 s @ .250 s / tick
	}

	  private void
	  GetChannels()
	  {
		  byte[] frame = new byte[3];

		  frame[0] = 0xF0;
		  frame[1] = 0x57;
		  frame[2] = 0xF1;

		  // Send the binary data out the port
		  comport.Write(frame, 0, 3);
		  ++cmdCounter;
		  
		  // Show the hex digits on in the terminal window
		  //Log(LogMsgType.Outgoing, ByteArrayToHexString(frame));
		  Que(ByteArrayToHexString(frame));
	  }
	  
	   

	  // VHF Mode bits
	  private enum
	  VHFModeSettings
	  {
		  NoFlag = 0x00,      // BIT0
		  Unsquelch = 0x01,   // BIT1
		  DualWatch = 0x02    // BIT2
	  }

	  private VHFModeSettings VHFmodeFlags;

	  // Dual Watch setting
	  private void
	  chkDualWatch_CheckedChanged(object sender, EventArgs e)
	  {
		  if (chkDualWatch.Checked)
		  {
			  VHFmodeFlags |= VHFModeSettings.DualWatch;
		  }
		  else
		  {
			  VHFmodeFlags &= ~VHFModeSettings.DualWatch;
		  }

		  SendNewModeSettings();
	  }

	  // Unsquelch setting
	  private void
	  chkUnsquelch_CheckedChanged(object sender, EventArgs e)
	  {
		  if (chkUnsquelch.Checked)
		  {
			  VHFmodeFlags |= VHFModeSettings.Unsquelch;
		  }
		  else
		  {
			  VHFmodeFlags &= ~VHFModeSettings.Unsquelch;
		  }

		  SendNewModeSettings();

	  }

	  

	  // VHF Volume setting
	  private void
	  SetVHFVolume_ValueChanged(object sender, EventArgs e)
	  {
		  // if the value changes, update the progress bar and the value
		  pbRadioVolume.Value = Decimal.ToInt16(Volume.Value);
		  SendNewModeSettings();
	  }

	  // Sidetone setting 
	  private void
	  SetSidetone_ValueChanged(object sender, EventArgs e)
	  {
		  // if the value changes, update the progress bar
		  pbSidetoneVolume.Value = Decimal.ToInt16(Sidetone.Value);
		  SendNewSidetone();
	  }

	  // Mic setting 
	  private void
	  SetMic_ValueChanged(object sender, EventArgs e)
	  {
		  SendState();
	  }


	  // Collect all data, create Tx buffer and send it
	  private void
	  SendNewModeSettings()
	  {
			SendState();
	  }

	  // Update the value of the sidetone
	  private void 
	  SendNewSidetone()
	  {
			SendState();
	  }
		// For 'ms' milliSeconds loop andling events
		
		public void
		Wait(int ms)
		{
			if (ms > 0)
			{
				int		ct = Environment.TickCount + ms;
			
				while(ct > Environment.TickCount) Application.DoEvents();
			}
		}
		
		public void
		SendCommand(byte [] frame, int length)
		{
			if (comport.IsOpen)
			{
				comport.Write(frame, 0, length);
						
				++cmdCounter;
			}
		}

	  // When click the frequency button, update values
	  private void
	  SecondaryFreq_Changed(object sender, EventArgs e)
	  {
		  SendState();
	  }

	  
	  private void
	  PrimaryFreq_Click(object sender, EventArgs e)
	  {
		  SendState();
	  }

	  void
	  SendState()
	  {
		double	pf = (double) PrimaryFreq.Value;
		double	sf = (double) SecondaryFreq.Value;
		
		  // Convert f to index
		  
		  pf -= 118.0;	sf -= 118.0;
		  pf *= 160; sf *= 160;
		  
		  byte[] frame = new byte[10];
		  
		  byte[] bPrim = BitConverter.GetBytes(Convert.ToUInt32(pf));
		  byte[] bScnd = BitConverter.GetBytes(Convert.ToUInt32(sf));

		  // by default BitConvert is little Endian
		  if (!BitConverter.IsLittleEndian)
		  {
			  bPrim = ReverseBytes(bPrim);
			  bScnd = ReverseBytes(bScnd);
		  }
		  
		  // Create the content of the msg to be sent
		  frame[0] = 0x74;        // SetState
		  frame[1] = (byte) (0x0E0 | (byte) (VHFmodeFlags));
		  frame[2] = Decimal.ToByte(Volume.Value);
		  frame[3] = Decimal.ToByte(Squelch.Value);
		  frame[4] = Decimal.ToByte(Sidetone.Value);
		  frame[5] = Decimal.ToByte(MicGain.Value);
		  frame[6] = bPrim[1];    // MSB prim freq
		  frame[7] = bPrim[0];    // LSB prim freq
		  frame[8] = bScnd[1];    // MSB scnd freq
		  frame[9] = bScnd[0];    // LSB scnd freq
	
		  // Add Start, Stop bytes, check for any reserved values and send data out the port
		 SendMsg(frame, 10);
	  }
	 
	  
	  private void
	  Squelch_ValueChanged(object sender, EventArgs e)
	  {
		  SendNewSquelch();
		  
		  pbSquelchVolume.Value = (int) Squelch.Value;
	  }

	  private void
	  SendNewSquelch()
	  {
		  byte[] frame = new byte[2];
		  byte bSquelch = Decimal.ToByte(Squelch.Value);

		  // Create a frame with content to send
		  frame[0] = 0x5E;
		  frame[1] = bSquelch;

		  // Activate flag which informs that the cmd is always active whatever the status of the security key
		  cmdAlwaysValid = true;

		  // Add Start, Stop bytes, check for any reserved values and send data out the port
		  SendMsg(frame, 2);
	  }

 
	  // Send the read ADC command

		private void
		ReadStatus()
		{
			byte[] frame = new byte[3];

			// Create a frame with content to send
			frame[0] = 0x70;

			SendMsg(frame, 1);
		}
	  
		private void
		ReadState()
		{
			byte[] frame = new byte[3];

			// Create a frame with content to send
			frame[0] = 0x72;

			SendMsg(frame, 1);
		}
		
	public void
	TimerChecks()
	{
		if (timerTest > 0)
		{
			if (timerTest == 120)
			{
				timerStart = Environment.TickCount;
				secondsStart = seconds;
			}
			
			--timerTest;
		
			if (timerTest == 0)
			{
				int		t = Environment.TickCount - timerStart;
				int		sx = seconds - secondsStart;
				
				modeCounter = 200;
			}
		}
	}
		
	// Whenever a PinValue record has been received.
	
	
	private void
	ShowStatus(byte[] data)
	{
		int		len = 0;
		double	vbat;
		int		rssi,temp;
		
		len = ExtractData(data, pinData);	// Get just the payload

		if (len == 9)
		{
			seconds = pinData[7] * 256 + pinData[8];
			
			vbat = pinData[3] * 256 + pinData[4];
			temp = pinData[2];
			rssi = pinData[1];
			
			lbVbat.Text = string.Format("{0}V",vbat/10);
			
			TimerChecks();		
			
			pbRSSI.Value = rssi;
			lRSSI.Text = string.Format("{0}",rssi);

			tbTemp.Text = string.Format("{0:d}F",temp);

			double v = pinData[4] * 256 + pinData[5];

			v = 5.0 * v / 1024;
			//tbVmod.Text = string.Format("{0:g3}V", v);
		}
	}

	  
	  
		// 0x51
		public void
		ReadMode(byte [] data)
		{
			byte	mode = data[2];
			
			if ((mode & 0x01) != 0) 
				 PrimaryFreq.BackColor = Color.LightGreen;
			else PrimaryFreq.BackColor = Color.LightGray;
			
			if ((mode & 0x02) != 0) 
				 SecondaryFreq.BackColor = Color.LightGreen;
			else SecondaryFreq.BackColor = Color.LightGray;
		}

		// Process received calibration values 
		     
		public  void
		ReadResponseRX(byte[] data)
		{
			int len = 0;

			++ack;
			len = data.Length;

			// Clear RX Buffer
			ClearBuffer(calRXData, calRXData.Length);

			// Remove start and stop bits and convert F0,F1,F2 bits
			len = ExtractData(data,  calRXData);
			
			Volume.Value = calRXData[9];
			pbRadioVolume.Value = calRXData[9];
			
			Squelch.Value = calRXData[10];
			pbSquelchVolume.Value = calRXData[10];
			
			MicGain.Value = calRXData[11];
			pbMicVolume.Value = calRXData[11];
			
			Sidetone.Value = calRXData[8];
			pbSidetoneVolume.Value = calRXData[8];
			
			int sn = calRXData[0] * 256 + calRXData[1];
			serialNumber.Text = string.Format("{0}",sn);
			
			int		pi = calRXData[15] * 256 + calRXData[16];
			int		si = calRXData[17] * 256 + calRXData[18];
			
			double	pf = 118.0 + ((pi / 4) * 0.025);
			double	sf = 118.0 + ((si / 4) * 0.025);
			
			PrimaryFreq.Value = (decimal) pf;
			SecondaryFreq.Value = (decimal) sf;
		}


		public  void
		ReadResponseTX(byte[] data)
		{
			int len = 0;

			++ack;
			len = data.Length;

			// Clear RX Buffer
			ClearBuffer(calTXData, calTXData.Length);

			// Remove start and stop bits and convert F0,F1,F2 bits
			len = ExtractData(data,  calTXData);
		}
	  
   
	 

	  private void
	  CreateFolder(string snb)
	  {
		  pathBoard = pathDirectory + "\\Board" + snb;
		  DirectoryInfo dir = new DirectoryInfo(pathBoard);
		  if (!dir.Exists)
		  {
			  dir.Create();
		  }
	  }

	  

	  public byte[]
	  GetInt16CalValue(TextBox name)
	  {
		  byte[] item = BitConverter.GetBytes(Convert.ToInt16(name.Text));

		  // by default BitConvert is little Endian
		  if (!BitConverter.IsLittleEndian)
		  {
			  item = ReverseBytes(item);
		  }

		  Array.Reverse(item);

		  return item;
	  }

	  public byte
	  GetInt8CalValue(TextBox name)
	  {
		  byte item = Convert.ToByte(name.Text);

		  return item;
	  }


	  private void
	  DisplayData(byte[] buffer, TextBox name, int index, int nbBytes)
	  {
		  if (nbBytes == 2)
		  {
			  byte[] Item = BitConverter.GetBytes(BitConverter.ToInt16(buffer, index));
			  // by default BitConvert is little Endian
			  if (!BitConverter.IsLittleEndian)
			  {
				  Item = ReverseBytes(Item);
			  }
 
			  string s = BitConverter.ToString(Item).Replace("-", "");
			  decimal dec = Convert.ToDecimal(Item[0] << 8 | Item[1]);

			  if (buffer == pinData)
			  {
				  name.Text = dec.ToString();
			  }
			  else
			  {
				  name.Text = dec.ToString();
			  }

			  //Log(LogMsgType.Normal, ByteArrayToHexString(calItem));
		  }
		  else if (nbBytes == 1)
		  {
			  string s = (buffer[index]).ToString("X");
			  decimal dec = Convert.ToDecimal(buffer[index]);

			  if (buffer == pinData)
			  {
				  name.Text = dec.ToString();
			  }
			  else
			  {
				  name.Text = dec.ToString();
			  }
			  //Log(LogMsgType.Normal, s);
		  }
	  }

	  // 
	  private void
	  DisplayData(byte[] buffer, Label name, int index, int nbBytes)
	  {
		  if (nbBytes == 2)
		  {
			  byte[] Item = BitConverter.GetBytes(BitConverter.ToInt16(buffer, index));
			  // by default BitConvert is little Endian
			  if (!BitConverter.IsLittleEndian)
			  {
				  Item = ReverseBytes(Item);
			  }

			  string s = BitConverter.ToString(Item).Replace("-", "");
			  decimal dec = Convert.ToDecimal(Item[0] << 8 | Item[1]);

			  if (buffer == pinData)
			  {
				  name.Text = dec.ToString();
			  }
			  else
			  {
				  name.Text = dec.ToString();
			  }

			  //Log(LogMsgType.Normal, ByteArrayToHexString(calItem));
		  }
		  else if (nbBytes == 1)
		  {
			  string s = (buffer[index]).ToString("X");
			  decimal dec = Convert.ToDecimal(buffer[index]);

			  if (buffer == pinData)
			  {
				  name.Text = dec.ToString();
			  }
			  else
			  {
				  name.Text = dec.ToString();
			  }
			  //Log(LogMsgType.Normal, s);
		  }
	  }


	  private int
	  ExtractData(byte[] msg, byte[] buffer)
	  {
			int		i = 0;
			int		j = 0;
			byte	b;
			int		length = msg.Length - 3;
			
			if (length <= 0) return 0;
			
			byte[] tempBuf = new byte[length];

			// Extract data (remove F0, F1 and type bytes)
			for (i = 0; i < length; ++i)
			{
				tempBuf[i] = msg[i+2];
			}

			// Check for a F2 byte in the frame (Escape Mechanism)
			for (i = 0; (i < length) && (j < tempBuf.Length); ++i)
			{
				b = tempBuf[j++];
				
				if (b == 0xF2)
				{
					b = tempBuf[j++];
					
					b = (byte)((b & 0x0F) | 0xF0);		// and replace it by its corresponding byte (F0,F1 or F0)
				}

				if (i < buffer.Length)
				{
					buffer[i] = b;           // copy values into the calibration buffer
				}
				else return i;
			}

		return i;
	  }

	  private void
	  ClearBuffer(byte[] buffer, int length)
	  {
		  int i;
		  for (i = 0; i < length; ++i)
		  {
			  buffer[i] = 0;
		  }
	  }

	  public void
	  SendMsg(byte[] frame, int length)
	  {
		  byte[] txBuffer = new byte[32];
		  int tx_i = 1;
		  int fra_i = 0;

		  if (!comport.IsOpen) return;

		  txBuffer[0] = 0xF0; // Start byte
		  
		  // Fill in the tx buffer with the content of the frame 
		  for (fra_i = 0; fra_i < length; fra_i++)
		  {
			  byte b = frame[fra_i];
			  if ((0xF0 <= b) && (b <= 0xF2)) // if data b is a 0xF0, 0xF1 or 0xF2 (reserved values)
			  {
				  txBuffer[tx_i++] = 0xF2;    // replace it by 0xF2
				  b &= 0x03;                  // and the corresponding code (see TMAP escape mechanism)
			  }
			  txBuffer[tx_i++] = b;
		  }
		  txBuffer[tx_i] = 0xF1; // Stop byte

		  // display tx buffer to visualize the bytes that have been sent
		  byte[] dispBuffer = new byte[tx_i + 1];
		  int j;
		  for (j = 0; j < (tx_i + 1); j++)
		  {
			  dispBuffer[j] = txBuffer[j];
		  }
		  // Display the binary data onto the terminal window
		  //Log(LogMsgType.Outgoing, ByteArrayToHexString(dispBuffer));

		  // Send the binary data out the port
		  comport.Write(txBuffer, 0, (tx_i + 1));
		  ++cmdCounter;
		  
		  // Que(ByteArrayToHexString(dispBuffer));
	  }

	  public void
	  Que(string s)
	  {
		  for (int i = 0; i < 10; ++i)
		  {
			  if (outputMessage == null) { outputMessage = s; return; }
			  Thread.Sleep(20);
		  }
	  }
	
	//	Called whenever a complete ascii packet arrives
	//	These are deliniated by \r\n EOP chars
	
	private void
	ParseAscii(byte [] data)
	{
		string	hex;
		string s = encoding.GetString(data,0, data.Length);
		
		s = string.Format("{0:X4} ", lastAddress) + s;
		
		Log(LogMsgType.Incoming, s);

		hex = NextFileLine(); // Advances psocAddress
		
		if (hex != null)
		{
			byte [] bh = encoding.GetBytes(hex);
			
			//Application.DoEvents();
			
			comport.Write(bh, 0, hex.Length);
		}
		else
		if (fileCount != 0)
		{
			fileCount = 0;
			
			byte	[] boot = new byte[4];
			
			boot[0] = (byte) 'B';
			boot[1] = (byte) 'O';
			boot[2] = (byte) 'O';
			boot[3] = (byte) 'T';
			
			comport.Write(boot, 0, 4);
			
			TMAP = true;
			modeCounter = 1;
		}
	}
	
	private void
	ParseTMAP(byte [] data)
	{
		if (data[1] == 0x5C)
		{
			// Version string
			
			if (data[2] != 0) 
			{
				string s = string.Format("Hardware {0}.{1} Firmware {2}.{3}",
						data[2],data[3],data[4],data[5]);
						
				lVersionString.Text = s;
				
				int	sn = data[6] * 256 + data[7];
				
				sn &= 0x7FFF;
				
				serialNumber.Text = string.Format("{0}", sn);
				
				if (!radioIsAlive) Log(LogMsgType.Warning, "Radio is attached!");
				
				radioIsAlive = true;
				//btnTest.BackColor = Color.Green;
			}
			else
			{
				radioIsAlive = false;
				//btnTest.BackColor = Color.Yellow;
				Log(LogMsgType.Warning, "Radio is in BootLoader mode");
				modeCounter = 1000;
			}
		}
		else 
		if (data[1] == 0x77)
		{
			ReadResponseRX(data);
		}
		else
		if (data[1] == 0x78)
		{
			ReadResponseTX(data);
		}
		else 
		if (data[1] == 0x71)	// Status
		{
			ShowStatus(data);	// builds pinData array

			int	sf = pinData[0];
			
			if ((sf & 0x01) != 0)
			{
				PrimaryFreq.BackColor = Color.Green;
			}
			else PrimaryFreq.BackColor = Color.LightGray;
			
			if ((sf & 0x02) != 0)
			{
			    SecondaryFreq.BackColor = Color.Green;
			}
			else SecondaryFreq.BackColor = Color.LightGray;
			
			if ((sf & 0x04) != 0)
			{
				tbPTT.Text = "ON";
				transmitting = true;
				tbPTT.BackColor = Color.Red;
			}
			else
			{
				tbPTT.Text = " ";
				transmitting = false;
				tbPTT.BackColor = SystemColors.Control;
			}
			
			if ((sf & 0x10) != 0)
			{
				pbMicVolume.BackColor = Color.Green;
				lMic.ForeColor = Color.Green;
			}
			else
			{
				pbMicVolume.BackColor = pbRadioVolume.BackColor;
				lMic.ForeColor = Color.Black;
			}
		}
		else 
		if (data[1] == 0x73)	// STATE response
		{
			int		pf = data[7] * 256 + data[8];
			int		sf = data[9] * 256 + data[10];
			
			if (updateFrequencies)
			{
				PrimaryFreq.Value = (Decimal) (118.0 + 0.025 * (pf >> 2));
				SecondaryFreq.Value = (Decimal) (118.0 + 0.025 * (sf >> 2));
			}
			updateFrequencies = false;
			
			Squelch.Value = (Decimal) data[4];
		}
		else
		if (data[1] == 0x7E)
		{
			// data[2,3] = pf, sf
			
			int		d = data[2];

			dataValue.Text = string.Format("{0:X2}",d);
		}
	}

		private void
		timer1_Tick(object sender, EventArgs e)
		{
			Packet p;

			if (modeCounter > 0) --modeCounter;

			if (outputMessage != null)
			{
				Log(LogMsgType.Outgoing, outputMessage);
				outputMessage = null;
			}

			// Any data back fromm radio -> it is attached.
			// BUT it must respond to a GetVersion to be alive!
			// deque packet and process it

			if (null != (p = PopPacket()))
			{
				radioAttached = true;

				int size = p.Size();

				byte[] message = new byte[size];

				int i, j;

				for (j = 0, i = p.start; j < size; )
				{
					message[j++] = que[i++ & 0x0FF];
				}

				if (size > 0)
				{
					if (TMAP) ParseTMAP(message);
					else ParseAscii(message);
				}
			}

			if (TMAP && comport.IsOpen && (modeCounter == 0))
			{
				if (!radioIsAlive) 
				{
					SendGetVersion();
					
					Thread.Sleep(20);
					
					SendGetFrequencies();
				}
				//btnTest.BackColor = Color.Orange;
				modeCounter = 100;	// Every 1 second look it see if the radio is alive
			}
		}


      // Counts the number of seconds in Transmit
	  // samples at 4 HZ
	  
		private void
		timer2_Tick(object sender, EventArgs e)
		{
			if (radioIsAlive)
			{
				ReadStatus();
				
				if (transmitting) ++txCounter;
				else txCounter = 0;

				lTxTimer.Text = string.Format("{0}s", txCounter / 4);
				
				Wait(20);
				
				ReadState();
			}
		}
      

		public	bool	TxTestState = false;

		private void 
		TxTest_Click(object sender, EventArgs e)
		{
			byte[] frame = new byte[6];

			// Create a frame with content to send

			frame[0] = 0x81;

			if (TxTestState)
			{
				frame[1] = 0x00;	// Passive off
				TxTest.BackColor = Color.LightGray;
				TxTestState = false;
				
				MicGain.Enabled = false;
				Sidetone.Enabled = false;
				
				Volume.Enabled = true;
				Squelch.Enabled = true;
				
				PrimaryFreq.Enabled = true;
				SecondaryFreq.Enabled = true;
			}
			else
			{
				frame[1] = 0x01;	// Passive on
				TxTest.BackColor = Color.LightCoral;
				TxTestState = true;

				MicGain.Enabled = true;
				Sidetone.Enabled = true;
								
				Volume.Enabled = false;
				Squelch.Enabled = false;
				
				PrimaryFreq.Enabled = false;
				// SecondaryFreq.Enabled = false;
			}
			// Add Start, Stop bytes, check for any reserved values and send data out the port
			SendMsg(frame, 2);
		}

		private void 
		toggle_Click(object sender, EventArgs e)
		{
			Decimal	pf = PrimaryFreq.Value;
			
			PrimaryFreq.Value = SecondaryFreq.Value;
			SecondaryFreq.Value = pf;
			
			SendState();
		}

		private void 
		address_DoubleClick(object sender, EventArgs e)
		{
			byte	ra = (byte) Convert.ToInt16( address.Text, 16);
			byte []	frame  = new byte[5];

			frame[0] = 0x7E; 
			frame[1] = ra; 

			SendMsg(frame, 2);
		}
	}
}


