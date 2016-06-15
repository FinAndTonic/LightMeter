using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using CyUSB;

namespace UATControl
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private App_PnP_Callback evHandler;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.readMeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.Output = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.gbPortSettings = new System.Windows.Forms.GroupBox();
            this.cmbPortName = new System.Windows.Forms.ComboBox();
            this.cmbBaudRate = new System.Windows.Forms.ComboBox();
            this.cmbStopBits = new System.Windows.Forms.ComboBox();
            this.cmbParity = new System.Windows.Forms.ComboBox();
            this.cmbDataBits = new System.Windows.Forms.ComboBox();
            this.btnOpenPort = new System.Windows.Forms.Button();
            this.lblComPort = new System.Windows.Forms.Label();
            this.lblStopBits = new System.Windows.Forms.Label();
            this.lblBaudRate = new System.Windows.Forms.Label();
            this.lblDataBits = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.CH4Data = new System.Windows.Forms.TextBox();
            this.CH3Data = new System.Windows.Forms.TextBox();
            this.CH2Data = new System.Windows.Forms.TextBox();
            this.CH1Data = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.RegisterPage1 = new System.Windows.Forms.TabPage();
            this.ReadCheckBox = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.Reg12_Test = new System.Windows.Forms.TextBox();
            this.Reg13Check = new System.Windows.Forms.CheckBox();
            this.Reg11_Shutdown = new System.Windows.Forms.TextBox();
            this.SendMax2112 = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.Reg1_NDivider = new System.Windows.Forms.TextBox();
            this.Reg10_Gain = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Reg3_ChargePump = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Reg9_LowPassFilter = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.Reg3_FDivider = new System.Windows.Forms.TextBox();
            this.Reg8_ADCRead = new System.Windows.Forms.CheckBox();
            this.Reg7_PLL = new System.Windows.Forms.ComboBox();
            this.Reg8_ADCLatch = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Reg8_VCOAutoselect = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Reg8_VCO = new System.Windows.Forms.NumericUpDown();
            this.Reg6_xtal = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.Reg6_ref = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.RegisterPage2 = new System.Windows.Forms.TabPage();
            this.label21 = new System.Windows.Forms.Label();
            this.MCP_SLA = new System.Windows.Forms.TextBox();
            this.ReadMCP4728 = new System.Windows.Forms.CheckBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.CH4Checkbox = new System.Windows.Forms.CheckBox();
            this.CH3Checkbox = new System.Windows.Forms.CheckBox();
            this.CH2Checkbox = new System.Windows.Forms.CheckBox();
            this.CH1Checkbox = new System.Windows.Forms.CheckBox();
            this.CH4Gain = new System.Windows.Forms.CheckBox();
            this.label19 = new System.Windows.Forms.Label();
            this.CH4PwrMode = new System.Windows.Forms.ComboBox();
            this.CH4Vref = new System.Windows.Forms.CheckBox();
            this.CH3Gain = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.CH3PwrMode = new System.Windows.Forms.ComboBox();
            this.CH3Vref = new System.Windows.Forms.CheckBox();
            this.CH2Gain = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.CH2PwrMode = new System.Windows.Forms.ComboBox();
            this.CH2Vref = new System.Windows.Forms.CheckBox();
            this.CH1Gain = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.CH1PwrMode = new System.Windows.Forms.ComboBox();
            this.CH1Vref = new System.Windows.Forms.CheckBox();
            this.SendMCP4728 = new System.Windows.Forms.Button();
            this.checkASIP = new System.Windows.Forms.CheckBox();
            this.RegisterPage3 = new System.Windows.Forms.TabPage();
            this.DACBar = new System.Windows.Forms.HScrollBar();
            this.DACvoltage = new System.Windows.Forms.Label();
            this.SendLPC = new System.Windows.Forms.Button();
            this.label22 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.gbPortSettings.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.RegisterPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Reg10_Gain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Reg8_VCO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Reg6_ref)).BeginInit();
            this.RegisterPage2.SuspendLayout();
            this.RegisterPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(472, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem1,
            this.readMeToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(113, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            this.aboutToolStripMenuItem1.Click += new System.EventHandler(this.aboutToolStripMenuItem1_Click);
            // 
            // readMeToolStripMenuItem
            // 
            this.readMeToolStripMenuItem.Name = "readMeToolStripMenuItem";
            this.readMeToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            this.readMeToolStripMenuItem.Text = "ReadMe";
            // 
            // Output
            // 
            this.Output.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Output.Location = new System.Drawing.Point(12, 323);
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output.Size = new System.Drawing.Size(448, 95);
            this.Output.TabIndex = 13;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            // 
            // gbPortSettings
            // 
            this.gbPortSettings.Controls.Add(this.cmbPortName);
            this.gbPortSettings.Controls.Add(this.cmbBaudRate);
            this.gbPortSettings.Controls.Add(this.cmbStopBits);
            this.gbPortSettings.Controls.Add(this.cmbParity);
            this.gbPortSettings.Controls.Add(this.cmbDataBits);
            this.gbPortSettings.Controls.Add(this.btnOpenPort);
            this.gbPortSettings.Controls.Add(this.lblComPort);
            this.gbPortSettings.Controls.Add(this.lblStopBits);
            this.gbPortSettings.Controls.Add(this.lblBaudRate);
            this.gbPortSettings.Controls.Add(this.lblDataBits);
            this.gbPortSettings.Controls.Add(this.label1);
            this.gbPortSettings.Location = new System.Drawing.Point(12, 27);
            this.gbPortSettings.Name = "gbPortSettings";
            this.gbPortSettings.Size = new System.Drawing.Size(448, 75);
            this.gbPortSettings.TabIndex = 62;
            this.gbPortSettings.TabStop = false;
            this.gbPortSettings.Text = "COM  Port";
            // 
            // cmbPortName
            // 
            this.cmbPortName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPortName.FormattingEnabled = true;
            this.cmbPortName.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6"});
            this.cmbPortName.Location = new System.Drawing.Point(172, 21);
            this.cmbPortName.Name = "cmbPortName";
            this.cmbPortName.Size = new System.Drawing.Size(84, 21);
            this.cmbPortName.TabIndex = 1;
            // 
            // cmbBaudRate
            // 
            this.cmbBaudRate.FormattingEnabled = true;
            this.cmbBaudRate.Items.AddRange(new object[] {
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cmbBaudRate.Location = new System.Drawing.Point(345, 21);
            this.cmbBaudRate.Name = "cmbBaudRate";
            this.cmbBaudRate.Size = new System.Drawing.Size(84, 21);
            this.cmbBaudRate.TabIndex = 3;
            // 
            // cmbStopBits
            // 
            this.cmbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStopBits.FormattingEnabled = true;
            this.cmbStopBits.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cmbStopBits.Location = new System.Drawing.Point(345, 75);
            this.cmbStopBits.Name = "cmbStopBits";
            this.cmbStopBits.Size = new System.Drawing.Size(65, 21);
            this.cmbStopBits.TabIndex = 9;
            // 
            // cmbParity
            // 
            this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParity.FormattingEnabled = true;
            this.cmbParity.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd"});
            this.cmbParity.Location = new System.Drawing.Point(172, 48);
            this.cmbParity.Name = "cmbParity";
            this.cmbParity.Size = new System.Drawing.Size(84, 21);
            this.cmbParity.TabIndex = 5;
            // 
            // cmbDataBits
            // 
            this.cmbDataBits.FormattingEnabled = true;
            this.cmbDataBits.Items.AddRange(new object[] {
            "7",
            "8",
            "9"});
            this.cmbDataBits.Location = new System.Drawing.Point(345, 48);
            this.cmbDataBits.Name = "cmbDataBits";
            this.cmbDataBits.Size = new System.Drawing.Size(84, 21);
            this.cmbDataBits.TabIndex = 7;
            // 
            // btnOpenPort
            // 
            this.btnOpenPort.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.btnOpenPort.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOpenPort.Location = new System.Drawing.Point(6, 19);
            this.btnOpenPort.Name = "btnOpenPort";
            this.btnOpenPort.Size = new System.Drawing.Size(84, 23);
            this.btnOpenPort.TabIndex = 6;
            this.btnOpenPort.Text = "&Open Port";
            this.btnOpenPort.UseVisualStyleBackColor = false;
            this.btnOpenPort.Click += new System.EventHandler(this.btnOpenPort_Click);
            // 
            // lblComPort
            // 
            this.lblComPort.AutoSize = true;
            this.lblComPort.Location = new System.Drawing.Point(110, 24);
            this.lblComPort.Name = "lblComPort";
            this.lblComPort.Size = new System.Drawing.Size(56, 13);
            this.lblComPort.TabIndex = 0;
            this.lblComPort.Text = "COM Port:";
            // 
            // lblStopBits
            // 
            this.lblStopBits.AutoSize = true;
            this.lblStopBits.Location = new System.Drawing.Point(287, 78);
            this.lblStopBits.Name = "lblStopBits";
            this.lblStopBits.Size = new System.Drawing.Size(52, 13);
            this.lblStopBits.TabIndex = 8;
            this.lblStopBits.Text = "Stop Bits:";
            // 
            // lblBaudRate
            // 
            this.lblBaudRate.AutoSize = true;
            this.lblBaudRate.Location = new System.Drawing.Point(278, 24);
            this.lblBaudRate.Name = "lblBaudRate";
            this.lblBaudRate.Size = new System.Drawing.Size(61, 13);
            this.lblBaudRate.TabIndex = 2;
            this.lblBaudRate.Text = "Baud Rate:";
            // 
            // lblDataBits
            // 
            this.lblDataBits.AutoSize = true;
            this.lblDataBits.Location = new System.Drawing.Point(286, 51);
            this.lblDataBits.Name = "lblDataBits";
            this.lblDataBits.Size = new System.Drawing.Size(53, 13);
            this.lblDataBits.TabIndex = 6;
            this.lblDataBits.Text = "Data Bits:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(130, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Parity:";
            // 
            // CH4Data
            // 
            this.CH4Data.Location = new System.Drawing.Point(392, 127);
            this.CH4Data.Name = "CH4Data";
            this.CH4Data.Size = new System.Drawing.Size(42, 20);
            this.CH4Data.TabIndex = 145;
            this.CH4Data.Text = "09B2";
            this.CH4Data.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.CH4Data, "9B2 creates 2.0 V");
            // 
            // CH3Data
            // 
            this.CH3Data.Location = new System.Drawing.Point(392, 100);
            this.CH3Data.Name = "CH3Data";
            this.CH3Data.Size = new System.Drawing.Size(42, 20);
            this.CH3Data.TabIndex = 143;
            this.CH3Data.Text = "0800";
            this.CH3Data.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.CH3Data, "800 creates 1.6 V");
            // 
            // CH2Data
            // 
            this.CH2Data.Location = new System.Drawing.Point(392, 73);
            this.CH2Data.Name = "CH2Data";
            this.CH2Data.Size = new System.Drawing.Size(42, 20);
            this.CH2Data.TabIndex = 141;
            this.CH2Data.Text = "0800";
            this.CH2Data.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.CH2Data, "800 creates 1.6 V");
            // 
            // CH1Data
            // 
            this.CH1Data.Location = new System.Drawing.Point(392, 46);
            this.CH1Data.Name = "CH1Data";
            this.CH1Data.Size = new System.Drawing.Size(42, 20);
            this.CH1Data.TabIndex = 139;
            this.CH1Data.Text = "04D9";
            this.CH1Data.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.toolTip1.SetToolTip(this.CH1Data, "4D9 creates 1.0V");
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.RegisterPage1);
            this.tabControl1.Controls.Add(this.RegisterPage2);
            this.tabControl1.Controls.Add(this.RegisterPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 108);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(448, 209);
            this.tabControl1.TabIndex = 102;
            // 
            // RegisterPage1
            // 
            this.RegisterPage1.Controls.Add(this.ReadCheckBox);
            this.RegisterPage1.Controls.Add(this.textBox1);
            this.RegisterPage1.Controls.Add(this.textBox2);
            this.RegisterPage1.Controls.Add(this.Reg12_Test);
            this.RegisterPage1.Controls.Add(this.Reg13Check);
            this.RegisterPage1.Controls.Add(this.Reg11_Shutdown);
            this.RegisterPage1.Controls.Add(this.SendMax2112);
            this.RegisterPage1.Controls.Add(this.label12);
            this.RegisterPage1.Controls.Add(this.label2);
            this.RegisterPage1.Controls.Add(this.label11);
            this.RegisterPage1.Controls.Add(this.Reg1_NDivider);
            this.RegisterPage1.Controls.Add(this.Reg10_Gain);
            this.RegisterPage1.Controls.Add(this.label3);
            this.RegisterPage1.Controls.Add(this.label10);
            this.RegisterPage1.Controls.Add(this.Reg3_ChargePump);
            this.RegisterPage1.Controls.Add(this.label4);
            this.RegisterPage1.Controls.Add(this.Reg9_LowPassFilter);
            this.RegisterPage1.Controls.Add(this.label9);
            this.RegisterPage1.Controls.Add(this.Reg3_FDivider);
            this.RegisterPage1.Controls.Add(this.Reg8_ADCRead);
            this.RegisterPage1.Controls.Add(this.Reg7_PLL);
            this.RegisterPage1.Controls.Add(this.Reg8_ADCLatch);
            this.RegisterPage1.Controls.Add(this.label5);
            this.RegisterPage1.Controls.Add(this.Reg8_VCOAutoselect);
            this.RegisterPage1.Controls.Add(this.label6);
            this.RegisterPage1.Controls.Add(this.Reg8_VCO);
            this.RegisterPage1.Controls.Add(this.Reg6_xtal);
            this.RegisterPage1.Controls.Add(this.label8);
            this.RegisterPage1.Controls.Add(this.Reg6_ref);
            this.RegisterPage1.Controls.Add(this.label7);
            this.RegisterPage1.Location = new System.Drawing.Point(4, 22);
            this.RegisterPage1.Name = "RegisterPage1";
            this.RegisterPage1.Padding = new System.Windows.Forms.Padding(3);
            this.RegisterPage1.Size = new System.Drawing.Size(440, 183);
            this.RegisterPage1.TabIndex = 0;
            this.RegisterPage1.Text = "Max2112";
            this.RegisterPage1.UseVisualStyleBackColor = true;
            // 
            // ReadCheckBox
            // 
            this.ReadCheckBox.AutoSize = true;
            this.ReadCheckBox.Location = new System.Drawing.Point(329, 150);
            this.ReadCheckBox.Name = "ReadCheckBox";
            this.ReadCheckBox.Size = new System.Drawing.Size(105, 17);
            this.ReadCheckBox.TabIndex = 130;
            this.ReadCheckBox.Text = "Read Everything";
            this.ReadCheckBox.UseVisualStyleBackColor = true;
            this.ReadCheckBox.CheckedChanged += new System.EventHandler(this.ReadCheckBox_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 8);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(340, 20);
            this.textBox1.TabIndex = 64;
            this.textBox1.Text = "00 00 00 00 00 00 00 00 00 00 00 00";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(125, 148);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(53, 20);
            this.textBox2.TabIndex = 102;
            this.textBox2.Text = "00 00";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Reg12_Test
            // 
            this.Reg12_Test.Location = new System.Drawing.Point(354, 114);
            this.Reg12_Test.Name = "Reg12_Test";
            this.Reg12_Test.Size = new System.Drawing.Size(38, 20);
            this.Reg12_Test.TabIndex = 129;
            this.Reg12_Test.Text = "08";
            this.Reg12_Test.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Reg13Check
            // 
            this.Reg13Check.AutoSize = true;
            this.Reg13Check.Location = new System.Drawing.Point(6, 150);
            this.Reg13Check.Name = "Reg13Check";
            this.Reg13Check.Size = new System.Drawing.Size(113, 17);
            this.Reg13Check.TabIndex = 101;
            this.Reg13Check.Text = "Read Regs 13, 14";
            this.Reg13Check.UseVisualStyleBackColor = true;
            this.Reg13Check.CheckedChanged += new System.EventHandler(this.Reg13Check_CheckedChanged);
            // 
            // Reg11_Shutdown
            // 
            this.Reg11_Shutdown.Location = new System.Drawing.Point(224, 114);
            this.Reg11_Shutdown.Name = "Reg11_Shutdown";
            this.Reg11_Shutdown.Size = new System.Drawing.Size(38, 20);
            this.Reg11_Shutdown.TabIndex = 128;
            this.Reg11_Shutdown.Text = "00";
            this.Reg11_Shutdown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // SendMax2112
            // 
            this.SendMax2112.Location = new System.Drawing.Point(352, 6);
            this.SendMax2112.Name = "SendMax2112";
            this.SendMax2112.Size = new System.Drawing.Size(75, 23);
            this.SendMax2112.TabIndex = 63;
            this.SendMax2112.Text = "Send";
            this.SendMax2112.UseVisualStyleBackColor = true;
            this.SendMax2112.Click += new System.EventHandler(this.SendMax2112_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(292, 117);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 127;
            this.label12.Text = "Test (Hex)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 104;
            this.label2.Text = "N-Divider (dec)";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(163, 117);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(55, 13);
            this.label11.TabIndex = 126;
            this.label11.Text = "Shutdown";
            // 
            // Reg1_NDivider
            // 
            this.Reg1_NDivider.Location = new System.Drawing.Point(90, 35);
            this.Reg1_NDivider.Name = "Reg1_NDivider";
            this.Reg1_NDivider.Size = new System.Drawing.Size(39, 20);
            this.Reg1_NDivider.TabIndex = 103;
            this.Reg1_NDivider.Text = "40";
            this.Reg1_NDivider.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Reg10_Gain
            // 
            this.Reg10_Gain.Location = new System.Drawing.Point(90, 115);
            this.Reg10_Gain.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.Reg10_Gain.Name = "Reg10_Gain";
            this.Reg10_Gain.Size = new System.Drawing.Size(45, 20);
            this.Reg10_Gain.TabIndex = 125;
            this.Reg10_Gain.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(147, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 105;
            this.label3.Text = "Charge Pump";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 117);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 124;
            this.label10.Text = "Gain";
            // 
            // Reg3_ChargePump
            // 
            this.Reg3_ChargePump.FormattingEnabled = true;
            this.Reg3_ChargePump.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
            this.Reg3_ChargePump.Location = new System.Drawing.Point(224, 35);
            this.Reg3_ChargePump.Name = "Reg3_ChargePump";
            this.Reg3_ChargePump.Size = new System.Drawing.Size(45, 21);
            this.Reg3_ChargePump.TabIndex = 106;
            this.Reg3_ChargePump.Text = "1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(276, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 107;
            this.label4.Text = "F-Divider (dec)";
            // 
            // Reg9_LowPassFilter
            // 
            this.Reg9_LowPassFilter.Location = new System.Drawing.Point(354, 88);
            this.Reg9_LowPassFilter.Name = "Reg9_LowPassFilter";
            this.Reg9_LowPassFilter.Size = new System.Drawing.Size(38, 20);
            this.Reg9_LowPassFilter.TabIndex = 123;
            this.Reg9_LowPassFilter.Text = "89";
            this.Reg9_LowPassFilter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(294, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 122;
            this.label9.Text = "LPF (Hex)";
            // 
            // Reg3_FDivider
            // 
            this.Reg3_FDivider.Location = new System.Drawing.Point(354, 35);
            this.Reg3_FDivider.Name = "Reg3_FDivider";
            this.Reg3_FDivider.Size = new System.Drawing.Size(54, 20);
            this.Reg3_FDivider.TabIndex = 108;
            this.Reg3_FDivider.Text = "327242";
            this.Reg3_FDivider.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Reg8_ADCRead
            // 
            this.Reg8_ADCRead.AutoSize = true;
            this.Reg8_ADCRead.Location = new System.Drawing.Point(234, 90);
            this.Reg8_ADCRead.Name = "Reg8_ADCRead";
            this.Reg8_ADCRead.Size = new System.Drawing.Size(49, 17);
            this.Reg8_ADCRead.TabIndex = 121;
            this.Reg8_ADCRead.Text = "ADR";
            this.Reg8_ADCRead.UseVisualStyleBackColor = true;
            // 
            // Reg7_PLL
            // 
            this.Reg7_PLL.FormattingEnabled = true;
            this.Reg7_PLL.Items.AddRange(new object[] {
            "000",
            "001",
            "010",
            "011",
            "100",
            "101",
            "110",
            "111"});
            this.Reg7_PLL.Location = new System.Drawing.Point(354, 61);
            this.Reg7_PLL.Name = "Reg7_PLL";
            this.Reg7_PLL.Size = new System.Drawing.Size(45, 21);
            this.Reg7_PLL.TabIndex = 116;
            this.Reg7_PLL.Text = "101";
            // 
            // Reg8_ADCLatch
            // 
            this.Reg8_ADCLatch.AutoSize = true;
            this.Reg8_ADCLatch.Location = new System.Drawing.Point(188, 90);
            this.Reg8_ADCLatch.Name = "Reg8_ADCLatch";
            this.Reg8_ADCLatch.Size = new System.Drawing.Size(47, 17);
            this.Reg8_ADCLatch.TabIndex = 120;
            this.Reg8_ADCLatch.Text = "ADL";
            this.Reg8_ADCLatch.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 109;
            this.label5.Text = "XTAL Divider";
            // 
            // Reg8_VCOAutoselect
            // 
            this.Reg8_VCOAutoselect.AutoSize = true;
            this.Reg8_VCOAutoselect.Checked = true;
            this.Reg8_VCOAutoselect.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Reg8_VCOAutoselect.Location = new System.Drawing.Point(142, 90);
            this.Reg8_VCOAutoselect.Name = "Reg8_VCOAutoselect";
            this.Reg8_VCOAutoselect.Size = new System.Drawing.Size(47, 17);
            this.Reg8_VCOAutoselect.TabIndex = 119;
            this.Reg8_VCOAutoselect.Text = "VAS";
            this.Reg8_VCOAutoselect.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(158, 64);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 13);
            this.label6.TabIndex = 110;
            this.label6.Text = "Ref Divider";
            // 
            // Reg8_VCO
            // 
            this.Reg8_VCO.Location = new System.Drawing.Point(90, 89);
            this.Reg8_VCO.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.Reg8_VCO.Name = "Reg8_VCO";
            this.Reg8_VCO.Size = new System.Drawing.Size(45, 20);
            this.Reg8_VCO.TabIndex = 118;
            this.Reg8_VCO.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // Reg6_xtal
            // 
            this.Reg6_xtal.FormattingEnabled = true;
            this.Reg6_xtal.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8"});
            this.Reg6_xtal.Location = new System.Drawing.Point(90, 61);
            this.Reg6_xtal.Name = "Reg6_xtal";
            this.Reg6_xtal.Size = new System.Drawing.Size(45, 21);
            this.Reg6_xtal.TabIndex = 113;
            this.Reg6_xtal.Text = "1";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 13);
            this.label8.TabIndex = 117;
            this.label8.Text = "VCO Control";
            // 
            // Reg6_ref
            // 
            this.Reg6_ref.Location = new System.Drawing.Point(224, 62);
            this.Reg6_ref.Maximum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.Reg6_ref.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.Reg6_ref.Name = "Reg6_ref";
            this.Reg6_ref.Size = new System.Drawing.Size(45, 20);
            this.Reg6_ref.TabIndex = 114;
            this.Reg6_ref.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(286, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 13);
            this.label7.TabIndex = 115;
            this.label7.Text = "PLL Control";
            // 
            // RegisterPage2
            // 
            this.RegisterPage2.Controls.Add(this.label21);
            this.RegisterPage2.Controls.Add(this.MCP_SLA);
            this.RegisterPage2.Controls.Add(this.ReadMCP4728);
            this.RegisterPage2.Controls.Add(this.label20);
            this.RegisterPage2.Controls.Add(this.CH4Data);
            this.RegisterPage2.Controls.Add(this.label18);
            this.RegisterPage2.Controls.Add(this.CH3Data);
            this.RegisterPage2.Controls.Add(this.label16);
            this.RegisterPage2.Controls.Add(this.CH2Data);
            this.RegisterPage2.Controls.Add(this.label14);
            this.RegisterPage2.Controls.Add(this.CH1Data);
            this.RegisterPage2.Controls.Add(this.CH4Checkbox);
            this.RegisterPage2.Controls.Add(this.CH3Checkbox);
            this.RegisterPage2.Controls.Add(this.CH2Checkbox);
            this.RegisterPage2.Controls.Add(this.CH1Checkbox);
            this.RegisterPage2.Controls.Add(this.CH4Gain);
            this.RegisterPage2.Controls.Add(this.label19);
            this.RegisterPage2.Controls.Add(this.CH4PwrMode);
            this.RegisterPage2.Controls.Add(this.CH4Vref);
            this.RegisterPage2.Controls.Add(this.CH3Gain);
            this.RegisterPage2.Controls.Add(this.label17);
            this.RegisterPage2.Controls.Add(this.CH3PwrMode);
            this.RegisterPage2.Controls.Add(this.CH3Vref);
            this.RegisterPage2.Controls.Add(this.CH2Gain);
            this.RegisterPage2.Controls.Add(this.label15);
            this.RegisterPage2.Controls.Add(this.CH2PwrMode);
            this.RegisterPage2.Controls.Add(this.CH2Vref);
            this.RegisterPage2.Controls.Add(this.CH1Gain);
            this.RegisterPage2.Controls.Add(this.label13);
            this.RegisterPage2.Controls.Add(this.CH1PwrMode);
            this.RegisterPage2.Controls.Add(this.CH1Vref);
            this.RegisterPage2.Controls.Add(this.SendMCP4728);
            this.RegisterPage2.Location = new System.Drawing.Point(4, 22);
            this.RegisterPage2.Name = "RegisterPage2";
            this.RegisterPage2.Padding = new System.Windows.Forms.Padding(3);
            this.RegisterPage2.Size = new System.Drawing.Size(440, 183);
            this.RegisterPage2.TabIndex = 1;
            this.RegisterPage2.Text = "MCP4728";
            this.RegisterPage2.UseVisualStyleBackColor = true;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(6, 11);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(90, 13);
            this.label21.TabIndex = 149;
            this.label21.Text = "MCP I2C Address";
            // 
            // MCP_SLA
            // 
            this.MCP_SLA.Location = new System.Drawing.Point(102, 8);
            this.MCP_SLA.Name = "MCP_SLA";
            this.MCP_SLA.Size = new System.Drawing.Size(33, 20);
            this.MCP_SLA.TabIndex = 148;
            this.MCP_SLA.Text = "61";
            this.MCP_SLA.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ReadMCP4728
            // 
            this.ReadMCP4728.AutoSize = true;
            this.ReadMCP4728.Location = new System.Drawing.Point(275, 10);
            this.ReadMCP4728.Name = "ReadMCP4728";
            this.ReadMCP4728.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ReadMCP4728.Size = new System.Drawing.Size(78, 17);
            this.ReadMCP4728.TabIndex = 147;
            this.ReadMCP4728.Text = "Read Data";
            this.ReadMCP4728.UseVisualStyleBackColor = true;
            this.ReadMCP4728.CheckedChanged += new System.EventHandler(this.ReadMCP4728_CheckedChanged);
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(356, 130);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(30, 13);
            this.label20.TabIndex = 146;
            this.label20.Text = "Data";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(356, 103);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(30, 13);
            this.label18.TabIndex = 144;
            this.label18.Text = "Data";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(356, 76);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(30, 13);
            this.label16.TabIndex = 142;
            this.label16.Text = "Data";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(356, 49);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(30, 13);
            this.label14.TabIndex = 140;
            this.label14.Text = "Data";
            // 
            // CH4Checkbox
            // 
            this.CH4Checkbox.AutoSize = true;
            this.CH4Checkbox.Checked = true;
            this.CH4Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CH4Checkbox.Location = new System.Drawing.Point(6, 129);
            this.CH4Checkbox.Name = "CH4Checkbox";
            this.CH4Checkbox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CH4Checkbox.Size = new System.Drawing.Size(53, 17);
            this.CH4Checkbox.TabIndex = 138;
            this.CH4Checkbox.Text = "CH4 :";
            this.CH4Checkbox.UseVisualStyleBackColor = true;
            this.CH4Checkbox.CheckedChanged += new System.EventHandler(this.CH4Checkbox_CheckedChanged);
            // 
            // CH3Checkbox
            // 
            this.CH3Checkbox.AutoSize = true;
            this.CH3Checkbox.Checked = true;
            this.CH3Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CH3Checkbox.Location = new System.Drawing.Point(6, 102);
            this.CH3Checkbox.Name = "CH3Checkbox";
            this.CH3Checkbox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CH3Checkbox.Size = new System.Drawing.Size(53, 17);
            this.CH3Checkbox.TabIndex = 137;
            this.CH3Checkbox.Text = "CH3 :";
            this.CH3Checkbox.UseVisualStyleBackColor = true;
            this.CH3Checkbox.CheckedChanged += new System.EventHandler(this.CH3Checkbox_CheckedChanged);
            // 
            // CH2Checkbox
            // 
            this.CH2Checkbox.AutoSize = true;
            this.CH2Checkbox.Checked = true;
            this.CH2Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CH2Checkbox.Location = new System.Drawing.Point(6, 75);
            this.CH2Checkbox.Name = "CH2Checkbox";
            this.CH2Checkbox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CH2Checkbox.Size = new System.Drawing.Size(53, 17);
            this.CH2Checkbox.TabIndex = 136;
            this.CH2Checkbox.Text = "CH2 :";
            this.CH2Checkbox.UseVisualStyleBackColor = true;
            this.CH2Checkbox.CheckedChanged += new System.EventHandler(this.CH2Checkbox_CheckedChanged);
            // 
            // CH1Checkbox
            // 
            this.CH1Checkbox.AutoSize = true;
            this.CH1Checkbox.Checked = true;
            this.CH1Checkbox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CH1Checkbox.Location = new System.Drawing.Point(6, 48);
            this.CH1Checkbox.Name = "CH1Checkbox";
            this.CH1Checkbox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CH1Checkbox.Size = new System.Drawing.Size(53, 17);
            this.CH1Checkbox.TabIndex = 135;
            this.CH1Checkbox.Text = "CH1 :";
            this.CH1Checkbox.UseVisualStyleBackColor = true;
            this.CH1Checkbox.CheckedChanged += new System.EventHandler(this.CH1Checkbox_CheckedChanged);
            // 
            // CH4Gain
            // 
            this.CH4Gain.AutoSize = true;
            this.CH4Gain.Enabled = false;
            this.CH4Gain.Location = new System.Drawing.Point(277, 129);
            this.CH4Gain.Name = "CH4Gain";
            this.CH4Gain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CH4Gain.Size = new System.Drawing.Size(48, 17);
            this.CH4Gain.TabIndex = 134;
            this.CH4Gain.Text = "Gain";
            this.CH4Gain.UseVisualStyleBackColor = true;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(138, 130);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(66, 13);
            this.label19.TabIndex = 133;
            this.label19.Text = "Power mode";
            // 
            // CH4PwrMode
            // 
            this.CH4PwrMode.FormattingEnabled = true;
            this.CH4PwrMode.Items.AddRange(new object[] {
            "00",
            "01",
            "10",
            "11"});
            this.CH4PwrMode.Location = new System.Drawing.Point(210, 127);
            this.CH4PwrMode.Name = "CH4PwrMode";
            this.CH4PwrMode.Size = new System.Drawing.Size(45, 21);
            this.CH4PwrMode.TabIndex = 132;
            this.CH4PwrMode.Text = "00";
            // 
            // CH4Vref
            // 
            this.CH4Vref.AutoSize = true;
            this.CH4Vref.Location = new System.Drawing.Point(65, 129);
            this.CH4Vref.Name = "CH4Vref";
            this.CH4Vref.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CH4Vref.Size = new System.Drawing.Size(58, 17);
            this.CH4Vref.TabIndex = 131;
            this.CH4Vref.Text = "Int Ref";
            this.CH4Vref.UseVisualStyleBackColor = true;
            this.CH4Vref.CheckedChanged += new System.EventHandler(this.CH4Vref_CheckedChanged);
            // 
            // CH3Gain
            // 
            this.CH3Gain.AutoSize = true;
            this.CH3Gain.Enabled = false;
            this.CH3Gain.Location = new System.Drawing.Point(277, 102);
            this.CH3Gain.Name = "CH3Gain";
            this.CH3Gain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CH3Gain.Size = new System.Drawing.Size(48, 17);
            this.CH3Gain.TabIndex = 129;
            this.CH3Gain.Text = "Gain";
            this.CH3Gain.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(138, 103);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(66, 13);
            this.label17.TabIndex = 128;
            this.label17.Text = "Power mode";
            // 
            // CH3PwrMode
            // 
            this.CH3PwrMode.FormattingEnabled = true;
            this.CH3PwrMode.Items.AddRange(new object[] {
            "00",
            "01",
            "10",
            "11"});
            this.CH3PwrMode.Location = new System.Drawing.Point(210, 100);
            this.CH3PwrMode.Name = "CH3PwrMode";
            this.CH3PwrMode.Size = new System.Drawing.Size(45, 21);
            this.CH3PwrMode.TabIndex = 127;
            this.CH3PwrMode.Text = "00";
            // 
            // CH3Vref
            // 
            this.CH3Vref.AutoSize = true;
            this.CH3Vref.Location = new System.Drawing.Point(65, 102);
            this.CH3Vref.Name = "CH3Vref";
            this.CH3Vref.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CH3Vref.Size = new System.Drawing.Size(58, 17);
            this.CH3Vref.TabIndex = 126;
            this.CH3Vref.Text = "Int Ref";
            this.CH3Vref.UseVisualStyleBackColor = true;
            this.CH3Vref.CheckedChanged += new System.EventHandler(this.CH3Vref_CheckedChanged);
            // 
            // CH2Gain
            // 
            this.CH2Gain.AutoSize = true;
            this.CH2Gain.Enabled = false;
            this.CH2Gain.Location = new System.Drawing.Point(277, 75);
            this.CH2Gain.Name = "CH2Gain";
            this.CH2Gain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CH2Gain.Size = new System.Drawing.Size(48, 17);
            this.CH2Gain.TabIndex = 124;
            this.CH2Gain.Text = "Gain";
            this.CH2Gain.UseVisualStyleBackColor = true;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(138, 76);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 13);
            this.label15.TabIndex = 123;
            this.label15.Text = "Power mode";
            // 
            // CH2PwrMode
            // 
            this.CH2PwrMode.FormattingEnabled = true;
            this.CH2PwrMode.Items.AddRange(new object[] {
            "00",
            "01",
            "10",
            "11"});
            this.CH2PwrMode.Location = new System.Drawing.Point(210, 73);
            this.CH2PwrMode.Name = "CH2PwrMode";
            this.CH2PwrMode.Size = new System.Drawing.Size(45, 21);
            this.CH2PwrMode.TabIndex = 122;
            this.CH2PwrMode.Text = "00";
            // 
            // CH2Vref
            // 
            this.CH2Vref.AutoSize = true;
            this.CH2Vref.Location = new System.Drawing.Point(65, 75);
            this.CH2Vref.Name = "CH2Vref";
            this.CH2Vref.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CH2Vref.Size = new System.Drawing.Size(58, 17);
            this.CH2Vref.TabIndex = 121;
            this.CH2Vref.Text = "Int Ref";
            this.CH2Vref.UseVisualStyleBackColor = true;
            this.CH2Vref.CheckedChanged += new System.EventHandler(this.CH2Vref_CheckedChanged);
            // 
            // CH1Gain
            // 
            this.CH1Gain.AutoSize = true;
            this.CH1Gain.Enabled = false;
            this.CH1Gain.Location = new System.Drawing.Point(277, 48);
            this.CH1Gain.Name = "CH1Gain";
            this.CH1Gain.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CH1Gain.Size = new System.Drawing.Size(48, 17);
            this.CH1Gain.TabIndex = 119;
            this.CH1Gain.Text = "Gain";
            this.CH1Gain.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(138, 49);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(66, 13);
            this.label13.TabIndex = 118;
            this.label13.Text = "Power mode";
            // 
            // CH1PwrMode
            // 
            this.CH1PwrMode.FormattingEnabled = true;
            this.CH1PwrMode.Items.AddRange(new object[] {
            "00",
            "01",
            "10",
            "11"});
            this.CH1PwrMode.Location = new System.Drawing.Point(210, 46);
            this.CH1PwrMode.Name = "CH1PwrMode";
            this.CH1PwrMode.Size = new System.Drawing.Size(45, 21);
            this.CH1PwrMode.TabIndex = 117;
            this.CH1PwrMode.Text = "00";
            // 
            // CH1Vref
            // 
            this.CH1Vref.AutoSize = true;
            this.CH1Vref.Location = new System.Drawing.Point(65, 48);
            this.CH1Vref.Name = "CH1Vref";
            this.CH1Vref.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.CH1Vref.Size = new System.Drawing.Size(58, 17);
            this.CH1Vref.TabIndex = 69;
            this.CH1Vref.Text = "Int Ref";
            this.CH1Vref.UseVisualStyleBackColor = true;
            this.CH1Vref.CheckedChanged += new System.EventHandler(this.CH1Vref_CheckedChanged);
            // 
            // SendMCP4728
            // 
            this.SendMCP4728.Location = new System.Drawing.Point(359, 6);
            this.SendMCP4728.Name = "SendMCP4728";
            this.SendMCP4728.Size = new System.Drawing.Size(75, 23);
            this.SendMCP4728.TabIndex = 64;
            this.SendMCP4728.Text = "Send";
            this.SendMCP4728.UseVisualStyleBackColor = true;
            this.SendMCP4728.Click += new System.EventHandler(this.SendMCP4728_Click);
            // 
            // checkASIP
            // 
            this.checkASIP.AutoSize = true;
            this.checkASIP.Location = new System.Drawing.Point(370, 108);
            this.checkASIP.Name = "checkASIP";
            this.checkASIP.Size = new System.Drawing.Size(88, 17);
            this.checkASIP.TabIndex = 103;
            this.checkASIP.Text = "ASIP Version";
            this.checkASIP.UseVisualStyleBackColor = true;
            // 
            // RegisterPage3
            // 
            this.RegisterPage3.Controls.Add(this.label22);
            this.RegisterPage3.Controls.Add(this.SendLPC);
            this.RegisterPage3.Controls.Add(this.DACvoltage);
            this.RegisterPage3.Controls.Add(this.DACBar);
            this.RegisterPage3.Location = new System.Drawing.Point(4, 22);
            this.RegisterPage3.Name = "RegisterPage3";
            this.RegisterPage3.Size = new System.Drawing.Size(440, 183);
            this.RegisterPage3.TabIndex = 2;
            this.RegisterPage3.Text = "LPC Control";
            this.RegisterPage3.UseVisualStyleBackColor = true;
            // 
            // DACBar
            // 
            this.DACBar.Location = new System.Drawing.Point(39, 10);
            this.DACBar.Maximum = 1033;
            this.DACBar.Minimum = 220;
            this.DACBar.Name = "DACBar";
            this.DACBar.Size = new System.Drawing.Size(240, 19);
            this.DACBar.TabIndex = 0;
            this.DACBar.Value = 722;
            this.DACBar.ValueChanged += new System.EventHandler(this.DACBar_ValueChanged);
            // 
            // DACvoltage
            // 
            this.DACvoltage.AutoSize = true;
            this.DACvoltage.Location = new System.Drawing.Point(282, 13);
            this.DACvoltage.Name = "DACvoltage";
            this.DACvoltage.Size = new System.Drawing.Size(32, 13);
            this.DACvoltage.TabIndex = 1;
            this.DACvoltage.Text = "1.6 V";
            // 
            // SendLPC
            // 
            this.SendLPC.Location = new System.Drawing.Point(354, 8);
            this.SendLPC.Name = "SendLPC";
            this.SendLPC.Size = new System.Drawing.Size(75, 23);
            this.SendLPC.TabIndex = 64;
            this.SendLPC.Text = "Send";
            this.SendLPC.UseVisualStyleBackColor = true;
            this.SendLPC.Click += new System.EventHandler(this.SendLPC_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(3, 13);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(29, 13);
            this.label22.TabIndex = 65;
            this.label22.Text = "DAC";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 430);
            this.Controls.Add(this.checkASIP);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gbPortSettings);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(480, 34);
            this.Name = "MainForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "Maxim Control";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.gbPortSettings.ResumeLayout(false);
            this.gbPortSettings.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.RegisterPage1.ResumeLayout(false);
            this.RegisterPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Reg10_Gain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Reg8_VCO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Reg6_ref)).EndInit();
            this.RegisterPage2.ResumeLayout(false);
            this.RegisterPage2.PerformLayout();
            this.RegisterPage3.ResumeLayout(false);
            this.RegisterPage3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion


		private USBDeviceList usbDevices;
		private CyHidDevice[] dongle = new CyHidDevice[8];
        private CyHidDevice[] dongle2 = new CyHidDevice[8];
        private String[] AccelDeviceList = new String[8];
        private String[] DACDeviceList = new String[8];

		public void
		PnP_Event_Handler(IntPtr pnpEvent, IntPtr hRemovedDevice)
		{
            Boolean haveDAC = false;
            Boolean haveAccel = false;
            String omsg;

			if (pnpEvent.Equals(CyConst.DBT_DEVICEREMOVECOMPLETE))
			{
                Boolean removedDAC = true;
                Boolean removedAccel = true;
				//usbDevices.Remove(hRemovedDevice);

                for (int n = 0; n < 8; n++)
                {
                    foreach (USBDevice dev in usbDevices)
                    {
                        if (dev.VendorID == 0x13A3 && dev.ProductID == 0x0004)
                        {
                            if (DACDeviceList[n] == dev.SerialNumber)
                                haveDAC = true;
                        }
                        if (dev.VendorID == 0x13A3 && dev.ProductID == 0x0005)
                        {
                            if (AccelDeviceList[n] == dev.SerialNumber)
                                haveAccel = true;
                        }
                    }
                }
                usbDevices.Remove(hRemovedDevice);

                for (int n = 0; n < 8; n++)
                {
                    removedAccel = true;
                    removedDAC = true;

                    foreach (USBDevice dev in usbDevices)
                    {
                        if (dev.ProductID == 0x0004)
                        {
                            if (DACDeviceList[n] == dev.SerialNumber)
                                removedDAC = false;
                        }
                        if (dev.ProductID == 0x0005)
                        {
                            if (AccelDeviceList[n] == dev.SerialNumber)
                                removedAccel = false;
                        }
                    }
                    if (removedAccel && haveAccel)
                    {
                        Output.AppendText("Accelerometer Removed\r\n");
                        break;
                    }
                    if (removedDAC && haveDAC)
                    {
                        Output.AppendText("DAC Removed\r\n");
                        break;
                    }
                }
                //RefreshDeviceList();
			}

			if (pnpEvent.Equals(CyConst.DBT_DEVICEARRIVAL))
			{
                usbDevices.Add();

                foreach (CyHidDevice dev in usbDevices)
                {
                    for (int n = 0; n < 8; n++)
                    {
                        if (dev.VendorID == 0x13A3 && dev.ProductID == 0x0005)
                        {
                            if (AccelDeviceList[n] == dev.SerialNumber)
                                break;
                            if (AccelDeviceList[n] == null)
                            {
                                omsg = "Attached Device: " + dev.Product + "\r\n";
                                Output.AppendText(omsg);
                                break;
                            }
                            omsg = "The Accelerometer device list is full.\rRemove another device first or reset the program.\r\n";
                            Output.AppendText(omsg);//if this ever gets called I will be surprised
                        }
                        if (dev.VendorID == 0x13A3 && dev.ProductID == 0x0004)
                        {
                            if (DACDeviceList[n] == dev.SerialNumber)
                                break;
                            if (DACDeviceList[n] == null)
                            {
                                omsg = "Attached Device: " + dev.Product + "\r\n";
                                Output.AppendText(omsg);
                                break;
                            }
                            omsg = "The DAC device list is full.\rRemove another device first or reset the program.\r\n";
                            Output.AppendText(omsg);
                        }
                    }
                }
                //RefreshDeviceList();
			}
		}

        /*private void
        RefreshDeviceList()
        {
            for (int i = 0; i < 8; i++)
            {//Reset all device lists
                dongle[i] = null;
                dongle2[i] = null;
                AccelDeviceList[i] = null;
                DACDeviceList[i] = null;
            }
            AccelList.Items.Clear();
            DACList.Items.Clear();

            foreach (CyHidDevice dev in usbDevices)
            {
                for (int n = 0; n < 8; n++)
                {
                    if (dev.VendorID == 0x13A3 && dev.ProductID == 0x0005 && dongle2[n] == null)
                    {
                        dongle2[n] = dev;       //re-write list of attached Accel
                        AccelDeviceList[n] = dev.SerialNumber;
                        AccelList.Items.Add(dev.SerialNumber);
                        break;
                    }
                    if (dev.VendorID == 0x13A3 && dev.ProductID == 0x0004 && dongle[n] == null)
                    {
                        dongle[n] = dev;
                        DACDeviceList[n] = dev.SerialNumber;
                        DACList.Items.Add(dev.SerialNumber);
                        break;
                    }
                }
            }
            if (!AccelList.Items.Contains(AccelList.Text))  //if the currently selected device no longer exists..
                AccelList.Text = "Unit List";               //reset the selection
            if (!DACList.Items.Contains(DACList.Text))
                DACList.Text = "Unit List";
        }*/

		private void
		HID_Scan()
		{
			if (usbDevices != null) usbDevices.Dispose();

			usbDevices = new USBDeviceList(CyConst.DEVICES_HID, evHandler);
            //AccelList.Items.Clear();
            //DACList.Items.Clear();

			foreach (USBDevice dev in usbDevices)
			{
				if (dev.VendorID == 0x13A3)
				{
					Output.Text += "Attached device: ";
					Output.Text += dev.Product;
					Output.Text += "\r\n";

                    if (dev.VendorID == 0x13A3 && dev.ProductID == 0x0004)
					{
                        for (int l = 0; l < dongle.Length; l++)
                        {
                            if (dongle[l] == null)
                            {
                                dongle[l] = dev as CyHidDevice;
                                DACDeviceList[l] = dev.SerialNumber;
                                //DACList.Items.Add(dev.SerialNumber);
                                break;
                            }
                        }
					}
                    if (dev.VendorID == 0x13A3 && dev.ProductID == 0x0005)
                    {
                        for (int m = 0; m < dongle2.Length; m++)
                        {
                            if (dongle2[m] == null)
                            {
                                dongle2[m] = dev as CyHidDevice;
                                AccelDeviceList[m] = dev.SerialNumber;
                                //AccelList.Items.Add(dev.SerialNumber);
                                break;
                            }
                        }
                    }
				}
			}
		}
		private MenuStrip menuStrip1;
		private ToolStripMenuItem fileToolStripMenuItem;
		private ToolStripMenuItem exitToolStripMenuItem;
		public OpenFileDialog openFileDialog1;
		private ToolStripMenuItem aboutToolStripMenuItem;
		private ToolStripMenuItem aboutToolStripMenuItem1;
		public SaveFileDialog saveFileDialog;
		private TextBox Output;
		private ToolStripMenuItem readMeToolStripMenuItem;
        public Timer timer1;
        private GroupBox gbPortSettings;
        private ComboBox cmbPortName;
        private ComboBox cmbBaudRate;
        private ComboBox cmbStopBits;
        private ComboBox cmbParity;
        private ComboBox cmbDataBits;
        private Button btnOpenPort;
        private Label lblComPort;
        private Label lblStopBits;
        private Label lblBaudRate;
        private Label lblDataBits;
        private Label label1;
        private ToolTip toolTip1;
        private TabControl tabControl1;
        private Button SendMax2112;
        private TextBox textBox1;
        private CheckBox Reg13Check;
        private TextBox textBox2;
        private TextBox Reg1_NDivider;
        private Label label2;
        private Label label3;
        private ComboBox Reg3_ChargePump;
        private Label label4;
        private TextBox Reg3_FDivider;
        private Label label5;
        private Label label6;
        private ComboBox Reg6_xtal;
        private NumericUpDown Reg6_ref;
        private Label label7;
        private ComboBox Reg7_PLL;
        private Label label8;
        private NumericUpDown Reg8_VCO;
        private CheckBox Reg8_VCOAutoselect;
        private CheckBox Reg8_ADCLatch;
        private CheckBox Reg8_ADCRead;
        private Label label9;
        private TextBox Reg9_LowPassFilter;
        private Label label10;
        private NumericUpDown Reg10_Gain;
        private Label label11;
        private Label label12;
        private TextBox Reg11_Shutdown;
        private TextBox Reg12_Test;
        private CheckBox ReadCheckBox;
        private Button SendMCP4728;
        private CheckBox CH1Vref;
        private CheckBox CH1Gain;
        private Label label13;
        private ComboBox CH1PwrMode;
        private CheckBox CH4Gain;
        private Label label19;
        private ComboBox CH4PwrMode;
        private CheckBox CH4Vref;
        private CheckBox CH3Gain;
        private Label label17;
        private ComboBox CH3PwrMode;
        private CheckBox CH3Vref;
        private CheckBox CH2Gain;
        private Label label15;
        private ComboBox CH2PwrMode;
        private CheckBox CH2Vref;
        private CheckBox CH4Checkbox;
        private CheckBox CH3Checkbox;
        private CheckBox CH2Checkbox;
        private CheckBox CH1Checkbox;
        private TextBox CH1Data;
        private Label label14;
        private Label label20;
        private TextBox CH4Data;
        private Label label18;
        private TextBox CH3Data;
        private Label label16;
        private TextBox CH2Data;
        private CheckBox ReadMCP4728;
        private Label label21;
        private TextBox MCP_SLA;
        private CheckBox checkASIP;
        private TabPage RegisterPage1;
        private TabPage RegisterPage2;
        private TabPage RegisterPage3;
        private Label DACvoltage;
        private HScrollBar DACBar;
        private Button SendLPC;
        private Label label22;


	}
}

