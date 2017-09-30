using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationHelper;

namespace GraduationHelper.Controllers
{
	public class Controller
	{
		#region Private Members
		private MainForm _mainForm;
		#endregion

		#region Public Properties
		public MainForm MainView
		{
			set { _mainForm = value; }
			get { return _mainForm; }
		}
		#endregion

		public Controller(MainForm mainForm)
		{
			_mainForm = mainForm;
		}

		
	}
}
