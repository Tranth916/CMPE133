using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using GraduationHelper.Utils;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GraduationHelper.Models
{
	public class Student
	{
		public Student()
		{
		}

		#region Personal Info
		public string FirstName
		{
			set; get;
		}
		public string MiddleName
		{
			set;
			get;
		}
		public string MiddleInitial
		{
			set; get;
		}
		public string LastName
		{
			set; get;
		}
		public int StudentID
		{
			set; get;
		}
		public string Major
		{
			set; get;
		}
		#endregion

		public bool LoadStudentXml(XDocument doc)
		{
			if (doc == null)
				return false;
			
			FirstName = XmlHelper.ReadXPathStrValue(doc, "Student/FirstName");
			MiddleName = XmlHelper.ReadXPathStrValue(doc, "Student/MiddleName");
			LastName = XmlHelper.ReadXPathStrValue(doc, "Student/LastName");
			Major = XmlHelper.ReadXPathStrValue(doc, "Student/Major");
			StudentID = XmlHelper.ReadXPathIntValue(doc, "Student/StudentID");
			
			return true;
		}

		public void SaveStudentXML(string path)
		{
			if (path == null || path.Length == 0)
				return;

			XDocument doc = new XDocument(new XDeclaration("1.0","UTF-8","yes"));
			
			doc.Add(new XElement("Student"));

			var root = doc.Root;

			root.Add(new XElement("FirstName", FirstName));
			root.Add(new XElement("MiddleName", MiddleName));
			root.Add(new XElement("LastName", LastName));
			root.Add(new XElement("StudentID", StudentID));
			root.Add(new XElement("Major", Major));

			doc.Save(path);
		}
	}
}
