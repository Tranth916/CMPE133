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
using PdfiumViewer;
using System.Linq;

namespace GraduationHelper
{
	public enum Indexes
	{
		FirstName = 0,
		MiddleName = 1,
		LastName = 2,
		StudentId = 3,
		MajorName = 4,
	}

	public partial class MainForm : Form, IView
	{
		private Controller _controller;
		private TableLayoutPanel _dataPageTable;
		private Dictionary<string, PdfDocument> _pdfs;
		private Dictionary<string, TextBox> _generalEdTextBoxes;

		public Dictionary<string, TextBox> GeneralEdTextBoxes
		{
			set
			{
				_generalEdTextBoxes = value;
			}
			get
			{
				if(_generalEdTextBoxes == null)
				{
					_generalEdTextBoxes = new Dictionary<string, TextBox>()
					{
						{ majorImportLbl.Name, majorTBox },
						{ programYearLbl.Name, programYearTB },
						{ areaTB_A1.Name, areaTB_A1 },
						{ areaTB_A2.Name, areaTB_A2 },
						{ areaTB_A3.Name, areaTB_A3 },
						{ areaTB_B1.Name, areaTB_B1 },
						{ areaTB_B2.Name, areaTB_B2 },
						{ areaTB_B3.Name, areaTB_B3 },
						{ areaTB_B4.Name, areaTB_B4 },
						{ areaTB_C1.Name, areaTB_C1 },
						{ areaTB_C2.Name, areaTB_C2 },
						{ areaTB_C3.Name, areaTB_C3 },
						{ areaTB_D1.Name, areaTB_D1 },
						{ areaTB_D2.Name, areaTB_D2 },
						{ areaTB_E1.Name, areaTB_E1 },
					};
				}
				
				return _generalEdTextBoxes;
			}
		}
		
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
			this.SizeChanged += (o, e) => 
			{
				if (geCoursesTab != null && displayGroupBox != null)
				{
					displayGroupBox.Width = (int)(.75 * this.Width);
					displayGroupBox.Height = (int)(.90 * this.Height);
					geCoursesTab.Height = displayGroupBox.Height - 50;
					geCoursesTab.Width = displayGroupBox.Width - 70;
				}

				if(geCoursesTab != null && geCoursesTab.HasChildren)
				{
					foreach(Control c in geCoursesTab.Controls)
					{
						c.Size = geCoursesTab.Size;
					}
				}
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

		private void OnTabIndexChanged(object sender, EventArgs e)
		{
			if(_dataPageTable == null)
			{
				string[] dataRows = new string[]
				{

				};

				_dataPageTable = ViewHelper.BuildTablePanel( dataRows , dataPage.Width >> 1, dataPage.Height);

				_dataPageTable.Name = "DataPageTable";

				dataPage.Controls.Add(_dataPageTable);
			}
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

		private void OnDownloadShowFolder(object sender, EventArgs e)
		{
			if (_controller == null)
				return;

			_controller.ShowDownloadFolder();
		}

		#region Test Stuff

		private void testButton_Click(object sender, EventArgs e)
		{
			_controller.TestExtractTestByRect();



			//SaveFileDialog sfd = new SaveFileDialog()
			//{
			//	DefaultExt = "*.pdf | *.PDF",
			//	InitialDirectory = Application.StartupPath,				
			//};

			//var result = sfd.ShowDialog();

			//if( result == DialogResult.OK)
			//{
			//	_controller.WriteToPDFFile(sfd.FileName);
			//}
		}
		#endregion

		private void OnClickImportBtn(object sender, EventArgs e)
		{
			if (_controller == null)
				return;

			OpenFileDialog ofd = new OpenFileDialog()
			{
				Multiselect = true,
				CheckFileExists = true,
				InitialDirectory = Application.StartupPath + "\\Docs",
				Filter = "*.pdf | *.PDF",
			};

			if ((ofd.ShowDialog() == DialogResult.OK))
			{
				bool gotData = _controller.ImportFiles(ofd.FileNames);

				if (gotData)
					_controller.PopulateTextFields();
			}
			else
				return;
		}
		
		public void UpdateGeneralEdTextBoxes()
		{

		}

		public void RenderImportedPdfs(Dictionary<string,PDFDoc> pdfs)
		{

		}

		public void SetDataGridView(ViewDataGrid vdg)
		{
			TabPage tb = new TabPage()
			{
				AutoScroll = true,
				
			};
			
			tabPage1.Controls.Add(vdg.DataTable);

			vdg.SetSizeBindingToParent(tabPage1);
		}

		#region Logger
		public void Log(string str)
		{
			if(logTextBox != null)
			{
				logTextBox.AppendText(str + Environment.NewLine);
			}
		}
		#endregion	
	}
}
