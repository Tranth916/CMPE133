using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraduationHelper.Interfaces;

namespace GraduationHelper.Views
{
	public partial class ViewDoc : Form, IView
	{
		#region Constructor
		public ViewDoc()
		{
			InitializeComponent();
			ViewTitle = "View Documents";
		}
		#endregion

		public string ViewTitle
		{
			set;
			get;
		}	
			
	}
}
