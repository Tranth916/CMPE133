using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using GraduationHelper.Models;
using GraduationHelper.Utils;
using GraduationHelper.Views;
using PdfiumViewer;
using ITEXT = iTextSharp.text.pdf;
using HELPER = Parser;
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
		public Dictionary<string, PDFDoc> ParsedPDFS
		{
			set;get;
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

		public void TestExtractTestByRect()
		{
			string tempPath = Application.StartupPath + "//templates//majorformtemplate.pdf";

			object data = PdfHelper.ParsePDF(ImportedPDF.MajorForm, null, tempPath);

			if(data != null && data is Dictionary<string,float[]>)
			{
				HELPER.PDFFileWriter.WriteDataToPdf(tempPath, null, (Dictionary<string,float[]>) data);
			}


		}

		public void DownloadForms(string[] forms)
		{
			if (forms == null || forms.Length == 0)
				return;

			Downloader d = new Downloader(this);
			d.GetFile(forms);
		}
		
		public void WriteToPDFFile(string fileName = "")
		{
			if(ParsedPDFS != null)
			{
				string tempPath = Application.StartupPath + "//templates//majorformtemplate.pdf";
				ITEXT.PdfReader reader = new ITEXT.PdfReader(tempPath);
				var pdf = ParsedPDFS.FirstOrDefault().Value;	
				pdf.WriteDataToPdf(fileName, reader);
			}
		}

		/// <summary>
		/// Import pdf files and build a dictionary of PdfDocuments.
		/// </summary>
		public bool ImportFiles(string[] files)
		{
			bool ret = false;
			
			if (_importedPdfs == null)
				_importedPdfs = new Dictionary<string, PdfDocument>();
			else
				_importedPdfs.Clear();

			if (ParsedPDFS == null)
				ParsedPDFS = new Dictionary<string, PDFDoc>();

			string name = "";

			PDFDoc myDoc = null;

			foreach (var file in files)
			{
				if (!File.Exists(file))
					continue;

				name = Path.GetFileNameWithoutExtension(file);

				myDoc = new PDFDoc(file);

				ParsedPDFS.Add(name, myDoc);

				if (myDoc.CourseDictionary != null)
				{
					ViewDataGrid dg = new ViewDataGrid(_mainForm, myDoc.GetInfoForView());
					_mainForm.SetDataGridView(dg);
				}
			}

			if (ParsedPDFS.Count > 0)
				ret = true;
			return ret;
		}

		public TableLayoutPanel BuildTablePanel(Dictionary<string,string> info, Control parent)
		{
			TableLayoutPanel tlp = new TableLayoutPanel();
			tlp.Size = parent.Size;
			












			return null;
		}

		public List<TextBox> BuildTextBox()
		{



			return new List<TextBox>();
		}
		
		public void PopulateTextFields()
		{
			if (ParsedPDFS == null)
				return;


		}
		
		private string semTokenFall = "Fall 2016";
		private string semTokenSpring = "Spring 2016";
		private string unitsToken3 = "3.00";
		private string unitsToken2 = "2.00";
		private string unitsToken1 = "1.00";
		private string digitRegex = @"^\d+$";
		private const int MaxGradeStrLength = 2;
	
		StringBuilder _stringB = new StringBuilder();

		public string BuildCourseName(string[] arr, int start, int end)
		{
			try
			{
				_stringB.Clear();
				string toAdd;

				// start : the course #
				// end : the units
				// start + 1 <----> end
				for (int i = start + 1; i < end; i++)
				{
					toAdd = arr[i].Replace("\r", "");
					_stringB.Append(toAdd + " ");
				}

				if(_stringB.Length > 0)
					return _stringB.ToString();

			}
			catch(Exception ex)
			{
				Debug.WriteLine(ex);
			}
			return "";
		}

		public bool CheckCourseUnits(string str)
		{	
			return str.Equals(unitsToken3) || str.Equals(unitsToken2) || str.Equals(unitsToken1);
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
