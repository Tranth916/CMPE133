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
using GraduationHelper.Views;
using GraduationHelper.Utils;
using GraduationHelper.Interfaces;
using GraduationHelper.Models;

namespace GraduationHelper
{
	public partial class MainForm : Form, IView
	{
		private Controller _controller;

		#region Constants
		public static readonly int FirstNameIndex = 0;
		public static readonly int MiddleNameIndex = 1;
		public static readonly int LastNameIndex = 2;
		public static readonly int StudentIdIndex = 3;
		public static readonly int MajorNameIndex = 4;
		
		public static readonly int NumberOfConfig = 5;
		#endregion

		#region IView Members
		public string ViewTitle { set; get; }
		#endregion

		public MainForm()
		{
			InitializeComponent();
			Init();
		}

		private void Init()
		{
			_controller = new Controller(this);

			LoadConfigOnStartup();

			this.FormClosing += (o, e) => 
			{
				//if(axAcroPDF1 != null)
				//{
				//	axAcroPDF1.Dispose();
				//	axAcroPDF1 = null;
				//}
			};
		}

		public void UpdatePersonalInfoFields(string[] infos)
		{
			if (infos.Length < 5)
				return;

			firstNameTxtBox.Text = infos[FirstNameIndex];
			middleNameTxtBox.Text = infos[MiddleNameIndex];
			lastNameTxtBox.Text = infos[LastNameIndex];
			stuIdTxtBox.Text = infos[StudentIdIndex];
			majorNameTxtBox.Text = infos[MajorNameIndex];
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

		private void OnOpenSessionBtnClick(object sender, EventArgs e)
		{
			if (_controller == null)
				return;

			OpenFileDialog ofd = new OpenFileDialog()
			{
				CheckFileExists = true,
				Filter = "*.XML | *.xml",
				Multiselect = false,
			};

			DialogResult dr = ofd.ShowDialog();
			
			if(dr == DialogResult.OK)
			{
				_controller.LoadSession(ofd.FileName);
			}

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

		private void OnAboutMenuItemClick(object sender, EventArgs e)
		{
			AboutForm af = new AboutForm();
			af.ShowDialog();
		}

		private void LoadConfigOnStartup()
		{
			string startPath = Application.StartupPath + "\\Config.xml";

			string[] data  = _controller.LoadConfiguration(startPath);

			firstNameTxtBox.Text = data[FirstNameIndex];
			middleNameTxtBox.Text = data[MiddleNameIndex];
			lastNameTxtBox.Text = data[LastNameIndex];
			stuIdTxtBox.Text = data[StudentIdIndex];
			majorNameTxtBox.Text = data[MajorNameIndex];
		}

		private void testButton_Click(object sender, EventArgs e)
		{
			try
			{
				PDFDoc pdf = new PDFDoc(Application.StartupPath + "\\empty.pdf");

				pdf.WriteAndSaveFirstName();

			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			
		}
	}
}
