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
			d.GetFile();
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

			bool result = _student.LoadStudentXml(XDocument.Load(path));

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
	}
}
