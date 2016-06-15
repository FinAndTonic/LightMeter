//#define SUPRESS

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using CyUSB;

namespace UATControl
{
	public partial class 
	MainForm : Form
	{
		String	pathName;
		Font	defFont = new Font("Courier New", 9, FontStyle.Regular);
		Font	boldFont = new Font("Courier New", 9, FontStyle.Bold);

        private SerialPort comport = new SerialPort();

        //bool fileLoaded = false;
        //String  pathOpened;
        //StreamReader src;
        //public Byte[] byteHolder = new Byte[557];
        //public byte[] uplinkHolder = new byte[432];
        //public Boolean adsbData = false;
        //public Boolean uplinkData = false;
        int numChecked;


		private void
		MainForm_Load(object sender, EventArgs e)
		{
			pathName = Application.StartupPath;

			string str = Convert.ToString(System.DateTime.Now);

			Output.Font = defFont;

			evHandler = new App_PnP_Callback(PnP_Event_Handler);
			
			//HID_Scan();
			
			//HIDCheck();

            RefreshComPortList();

            numChecked = 0;

            //checks = new CheckBox[] {Reg1Check, Reg2Check, Reg3Check, Reg4Check, Reg5Check, Reg6Check, Reg7Check, Reg8Check, Reg9Check, Reg10Check, Reg11Check, Reg12Check};

            //texts = new TextBox[] { Reg1Box, Reg2Box, Reg3Box, Reg4Box, Reg5Box, Reg6Box, Reg7Box, Reg8Box, Reg9Box, Reg10Box, Reg11Box, Reg12Box };
			
		}
		
		public 
		MainForm()
		{
			InitializeComponent();
		}
		
		private void 
		exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Close();
		}
		
		private void 
		loadToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void 
		MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{//does anything need to be shut down or paused if the program is closed?

		}

