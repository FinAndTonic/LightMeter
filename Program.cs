using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace UATControl
{
	static class Program
	{
		public static MainForm	mainForm;
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(mainForm = new MainForm());
		}
	}
}