using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GradHelperWPF.Models;
using Microsoft.Win32;

namespace GradHelperWPF.Views
{
	public partial class ExportMajorFormView
	{
		public ExportMajorFormView()
		{
			InitializeComponent();
		}

		private void ExportMajorFormDocBtn_Click(object sender, RoutedEventArgs e)
		{
			WordModel wm = WordModel.GetInstance();
			GradAppModel gm = GradAppModel.GetInstance();
			SaveFileDialog sfd = new SaveFileDialog()
			{
				DefaultExt = "*.docx",
				Filter = "Word Document |*.docx",
				InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
				FileName = gm.firstName + "_" + gm.lastName + ".docx" 
			};
			CourseModel tech1 = new CourseModel()
			{
				CourseAbbreviation = TechElective1Course.Text ?? "Tech",
				CourseNumber = TechElective1Number.Text ?? "1",
				CourseTitle = TechElective1Title.Text ?? "Tech Elective",
				CourseUnit = TechElective1Units.Text ?? "3",
				CourseGrade = TechElective1Grade.Text ?? "",
				IsTransferCourse = false,
			};
			CourseModel tech2 = new CourseModel()
			{
				CourseAbbreviation =	TechElective2Course.Text ?? "Tech",
				CourseNumber =			TechElective2Number.Text ?? "2",
				CourseTitle =			TechElective2Title.Text ?? "Tech Elective",
				CourseUnit =			TechElective2Units.Text ?? "3",
				CourseGrade =			TechElective2Grade.Text ?? "",
				IsTransferCourse = false,
			};

			if (!CourseModel.CoursesDictionary.ContainsKey(tech1.ToString()))
				CourseModel.CoursesDictionary.Add(tech1.ToString(), tech1);
			if (!CourseModel.CoursesDictionary.ContainsKey(tech2.ToString()))
				CourseModel.CoursesDictionary.Add(tech2.ToString(), tech2);

			//refrence to the word model.
			var sfdResult = sfd.ShowDialog();
			string outputFileName = "";
			if( sfdResult.Value != null && sfdResult.Value )
			{
				outputFileName = sfd.FileName;
			}
			//Begin writing name.
			if (!string.IsNullOrEmpty(outputFileName))
			{
				wm.WriteNameYear("first",	gm.firstName);
				wm.WriteNameYear("middle",	gm.middleName);
				wm.WriteNameYear("last",	gm.lastName);
				wm.WriteNameYear("year",	gm.gradYear);
				wm.WriteNameYear("studentid", gm.studentID);
				wm.WriteAllToFile(outputFileName);
			}
			gm.Close();
			wm.Close();
		}
	}
}
