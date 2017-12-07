using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using ExcelDataReader;
using GradHelperWPF.Utils;
using Prism.Mvvm;

namespace GradHelperWPF.Models
{
    public class ExcelModel : BindableBase
    {
        private const int MatrixRowCount = 50;

        private Dictionary<string, string> _dataTable;

        private readonly string _filePath;

        private Dictionary<string, string> SjsuCourseTextBoxMap = new Dictionary<string, string>
        {
            {"Course", ""},
            {"Description", ""},
            {"Term", ""},
            {"Units", ""},
            {"Grd Points", ""},
            {"Repeat Code", ""},
            {"Reqmnt Desig", ""},
            {"Status", ""},
            {"Transcript Note", ""}
        };

        public ExcelModel(string filePath)
        {
            _filePath = filePath;
            Init();
        }

        public Dictionary<string, string> DataTable
        {
            set => _dataTable = value;
            get
            {
                if (_dataTable == null)
                    _dataTable = GetExcelData(_filePath);

                return _dataTable;
            }
        }

        private void Init()
        {
            _dataTable = GetExcelData(_filePath);
        }

        public List<List<string>> Get2DTable(string filePath)
        {
            var matrix = new List<List<string>>();

            var pathOfCopy = FileUtil.MakeWorkingCopy(filePath);

            if (string.IsNullOrEmpty(pathOfCopy))
                return matrix;

            try
            {
                FileStream fs;
                using (fs = new FileStream(pathOfCopy, FileMode.OpenOrCreate, FileAccess.Read))
                {
                    var excel = ExcelReaderFactory.CreateReader(fs);

                    var rowCount = 0;
                    //Get the column headers:
                    do
                    {
                        while (excel.Read())
                            for (var i = 0; i < excel.FieldCount; i++)
                            {
                                if (excel.GetValue(i) == null)
                                    continue;

                                var val = excel.GetValue(i) as string;

                                matrix.Add(new List<string> {val});
                            }
                        ++rowCount;
                    } while (excel.NextResult());
                }
            }
            catch (Exception ex)
            {
            }
            return matrix;
        }

        public static List<ExcelCell> GetExcelDataCells(string path)
        {
            var cells = new List<ExcelCell>();
            List<string> header;
            Dictionary<string, string> data;
            var hasData = GetExcelDataWithHeaders(out header, out data, path);
            if (!hasData)
            {
                var w = new Window();


                throw new Exception("No Data found in file: " + path);
            }

            foreach (var entry in data)
                try
                {
                    if (string.IsNullOrEmpty(entry.Value))
                        continue;

                    var split = entry.Key.Split(',');
                    int row = -1, col = -1;
                    int.TryParse(split[0], out row);
                    int.TryParse(split[1], out col);

                    if (row < 0 || col < 0)
                        continue;

                    cells.Add(new ExcelCell(row, col, entry.Value, header[col]));
                }
                catch (Exception ex)
                {
                }

            var sorted = from cell in cells
                orderby cell.Row
                orderby cell.Column
                select cell;

            return sorted.ToList();
        }

        /// <summary>
        /// </summary>
        /// <param name="header"></param>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool GetExcelDataWithHeaders(out List<string> header, out Dictionary<string, string> data,
            string filePath)
        {
            var isTransferCourseFlag = false;
            var transferCourseString = "transfer";
            header = new List<string>();
            data = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(filePath))
                return false;

