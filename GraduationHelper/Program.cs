using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
/**
 * Graduation Helper
 * @author Thao Tran
 * CMPE133 - SOFTWARE ENGINEERING II
 * FALL 2017
 * Instructor: Eswar
 */
namespace GraduationHelper
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
