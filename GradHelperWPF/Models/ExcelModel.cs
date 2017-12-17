using ExcelDataReader;
using GradHelperWPF.Utils;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace GradHelperWPF.Models
{
    public class ExcelModel : BindableBase
    {
        private Dictionary<string, string> _dataTable;

        private readonly string _filePath;

        public ExcelModel( string filePath )
        {
            _filePath = filePath;
            Init( );
        }

        public Dictionary<string, string> DataTable
        {
            set => _dataTable = value;
            get => _dataTable ?? (_dataTable = GetExcelData(_filePath));
        }

        private void Init( )
        {
            _dataTable = GetExcelData( _filePath );
        }

        public static List<ExcelCell> GetExcelDataCells( string path )
        {
            var cells = new List<ExcelCell>();
            var hasData = GetExcelDataWithHeaders(out var header, out var data, path);
            if ( !hasData )
            {
                var w = new Window();

                throw new Exception( "No Data found in file: " + path );
            }

            foreach ( var entry in data )
                try
                {
                    if ( string.IsNullOrEmpty( entry.Value ) )
                        continue;

                    var split = entry.Key.Split(',');
                    int.TryParse( split[0], out var row );
                    int.TryParse( split[1], out var col );

                    if ( row < 0 || col < 0 )
                        continue;

                    cells.Add( new ExcelCell( row, col, entry.Value, header[col] ) );
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception throw while reading excel file." + ex.StackTrace);
                }

            var sorted = from cell in cells
                         orderby cell.Row
                         orderby cell.Column
                         select cell;

            return sorted.ToList( );
        }

        /// <summary>
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool GetExcelDataWithHeaders( out List<string> header, out Dictionary<string, string> data,
            string filePath )
        {
            var isTransferCourseFlag = false;
            var transferCourseString = "transfer";
            header = new List<string>( );
            data = new Dictionary<string, string>( );

            if ( string.IsNullOrEmpty( filePath ) )
                return false;

            var workingCopy = FileUtil.MakeWorkingCopy(filePath);
            using ( var fs = new FileStream( workingCopy, FileMode.Open, FileAccess.Read, FileShare.Read ) )
            {
                try
                {
                    var excelReader = ExcelReaderFactory.CreateReader(fs);
                    do
                    {
                        //prep the header
                        for ( var i = 0; i < excelReader.FieldCount; i++ )
                            header.Add( "" );

                        var row = 0;
                        while ( excelReader.Read( ) )
                        {
                            for ( var col = 0; col < excelReader.FieldCount; col++ )
                            {
                                if ( row == 0 && excelReader.GetValue( col ) != null )
                                {
                                    header[col] = excelReader.GetValue( col ) as string;
                                    // Check this flag only if its false.
                                    if ( !isTransferCourseFlag )
                                        isTransferCourseFlag = header.Any(c => c.ToLower( )
                                                                        .Contains( transferCourseString ));
                                    continue;
                                }

                                // This should only execute for transfer course excels.
                                if ( row == 1 && isTransferCourseFlag && excelReader.GetValue( col ) != null )
                                {
                                    var nextHeaderLine = excelReader.GetValue(col) as string;

                                    if ( header[col] != null )
                                        header[col] = header[col] + " " + nextHeaderLine;

                                    continue;
                                }

                                var key = $"{row},{col}";

                                if ( !data.ContainsKey( key ) )
                                    data.Add( key, "" );

                                var cellValue = excelReader.GetValue( col ) as string;

                                if ( cellValue == null && excelReader.GetFieldType( col ) != null )
                                {
                                    var type = excelReader.GetFieldType(col);
                                    var val = "";
                                    switch ( type.Name )
                                    {
                                        case "Double":
                                            val = $"{excelReader.GetDouble( col )}";
                                            break;

                                        case "Float":
                                            val = $"{excelReader.GetFloat( col )}";
                                            break;

                                        case "Integer":
                                            val = $"{excelReader.GetInt32( col )}";
                                            break;
                                    }
                                    data[key] = val;
                                }
                                else if ( cellValue != null )
                                {
                                    data[key] = cellValue;
                                }

                                if ( data.ContainsKey( key ) &&
                                    data[key].StartsWith( "TRLD", StringComparison.CurrentCultureIgnoreCase ) )
                                {
                                    data.Remove( key );
                                    //break out the loop.
                                    break;
                                }
                            }
                            row++;
                        }
                    } while ( excelReader.NextResult( ) );
                }
                catch ( Exception ex )
                {
                    if ( ex.Message.Contains( "Invalid signature" ) )
                        throw new Exception( "Need to manually fix the excel file." );
                }
            }
            // remove all entries that have a TRLD PE
            return header.Count > 0;
        }

        public static Dictionary<string, string> GetExcelData( string filePath )
        {
            var data = new Dictionary<string, string>();

            if ( !File.Exists( filePath ) )
                return data;

            //Make a working copy;
            var pathOfCopy = FileUtil.MakeWorkingCopy(filePath);
         
            try
            {
                FileStream fs;

                using ( fs = new FileStream( pathOfCopy, FileMode.Open, FileAccess.Read, FileShare.Read ) )
                {
                    var excelReader = ExcelReaderFactory.CreateReader(fs);

                    // put the row as the key, a list<string> as the values;
                    var sb = new StringBuilder();

                    var columnHeader = new OrderedDictionary();

                    var rowCount = 0;
                    do
                    {
                        while ( excelReader.Read( ) )
                        {
                            // The first row of the spreadsheet contains the headers.
                            if ( rowCount == 0 )
                            {
                                for ( var col = 0; col < excelReader.FieldCount; col++ )
                                    if ( excelReader.GetValue( col ) != null )
                                    {
                                        var name = excelReader.GetValue(col) as string;

                                        var duplicate = false;
                                        for ( var i = 0; i < columnHeader.Count; i++ )
                                            if ( columnHeader[i].ToString( ) == name )
                                                duplicate = true;
                                        if ( !duplicate )
                                            columnHeader.Add( col, name );
                                    }
                            }
                            else
                            {
                                // This is data across the row.
                                var columnData = new List<string>();

                               // orderedData.Add( rowCount, columnData );

                                // read across the columns.
                                for ( var col = 0; col < excelReader.FieldCount; col++ )
                                {
                                    if ( excelReader.GetValue( col ) == null )
                                        continue;

                                    var currentColumnName = ( columnHeader[col] as string )?.ToLower( );
                                    var cellValue = excelReader.GetValue( col ) as string;

                                    if ( currentColumnName != null && currentColumnName.Contains( "course" ) )
                                    {
                                        // values like: needs to be split "CMPE 102";
                                        if (cellValue != null)
                                        {
                                            var split = cellValue.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                                            columnData.AddRange(split);
                                        }
                                    }
                                    else
                                    {
                                        columnData.Add( cellValue );
                                    }
                                }

                                // Build the key;
                                foreach ( var str in columnData )
                                    sb.Append( str + "|" );

                                var key = sb.ToString();

                                if ( !data.ContainsKey( key ) )
                                    data.Add( $"{rowCount}", key );
                                sb.Clear( );
                            }
                            ++rowCount;
                        }
                    } while ( excelReader.NextResult( ) );
                }
            }
            catch ( Exception ex )
            {
                var msg = ex.Message.ToLower();

                if ( msg.Contains( "invalid signature" ) )
                    MessageBox.Show( "Need to fix the .xls file manually first!" );
            }
            return data;
        }
    }
}