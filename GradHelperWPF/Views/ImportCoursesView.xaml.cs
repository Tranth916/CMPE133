using GradHelperWPF.Models;
using GradHelperWPF.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GradHelperWPF.Views
{
    /// <summary>
    ///     Interaction logic for ImportCoursesView.xaml
    /// </summary>
    public partial class ImportCoursesView : StackPanel
    {
        public ImportCoursesView( )
        {
            InitializeComponent( );
            Init( );
        }

        private void Init( )
        {
            DataContext = GradApplicationView.gradAppViewModelStatic;

            AllowDrop = true;
            PreviewDragOver += Grid_PreviewDragOver;
            Drop += Grid_Drop;
        }

        private void Grid_PreviewDragOver( object sender, DragEventArgs e )
        {
            e.Handled = true;
        }

        private void Grid_Drop( object sender, DragEventArgs e )
        {
            var hasData = e != null && e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);
            if ( hasData )
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                if ( files == null || files.Length == 0 )
                    return;

                var xlsFile = files.Where(f => f.ToLower().Contains(".xls")).FirstOrDefault();
                if ( xlsFile == null || xlsFile.Count( ) == 0 )
                    xlsFile = files.Where( f => f.ToLower( ).Contains( ".xlsx" ) ).FirstOrDefault( );

                var cells = ExcelModel.GetExcelDataCells(xlsFile);

                if ( cells == null || cells.Count == 0 )
                    throw new Exception( "No data from excel file" );

                // have data, now build the list of courses model
                var courseDict = CourseModel.BuildCourseDictionary(cells);

                if ( courseDict == null || courseDict.Count == 0 )
                    throw new Exception( "Exception throw while converting excel to course models" );

                var transferCouresOnly = courseDict.Where(c => !c.Value.IsTransferCourse).Select(c => c.Value).ToList();
                ViewUtil.AddCourseRowToGrid( ref TransferCourseGrid, transferCouresOnly );
            }
        }

        private List<TextBox> BuildGridRow( List<string> data )
        {
            // Need 5 columns for 5 textboxes, only for the initial load.
            if ( TransferCourseGrid != null && TransferCourseGrid.ColumnDefinitions.Count < 5 )
            {
                TransferCourseGrid.ColumnDefinitions.Clear( );

                // ENGR | 102 | INTRO  TO ... | UNIT | GRADE
                for ( var i = 0; i < 5; i++ )
                {
                    var length = "";

                    switch ( i )
                    {
                        case 0:
                            length = "35*";
                            break;

                        case 1:
                            length = "35*";
                            break;

                        case 2:
                            length = "100*";
                            break;

                        case 3:
                            length = "40*";
                            break;

                        case 4:
                            length = "40*";
                            break;
                    }

                    var colDef = new ColumnDefinition
                    {
                        Width = (GridLength) new GridLengthConverter().ConvertFromString(length)
                    };

                    TransferCourseGrid.ColumnDefinitions.Add( colDef );
                }
            }

            var rowDef = new RowDefinition
            {
                Height = GridLength.Auto
            };

            TransferCourseGrid.RowDefinitions.Add( rowDef );

            var currentRowIndex = TransferCourseGrid.RowDefinitions.IndexOf(rowDef);

            if ( currentRowIndex < 0 )
                throw new Exception( "Row Index is :" + currentRowIndex );

            var textboxes = new List<TextBox>();

            // Add a new text box for each column;
            for ( var i = 0; i < data.Count; i++ )
            {
                var tb = new TextBox
                {
                    Text = data[i]
                };

                TransferCourseGrid.Children.Add( tb );

                Grid.SetRow( tb, currentRowIndex );

                Grid.SetColumn( tb, i );

                textboxes.Add( tb );
            }

            return textboxes;
        }

        private void ImportBtn_OnClick( object sender, RoutedEventArgs e )
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "*.xls | *.XLS | *.xlsx | *.XLSX",
                CheckFileExists = true
            };

            var opened = ofd.ShowDialog();

            if ( opened.Value )
            {
                var fileName = ofd.FileName;

                var em = new ExcelModel(fileName);

                var data = em.DataTable;

                foreach ( var row in data )
                {
                    var split = row.Value.Split('|');

                    var firstFive = new List<string>();

                    for ( var i = 0; i < split.Length; i++ )
                    {
                        if ( i > 4 )
                            continue;

                        if ( split[i].Contains( "Fall" ) || split[i].Contains( "Spring" ) )
                            continue;

                        firstFive.Add( split[i] );
                    }

                    BuildGridRow( firstFive );
                }
            }
        }

        private void TestMakeDummyRows( )
        {
            var dummyData = new Dictionary<string, List<string>>();

            for ( var i = 0; i < 10; i++ )
            {
                var key = DateTime.Now.Second + "_time";

                var list = new List<string>();
                for ( var j = 0; j < 5; j++ )
                    list.Add( " random " );

                BuildGridRow( list );
            }
        }

        private void TestButton_Click( object sender, RoutedEventArgs e )
        {
        }
    }
}