using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GradHelperWPF.Utils;
using ExcelDataReader;
using System.Collections.Specialized;

namespace GradHelperWPF.Models
{
    public class ExcelModel
    {
		private Dictionary<string, string> SjsuCourseTextBoxMap = new Dictionary<string, string>()
		{
			{"Course",""},
			{"Description",""},
			{"Term",""},
			{"Units",""},
			{"Grd Points",""},
			{"Repeat Code",""},
			{"Reqmnt Desig",""},
			{"Status",""},
			{"Transcript Note",""}
		};
		
		private string _filePath;

		private Dictionary<string, string> _dataTable;

		public Dictionary<string,string> DataTable
		{
			set
			{
				_dataTable = value;
			}
			get
			{
				if( _dataTable == null )
				{
					_dataTable = GetExcelData(_filePath);
				}

				return _dataTable;
			}
		}

		public ExcelModel(string filePath)
		{
			_filePath = filePath;
			Init();
		}

		private void Init()
		{
			_dataTable = GetExcelData(_filePath);
		}
		
		public static Dictionary<string,string> GetExcelData(string filePath)
		{
			Dictionary<string, string> data = new Dictionary<string, string>();

			if (!File.Exists(filePath))
				return data;

			//Make a working copy;
			string pathOfCopy = FileUtil.MakeWorkingCopy(filePath);

			try
			{
				FileStream fs;

				using (fs = new FileStream(pathOfCopy, FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					var excelReader = ExcelReaderFactory.CreateReader(fs);

					// put the row as the key, a list<string> as the values;
					StringBuilder sb = new StringBuilder();

					OrderedDictionary orderedData = new OrderedDictionary();

					OrderedDictionary columnHeader = new OrderedDictionary();

					string cellValue, currentColumnName;

					int rowCount = 0;
					do
					{
						while( excelReader.Read() )
						{
							// The first row of the spreadsheet contains the headers.
							if(rowCount == 0)
							{
								for (int col = 0; col < excelReader.FieldCount; col++)
								{
									if(excelReader.GetValue(col) != null)
									{
										string name = excelReader.GetValue(col) as string;

										bool duplicate = false;
										for(int i = 0; i < columnHeader.Count; i++)
										{
											if (columnHeader[i].ToString() == name)
												duplicate = true;
										}
										if (!duplicate)
										{
											columnHeader.Add(col, name);
										}
									}
								}
							}
							else
							{
								// This is data across the row.
								List<string> columnData = new List<string>();

								orderedData.Add(rowCount, columnData);

								// read across the columns.
								for (int col = 0; col < excelReader.FieldCount; col++)
								{
									if (excelReader.GetValue(col) == null)
										continue;

									currentColumnName = (columnHeader[col] as string).ToLower();
									cellValue = excelReader.GetValue(col) as string;

									if(currentColumnName.Contains("course"))
									{
										// values like: needs to be split "CMPE 102";
										var split = cellValue.Split(new char[] { ' '}, StringSplitOptions.RemoveEmptyEntries);

										foreach (var s in split)
										{
											columnData.Add(s);
										}
									}
									else
									{
										columnData.Add(cellValue);
									}
								}
								
								// Build the key;
								foreach(var str in columnData)
								{
									sb.Append(str + "|");
								}

								string key = sb.ToString();

								if ( !data.ContainsKey(key) )
								{
									data.Add($"{rowCount}", key);
								}
								sb.Clear();
							}
							++rowCount;
						}

					} while (excelReader.NextResult());					
				}
			}
			catch (Exception ex)
			{
				string msg = ex.Message.ToLower();

				if (msg.Contains("invalid signature"))
				{
					System.Windows.MessageBox.Show("Need to fix the .xls file manually first!");
				}
			}
			return data;
		}

		//public void ReadExcel2003(string path)
		//{
		//    Dictionary<string, List<string>> table = new Dictionary<string, List<string>>();

		//    IExcelDataReader excel = null;

		//    Stream stream = null;

		//    // Must be careful here! Ensure that the file is closed after done reading.
		//    using (stream = new FileStream(path, FileMode.OpenOrCreate))
		//    {
		//        try
		//        {
		//            // excel = ExcelReaderFactory.CreateOpenXmlReader(stream);
		//            excel = ExcelReaderFactory.CreateReader(stream);
		//            //excel = ExcelReaderFactory.CreateBinaryReader(stream);
		//        }
		//        catch (Exception ex)
		//        {
		//            //Debug.WriteLine(ex.StackTrace);
		//            if (ex.Message.ToLower().Contains("invalid sign"))
		//            {
		//                throw new Exception(ex.StackTrace);
		//            }
		//            else
		//                throw new Exception(ex.StackTrace);
		//        }
		//        finally
		//        {
		//            if (excel != null)
		//            {

		//                string value = "";
		//                int columns, rowNumber = 0;

		//                do
		//                {
		//                    while (excel.Read())
		//                    {
		//                        columns = excel.FieldCount;
		//                        // Read across the columns
		//                        for (int i = 0; i < columns; i++)
		//                        {
		//                            if (excel.GetValue(i) == null)
		//                                continue;

		//                            value = excel.GetValue(i) as String;
		//                            // The first row is the column headers in the SJSU Course history.
		//                            // Course	Description	Term	Grade	Units	Grd Points	Repeat Code	Reqmnt Desig Status	Transcript Note
		//                            if (rowNumber == 0 && !table.ContainsKey(value))
		//                            {
		//                                // Each key should look like: 1_Course.. 2_Description.
		//                                table.Add($"{i}_{value}", new List<string>());
		//                            }
		//                            else
		//                            {
		//                                // Find the coulmn to insert into.
		//                                var key = (from k in table.Keys
		//                                           let columnNumber = k.Substring(0, k.IndexOf("_"))
		//                                           where i.ToString() == columnNumber
		//                                           select k).FirstOrDefault();

		//                                if (!string.IsNullOrEmpty(key))
		//                                {
		//                                    table[key].Add(value);
		//                                }
		//                            }
		//                        }
		//                        rowNumber++;
		//                    }
		//                    // advance to next row.
		//                } while (excel.NextResult());

		//            }
		//        }
		//    } // Stream will auto close after this statement.

		//    AddCoursesToGridCell(table);
		//}
	}
}
