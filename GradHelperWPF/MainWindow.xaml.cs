using ExcelDataReader;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using GradHelperWPF.ViewModel;
using GradHelperWPF.Model;
using System.Runtime.InteropServices;
using System.Security;

namespace GradHelperWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Constants
        private const int SecondPageColumnNumber = 5;
        private const int SecondPageRowNumber = 15;

        private const double ColDefCourseWidth = 50;
        private const double ColDefNumWidth = 35;
        private const double ColDefTitleWidth = 200;
        private const double ColDefUnitWidth = 35;
        private const double ColDefGradeWidth = 50;

        #endregion  
        private Dictionary<string, List<TextBox>> _textFieldRows;

        // All Views here.
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {

        }

		///// <summary>
		///// Dynamically create all the rows and columns for the 2nd page.
		///// </summary>
		//private void CreateImportSJSUPage()
		//{
		//    ScrollViewer scrollViewer = new ScrollViewer();

		//    _textFieldRows = new Dictionary<string, List<TextBox>>();

		//    Grid secondPage = new Grid()
		//    {
		//        HorizontalAlignment = HorizontalAlignment.Center,
		//        Width = SecondFlipGrid.Width,
		//        Height = SecondFlipGrid.Height
		//    };

		//    Grid headerGrid = new Grid()
		//    {
		//        Width = MainFrame.Width,
		//        Height = 100,
		//        AllowDrop = true,
		//    };

		//    headerGrid.Children.Add(new Label() { Content = "Drag Item Here" });
		//    headerGrid.Drop += HeaderGrid_Drop;
		//    headerGrid.PreviewDragOver += (o, e) =>
		//    {
		//        e.Handled = true;
		//    };


		//    /*  
		//     *  [ some label to indicate drag & drop ]
		//     *  [ some button to browse for the file ]
		//     *  [               ]
		//     * 
		//     */

		//    secondPage.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
		//    secondPage.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

		//    SecondFlipGrid.Children.Add(headerGrid);
		//    Grid.SetRow(headerGrid, 0);
		//    Grid.SetColumn(headerGrid, 0);

		//    // Make the entire table.
		//    SecondFlipGrid.Children.Add(secondPage);
		//    Grid.SetRow(secondPage, 1);

		//    var rowDef = SecondFlipGrid.RowDefinitions.FirstOrDefault();
		//    int columnDefsCount = 0;
		//    for (int i = 1; i < SecondPageRowNumber; i++)
		//    {
		//        secondPage.RowDefinitions.Add(new RowDefinition() { Height = rowDef.Height });

		//        var list = new List<TextBox>()
		//        {   new TextBox(){ Text="Course"}, // 5 char max.
		//            new TextBox(){ Text="No"},    // 3 char max.
		//            new TextBox(){ Text="Title"},  // 
		//            new TextBox(){ Text="Unit"},   // 5 char max.
		//            new TextBox(){ Text="Grade"}   // 5 char max.
		//        };

		//        double sumOfWidth = ColDefCourseWidth +
		//                            ColDefNumWidth +
		//                            ColDefTitleWidth +
		//                            ColDefUnitWidth +
		//                            ColDefGradeWidth;

		//        int startColumn = 1;

		//        foreach (var tbx in list)
		//        {
		//            if (secondPage.ColumnDefinitions.FirstOrDefault(cc => cc.Name == "GradeColDef") == null)
		//            {
		//                ColumnDefinition colDef = new ColumnDefinition();
		//                switch (tbx.Text)
		//                {
		//                    case "Course":
		//                        colDef.MinWidth = ColDefCourseWidth;
		//                        colDef.Width = new GridLength(ColDefCourseWidth / sumOfWidth, GridUnitType.Star);
		//                        colDef.Name = "CourseColDef";
		//                        break;
		//                    case "No":
		//                        colDef.MinWidth = ColDefNumWidth;
		//                        colDef.Width = new GridLength(ColDefNumWidth / sumOfWidth, GridUnitType.Star);
		//                        colDef.Name = "NoColDef";
		//                        break;
		//                    case "Title":
		//                        colDef.MinWidth = ColDefTitleWidth;
		//                        colDef.Width = new GridLength(ColDefTitleWidth / sumOfWidth, GridUnitType.Star);
		//                        colDef.Name = "TitleColDef";
		//                        break;
		//                    case "Unit":
		//                        colDef.MaxWidth = ColDefUnitWidth;
		//                        colDef.Width = new GridLength(ColDefUnitWidth / sumOfWidth, GridUnitType.Star);
		//                        colDef.Name = "UnitColDef";
		//                        break;
		//                    case "Grade":
		//                        colDef.MinWidth = ColDefGradeWidth;
		//                        colDef.Width = new GridLength(ColDefGradeWidth / sumOfWidth, GridUnitType.Star);
		//                        colDef.Name = "GradeColDef";
		//                        break;
		//                    default:
		//                        colDef.Width = GridLength.Auto;
		//                        break;
		//                }

		//                secondPage.ColumnDefinitions.Add(colDef);

		//                ++columnDefsCount;
		//            }

		//            secondPage.Children.Add(tbx);

		//            Grid.SetRow(tbx, i);
		//            Grid.SetColumn(tbx, startColumn);

		//            tbx.Name = $"{tbx.Text}_{i}_{startColumn}";
		//            tbx.Tag = new int[] { i, startColumn };
		//            startColumn++;
		//        }
		//        _textFieldRows.Add($"Row={i}", list);
		//    }
		//}
		//private void HeaderGrid_Drop(object sender, DragEventArgs e)
		//{
		//    try
		//    {
		//        if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
		//        {
		//            var files = e.Data.GetData(DataFormats.FileDrop) as string[];

		//            if (files != null)
		//            {
		//                // select the file with the .xls extension. 
		//                // Need to be careful here since the .xls will often crash.
		//                string excelF = (from file in files
		//                                 where file.EndsWith(".xls", StringComparison.CurrentCultureIgnoreCase)
		//                                 select file as string).FirstOrDefault();

		//                string excelXLSX = (from file in files
		//                                    where file.EndsWith(".xlsx", StringComparison.CurrentCultureIgnoreCase)
		//                                    select file as string).FirstOrDefault();

		//                // pick the excel xlsx over the xls if possible.
		//                if (!string.IsNullOrEmpty(excelXLSX))
		//                {
		//                    try
		//                    {
		//                        ReadExcel2003(excelXLSX);
		//                    }
		//                    catch (Exception exx)
		//                    {
		//                        Debug.WriteLine(exx.StackTrace);
		//                    }
		//                }

		//                if (!string.IsNullOrEmpty(excelF))
		//                {
		//                    try
		//                    {
		//                        ReadExcel2003(excelF);
		//                    }
		//                    catch (Exception exxxx)
		//                    {
		//                        Debug.WriteLine(exxxx.StackTrace);
		//                    }
		//                }
		//            }
		//        }
		//    }
		//    catch (Exception ex)
		//    {
		//        if (ex.Message.Contains("Invalid signature"))
		//        {
		//            // Tell user to manually fix the XLS spreadsheet using MSExcel.
		//        }
		//    }
		//    finally
		//    {

		//    }
		//}
		//private void ImportSJSUBtn_OnClick(object sender, RoutedEventArgs e)
		//{
		//    OpenFileDialog ofd = new OpenFileDialog()
		//    {
		//        InitialDirectory = System.Reflection.Assembly.GetExecutingAssembly().Location,
		//        Filter = "*.xsls|*.xls"
		//    };

		//    var result = ofd.ShowDialog();

		//    if (String.IsNullOrEmpty(ofd.FileName))
		//        return;

		//    //FileStream stream = File.Open(ofd.FileName, FileMode.OpenOrCreate);

		//    try
		//    {

		//    }
		//    catch (Exception ex)
		//    {
		//        Debug.WriteLine(ex.StackTrace);
		//    }
		//    finally
		//    {

		//    }


		//}
		//public void AddCoursesToGridCell(Dictionary<string, List<string>> data)
		//{
		//    var columnHeaders = data.Keys.ToList(); 

		//    // exclude the header;
		//    int rowCount = data[columnHeaders[0]].Count();

		//    List<string> list;
		//    List<TextBox> tbList;

		//    var textBoxRows = _textFieldRows.Values.ToList();             
		//    // traverse the rows.
		//    for( int row = 0; row < textBoxRows.Count; row++)
		//    {
		//        tbList = textBoxRows[row];

		//        for(int column = 0; column < tbList.Count; column++)
		//        {
		//            list = data[columnHeaders[column]];
		//            tbList[column].Text = list[row];
		//        }

		//        //test
		//        if (row >= _textFieldRows.Count)
		//            break;
		//    }
		//}       
	}
}

