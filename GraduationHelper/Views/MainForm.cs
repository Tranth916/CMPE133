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
//using Gnostice.Documents.PDF;

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
		private string _currentPdfShown;

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
				if (dataTabPages != null && displayGroupBox != null)
				{
					displayGroupBox.Width = (int)(.75 * this.Width);
					displayGroupBox.Height = (int)(.90 * this.Height);

					dataTabPages.Height = displayGroupBox.Height - 50;
					dataTabPages.Width = displayGroupBox.Width - 70;
				}
				if(tabPage1 != null && tabPage1.HasChildren)
				{
					foreach(Control c in tabPage1.Controls)
					{
						if (c.Name == "pdfView")
							c.Size = dataTabPages.Size;
					}
				}

				tabPage1.SizeChanged += (oo, x) =>
				{
					foreach (Control c in tabPage1.Controls)
					{
						c.Size = tabPage1.Size;
					}
				};
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
		int ptr = 0;
		string[] pdfNames = new string[] 
		{
			"grad_app","test1","stupid","test2"
		};
		PdfViewer v;
		private void testButton_Click(object sender, EventArgs e)
		{
			if (_pdfs == null)
				_pdfs = _controller.GetPdfDictionary();
			
			if(_currentPdfShown == null || _currentPdfShown.Length == 0)
			{
				_currentPdfShown = _pdfs.Keys.FirstOrDefault();
			}
			
				v = _controller.GetPdfView(pdfNames[ptr++]);

				if (ptr == pdfNames.Length)
					ptr = 0;

			tabPage1.Controls.Clear();
			tabPage1.Controls.Add(v);
			
			v.Size = tabPage1.Size;
		}
		#endregion

		private void OnClickImportBtn(object sender, EventArgs e)
		{
			if (_controller == null)
				return;

			bool hasImports = _controller.ImportFiles();

			if (!hasImports)
				return;

			foreach(var entry in _controller.ImportedPdfs)
			{
				PdfViewer vi = new PdfViewer()
				{
					Document = entry.Value,
					Name = entry.Key,
					ZoomMode = PdfViewerZoomMode.FitWidth,
				};

				TabPage tp = new TabPage(entry.Key);
				tp.Controls.Add(vi);
				dataTabPages.TabPages.Add(tp);
				vi.Size = tp.Size;

				tp.SizeChanged += (o, x) => 
				{
					vi.Size = tp.Size;
				};
				dataTabPages.SelectedIndex = dataTabPages.TabCount - 1;
			}
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
