namespace SV_COM_425
{
  partial class frmTerminal
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

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
		this.rtfTerminal = new System.Windows.Forms.RichTextBox();
		this.cmbPortName = new System.Windows.Forms.ComboBox();
		this.cmbBaudRate = new System.Windows.Forms.ComboBox();
		this.lblComPort = new System.Windows.Forms.Label();
		this.lblBaudRate = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbParity = new System.Windows.Forms.ComboBox();
		this.lblDataBits = new System.Windows.Forms.Label();
		this.cmbDataBits = new System.Windows.Forms.ComboBox();
		this.lblStopBits = new System.Windows.Forms.Label();
		this.cmbStopBits = new System.Windows.Forms.ComboBox();
		this.btnOpenPort = new System.Windows.Forms.Button();
		this.gbPortSettings = new System.Windows.Forms.GroupBox();
		this.btnClear = new System.Windows.Forms.Button();
		this.gbVolume = new System.Windows.Forms.GroupBox();
		this.TxTest = new System.Windows.Forms.Button();
		this.chkUnsquelch = new System.Windows.Forms.CheckBox();
		this.pbSquelchVolume = new System.Windows.Forms.ProgressBar();
		this.label3 = new System.Windows.Forms.Label();
		this.Squelch = new System.Windows.Forms.NumericUpDown();
		this.pbMicVolume = new System.Windows.Forms.ProgressBar();
		this.lMic = new System.Windows.Forms.Label();
		this.MicGain = new System.Windows.Forms.NumericUpDown();
		this.pbSidetoneVolume = new System.Windows.Forms.ProgressBar();
		this.pbRadioVolume = new System.Windows.Forms.ProgressBar();
		this.lblSidetone = new System.Windows.Forms.Label();
		this.Sidetone = new System.Windows.Forms.NumericUpDown();
		this.lblVHFVolume = new System.Windows.Forms.Label();
		this.Volume = new System.Windows.Forms.NumericUpDown();
		this.chkDualWatch = new System.Windows.Forms.CheckBox();
		this.gbChannels25 = new System.Windows.Forms.GroupBox();
		this.toggle = new System.Windows.Forms.Button();
		this.SecondaryFreq = new System.Windows.Forms.NumericUpDown();
		this.PrimaryFreq = new System.Windows.Forms.NumericUpDown();
		this.lblfreq1 = new System.Windows.Forms.Label();
		this.lblfreq2 = new System.Windows.Forms.Label();
		this.gbCmds = new System.Windows.Forms.GroupBox();
		this.lRSSI = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.tbTemp = new System.Windows.Forms.Label();
		this.lbVbat = new System.Windows.Forms.Label();
		this.pbRSSI = new System.Windows.Forms.ProgressBar();
		this.lblRSSI = new System.Windows.Forms.Label();
		this.lblTemp = new System.Windows.Forms.Label();
		this.tbPTT = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.lTxTimer = new System.Windows.Forms.Label();
		this.lVersionString = new System.Windows.Forms.Label();
		this.btnDwnld = new System.Windows.Forms.Button();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.timer2 = new System.Windows.Forms.Timer(this.components);
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.serialNumber = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
		this.label8 = new System.Windows.Forms.Label();
		this.address = new System.Windows.Forms.TextBox();
		this.dataValue = new System.Windows.Forms.Label();
		this.gbPortSettings.SuspendLayout();
		this.gbVolume.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)(this.Squelch)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.MicGain)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.Sidetone)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.Volume)).BeginInit();
		this.gbChannels25.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)(this.SecondaryFreq)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.PrimaryFreq)).BeginInit();
		this.gbCmds.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.SuspendLayout();
		// 
		// rtfTerminal
		// 
		this.rtfTerminal.Location = new System.Drawing.Point(4, 2);
		this.rtfTerminal.Name = "rtfTerminal";
		this.rtfTerminal.Size = new System.Drawing.Size(499, 140);
		this.rtfTerminal.TabIndex = 0;
		this.rtfTerminal.Text = "";
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
		this.cmbPortName.Location = new System.Drawing.Point(6, 68);
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
		this.cmbBaudRate.Location = new System.Drawing.Point(6, 108);
		this.cmbBaudRate.Name = "cmbBaudRate";
		this.cmbBaudRate.Size = new System.Drawing.Size(84, 21);
		this.cmbBaudRate.TabIndex = 3;
		this.cmbBaudRate.Validating += new System.ComponentModel.CancelEventHandler(this.cmbBaudRate_Validating);
		// 
		// lblComPort
		// 
		this.lblComPort.AutoSize = true;
		this.lblComPort.Location = new System.Drawing.Point(5, 52);
		this.lblComPort.Name = "lblComPort";
		this.lblComPort.Size = new System.Drawing.Size(56, 13);
		this.lblComPort.TabIndex = 0;
		this.lblComPort.Text = "COM Port:";
		// 
		// lblBaudRate
		// 
		this.lblBaudRate.AutoSize = true;
		this.lblBaudRate.Location = new System.Drawing.Point(6, 92);
		this.lblBaudRate.Name = "lblBaudRate";
		this.lblBaudRate.Size = new System.Drawing.Size(61, 13);
		this.lblBaudRate.TabIndex = 2;
		this.lblBaudRate.Text = "Baud Rate:";
		// 
		// label1
		// 
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(9, 133);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(36, 13);
		this.label1.TabIndex = 4;
		this.label1.Text = "Parity:";
		// 
		// cmbParity
		// 
		this.cmbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbParity.FormattingEnabled = true;
		this.cmbParity.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd"});
		this.cmbParity.Location = new System.Drawing.Point(6, 149);
		this.cmbParity.Name = "cmbParity";
		this.cmbParity.Size = new System.Drawing.Size(84, 21);
		this.cmbParity.TabIndex = 5;
		// 
		// lblDataBits
		// 
		this.lblDataBits.AutoSize = true;
		this.lblDataBits.Location = new System.Drawing.Point(6, 170);
		this.lblDataBits.Name = "lblDataBits";
		this.lblDataBits.Size = new System.Drawing.Size(53, 13);
		this.lblDataBits.TabIndex = 6;
		this.lblDataBits.Text = "Data Bits:";
		// 
		// cmbDataBits
		// 
		this.cmbDataBits.FormattingEnabled = true;
		this.cmbDataBits.Items.AddRange(new object[] {
            "7",
            "8",
            "9"});
		this.cmbDataBits.Location = new System.Drawing.Point(6, 186);
		this.cmbDataBits.Name = "cmbDataBits";
		this.cmbDataBits.Size = new System.Drawing.Size(84, 21);
		this.cmbDataBits.TabIndex = 7;
		this.cmbDataBits.Validating += new System.ComponentModel.CancelEventHandler(this.cmbDataBits_Validating);
		// 
		// lblStopBits
		// 
		this.lblStopBits.AutoSize = true;
		this.lblStopBits.Location = new System.Drawing.Point(295, 19);
		this.lblStopBits.Name = "lblStopBits";
		this.lblStopBits.Size = new System.Drawing.Size(52, 13);
		this.lblStopBits.TabIndex = 8;
		this.lblStopBits.Text = "Stop Bits:";
		// 
		// cmbStopBits
		// 
		this.cmbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cmbStopBits.FormattingEnabled = true;
		this.cmbStopBits.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
		this.cmbStopBits.Location = new System.Drawing.Point(293, 35);
		this.cmbStopBits.Name = "cmbStopBits";
		this.cmbStopBits.Size = new System.Drawing.Size(65, 21);
		this.cmbStopBits.TabIndex = 9;
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
		this.gbPortSettings.Location = new System.Drawing.Point(509, 142);
		this.gbPortSettings.Name = "gbPortSettings";
		this.gbPortSettings.Size = new System.Drawing.Size(98, 219);
		this.gbPortSettings.TabIndex = 4;
		this.gbPortSettings.TabStop = false;
		this.gbPortSettings.Text = "COM  Port";
		// 
		// btnClear
		// 
		this.btnClear.Location = new System.Drawing.Point(509, 12);
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(96, 30);
		this.btnClear.TabIndex = 9;
		this.btnClear.Text = "&Clear";
		this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
		// 
		// gbVolume
		// 
		this.gbVolume.Controls.Add(this.TxTest);
		this.gbVolume.Controls.Add(this.chkUnsquelch);
		this.gbVolume.Controls.Add(this.pbSquelchVolume);
		this.gbVolume.Controls.Add(this.label3);
		this.gbVolume.Controls.Add(this.Squelch);
		this.gbVolume.Controls.Add(this.pbMicVolume);
		this.gbVolume.Controls.Add(this.lMic);
		this.gbVolume.Controls.Add(this.MicGain);
		this.gbVolume.Controls.Add(this.pbSidetoneVolume);
		this.gbVolume.Controls.Add(this.pbRadioVolume);
		this.gbVolume.Controls.Add(this.lblSidetone);
		this.gbVolume.Controls.Add(this.Sidetone);
		this.gbVolume.Controls.Add(this.lblVHFVolume);
		this.gbVolume.Controls.Add(this.Volume);
		this.gbVolume.Location = new System.Drawing.Point(271, 247);
		this.gbVolume.Name = "gbVolume";
		this.gbVolume.Size = new System.Drawing.Size(232, 114);
		this.gbVolume.TabIndex = 21;
		this.gbVolume.TabStop = false;
		this.gbVolume.Text = "Volumes";
		// 
		// TxTest
		// 
		this.TxTest.Location = new System.Drawing.Point(68, 12);
		this.TxTest.Margin = new System.Windows.Forms.Padding(0);
		this.TxTest.Name = "TxTest";
		this.TxTest.Size = new System.Drawing.Size(99, 20);
		this.TxTest.TabIndex = 87;
		this.TxTest.Text = "Test";
		this.TxTest.Click += new System.EventHandler(this.TxTest_Click);
		// 
		// chkUnsquelch
		// 
		this.chkUnsquelch.AutoSize = true;
		this.chkUnsquelch.Location = new System.Drawing.Point(173, 12);
		this.chkUnsquelch.Name = "chkUnsquelch";
		this.chkUnsquelch.Size = new System.Drawing.Size(40, 17);
		this.chkUnsquelch.TabIndex = 25;
		this.chkUnsquelch.Text = "Off";
		this.chkUnsquelch.UseVisualStyleBackColor = true;
		this.chkUnsquelch.CheckedChanged += new System.EventHandler(this.chkUnsquelch_CheckedChanged);
		// 
		// pbSquelchVolume
		// 
		this.pbSquelchVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.pbSquelchVolume.Location = new System.Drawing.Point(173, 80);
		this.pbSquelchVolume.Maximum = 255;
		this.pbSquelchVolume.Name = "pbSquelchVolume";
		this.pbSquelchVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.pbSquelchVolume.Size = new System.Drawing.Size(47, 18);
		this.pbSquelchVolume.Step = 1;
		this.pbSquelchVolume.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.pbSquelchVolume.TabIndex = 41;
		this.pbSquelchVolume.Value = 1;
		// 
		// label3
		// 
		this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(170, 32);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(46, 13);
		this.label3.TabIndex = 40;
		this.label3.Text = "Squelch";
		// 
		// Squelch
		// 
		this.Squelch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.Squelch.BackColor = System.Drawing.SystemColors.Info;
		this.Squelch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.Squelch.Location = new System.Drawing.Point(173, 51);
		this.Squelch.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
		this.Squelch.Name = "Squelch";
		this.Squelch.Size = new System.Drawing.Size(47, 20);
		this.Squelch.TabIndex = 35;
		this.Squelch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.Squelch.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
		this.Squelch.ValueChanged += new System.EventHandler(this.Squelch_ValueChanged);
		// 
		// pbMicVolume
		// 
		this.pbMicVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.pbMicVolume.Location = new System.Drawing.Point(120, 80);
		this.pbMicVolume.Maximum = 255;
		this.pbMicVolume.Name = "pbMicVolume";
		this.pbMicVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.pbMicVolume.Size = new System.Drawing.Size(47, 18);
		this.pbMicVolume.Step = 1;
		this.pbMicVolume.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.pbMicVolume.TabIndex = 39;
		this.pbMicVolume.Value = 32;
		// 
		// lMic
		// 
		this.lMic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lMic.AutoSize = true;
		this.lMic.Location = new System.Drawing.Point(130, 33);
		this.lMic.Name = "lMic";
		this.lMic.Size = new System.Drawing.Size(27, 13);
		this.lMic.TabIndex = 38;
		this.lMic.Text = "Mic:";
		// 
		// MicGain
		// 
		this.MicGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.MicGain.BackColor = System.Drawing.SystemColors.Info;
		this.MicGain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.MicGain.Enabled = false;
		this.MicGain.Location = new System.Drawing.Point(120, 51);
		this.MicGain.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
		this.MicGain.Name = "MicGain";
		this.MicGain.Size = new System.Drawing.Size(47, 20);
		this.MicGain.TabIndex = 37;
		this.MicGain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.MicGain.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
		this.MicGain.ValueChanged += new System.EventHandler(this.SetMic_ValueChanged);
		// 
		// pbSidetoneVolume
		// 
		this.pbSidetoneVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.pbSidetoneVolume.Location = new System.Drawing.Point(67, 80);
		this.pbSidetoneVolume.Maximum = 255;
		this.pbSidetoneVolume.Name = "pbSidetoneVolume";
		this.pbSidetoneVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.pbSidetoneVolume.Size = new System.Drawing.Size(47, 18);
		this.pbSidetoneVolume.Step = 1;
		this.pbSidetoneVolume.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.pbSidetoneVolume.TabIndex = 36;
		this.pbSidetoneVolume.Value = 64;
		// 
		// pbRadioVolume
		// 
		this.pbRadioVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.pbRadioVolume.Location = new System.Drawing.Point(14, 79);
		this.pbRadioVolume.Maximum = 255;
		this.pbRadioVolume.Name = "pbRadioVolume";
		this.pbRadioVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.pbRadioVolume.Size = new System.Drawing.Size(47, 18);
		this.pbRadioVolume.Step = 1;
		this.pbRadioVolume.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.pbRadioVolume.TabIndex = 34;
		this.pbRadioVolume.Value = 64;
		// 
		// lblSidetone
		// 
		this.lblSidetone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblSidetone.AutoSize = true;
		this.lblSidetone.Location = new System.Drawing.Point(65, 33);
		this.lblSidetone.Name = "lblSidetone";
		this.lblSidetone.Size = new System.Drawing.Size(52, 13);
		this.lblSidetone.TabIndex = 25;
		this.lblSidetone.Text = "Sidetone:";
		// 
		// Sidetone
		// 
		this.Sidetone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.Sidetone.BackColor = System.Drawing.SystemColors.Info;
		this.Sidetone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.Sidetone.Enabled = false;
		this.Sidetone.Location = new System.Drawing.Point(67, 51);
		this.Sidetone.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
		this.Sidetone.Name = "Sidetone";
		this.Sidetone.Size = new System.Drawing.Size(47, 20);
		this.Sidetone.TabIndex = 24;
		this.Sidetone.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.Sidetone.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
		this.Sidetone.ValueChanged += new System.EventHandler(this.SetSidetone_ValueChanged);
		// 
		// lblVHFVolume
		// 
		this.lblVHFVolume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblVHFVolume.AutoSize = true;
		this.lblVHFVolume.Location = new System.Drawing.Point(20, 32);
		this.lblVHFVolume.Name = "lblVHFVolume";
		this.lblVHFVolume.Size = new System.Drawing.Size(38, 13);
		this.lblVHFVolume.TabIndex = 23;
		this.lblVHFVolume.Text = "Radio:";
		// 
		// Volume
		// 
		this.Volume.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.Volume.BackColor = System.Drawing.SystemColors.Info;
		this.Volume.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.Volume.Location = new System.Drawing.Point(14, 50);
		this.Volume.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
		this.Volume.Name = "Volume";
		this.Volume.Size = new System.Drawing.Size(47, 20);
		this.Volume.TabIndex = 22;
		this.Volume.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.Volume.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
		this.Volume.ValueChanged += new System.EventHandler(this.SetVHFVolume_ValueChanged);
		// 
		// chkDualWatch
		// 
		this.chkDualWatch.AutoSize = true;
		this.chkDualWatch.Location = new System.Drawing.Point(14, 27);
		this.chkDualWatch.Name = "chkDualWatch";
		this.chkDualWatch.Size = new System.Drawing.Size(80, 17);
		this.chkDualWatch.TabIndex = 25;
		this.chkDualWatch.Text = "DualWatch";
		this.chkDualWatch.UseVisualStyleBackColor = true;
		this.chkDualWatch.CheckedChanged += new System.EventHandler(this.chkDualWatch_CheckedChanged);
		// 
		// gbChannels25
		// 
		this.gbChannels25.Controls.Add(this.toggle);
		this.gbChannels25.Controls.Add(this.SecondaryFreq);
		this.gbChannels25.Controls.Add(this.chkDualWatch);
		this.gbChannels25.Controls.Add(this.PrimaryFreq);
		this.gbChannels25.Controls.Add(this.lblfreq1);
		this.gbChannels25.Controls.Add(this.lblfreq2);
		this.gbChannels25.Location = new System.Drawing.Point(4, 247);
		this.gbChannels25.Name = "gbChannels25";
		this.gbChannels25.Size = new System.Drawing.Size(261, 114);
		this.gbChannels25.TabIndex = 27;
		this.gbChannels25.TabStop = false;
		this.gbChannels25.Text = "VHF Channels";
		// 
		// toggle
		// 
		this.toggle.Location = new System.Drawing.Point(142, 25);
		this.toggle.Name = "toggle";
		this.toggle.Size = new System.Drawing.Size(46, 21);
		this.toggle.TabIndex = 95;
		this.toggle.Text = "<--->";
		this.toggle.Click += new System.EventHandler(this.toggle_Click);
		// 
		// SecondaryFreq
		// 
		this.SecondaryFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.SecondaryFreq.BackColor = System.Drawing.Color.LightGray;
		this.SecondaryFreq.DecimalPlaces = 3;
		this.SecondaryFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.SecondaryFreq.Increment = new decimal(new int[] {
            25,
            0,
            0,
            196608});
		this.SecondaryFreq.Location = new System.Drawing.Point(137, 65);
		this.SecondaryFreq.Maximum = new decimal(new int[] {
            136975,
            0,
            0,
            196608});
		this.SecondaryFreq.Minimum = new decimal(new int[] {
            118000,
            0,
            0,
            196608});
		this.SecondaryFreq.Name = "SecondaryFreq";
		this.SecondaryFreq.Size = new System.Drawing.Size(115, 32);
		this.SecondaryFreq.TabIndex = 32;
		this.SecondaryFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.SecondaryFreq.Value = new decimal(new int[] {
            118000,
            0,
            0,
            196608});
		this.SecondaryFreq.Click += new System.EventHandler(this.SecondaryFreq_Changed);
		// 
		// PrimaryFreq
		// 
		this.PrimaryFreq.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.PrimaryFreq.BackColor = System.Drawing.Color.LightGray;
		this.PrimaryFreq.DecimalPlaces = 3;
		this.PrimaryFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.PrimaryFreq.Increment = new decimal(new int[] {
            25,
            0,
            0,
            196608});
		this.PrimaryFreq.Location = new System.Drawing.Point(8, 65);
		this.PrimaryFreq.Maximum = new decimal(new int[] {
            136975,
            0,
            0,
            196608});
		this.PrimaryFreq.Minimum = new decimal(new int[] {
            118000,
            0,
            0,
            196608});
		this.PrimaryFreq.Name = "PrimaryFreq";
		this.PrimaryFreq.Size = new System.Drawing.Size(112, 32);
		this.PrimaryFreq.TabIndex = 31;
		this.PrimaryFreq.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.PrimaryFreq.Value = new decimal(new int[] {
            118000,
            0,
            0,
            196608});
		this.PrimaryFreq.Click += new System.EventHandler(this.PrimaryFreq_Click);
		// 
		// lblfreq1
		// 
		this.lblfreq1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblfreq1.AutoSize = true;
		this.lblfreq1.Location = new System.Drawing.Point(21, 49);
		this.lblfreq1.Name = "lblfreq1";
		this.lblfreq1.Size = new System.Drawing.Size(41, 13);
		this.lblfreq1.TabIndex = 28;
		this.lblfreq1.Text = "Primary";
		this.lblfreq1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		// 
		// lblfreq2
		// 
		this.lblfreq2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblfreq2.AutoSize = true;
		this.lblfreq2.Location = new System.Drawing.Point(139, 49);
		this.lblfreq2.Name = "lblfreq2";
		this.lblfreq2.Size = new System.Drawing.Size(58, 13);
		this.lblfreq2.TabIndex = 27;
		this.lblfreq2.Text = "Secondary";
		this.lblfreq2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
		// 
		// gbCmds
		// 
		this.gbCmds.BackColor = System.Drawing.SystemColors.Control;
		this.gbCmds.Controls.Add(this.lRSSI);
		this.gbCmds.Controls.Add(this.label4);
		this.gbCmds.Controls.Add(this.tbTemp);
		this.gbCmds.Controls.Add(this.lbVbat);
		this.gbCmds.Controls.Add(this.pbRSSI);
		this.gbCmds.Controls.Add(this.lblRSSI);
		this.gbCmds.Controls.Add(this.lblTemp);
		this.gbCmds.Controls.Add(this.tbPTT);
		this.gbCmds.Controls.Add(this.label11);
		this.gbCmds.Controls.Add(this.lTxTimer);
		this.gbCmds.Location = new System.Drawing.Point(6, 192);
		this.gbCmds.Name = "gbCmds";
		this.gbCmds.Size = new System.Drawing.Size(497, 55);
		this.gbCmds.TabIndex = 29;
		this.gbCmds.TabStop = false;
		this.gbCmds.Text = "Status";
		// 
		// lRSSI
		// 
		this.lRSSI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lRSSI.AutoSize = true;
		this.lRSSI.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lRSSI.Location = new System.Drawing.Point(435, 25);
		this.lRSSI.Name = "lRSSI";
		this.lRSSI.Size = new System.Drawing.Size(13, 13);
		this.lRSSI.TabIndex = 89;
		this.lRSSI.Text = "0";
		// 
		// label4
		// 
		this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(29, 25);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(43, 13);
		this.label4.TabIndex = 88;
		this.label4.Text = "Voltage";
		// 
		// tbTemp
		// 
		this.tbTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.tbTemp.AutoSize = true;
		this.tbTemp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.tbTemp.Location = new System.Drawing.Point(178, 24);
		this.tbTemp.Name = "tbTemp";
		this.tbTemp.Size = new System.Drawing.Size(17, 17);
		this.tbTemp.TabIndex = 86;
		this.tbTemp.Text = "0";
		// 
		// lbVbat
		// 
		this.lbVbat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lbVbat.AutoSize = true;
		this.lbVbat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lbVbat.Location = new System.Drawing.Point(78, 24);
		this.lbVbat.Name = "lbVbat";
		this.lbVbat.Size = new System.Drawing.Size(41, 17);
		this.lbVbat.TabIndex = 87;
		this.lbVbat.Text = "0.0V";
		// 
		// pbRSSI
		// 
		this.pbRSSI.Anchor = System.Windows.Forms.AnchorStyles.Left;
		this.pbRSSI.Location = new System.Drawing.Point(386, 18);
		this.pbRSSI.Margin = new System.Windows.Forms.Padding(0);
		this.pbRSSI.Maximum = 255;
		this.pbRSSI.Name = "pbRSSI";
		this.pbRSSI.RightToLeft = System.Windows.Forms.RightToLeft.No;
		this.pbRSSI.Size = new System.Drawing.Size(99, 27);
		this.pbRSSI.Step = 1;
		this.pbRSSI.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
		this.pbRSSI.TabIndex = 74;
		// 
		// lblRSSI
		// 
		this.lblRSSI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblRSSI.AutoSize = true;
		this.lblRSSI.Location = new System.Drawing.Point(351, 27);
		this.lblRSSI.Name = "lblRSSI";
		this.lblRSSI.Size = new System.Drawing.Size(32, 13);
		this.lblRSSI.TabIndex = 70;
		this.lblRSSI.Text = "RSSI";
		// 
		// lblTemp
		// 
		this.lblTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lblTemp.AutoSize = true;
		this.lblTemp.Location = new System.Drawing.Point(137, 26);
		this.lblTemp.Name = "lblTemp";
		this.lblTemp.Size = new System.Drawing.Size(34, 13);
		this.lblTemp.TabIndex = 73;
		this.lblTemp.Text = "Temp";
		// 
		// tbPTT
		// 
		this.tbPTT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.tbPTT.Location = new System.Drawing.Point(278, 24);
		this.tbPTT.Name = "tbPTT";
		this.tbPTT.Size = new System.Drawing.Size(48, 20);
		this.tbPTT.TabIndex = 75;
		this.tbPTT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		// 
		// label11
		// 
		this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.label11.AutoSize = true;
		this.label11.Location = new System.Drawing.Point(244, 28);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(28, 13);
		this.label11.TabIndex = 76;
		this.label11.Text = "PTT";
		// 
		// lTxTimer
		// 
		this.lTxTimer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lTxTimer.AutoSize = true;
		this.lTxTimer.Location = new System.Drawing.Point(332, 28);
		this.lTxTimer.Name = "lTxTimer";
		this.lTxTimer.Size = new System.Drawing.Size(18, 13);
		this.lTxTimer.TabIndex = 77;
		this.lTxTimer.Text = "0s";
		// 
		// lVersionString
		// 
		this.lVersionString.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.lVersionString.AutoSize = true;
		this.lVersionString.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.lVersionString.Location = new System.Drawing.Point(137, 22);
		this.lVersionString.Name = "lVersionString";
		this.lVersionString.Size = new System.Drawing.Size(208, 17);
		this.lVersionString.TabIndex = 82;
		this.lVersionString.Text = "Hardware 0.0  Firmware 0.0";
		// 
		// btnDwnld
		// 
		this.btnDwnld.BackColor = System.Drawing.Color.WhiteSmoke;
		this.btnDwnld.Location = new System.Drawing.Point(418, 19);
		this.btnDwnld.Name = "btnDwnld";
		this.btnDwnld.Size = new System.Drawing.Size(67, 23);
		this.btnDwnld.TabIndex = 81;
		this.btnDwnld.Text = "Download";
		this.btnDwnld.UseVisualStyleBackColor = false;
		this.btnDwnld.Click += new System.EventHandler(this.btnDwnld_Click);
		// 
		// timer1
		// 
		this.timer1.Enabled = true;
		this.timer1.Interval = 10;
		this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
		// 
		// timer2
		// 
		this.timer2.Enabled = true;
		this.timer2.Interval = 250;
		this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
		// 
		// groupBox1
		// 
		this.groupBox1.Controls.Add(this.serialNumber);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.btnDwnld);
		this.groupBox1.Controls.Add(this.lVersionString);
		this.groupBox1.Location = new System.Drawing.Point(6, 142);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(497, 49);
		this.groupBox1.TabIndex = 82;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Version";
		// 
		// serialNumber
		// 
		this.serialNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.serialNumber.AutoSize = true;
		this.serialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.serialNumber.Location = new System.Drawing.Point(93, 22);
		this.serialNumber.Name = "serialNumber";
		this.serialNumber.Size = new System.Drawing.Size(17, 17);
		this.serialNumber.TabIndex = 93;
		this.serialNumber.Text = "0";
		// 
		// label7
		// 
		this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(370, 24);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(42, 13);
		this.label7.TabIndex = 92;
		this.label7.Text = "Update";
		// 
		// label5
		// 
		this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(19, 24);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(73, 13);
		this.label5.TabIndex = 90;
		this.label5.Text = "Serial Number";
		// 
		// openFileDialog
		// 
		this.openFileDialog.FileName = "openFileDialog";
		// 
		// label8
		// 
		this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.label8.Location = new System.Drawing.Point(509, 45);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(92, 30);
		this.label8.TabIndex = 94;
		this.label8.Text = "(C) 2013\r\n Dynon Avionics";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		// 
		// address
		// 
		this.address.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.address.Location = new System.Drawing.Point(515, 102);
		this.address.Name = "address";
		this.address.Size = new System.Drawing.Size(39, 20);
		this.address.TabIndex = 90;
		this.address.Text = "00";
		this.address.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.address.DoubleClick += new System.EventHandler(this.address_DoubleClick);
		// 
		// dataValue
		// 
		this.dataValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
		this.dataValue.AutoSize = true;
		this.dataValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
		this.dataValue.Location = new System.Drawing.Point(560, 103);
		this.dataValue.Name = "dataValue";
		this.dataValue.Size = new System.Drawing.Size(26, 17);
		this.dataValue.TabIndex = 90;
		this.dataValue.Text = "00";
		// 
		// frmTerminal
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.ClientSize = new System.Drawing.Size(608, 366);
		this.Controls.Add(this.dataValue);
		this.Controls.Add(this.address);
		this.Controls.Add(this.label8);
		this.Controls.Add(this.groupBox1);
		this.Controls.Add(this.gbCmds);
		this.Controls.Add(this.gbChannels25);
		this.Controls.Add(this.gbVolume);
		this.Controls.Add(this.btnClear);
		this.Controls.Add(this.gbPortSettings);
		this.Controls.Add(this.rtfTerminal);
		this.MinimumSize = new System.Drawing.Size(505, 250);
		this.Name = "frmTerminal";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "SV-COM-425 Configiration Program         Feb 1, 2013";
		this.Load += new System.EventHandler(this.frmTerminal_Load);
		this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmTerminal_FormClosing);
		this.gbPortSettings.ResumeLayout(false);
		this.gbPortSettings.PerformLayout();
		this.gbVolume.ResumeLayout(false);
		this.gbVolume.PerformLayout();
		((System.ComponentModel.ISupportInitialize)(this.Squelch)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.MicGain)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.Sidetone)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.Volume)).EndInit();
		this.gbChannels25.ResumeLayout(false);
		this.gbChannels25.PerformLayout();
		((System.ComponentModel.ISupportInitialize)(this.SecondaryFreq)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.PrimaryFreq)).EndInit();
		this.gbCmds.ResumeLayout(false);
		this.gbCmds.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.ResumeLayout(false);
		this.PerformLayout();

    }

    #endregion

	private System.Windows.Forms.RichTextBox rtfTerminal;
    private System.Windows.Forms.ComboBox cmbPortName;
	private System.Windows.Forms.ComboBox cmbBaudRate;
    private System.Windows.Forms.Label lblComPort;
    private System.Windows.Forms.Label lblBaudRate;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.ComboBox cmbParity;
    private System.Windows.Forms.Label lblDataBits;
    private System.Windows.Forms.ComboBox cmbDataBits;
    private System.Windows.Forms.Label lblStopBits;
    private System.Windows.Forms.ComboBox cmbStopBits;
    private System.Windows.Forms.Button btnOpenPort;
    private System.Windows.Forms.GroupBox gbPortSettings;
	private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox gbVolume;
        private System.Windows.Forms.Label lblVHFVolume;
        private System.Windows.Forms.NumericUpDown Volume;
        private System.Windows.Forms.Label lblSidetone;
		private System.Windows.Forms.NumericUpDown Sidetone;
        private System.Windows.Forms.CheckBox chkDualWatch;
		private System.Windows.Forms.CheckBox chkUnsquelch;
        private System.Windows.Forms.GroupBox gbChannels25;
        private System.Windows.Forms.Label lblfreq1;
		private System.Windows.Forms.Label lblfreq2;
        private System.Windows.Forms.GroupBox gbCmds;
		private System.Windows.Forms.NumericUpDown SecondaryFreq;
        private System.Windows.Forms.ProgressBar pbRadioVolume;
        private System.Windows.Forms.ProgressBar pbSidetoneVolume;
		private System.Windows.Forms.NumericUpDown Squelch;
        private System.Windows.Forms.ProgressBar pbMicVolume;
        private System.Windows.Forms.Label lMic;
		private System.Windows.Forms.NumericUpDown MicGain;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Label lblTemp;
		private System.Windows.Forms.Label lblRSSI;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbPTT;
		private System.Windows.Forms.Label lTxTimer;
		public System.Windows.Forms.Timer timer2;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ProgressBar pbRSSI;
		private System.Windows.Forms.Button btnDwnld;
		private System.Windows.Forms.OpenFileDialog openFileDialog;
		private System.Windows.Forms.Label lVersionString;
		private System.Windows.Forms.Label tbTemp;
		public System.Windows.Forms.NumericUpDown PrimaryFreq;
		private System.Windows.Forms.Label lbVbat;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ProgressBar pbSquelchVolume;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label lRSSI;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button TxTest;
		private System.Windows.Forms.Label serialNumber;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button toggle;
		private System.Windows.Forms.TextBox address;
		private System.Windows.Forms.Label dataValue;
  }
}

