using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraduationHelper.Controllers;
using PdfSharp.Pdf.IO;

namespace GraduationHelper
{
	public partial class MainForm : Form
	{
		private Controller _controller;

		public MainForm()
		{
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			_controller = new Controller(this);
		}


		private void OnTextFieldValueChanged(object sender, EventArgs e)
		{
			if (_controller == null)
				return;

			_controller.TextFieldsDataChanged(sender);
		}

		private void OnExitBtnClick(object sender, EventArgs e)
		{
			DialogResult userChoice;
			userChoice = MessageBox.Show("Exit the Program?", "Exit" ,MessageBoxButtons.OKCancel);
			if(userChoice == DialogResult.OK)
			{
				this.Close();
			}
		}

		private void OnDownloadFormsBtnClick(object sender, EventArgs e)
		{
			if (downloadFormsCheckList.CheckedItems.Count == 0)
				return;

			List<string> forms = new List<string>();

			foreach (var item in downloadFormsCheckList.CheckedItems)
			{
				forms.Add(item.ToString());
			}
			
			_controller.DownloadForms(forms.ToArray());
		}
	}
}
