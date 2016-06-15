using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace dongle
{
	public partial class Versions : Form
	{
		public 
		Versions()
		{
			byte	i2c;

			InitializeComponent();

			AvailableRevisions.ScrollAlwaysVisible = true;

			DongleForm.moduleId = i2c = 1;
			Program.dongleForm.doDeviceBlink(i2c,1);

			ModuleName.Text = DongleForm.modules[1].name;
			ModuleRevision.Text = DongleForm.modules[1].rev;

			AvailableRevisions.Items.Add(DongleForm.modules[1].rev);
		}

		private void 
		VersionsUpdate_Click(object sender, EventArgs e)
		{
			byte i2c = DongleForm.moduleId;
		}

		private void 
		VersionsNext_Click(object sender, EventArgs e)
		{
			byte	i2c = DongleForm.moduleId;

			Program.dongleForm.doDeviceBlink(i2c, 0);
			
			Program.dongleForm.Wait(1);

			DongleForm.moduleId += 1;

			if (DongleForm.moduleId >= DongleForm.slaveCount)
			{
				DongleForm.moduleId = 1;
			}
			i2c = DongleForm.moduleId;
			Program.dongleForm.doDeviceBlink(i2c, 1);
			
			ModuleName.Text = DongleForm.modules[i2c].name;
			ModuleRevision.Text = DongleForm.modules[i2c].rev;

			AvailableRevisions.Items.Clear();
			AvailableRevisions.Items.Add(DongleForm.modules[i2c].rev);
		}

		private void VersionsCancel_Click(object sender, EventArgs e)
		{
			byte i2c = DongleForm.moduleId;
			Program.dongleForm.doDeviceBlink(i2c, 0);
		}
	}
}