using System.Windows;
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

			string initDirectory = @"C:\Users\Thao Tran\Desktop\Live Demo Files";

			if (!System.IO.Directory.Exists(initDirectory))
				initDirectory = System.IO.Directory.GetCurrentDirectory();

			SaveFileDialog sfd = new SaveFileDialog()
			{
				DefaultExt = "*.docx",
				Filter = "Word Document |*.docx",
				InitialDirectory = initDirectory,
				FileName = gm.firstName + "_" + gm.lastName + ".docx" 
			};

			CourseModel tech1 = new CourseModel()
			{
				CourseAbbreviation = TechElective1Course.Text ?? "",
				CourseNumber = TechElective1Number.Text ?? "",
				CourseTitle = TechElective1Title.Text ?? "",
				CourseUnit = TechElective1Units.Text ?? "",
				CourseGrade = TechElective1Grade.Text ?? "",
				IsTransferCourse = false,
			};
			CourseModel tech2 = new CourseModel()
			{
				CourseAbbreviation =	TechElective2Course.Text ?? "",
				CourseNumber =			TechElective2Number.Text ?? "",
				CourseTitle =			TechElective2Title.Text ?? "",
				CourseUnit =			TechElective2Units.Text ?? "",
				CourseGrade =			TechElective2Grade.Text ?? "",
				IsTransferCourse = false,
			};

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


				wm.WriteTechElective(tech1, 1);
				wm.WriteTechElective(tech2, 2);
				wm.WriteAllToFile(outputFileName);
			}
			gm.Close();
			wm.Close();
		}
	}
}
