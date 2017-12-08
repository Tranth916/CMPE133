using GradHelperWPF.Models;
using GradHelperWPF.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GradHelperWPF.Views
{
    public partial class ImportTransferCoursesView : StackPanel
    {
        public ImportTransferCoursesView( )
        {
            InitializeComponent( );
        }

        private void TransferCoursesGrid_Drop( object sender, DragEventArgs e )
        {
			try
			{
				var hasData = e?.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);

				if (!hasData)
					return;

				if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files) || files.Length == 0)
					return;

				var xlsFile = files.FirstOrDefault(f => f.ToLower().Contains(".xls") || f.ToLower().Contains(".xlsx"));
				if (xlsFile == null || !xlsFile.Any())
					return;

				var fileStatus = FileUtil.CheckFileBeforeOpen(xlsFile);
				
				if( fileStatus != FileUtil.FileStatus.TransferCourses )
				{
					switch (fileStatus)
					{
						case FileUtil.FileStatus.Corrupted:
							MessageBox.Show("This excel file is corrupted and it needs to be manually fixed! Fix it and then try again!","Corrupted File",
										MessageBoxButton.OK,MessageBoxImage.Stop);
							break;

						case FileUtil.FileStatus.Empty:
							MessageBox.Show("This excel file is empty! Load a file with your transfer courses!", "Empty File",
										MessageBoxButton.OK, MessageBoxImage.Stop);
							break;

						case FileUtil.FileStatus.SjsuCourses:
							MessageBox.Show("This excel file does not have your transfer courses!", "Wrong File",
										MessageBoxButton.OK, MessageBoxImage.Warning);
							break;
					}
					return;
				}

				DragDropTransferInstructTextBox.Text = xlsFile;

				var cells = ExcelModel.GetExcelDataCells(xlsFile);

				if (cells == null || cells.Count == 0)
					throw new Exception("No data from excel file");

				// have data, now build the list of courses model
				var courseDict = CourseModel.BuildCourseDictionary(cells);

				if (courseDict == null || courseDict.Count == 0)
					throw new Exception("Exception throw while converting excel to course models");

				var transferCouresOnly = courseDict.Where(c => c.Value.IsTransferCourse).Select(c => c.Value).ToList();

				ViewUtil.AddCourseRowToGrid(ref TransferCourseGrid, transferCouresOnly);
			}
			catch(Exception ex)
			{

			}
        }

        private void TransferCoursesGrid_PreviewDragOver( object sender, DragEventArgs e )
        {
            if ( e != null )
                e.Handled = true;

			var hasData = e?.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);

			if (!hasData)
				return;

			if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files) || files.Length == 0)
				return;

			var xlsFile = files.FirstOrDefault(f => f.ToLower().Contains(".xls") || f.ToLower().Contains(".xlsx"));

			if (xlsFile == null || !xlsFile.Any())
			{

			}
			//DragDropTransferInstructTextBox.Text = (DragDropTransferInstructTextBox.Tag as String) ?? "";
        }

		private void DashedRectangle_PreviewDragEnter(object sender, DragEventArgs e)
		{
			//if (e != null)
			//	e.Handled = true;

			//var hasData = e?.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);

			//if (!hasData)
			//	return;

			//if (!(e.Data.GetData(DataFormats.FileDrop) is string[] files) || files.Length == 0)
			//	return;

			//var xlsFile = files.FirstOrDefault(f => f.ToLower().Contains(".xls") || f.ToLower().Contains(".xlsx"));

			//DragDropTransferInstructTextBox.Tag = DragDropTransferInstructTextBox.Text;
			//DragDropTransferInstructTextBox.Text = xlsFile;
		}
	}
}