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
UATControl
{
	partial class
	MainForm
	{
		public bool debug = false;
		public int	sidRevision = 0;
        public int sid2Revision = 0;
		static	int	K = 1024;

		byte[]	cmd = new byte[8];
		byte[]	status = new byte[8];
		byte[]	fileBuffer1 = new byte[64 * K];
		byte[]	fileBuffer2 = new byte[128 * K];
		byte[]	readBuffer = new byte[128 * K];
		byte[]	protectBuffer = new byte[512];

		//double		cal0dBm = 2000;
		//double		cal50dBm = 1170;
		
		void
		clearIt()
		{
			int	i;

			for(i = 0; i < 8; ++i ) status[i] = cmd[i] = 0;
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

	

		public void
		debugMessage(string s)
		{
			if (debug) message(s);
		}
		public void
		debugStatus(string s, bool status)
		{
			if (debug && !status)
			{
				message(s);
			}
		}
		
        //public bool
        //HIDCheck()
        //{
        //    bool flag = false;

        //    if (dongle[0] != null) 
        //    {
        //        if (sidRevision == 0)
        //        {
        //            clearIt();
        //            cmd[0] = 0x0F;

        //            USB_Command2(2,dongle[0]);

        //            if (status[1] == 0)
        //            {
        //                sidRevision = ((status[2] & 0x00FF) << 8) + status[3];
        //            }
        //            else sidRevision = 0x0101;

        //            Output.Text += string.Format(dongle[0].Product + " Revision = {0:X4}\r\n", sidRevision);
        //        }
        //        flag = true;
        //    }
        //    if (dongle2[0] != null)
        //    {
        //        if (sid2Revision == 0)
        //        {
        //            clearIt();
        //            cmd[0] = 0x0F;

        //            USB_Command2(2,dongle2[0]);

        //            if (status[1] == 0)
        //            {
        //                sid2Revision = ((status[2] & 0x00FF) << 8) + status[3];
        //            }
        //            else sid2Revision = 0x0101;

        //            Output.Text += string.Format(dongle2[0].Product + " Revision = {0:X4}\r\n", sid2Revision);
        //        }
        //        flag = true;
        //    }

        //    if (flag)
        //        return true;

        //    sidRevision = 0;
        //    sid2Revision = 0;
			
        //    message("No unit attached!");
        //    return false;
        //}
		
		public void
		message(string s)
		{
			Output.Text += s + "\r\n";
			scroll();

		}
		public void
		StatusMessage(string s, int status)
		{
			if ((status & 0x080) != 0)
			{
				Output.Text += s + " TIMEOUT\r\n";
			}
			else
			if ((status & 0x040) != 0)
			{
				Output.Text += s + " BUSY\r\n";
			}
			else
			if ((status & 0x020) != 0)
			{
				Output.Text += s + " ERROR\r\n";
			}
			else
			if ((status & 0x010) != 0)
			{
				Output.Text += s + " NAK\r\n";
			}
			else
			if (status != 0)
				Output.Text += s + " ???\r\n";

			scroll();
		}
        
		/*public bool
		USB_Command(int delay)
		{
			int		i;
			byte	sum = 0;

			if (dongle != null)
			{
				dongle.Features.DataBuf[0] = 0;

				for (i = 0; i < 7; ++i) 
				{
					sum += cmd[i];
					dongle.Features.DataBuf[i + 1] = cmd[i];
				}
				dongle.Features.DataBuf[8] = (byte)(1 + ~sum);

				if (!dongle.SetFeature(dongle.Features.ID))
				{
					debugMessage("failed:Command");
					return false;
				}
				debugMessage("passed:Command");

				// Now wait for the response to see if the command completed.

				if (delay == 0) return true;
				
				// Always delay before getting status

				Thread.Sleep(delay);
				
				// Try 3 times to get command status
				
				for (i = 3; i > 0; --i)
				{
					GetFeatureReport(status);

					if (status[0] != 0) return true;

					Thread.Sleep(delay);
				}
				if (status[0] == 0)
				{
					Output.Text += "!!! Command failed to complete\r\n";
					scroll();
				}
			}
			else message("No unit attached!");

			return false;
		}*/
        
        public bool
        USB_Command2(int delay, CyHidDevice whichDev)
        {
            int i;
            byte sum = 0;

            if (whichDev != null)
            {
                whichDev.Features.DataBuf[0] = 0;

                for (i = 0; i < 7; ++i)
                {
                    sum += cmd[i];
                    whichDev.Features.DataBuf[i + 1] = cmd[i];
                }
                whichDev.Features.DataBuf[8] = (byte)(1 + ~sum);

                if (!whichDev.SetFeature(whichDev.Features.ID))
                {
                    debugMessage("failed:Command");
                    return false;
                }
                debugMessage("passed:Command");

                // Now wait for the response to see if the command completed.

                if (delay == 0) return true;

                // Always delay before getting status

                Thread.Sleep(delay);

                // Try 3 times to get command status

                for (i = 3; i > 0; --i)
                {
                    GetFeatureReport2(status,whichDev);

                    if (status[0] != 0) return true;

                    Thread.Sleep(delay);
                }
                if (status[0] == 0)
                {
                    Output.Text += "!!! Command failed to complete\r\n";
                    scroll();
                }
            }
            else message("No unit attached!");

            return false;
        }

		public void
		DisplayData(byte[] buf, int bCnt)
		{
			StringBuilder dataStr = new StringBuilder();

			for (int i = 0; i < bCnt; ++i)
			{
				dataStr.Append(string.Format(" {0:X2}", buf[i]));
			}

			Output.Text += dataStr.ToString() + "\r\n" + "\r\n";

			Output.SelectionStart = Output.Text.Length;
			Output.ScrollToCaret();
		}

		/*public void
		GetFeatureReport(byte [] status)
		{
			bool s;

			if (dongle != null)
			{
				s = dongle.GetFeature(0);

				if (s)
					for (int i = 0; i < 8; ++i)
						status[i] = dongle.Features.DataBuf[i + 1];
			}
			else Output.Text += "No USB device\r\n";
		}*/

        public void
        GetFeatureReport2(byte[] status, CyHidDevice whichDev)
        {
            bool s;

            if (whichDev != null)
            {
                s = whichDev.GetFeature(0);

                if (s)
                    for (int i = 0; i < 8; ++i)
                        status[i] = whichDev.Features.DataBuf[i + 1];
            }
            else Output.Text += "No USB device\r\n";
        }

		/*public bool
		GetInputReport(byte [] dout, int count, byte xor)
		{
			bool status;

			if (dongle != null)
			{
				status = dongle.GetInput(dongle.Inputs.ID);
				debugStatus("GetInputReport:failed", status);

				for (int i = 0; i < 8 && i < count; ++i)
				{
					dout[i] = (byte) (xor ^ dongle.Inputs.DataBuf[i+1]);
				}
				return status;
			}
			return false;
		}*/

        public bool
        GetInputReport2(byte[] dout, int count, byte xor, CyHidDevice whichDev)
        {
            bool status;

            if (whichDev != null)
            {
                status = whichDev.GetInput(whichDev.Inputs.ID);
                debugStatus("GetInputReport:failed", status);

                for (int i = 0; i < 8 && i < count; ++i)
                {
                    dout[i] = (byte)(xor ^ whichDev.Inputs.DataBuf[i + 1]);
                }
                return status;
            }
            return false;
        }
		     
		/*public void
		SetOutputReport(byte [] din, int count)
		{
			int	 tsize,j;
			
			if (dongle == null)
			{
				message("No SG400 attached!");
				return;
			}
			
			j = 0;
			while(count > 0)
			{
				dongle.Outputs.DataBuf[0] = 0;
				
				if (count > 8) tsize = 8;
				else tsize = count;
				
				for (byte i = 0; i < tsize; ++i) dongle.Outputs.DataBuf[i+1] = din[j++];

				if (!dongle.SetOutput(dongle.Outputs.ID))
				{
					debugMessage("failed:Output");
					return;
				}
				count -= tsize;
			}
			debugMessage("passed:Output");
		}*/

        public void
        SetOutputReport2(byte[] din, int count, CyHidDevice whichDev)
        {
            int tsize, j;

            if (whichDev == null)
            {
                message("No unit attached!");
                return;
            }

            j = 0;
            while (count > 0)
            {
                whichDev.Outputs.DataBuf[0] = 0;

                if (count > 8) tsize = 8;
                else tsize = count;

                for (byte i = 0; i < tsize; ++i) whichDev.Outputs.DataBuf[i + 1] = din[j++];

                if (!whichDev.SetOutput(whichDev.Outputs.ID))
                {
                    debugMessage("failed:Output");
                    return;
                }
                count -= tsize;
            }
            debugMessage("passed:Output");
        }
	}


	public class 
	PING
	{
		public const byte GET_DATA = 1;
	}
}
