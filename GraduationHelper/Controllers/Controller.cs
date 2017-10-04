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
		private const string PdfLibName = "pdfium.dll";
		private const string x64LibPath = "\\libs\\x64\\pdfium.dll";
		private const string x86LibPath = "\\libs\\x86\\pdfium.dll";
		private const string TestPdfDir = "C:\\testpdfs\\";

		private Dictionary<string, PdfDocument> _importedPdfs;
		private Dictionary<string, PdfDocument> _pdfs;

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
		public Dictionary<string, PdfDocument> ImportedPdfs
		{
			get { return _importedPdfs; }
		}
		#endregion

		#region Constructor

		public Controller(MainForm mainForm)
		{
			_mainForm = mainForm;

			Init();
		}
		
		private void Init()
		{
			CheckDependency();	
		}

		private void CheckDependency()
		{
			try
			{
				// Ensure pdfium.dll is inside this working directory.
				var dllFile = Directory.GetFiles(Application.StartupPath)
					.Where(f => Path.GetFileName(f).ToLower().Equals(PdfLibName));
				
				if (dllFile == null || dllFile.Count() == 0)
				{
					string src = "";

					if (IntPtr.Size == 4)
					{
						src = $"{Application.StartupPath}{x86LibPath}";
					}
					else if (IntPtr.Size == 8)
					{
						src = $"{Application.StartupPath}{x64LibPath}";
					}

					if(!File.Exists(src))
					{
						MessageBox.Show("","Missing x64 & x86 lib folders. Exiting.",MessageBoxButtons.OK, MessageBoxIcon.Error);

						_mainForm.Close();				
					}

					string dest = $"{Application.StartupPath}\\{PdfLibName}";

					File.Copy(src, dest, true);
				}
			}
			catch (Exception)
			{

			}

		}
		#endregion

		public void DownloadForms(string[] forms)
		{
			if (forms == null || forms.Length == 0)
				return;

			Downloader d = new Downloader(this);
			d.GetFile(forms);
		}
		
		/// <summary>
		/// Import pdf files and build a dictionary of PdfDocuments.
		/// </summary>
		public bool ImportFiles()
		{
			bool ret = false;
			OpenFileDialog ofd = new OpenFileDialog()
			{
				Multiselect = true,
				CheckFileExists = true,
				InitialDirectory = Application.StartupPath,
			};

			var result = ofd.ShowDialog();

			if (result == DialogResult.Cancel)
				return ret;

			string[] files = ofd.FileNames;

			if (_importedPdfs == null)
				_importedPdfs = new Dictionary<string, PdfDocument>();
			else
				_importedPdfs.Clear();

			string name = "";
			PdfDocument doc = null;

			foreach(var file in ofd.FileNames)
			{
				if (!File.Exists(file))
					continue;

				name = Path.GetFileNameWithoutExtension(file);

				if (_importedPdfs.ContainsKey(name))
					continue;
				
				doc = PdfDocument.Load(file);

				_mainForm.Log($"Loaded {name} pdf");

				_importedPdfs.Add(name, doc);	
			}

			if (_importedPdfs.Count > 0)
				ret = true;

			return ret;
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
		
		public Dictionary<string, PdfDocument> GetPdfDictionary()
		{
			_pdfs = new Dictionary<string, PdfDocument>();

			string[] files = Directory.GetFiles(TestPdfDir);

			string name = "";
			PdfDocument loaded;

			foreach(var file in files)
			{
				name = Path.GetFileName(file).Replace(".pdf","");

				loaded = PdfDocument.Load(file);	

				if(loaded != null && !_pdfs.ContainsKey(name))
				{
					_pdfs.Add(name, loaded);
				}
			}
			
			return _pdfs;
		}

		public PdfDocument GetPdfDoc()
		{
			string url = @"C:\GraduationHelper\GraduationHelper\GraduationHelper\bin\Debug\stupid.pdf";
			PdfDocument doc = PdfDocument.Load(url);







			return doc;
		}

		public PdfViewer GetPdfView(string pdfName = null)
		{
			try
			{
				if(pdfName == null)
					pdfName = "stupid";

				if (_pdfs == null)
					GetPdfDictionary();

				if (_pdfs.ContainsKey(pdfName))
				{
					PdfViewer viewer = new PdfViewer()
					{
						Document = _pdfs[pdfName],
						ZoomMode = PdfViewerZoomMode.FitWidth,
						ShowToolbar = true,
					};

					return viewer;
				}
				
			}
			catch(Exception ex)
			{
				Debug.WriteLine("Error");
			}
			return null;
		}


	}
}
