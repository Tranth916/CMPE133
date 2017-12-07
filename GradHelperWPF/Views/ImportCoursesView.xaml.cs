﻿using GradHelperWPF.Models;
using GradHelperWPF.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Controls;

namespace GradHelperWPF.Views
{
    /// <summary>
    ///     Interaction logic for ImportCoursesView.xaml
    /// </summary>
    public partial class ImportCoursesView
    {
        public ImportCoursesView( )
        {
            InitializeComponent( );
            Init( );
        }

        private void Init( )
        {
            DataContext = GradApplicationView.gradAppViewModelStatic;
            //AllowDrop = true;
            //PreviewDragOver += Grid_PreviewDragOver;
            //Drop += SjsuCourseExcel_OnDrop;
        }

        private void Grid_PreviewDragOver( object sender, DragEventArgs e )
        {
            e.Handled = true;
        }

        private void SjsuCourseExcel_OnDrop( object sender, DragEventArgs e )
        {
            var hasData = e?.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);

            if (!hasData) return;

            if ( !(e.Data.GetData(DataFormats.FileDrop) is string[] files) || files.Length == 0 )
                return;

            var xlsFile = files.FirstOrDefault(f => f.ToLower().Contains(".xls"));
            if ( xlsFile == null || !xlsFile.Any() )
                xlsFile = files.FirstOrDefault(f => f.ToLower( ).Contains( ".xlsx" ) );

            FileUtil.FileStatus status = FileUtil.CheckFileBeforeOpen(xlsFile);

            if (status != FileUtil.FileStatus.SjsuCourses )
            {
                string typeOfFix = "";

                switch (status)
                {
                    case FileUtil.FileStatus.TransferCourses:
                        typeOfFix = "NotSjsuCourse"
                        break;

                    case FileUtil.FileStatus.Corrupted:
                        typeOfFix = "FixExcel";
                        break;
                }






                if (string.IsNullOrEmpty(typeOfFix))
                    return;

                ErrorWindowView mfx = new ErrorWindowView(typeOfFix)
                {
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                };
                mfx.ShowDialog();
                return;
            }

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

        private void ImportBtn_OnClick( object sender, RoutedEventArgs e )
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "*.xls | *.XLS | *.xlsx | *.XLSX",
                CheckFileExists = true
            };

            var opened = ofd.ShowDialog();

            if (opened == null || !opened.Value) return;
            var fileName = ofd.FileName;

            var em = new ExcelModel(fileName);

            var data = em.DataTable;

            foreach ( var row in data )
            {
                var split = row.Value.Split('|');

                var firstFive = split.Where((t, i) => i <= 4).Where(t => !t.Contains("Fall") && !t.Contains("Spring")).ToList();
            }
        }

        private void TestMakeDummyRows( )
        {
            var dummyData = new Dictionary<string, List<string>>();

            var list = new List<string>();
            for ( var i = 0; i < 10; i++ )
            {
                var key = DateTime.Now.Second + "_time";

                for ( var j = 0; j < 5; j++ )
                    list.Add( " random " );
            }
        }

        private void TestButton_Click( object sender, RoutedEventArgs e )
        {
        }
    }
}