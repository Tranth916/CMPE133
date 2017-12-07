using GradHelperWPF.Models;
using GradHelperWPF.Utils;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GradHelperWPF.Views
{
    /// <summary>
    ///     Interaction logic for ImportTransferCoursesView.xaml
    /// </summary>
    public partial class ImportTransferCoursesView : StackPanel
    {
        public ImportTransferCoursesView( )
        {
            InitializeComponent( );
        }

        private string[] ExcelExtensions => new[]
            {"2003 Excel *.xls", "2007 Excel *.xlsx"};

        private void TextBtn_Click( object sender, RoutedEventArgs e )
        {
            var wm = WordModel.GetInstance();

            var engrCm = CourseModel.CoursesDictionary.Values
                .FirstOrDefault(v => v.CourseAbbreviation == "CIS");

            if ( engrCm == null )
            {
                wm.Close( );
                return;
            }

            wm.WriteCourseToRow( engrCm );
            wm.Close( );
            wm.ShowDoc( );
        }

        private void TransferCourseImportBtn_Click( object sender, RoutedEventArgs e )
        {
            var filePath = FileUtil.ShowOpenFileDialog(ExcelExtensions);
            if ( string.IsNullOrEmpty( filePath ) )
                return;
        }

        private void TransferCoursesGrid_Drop( object sender, DragEventArgs e )
        {
            var hasData = e?.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);

            if ( !hasData ) return;

            if ( !( e.Data.GetData( DataFormats.FileDrop ) is string[] files ) || files.Length == 0 )
                return;

            var xlsFile = files.FirstOrDefault(f => f.ToLower().Contains(".xls"));
            if ( xlsFile == null || !xlsFile.Any( ) )
                xlsFile = files.FirstOrDefault( f => f.ToLower( ).Contains( ".xlsx" ) );

            var cells = ExcelModel.GetExcelDataCells(xlsFile);

            if ( cells == null || cells.Count == 0 )
                throw new Exception( "No data from excel file" );

            // have data, now build the list of courses model
            var courseDict = CourseModel.BuildCourseDictionary(cells);

            if ( courseDict == null || courseDict.Count == 0 )
                throw new Exception( "Exception throw while converting excel to course models" );

            var transferCouresOnly = courseDict.Where(c => c.Value.IsTransferCourse).Select(c => c.Value).ToList();

            ViewUtil.AddCourseRowToGrid( ref TransferCourseGrid, transferCouresOnly );
        }

        private void TransferCoursesGrid_PreviewDragOver( object sender, DragEventArgs e )
        {
            if ( e != null )
                e.Handled = true;

            var hasData = e?.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);

            if ( !hasData ) return;

            if ( !( e.Data.GetData( DataFormats.FileDrop ) is string[] files ) || files.Length == 0 )
                return;

            var xlsFile = files.FirstOrDefault(f => f.ToLower().Contains(".xls") || f.ToLower( ).Contains( ".xlsx" ) );

            if (xlsFile == null || !xlsFile.Any())
            {
                System.Diagnostics.Debug.WriteLine("Excel File Detected!");
            }
        }
    }
}