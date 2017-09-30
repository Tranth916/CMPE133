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
using System.Diagnostics;

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

		private void OnSaveSessionBtnClick(object sender, EventArgs e)
		{
			if (_controller == null)
				return;

			string saveType = "";

			if(sender is ToolStripMenuItem)
				saveType = (sender as ToolStripMenuItem).Name;

			SaveFileDialog sfd = new SaveFileDialog()
			{
				CheckPathExists = true,
				Filter = ".xml | .XML",
				DefaultExt = ".xml | .XML",
			};
			
			if(saveType == "saveToolStripMenuItem")
			{

			}
			else if(saveType == "saveAsToolStripMenuItem")
			{
				DialogResult dr = sfd.ShowDialog();

				if(dr == DialogResult.OK && sfd.FileName != null)
				{
					_controller.SaveCurrentSession(sfd.FileName);
				}
			}
		}

		private void testButton_Click(object sender, EventArgs e)
		{
			webBrowser1.Navigate($"C:\\Downloader\\test.pdf");
			
		}
	}
}
