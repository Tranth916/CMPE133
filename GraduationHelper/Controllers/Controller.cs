using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using GraduationHelper;
using GraduationHelper.Models;
using System.Xml.Linq;
using GraduationHelper.Utils;
using System.IO;
using System.Diagnostics;
using PdfiumViewer;

namespace GraduationHelper.Controllers
{
	public class Controller
	{
		#region Private Members
		private MainForm _mainForm;
		private Student _student;
		private string SessionFileLocation
		{
			set;get;
		}
		#endregion

		#region Public Properties
		public MainForm MainView
		{
			set { _mainForm = value; }
			get { return _mainForm; }
		}
		public XDocument ConfigFile
		{
			private set;
			get;
		}
		#endregion

		#region Constructor

		public Controller(MainForm mainForm)
				{
					_mainForm = mainForm;
				}
	
		#endregion

		public void DownloadForms(string[] forms)
		{
			if (forms == null || forms.Length == 0)
				return;

			Downloader d = new Downloader(this);
			d.GetFile(forms);
		}

		public void ImportFiles()
		{
			OpenFileDialog ofd = new OpenFileDialog()
			{
				Multiselect = true,
				CheckFileExists = true,
				InitialDirectory = Application.StartupPath,
				Filter = "*.pdf | *.PDF | *.doc | *.DOC",
			};

			var result = ofd.ShowDialog();

			if (result == DialogResult.Cancel)
				return;

			string[] files = ofd.FileNames;

			foreach(var f in files)
			{
				Debug.WriteLine(f);
			}


		}

		public void ShowDownloadFolder()
		{
			try
			{
				string currentDirectory = Application.StartupPath + "\\Download";

				if (!Directory.Exists(currentDirectory))
				{
					Directory.CreateDirectory(currentDirectory);
				}
				Process.Start("explorer.exe", currentDirectory);
			}
			catch(Exception ex)
			{
				Debug.WriteLine(" Exception Thrown " + ex);
			}
		
		}

		public void TextFieldsDataChanged(object sender)
		{
			if (!(sender is TextBox))
				return;

			if (_student == null)
				_student = new Student();
			
			TextBox tf = sender as TextBox;

			string fieldName = tf.Tag as string ?? "";
			string fieldValue = tf.Text;
		
			switch (fieldName)
			{
				case "FirstName":
					_student.FirstName = fieldValue ?? "";
					break;

				case "LastName":
					_student.LastName = fieldValue ?? "";
					break;

				case "MiddleName":
					_student.MiddleName = fieldValue ?? "";
					if(_student.MiddleName.Length > 0)
						_student.MiddleInitial = $"{_student.MiddleName[0]}";
					break;

				case "StudentID":
					_student.StudentID = ConvertTextToNumber(fieldValue);
					break;

				case "Major":
					_student.Major = fieldValue ?? "";
					break;
			}
		}
		
		public string[] LoadConfiguration(string filePath)
		{
			if (File.Exists(filePath))
				ConfigFile = XDocument.Load(filePath);

			Dictionary<string, string> configs = XmlHelper.LoadXMLConfiguration(filePath);

			string[] data = new string[]
			{
				configs["FirstName"] ?? "",
				configs["MiddleName"] ?? "",
				configs["LastName"] ?? "",
				configs["StudentID"] ?? "",
				configs["Major"] ?? "",
			};
			
			return data;
		}

		public void LoadSession(string path)
		{
			if (path == null || path.Length == 0)
				return;

			if (_student == null)
				_student = new Student();

			ConfigFile = XDocument.Load(path);

			bool result = _student.LoadStudentXml(ConfigFile);

			if (result)
			{
				string[] data = new string[] 
				{
					_student.FirstName,
					_student.MiddleName,
					_student.LastName,
					_student.StudentID.ToString(),
					_student.Major,
				};

				_mainForm.UpdatePersonalInfoFields(data);
			}
		}

		public void SaveCurrentSession(string path = "")
		{
			if (_student == null || path.Length == 0)
				return;

			SessionFileLocation = path;
			
			_student.SaveStudentXML(path);
		}

		private int ConvertTextToNumber(string str)
		{
			int retValue = 0;

			int.TryParse(str, out retValue);

			return retValue;
		}

		public PdfViewer GetPdfView()
		{
			try
			{
				string[] urls = new string[]
				{
				@"C:\GraduationHelper\GraduationHelper\GraduationHelper\bin\Debug\stupid.pdf",
				@"C:\GraduationHelper\GraduationHelper\GraduationHelper\bin\Debug\test1.pdf",
				@"C:\GraduationHelper\GraduationHelper\GraduationHelper\bin\Debug\test2.pdf",
				@"C:\GraduationHelper\GraduationHelper\GraduationHelper\bin\Debug\test3.pdf",
				};

				PdfDocument fdoc = PdfDocument.Load(urls[0]);

				PdfViewer viewer = new PdfViewer()
				{
					Document = fdoc,
					ZoomMode = PdfViewerZoomMode.FitWidth,
					ShowToolbar = true,
				};

				return viewer;
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Error");
			}
			return null;
		}
	}
}