		private void 
		aboutToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			About about = new About();
			about.ShowDialog();
		}


		public void
		PopUp(string ms, bool error)
		{
			message(ms);
			
			if (error)
				 MessageBox.Show(ms,"Attention: Problems exist!", MessageBoxButtons.OK, MessageBoxIcon.Error);
			else MessageBox.Show(ms,"Information",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
		
		public bool
		Query(string ms)
		{
			System.Windows.Forms.DialogResult	dr;
			
			dr = MessageBox.Show(ms,"Attention: Please respond!",
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation,MessageBoxDefaultButton.Button2);
			
			if (dr == DialogResult.Yes) return true;
			return false;
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
                btnOpenPort.Text = "Open Port";
            }
            else
            {
                // Set the port's settings
                if (cmbBaudRate.Text != "" && cmbDataBits.Text != "" &&
                    cmbParity.Text != "" && cmbPortName.Text != "")
                {
                    comport.BaudRate = int.Parse(cmbBaudRate.Text);
                    comport.DataBits = int.Parse(cmbDataBits.Text);
                    comport.StopBits = (StopBits)Enum.Parse(typeof(StopBits), "1");
                    comport.Parity = (Parity)Enum.Parse(typeof(Parity), cmbParity.Text);
                    comport.PortName = cmbPortName.Text;

                    try
                    {
                        comport.Open();

                        btnOpenPort.BackColor = Color.LightGreen;
                        btnOpenPort.Text = "Close Port";
                    }
                    catch (UnauthorizedAccessException) { error = true; }
                    catch (IOException) { error = true; }
                    catch (ArgumentException) { error = true; }

                    if (error) MessageBox.Show(this, "Could not open the COM port.  Most likely it is already in use, has been removed, or is unavailable.", "COM Port Unavalible", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            OpenThePort();
        }

        private void
        RefreshComPortList()
        {
            cmbPortName.Items.Clear();
            cmbPortName.Items.AddRange(SerialPort.GetPortNames());
        }
        
        private UInt16 Fletcher16(byte[] data, UInt16 length)
        {
            // these must be 16 bits
            UInt16 sum1 = 0x00FF;
            UInt16 sum2 = 0x00FF;
            UInt16 n = 0;

            while (length-- > 0)
            {
                sum1 += data[n];
                if (sum1 > 0x00FF)
                    sum1 -= 0x00FF;

                sum2 += sum1;
                if (sum2 > 0x00FF)
                    sum2 -= 0x00FF;
                n++;
            }

            return (UInt16)((sum2 << 8) | sum1);
        }

        private void SendMax2112_ASIP()
        {
            byte[] txBuf = new byte[22];
            byte[] tmpBuf = new byte[14];
            byte[] rxBuf = new byte[14];
            //int n;
            char[] delimiters = new char[] { ' ', '\t' };
            string[] hb = textBox1.Text.Split(delimiters);
            UInt16 checkedsum;

            txBuf[0] = 0xC2;
            txBuf[1] = 0x52;
            txBuf[2] = 0xFF;
            txBuf[3] = 0x6F;
            //txBuf[4] = 0;//LSB length
            txBuf[5] = 0;//MSB length

            if (ReadCheckBox.Checked)
            {
                txBuf[4] = 3;
                txBuf[6] = (byte)0x60;       //Start at register 0
                txBuf[7] = 14;      //read all 14 registers 
                txBuf[8] = 0;

                checkedsum = Fletcher16(txBuf, 9);

                txBuf[9] = (byte)(checkedsum & 0x00FF);
                txBuf[10] = (byte)((checkedsum & 0xFF00) >> 8);
            }
            else if (Reg13Check.Checked)
            {
                txBuf[4] = 3;
                txBuf[6] = (byte)0x60;
                txBuf[7] = 2;
                txBuf[8] = (byte)0x0C;

                checkedsum = Fletcher16(txBuf, 9);

                txBuf[9] = (byte)(checkedsum & 0x00FF);
                txBuf[10] = (byte)((checkedsum & 0xFF00) >> 8);
            }
            else
            {
                txBuf[4] = 14;

                txBuf[6] = (byte)0x60;
                txBuf[7] = 0;
                txBuf[8] = (byte)(0x80 | (Convert.ToInt16(Reg1_NDivider.Text, 10) / 256));
                txBuf[9] = (byte)(Convert.ToInt16(Reg1_NDivider.Text, 10) % 256);
                txBuf[10] = (byte)((Convert.ToByte(Reg3_ChargePump.Text, 10) << 4) | (Convert.ToInt32(Reg3_FDivider.Text, 10) >> 16));
                txBuf[11] = (byte)((Convert.ToInt32(Reg3_FDivider.Text, 10) >> 8) & 0xFF);
                txBuf[12] = (byte)(Convert.ToInt32(Reg3_FDivider.Text, 10) & 0xFF);
                txBuf[13] = (byte)(((Convert.ToByte(Reg6_xtal.Text, 10) - 1) << 5) | Convert.ToByte(Reg6_ref.Value) & 0x1F);
                txBuf[14] = (byte)((Convert.ToByte(Reg7_PLL.Text, 2) << 5) | 0x08);
                txBuf[15] = (byte)(((Convert.ToByte(Reg8_VCO.Value) + 8) << 3) & 0xF8);
                txBuf[15] |= (byte)((Reg8_VCOAutoselect.Checked ? 0x04 : 0) | (Reg8_ADCLatch.Checked ? 0x02 : 0) | (Reg8_ADCRead.Checked ? 0x01 : 0));
                txBuf[16] = Convert.ToByte(Reg9_LowPassFilter.Text, 16);
                txBuf[17] = (byte)(Convert.ToByte(Reg10_Gain.Value) & 0x0F);
                txBuf[18] = Convert.ToByte(Reg11_Shutdown.Text, 16);
                txBuf[19] = Convert.ToByte(Reg12_Test.Text, 16);

                checkedsum = Fletcher16(txBuf, 20);

                txBuf[20] = (byte)(checkedsum & 0x00FF);
                txBuf[21] = (byte)((checkedsum & 0xFF00) >> 8);
            }

            comport.Write(txBuf, 0, (txBuf[4] + 8));

            if (!Reg13Check.Checked && !ReadCheckBox.Checked)
            {
                Thread.Sleep(25);

                txBuf[4] = 3;//#
                txBuf[6] = 0x60;//slave id
                txBuf[7] = 0x04;//reg
                txBuf[8] = (byte)(Convert.ToInt32(Reg3_FDivider.Text, 10) & 0xFF);  //re-send LSB of F-Divider

                checkedsum = Fletcher16(txBuf, 9);

                txBuf[9] = (byte)(checkedsum & 0x00FF);
                txBuf[10] = (byte)((checkedsum & 0xFF00) >> 8);

                comport.Write(txBuf, 0, 11);
            }

            if (Reg13Check.Checked)
            {
                System.Threading.Thread.Sleep(250);

                comport.Read(rxBuf, 0, 2);

                textBox2.Text = Convert.ToString(rxBuf[0], 16) + " " + Convert.ToString(rxBuf[1], 16);
            }
            else if (ReadCheckBox.Checked)
            {
                System.Threading.Thread.Sleep(250);

                comport.Read(rxBuf, 0, 14);

                textBox1.Text = string.Format("{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2} {9:X2} {10:X2} {11:X2} {12:X2} {13:X2}",
                                rxBuf[0], rxBuf[1], rxBuf[2], rxBuf[3], rxBuf[4], rxBuf[5], rxBuf[6], rxBuf[7], rxBuf[8], rxBuf[9], rxBuf[10], rxBuf[11], rxBuf[12], rxBuf[13]);
            }
        }

        private void SendMax2112_Click(object sender, EventArgs e)
        {
            if (!comport.IsOpen)
            {
                Output.AppendText("You have not yet opened a COM port!\r\n");
                return;
            }

            if (checkASIP.Checked)
                SendMax2112_ASIP();
            else
            {

                byte[] txBuf = new byte[17];
                byte[] rxBuf = new byte[14];
                //int n, m;
                char[] delimiters = new char[] { ' ', '\t' };
                string[] hb = textBox1.Text.Split(delimiters);

                if (ReadCheckBox.Checked)
                {
                    txBuf[2] = 0;       //Start at register 0
                    numChecked = 14;    //read all 14 registers 
                }
                else if (Reg13Check.Checked)
                {
                    txBuf[2] = (byte)0x0C;
                    numChecked = 2;
                }
                else
                {
                    txBuf[2] = 0;   //Register to start writing at

                    txBuf[3] = (byte)(0x80 | (Convert.ToInt16(Reg1_NDivider.Text, 10) / 256));
                    txBuf[4] = (byte)(Convert.ToInt16(Reg1_NDivider.Text, 10) % 256);
                    txBuf[5] = (byte)((Convert.ToByte(Reg3_ChargePump.Text, 10) << 4) | (Convert.ToInt32(Reg3_FDivider.Text, 10) >> 16));
                    txBuf[6] = (byte)((Convert.ToInt32(Reg3_FDivider.Text, 10) >> 8) & 0xFF);
                    txBuf[7] = (byte)(Convert.ToInt32(Reg3_FDivider.Text, 10) & 0xFF);
                    txBuf[8] = (byte)(((Convert.ToByte(Reg6_xtal.Text, 10) - 1) << 5) | Convert.ToByte(Reg6_ref.Value) & 0x1F);
                    txBuf[9] = (byte)((Convert.ToByte(Reg7_PLL.Text, 2) << 5) | 0x08);
                    txBuf[10] = (byte)(((Convert.ToByte(Reg8_VCO.Value) + 8) << 3) & 0xF8);
                    txBuf[10] |= (byte)((Reg8_VCOAutoselect.Checked ? 0x04 : 0) | (Reg8_ADCLatch.Checked ? 0x02 : 0) | (Reg8_ADCRead.Checked ? 0x01 : 0));
                    txBuf[11] = Convert.ToByte(Reg9_LowPassFilter.Text, 16);
                    txBuf[12] = (byte)(Convert.ToByte(Reg10_Gain.Value) & 0x0F);
                    txBuf[13] = Convert.ToByte(Reg11_Shutdown.Text, 16);
                    txBuf[14] = Convert.ToByte(Reg12_Test.Text, 16);

                    numChecked = 12;
                }

                txBuf[0] = (byte)(numChecked + 2);  //the number of bytes we're sending, that aren't this byte
                txBuf[1] = (byte)0x60;             //I2C slave address of the MAX2112

                //if (!comport.IsOpen) return;

                comport.Write(txBuf, 0, (numChecked + 3));

                if (!Reg13Check.Checked && !ReadCheckBox.Checked)
                {
                    Thread.Sleep(250);

                    txBuf[0] = 3;
                    txBuf[1] = 0x60;
                    txBuf[2] = 0x04;
                    txBuf[3] = (byte)(Convert.ToInt32(Reg3_FDivider.Text, 10) & 0xFF);  //re-send LSB of F-Divider

                    comport.Write(txBuf, 0, 4);
                }

                if (Reg13Check.Checked)
                {
                    System.Threading.Thread.Sleep(250);

                    comport.Read(rxBuf, 0, 2);

                    textBox2.Text = Convert.ToString(rxBuf[0], 16) + " " + Convert.ToString(rxBuf[1], 16);
                }
                else if (ReadCheckBox.Checked)
                {
                    System.Threading.Thread.Sleep(250);

                    comport.Read(rxBuf, 0, 14);

                    textBox1.Text = string.Format("{0:X2} {1:X2} {2:X2} {3:X2} {4:X2} {5:X2} {6:X2} {7:X2} {8:X2} {9:X2} {10:X2} {11:X2} {12:X2} {13:X2}",
                                    rxBuf[0], rxBuf[1], rxBuf[2], rxBuf[3], rxBuf[4], rxBuf[5], rxBuf[6], rxBuf[7], rxBuf[8], rxBuf[9], rxBuf[10], rxBuf[11], rxBuf[12], rxBuf[13]);
                }
            }
        }

        private void SendMCP4728_Click(object sender, EventArgs e)
        {
            if (!comport.IsOpen)
            {
                Output.AppendText("You have not yet opened a COM port!\r\n");
                return;
            }
            if (checkASIP.Checked)
            {
                Output.AppendText("ASIP is checked! MCP4728 does not exist on this board.\r\n");
                return;
            }

            byte[] txBuf = new byte[26];
            byte[] rxBuf = new byte[24];
            int n;

            if (ReadMCP4728.Checked)
            {
                numChecked = 24;
            }
            else if (CH1Checkbox.Checked && CH2Checkbox.Checked && CH3Checkbox.Checked && CH4Checkbox.Checked)
            {
                txBuf[2] = 0x50;    //01010 = write sequentially; 00 = starting from channel 1, through 4; 0
                
                txBuf[3] = (byte)((CH1Vref.Checked ? 0x80 : 0) | (Convert.ToByte(CH1PwrMode.Text, 2) << 5) | (CH1Gain.Checked ? 0x10 : 0));
                txBuf[3] |= (byte)((Convert.ToInt16(CH1Data.Text, 16) & 0xF00) >> 8);
                txBuf[4] = (byte)(Convert.ToInt16(CH1Data.Text, 16) & 0x0FF);

                txBuf[5] = (byte)((CH2Vref.Checked ? 0x80 : 0) | (Convert.ToByte(CH2PwrMode.Text, 2) << 5) | (CH2Gain.Checked ? 0x10 : 0));
                txBuf[5] |= (byte)((Convert.ToInt16(CH2Data.Text, 16) & 0xF00) >> 8);
                txBuf[6] = (byte)(Convert.ToInt16(CH2Data.Text, 16) & 0x0FF);

                txBuf[7] = (byte)((CH3Vref.Checked ? 0x80 : 0) | (Convert.ToByte(CH3PwrMode.Text, 2) << 5) | (CH3Gain.Checked ? 0x10 : 0));
                txBuf[7] |= (byte)((Convert.ToInt16(CH3Data.Text, 16) & 0xF00) >> 8);
                txBuf[8] = (byte)(Convert.ToInt16(CH3Data.Text, 16) & 0x0FF);

                txBuf[9] = (byte)((CH4Vref.Checked ? 0x80 : 0) | (Convert.ToByte(CH4PwrMode.Text, 2) << 5) | (CH4Gain.Checked ? 0x10 : 0));
                txBuf[9] |= (byte)((Convert.ToInt16(CH4Data.Text, 16) & 0xF00) >> 8);
                txBuf[10] = (byte)(Convert.ToInt16(CH4Data.Text, 16) & 0x0FF);

                numChecked = 9;     //command + 2 bytes per channel x4
            }
            else
            {
                n = 2;

                if (CH1Checkbox.Checked)
                {
                    txBuf[n] = 0x40;    //01000 = write multiple channels; 00 starting at CH1; 0
                    txBuf[n + 1] = (byte)((CH1Vref.Checked ? 0x80 : 0) | (Convert.ToByte(CH1PwrMode.Text, 2) << 5) | (CH1Gain.Checked ? 0x10 : 0));
                    txBuf[n + 1] |= (byte)((Convert.ToInt16(CH1Data.Text, 16) & 0xF00) >> 8);
                    txBuf[n + 2] = (byte)(Convert.ToInt16(CH1Data.Text, 16) & 0x0FF);
                    n += 3;
                }
                if (CH2Checkbox.Checked)
                {
                    txBuf[n] = 0x42;    //01000 = write multiple channels; 01 starting at CH2; 0
                    txBuf[n + 1] = (byte)((CH2Vref.Checked ? 0x80 : 0) | (Convert.ToByte(CH2PwrMode.Text, 2) << 5) | (CH2Gain.Checked ? 0x10 : 0));
                    txBuf[n + 1] |= (byte)((Convert.ToInt16(CH2Data.Text, 16) & 0xF00) >> 8);
                    txBuf[n + 2] = (byte)(Convert.ToInt16(CH2Data.Text, 16) & 0x0FF);
                    n += 3;
                }
                if (CH3Checkbox.Checked)
                {
                    txBuf[n] = 0x44;    //01000 = write multiple channels; 10 starting at CH3; 0
                    txBuf[n + 1] = (byte)((CH3Vref.Checked ? 0x80 : 0) | (Convert.ToByte(CH3PwrMode.Text, 2) << 5) | (CH3Gain.Checked ? 0x10 : 0));
                    txBuf[n + 1] |= (byte)((Convert.ToInt16(CH3Data.Text, 16) & 0xF00) >> 8);
                    txBuf[n + 2] = (byte)(Convert.ToInt16(CH3Data.Text, 16) & 0x0FF);
                    n += 3;
                }
                if (CH4Checkbox.Checked)
                {
                    txBuf[n] = 0x46;    //01000 = write multiple channels; 11 starting at CH4; 0
                    txBuf[n + 1] = (byte)((CH4Vref.Checked ? 0x80 : 0) | (Convert.ToByte(CH4PwrMode.Text, 2) << 5) | (CH4Gain.Checked ? 0x10 : 0));
                    txBuf[n + 1] |= (byte)((Convert.ToInt16(CH4Data.Text, 16) & 0xF00) >> 8);
                    txBuf[n + 2] = (byte)(Convert.ToInt16(CH4Data.Text, 16) & 0x0FF);
                    n += 3;
                }
                //of the 0x4n "addresses", the top 5 bits don't count after the first address.
                //Thusly, CH4 could write 0xF6 to txBuf[n], and it would only care about the 0110 portion,
                //ONLY IF CH4 was not the first one sent.

                numChecked = n - 2;
            }
            if (numChecked <= 0)
            {
                Output.AppendText("You must have at least 1 channel selected!\r\n");
                return;
            }

            txBuf[0] = (byte)(numChecked + 1);  //the number of bytes we're sending, not counting [0]

            Int16 slaveAddress = Convert.ToByte(MCP_SLA.Text, 16);
            if (slaveAddress > 255) 
            {
                Output.AppendText("The slave address entered does not fit.\r\n");
                return;
            }
            txBuf[1] = (byte)slaveAddress;    //I2C slave address of the MCP4728 (the new one) (0x60 was old, 0x61 is new)

            comport.Write(txBuf, 0, (numChecked + 2));

            //Reading is strange:
            //Send the address with the read bit, and it throws everything at you.

            if (ReadMCP4728.Checked)
            {
                System.Threading.Thread.Sleep(250);

                comport.Read(rxBuf, 0, 24);

                string received = string.Format("{0:X2} {1:X2} {2:X2} - {3:X2} {4:X2} {5:X2}\r\n", rxBuf[0], rxBuf[1], rxBuf[2], rxBuf[3], rxBuf[4], rxBuf[5]);
                Output.AppendText(received);
                received = string.Format("{0:X2} {1:X2} {2:X2} - {3:X2} {4:X2} {5:X2}\r\n", rxBuf[6], rxBuf[7], rxBuf[8], rxBuf[9], rxBuf[10], rxBuf[11]);
                Output.AppendText(received);
                received = string.Format("{0:X2} {1:X2} {2:X2} - {3:X2} {4:X2} {5:X2}\r\n", rxBuf[12], rxBuf[13], rxBuf[14], rxBuf[15], rxBuf[16], rxBuf[17]);
                Output.AppendText(received);
                received = string.Format("{0:X2} {1:X2} {2:X2} - {3:X2} {4:X2} {5:X2}\r\n", rxBuf[18], rxBuf[19], rxBuf[20], rxBuf[21], rxBuf[22], rxBuf[23]);
                Output.AppendText(received);
            }
        }

        private void Reg13Check_CheckedChanged(object sender, EventArgs e)
        {
            if (Reg13Check.Checked)
            {
                numChecked = 2;
                SendMax2112.Text = "Read";
                ReadCheckBox.Checked = false;
            }
            else if (!ReadCheckBox.Checked)
            {
                SendMax2112.Text = "Send";
            }
        }

        private void ReadCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ReadCheckBox.Checked)
            {
                SendMax2112.Text = "Read";
                Reg13Check.Checked = false;
            }
            else if (!Reg13Check.Checked)
                SendMax2112.Text = "Send";
        }

        private void CH1Vref_CheckedChanged(object sender, EventArgs e)
        {
            if (!CH1Vref.Checked)
            {
                CH1Gain.Enabled = false;
                CH1Gain.Checked = false;
            }
            if (CH1Vref.Checked)
                CH1Gain.Enabled = true;
        }

        private void CH2Vref_CheckedChanged(object sender, EventArgs e)
        {
            if (!CH2Vref.Checked)
            {
                CH2Gain.Enabled = false;
                CH2Gain.Checked = false;
            }
            if (CH2Vref.Checked)
                CH2Gain.Enabled = true;
        }

        private void CH3Vref_CheckedChanged(object sender, EventArgs e)
        {
            if (!CH3Vref.Checked)
            {
                CH3Gain.Enabled = false;
                CH3Gain.Checked = false;
            }
            if (CH3Vref.Checked)
                CH3Gain.Enabled = true;
        }

        private void CH4Vref_CheckedChanged(object sender, EventArgs e)
        {
            if (!CH4Vref.Checked)
            {
                CH4Gain.Enabled = false;
                CH4Gain.Checked = false;
            }
            if (CH4Vref.Checked)
                CH4Gain.Enabled = true;
        }

        private void CH1Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!CH1Checkbox.Checked)
            {
                CH1Vref.Checked = false;
                CH1Vref.Enabled = false;
                CH1PwrMode.Enabled = false;
                CH1Data.Enabled = false;
            }
            else
            {
                CH1Vref.Enabled = true;
                CH1PwrMode.Enabled = true;
                CH1Data.Enabled = true;
            }
        }

        private void CH2Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!CH2Checkbox.Checked)
            {
                CH2Vref.Checked = false;
                CH2Vref.Enabled = false;
                CH2PwrMode.Enabled = false;
                CH2Data.Enabled = false;
            }
            else
            {
                CH2Vref.Enabled = true;
                CH2PwrMode.Enabled = true;
                CH2Data.Enabled = true;
            }
        }

        private void CH3Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!CH3Checkbox.Checked)
            {
                CH3Vref.Checked = false;
                CH3Vref.Enabled = false;
                CH3PwrMode.Enabled = false;
                CH3Data.Enabled = false;
            }
            else
            {
                CH3Vref.Enabled = true;
                CH3PwrMode.Enabled = true;
                CH3Data.Enabled = true;
            }
        }

        private void CH4Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!CH4Checkbox.Checked)
            {
                CH4Vref.Checked = false;
                CH4Vref.Enabled = false;
                CH4PwrMode.Enabled = false;
                CH4Data.Enabled = false;
            }
            else
            {
                CH4Vref.Enabled = true;
                CH4PwrMode.Enabled = true;
                CH4Data.Enabled = true;
            }
        }

        private void ReadMCP4728_CheckedChanged(object sender, EventArgs e)
        {
            if (ReadMCP4728.Checked)
            {
                SendMCP4728.Text = "Read";
            }
            else
                SendMCP4728.Text = "Send";
        }

        private void DACBar_ValueChanged(object sender, EventArgs e)
        {
            //3.26 full scale. 1023 segments
            //3.26*(722/1023) - .7 = 1.6
            double newValue = (3.26*DACBar.Value/1023)-0.7;
            DACvoltage.Text = String.Format("{0:f3}",newValue);
        }

        private void SendLPC_Click(object sender, EventArgs e)
        {
            if (!comport.IsOpen)
            {
                Output.AppendText("You have not yet opened a COM port!\r\n");
                return;
            }

            byte[] txBuf = new byte[10];
            UInt16 checkedsum;

            txBuf[0] = 0xC2;
            txBuf[1] = 0x52;
            txBuf[2] = 0xFF;
            txBuf[3] = 0x6C;    //randomly chosen to indicate the LPC itself
            txBuf[4] = 2;//LSB length
            txBuf[5] = 0;//MSB length

            txBuf[6] = (byte)((DACBar.Value & 0x0F00) >> 8);    //MSb
            txBuf[7] = (byte)(DACBar.Value & 0x0FF);          //LSB

            checkedsum = Fletcher16(txBuf, 8);

            txBuf[8] = (byte)(checkedsum & 0x00FF);
            txBuf[9] = (byte)((checkedsum & 0xFF00) >> 8);

            comport.Write(txBuf, 0, (txBuf[4] + 8));
        }

        
    }
}