            var workingCopy = FileUtil.MakeWorkingCopy(filePath);
            using (var fs = new FileStream(workingCopy, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                if (fs == null)
                    return false;
                try
                {
                    var excelReader = ExcelReaderFactory.CreateReader(fs);
                    do
                    {
                        //prep the header
                        for (var i = 0; i < excelReader.FieldCount; i++)
                            header.Add("");

                        var row = 0;
                        string key, cellValue;
                        while (excelReader.Read())
                        {
                            for (var col = 0; col < excelReader.FieldCount; col++)
                            {
                                if (row == 0 && excelReader.GetValue(col) != null)
                                {
                                    header[col] = excelReader.GetValue(col) as string;
                                    // Check this flag only if its false.
                                    if (!isTransferCourseFlag)
                                        isTransferCourseFlag = header
                                                                   .Where(c => c.ToLower()
                                                                       .Contains(transferCourseString))
                                                                   .Count() > 0;
                                    continue;
                                }

                                // This should only execute for transfer course excels.
                                if (row == 1 && isTransferCourseFlag && excelReader.GetValue(col) != null)
                                {
                                    var nextHeaderLine = excelReader.GetValue(col) as string;

                                    if (header[col] != null)
                                        header[col] = header[col] + " " + nextHeaderLine;

                                    continue;
                                }

                                key = $"{row},{col}";

                                if (!data.ContainsKey(key))
                                    data.Add(key, "");

                                cellValue = excelReader.GetValue(col) as string;

                                if (cellValue == null && excelReader.GetFieldType(col) != null)
                                {
                                    var type = excelReader.GetFieldType(col);
                                    var val = "";
                                    switch (type.Name)
                                    {
                                        case "Double":
                                            val = $"{excelReader.GetDouble(col)}";
                                            break;

                                        case "Float":
                                            val = $"{excelReader.GetFloat(col)}";
                                            break;

                                        case "Integer":
                                            val = $"{excelReader.GetInt32(col)}";
                                            break;
                                    }
                                    data[key] = val;
                                }
                                else if (cellValue != null)
                                {
                                    data[key] = cellValue;
                                }

                                if (data.ContainsKey(key) &&
                                    data[key].StartsWith("TRLD", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    data.Remove(key);
                                    //break out the loop.
                                    break;
                                }
                            }
                            row++;
                        }
                    } while (excelReader.NextResult());
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Invalid signature"))
                        throw new Exception("Need to manually fix the excel file.");
                }
            }
            // remove all entries that have a TRLD PE
            return header.Count > 0;
        }

        public static Dictionary<string, string> GetExcelData(string filePath)
        {
            var data = new Dictionary<string, string>();

            if (!File.Exists(filePath))
                return data;

            //Make a working copy;
            var pathOfCopy = FileUtil.MakeWorkingCopy(filePath);

            try
            {
                FileStream fs;

                using (fs = new FileStream(pathOfCopy, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var excelReader = ExcelReaderFactory.CreateReader(fs);

                    // put the row as the key, a list<string> as the values;
                    var sb = new StringBuilder();

                    var orderedData = new OrderedDictionary();

                    var columnHeader = new OrderedDictionary();

                    string cellValue, currentColumnName;

                    var rowCount = 0;
                    do
                    {
                        while (excelReader.Read())
                        {
                            // The first row of the spreadsheet contains the headers.
                            if (rowCount == 0)
                            {
                                for (var col = 0; col < excelReader.FieldCount; col++)
                                    if (excelReader.GetValue(col) != null)
                                    {
                                        var name = excelReader.GetValue(col) as string;

                                        var duplicate = false;
                                        for (var i = 0; i < columnHeader.Count; i++)
                                            if (columnHeader[i].ToString() == name)
                                                duplicate = true;
                                        if (!duplicate)
                                            columnHeader.Add(col, name);
                                    }
                            }
                            else
                            {
                                // This is data across the row.
                                var columnData = new List<string>();

                                orderedData.Add(rowCount, columnData);

                                // read across the columns.
                                for (var col = 0; col < excelReader.FieldCount; col++)
                                {
                                    if (excelReader.GetValue(col) == null)
                                        continue;

                                    currentColumnName = (columnHeader[col] as string).ToLower();
                                    cellValue = excelReader.GetValue(col) as string;

                                    if (currentColumnName.Contains("course"))
                                    {
                                        // values like: needs to be split "CMPE 102";
                                        var split = cellValue.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                                        foreach (var s in split)
                                            columnData.Add(s);
                                    }
                                    else
                                    {
                                        columnData.Add(cellValue);
                                    }
                                }

                                // Build the key;
                                foreach (var str in columnData)
                                    sb.Append(str + "|");

                                var key = sb.ToString();

                                if (!data.ContainsKey(key))
                                    data.Add($"{rowCount}", key);
                                sb.Clear();
                            }
                            ++rowCount;
                        }
                    } while (excelReader.NextResult());
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message.ToLower();

                if (msg.Contains("invalid signature"))
                    MessageBox.Show("Need to fix the .xls file manually first!");
            }
            return data;
        }
    }
}