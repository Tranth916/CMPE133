using ExcelDataReader;
using GradHelperWPF.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using Prism.Mvvm;
using System.Linq;

namespace GradHelperWPF.Models
{
    public class ExcelModel : BindableBase
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

		private const int MatrixRowCount = 50;

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

		public List<List<string>> Get2DTable(string filePath)
		{
			List<List<string>> matrix = new List<List<string>>();

			string pathOfCopy = FileUtil.MakeWorkingCopy(filePath);

			if (string.IsNullOrEmpty(pathOfCopy))
				return matrix;

			try
			{
				FileStream fs;
				using (fs = new FileStream(pathOfCopy, FileMode.OpenOrCreate, FileAccess.Read))
				{
					var excel = ExcelDataReader.ExcelReaderFactory.CreateReader(fs);

					int rowCount = 0;
					//Get the column headers:
					do
					{
						while (excel.Read())
						{
							for (int i = 0; i < excel.FieldCount; i++)
							{
								if (excel.GetValue(i) == null)
									continue;

								string val = excel.GetValue(i) as string;

								matrix.Add(new List<string>() { val });

							}
						}
						++rowCount;
					}
					while (excel.NextResult());
				}
			}
			catch (Exception ex)
			{

			}
			return matrix;
		}
	
		/// <summary>
		/// 
		/// </summary>
		/// <param name="header"></param>
		/// <param name="data"></param>
		/// <param name="filePath"></param>
		/// <returns></returns>
		public static bool GetExcelDataWithHeaders(out List<string> header, out Dictionary<string,string> data, string filePath)
		{
			header = new List<string>();
			data = new Dictionary<string, string>();

			if (string.IsNullOrEmpty(filePath))
				return false;

			string workingCopy = FileUtil.MakeWorkingCopy(filePath);

			FileStream fs;

			using( fs = new FileStream(workingCopy, FileMode.Open, FileAccess.Read, FileShare.Read) )
			{
				if (fs == null)
					return false;
				try
				{
					var excelReader = ExcelDataReader.ExcelReaderFactory.CreateReader(fs);

					do
					{
						int row = 0;

						string key, cellValue;

						while ( excelReader.Read() )
						{
							for(int col = 0; col < excelReader.FieldCount; col++)
							{
								if ( row == 0 && excelReader.GetValue(col) != null )
								{
									header.Add(excelReader.GetValue(col) as String);
									continue;
								}

								key = $"{row},{col}";

								if (!data.ContainsKey(key))
									data.Add(key, "");

								cellValue = excelReader.GetValue(col) as String;

								if ( cellValue == null && excelReader.GetFieldType(col) != null )
								{
									var type = excelReader.GetFieldType(col);

									string val = "";

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
								else if( cellValue != null )
								{
									data[key] = cellValue;
								}
								
								if( data.ContainsKey(key) && data[key].StartsWith("TRLD", StringComparison.CurrentCultureIgnoreCase) )
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
				catch(Exception ex)
				{
					if( ex.Message.Contains("Invalid signature") )
						throw new Exception("Need to manually fix the excel file.");
				}

			}

			// remove all entries that have a TRLD PE
			return header.Count > 0;
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
	}
}
