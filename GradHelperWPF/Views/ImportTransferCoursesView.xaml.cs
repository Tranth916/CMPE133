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
using GradHelperWPF.Utils;

namespace GradHelperWPF.Views
{
	/// <summary>
	/// Interaction logic for ImportTransferCoursesView.xaml
	/// </summary>
	public partial class ImportTransferCoursesView : StackPanel
	{
		private string[] ExcelExtensions
		{
			get
			{
				return new string[] 
				{ "2003 Excel *.xls", "2007 Excel *.xlsx" };
			}
		}
		public ImportTransferCoursesView()
		{
			InitializeComponent();
		}		
		private void TransferCoursesGrid_Drop(object sender, DragEventArgs e)
		{
			bool hasData = e != null && e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);
			if (hasData)
			{
				var files = e.Data.GetData(DataFormats.FileDrop) as string[];				
				if (files == null || files.Length == 0)
					return;

				var xlsFile = files.Where(f => f.ToLower().Contains(".xls")).FirstOrDefault();
				if (xlsFile == null || xlsFile.Count() == 0)
					xlsFile = files.Where(f => f.ToLower().Contains(".xlsx")).FirstOrDefault();

				var cells = ExcelModel.GetExcelDataCells(xlsFile);

				if (cells == null || cells.Count == 0)
					throw new Exception("No data from excel file");

				// have data, now build the list of courses model
				Dictionary<string, CourseModel> courseDict = CourseModel.BuildCourseDictionary(cells);

				if ( courseDict == null || courseDict.Count == 0 )
					throw new Exception("Exception throw while converting excel to course models");
				
				var transferCouresOnly = courseDict.Where(c => c.Value.IsTransferCourse).Select(c => c.Value).ToList();
				ViewUtil.AddCourseRowToGrid(ref TransferCourseGrid, transferCouresOnly);
			}
		}
		private void TransferCoursesGrid_PreviewDragOver(object sender, DragEventArgs e)
		{
			if (e != null)
				e.Handled = true;
		}
		private void TransferCourseImportBtn_Click(object sender, RoutedEventArgs e)
		{
			string filePath = FileUtil.ShowOpenFileDialog(ExcelExtensions);
			if (string.IsNullOrEmpty(filePath))
				return;
		}

		private void TextBtn_Click(object sender, RoutedEventArgs e)
		{
			WordModel wm = WordModel.GetInstance();

			var engrCM = CourseModel.CoursesDictionary.Values.Where(v => v.CourseAbbreviation == "CIS").FirstOrDefault();

			if (engrCM == null)
			{
				wm.Close();
				return;
			}

			wm.WriteCourseToRow(engrCM);
			wm.Close();
			wm.ShowDoc();
		}
	}
}
