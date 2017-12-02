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
	public partial class ImportTransferCoursesView : Grid
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

		private void Grid_Drop(object sender, DragEventArgs e)
		{
		}

		private void TransferCoursesGrid_Drop(object sender, DragEventArgs e)
		{
			bool hasData = e != null && e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);

			if (hasData)
			{
				var files = e.Data.GetData(DataFormats.FileDrop) as string[];
				// Load the excel file.

				int labelRowIndex = TransferCourseGrid.RowDefinitions
													  .Where(rr => rr.Name.Contains("Labels"))
													  .Select(rr => TransferCourseGrid.RowDefinitions.IndexOf(rr))
													  .FirstOrDefault() + 1;

				if (files == null || files.Length == 0)
					return;

				var xlsFile = files.Where(f => f.ToLower().Contains(".xls")).FirstOrDefault();

				List<string> header;
				Dictionary<string, string> table;

				bool gotData = ExcelModel.GetExcelDataWithHeaders(out header, out table, xlsFile);

				if (!gotData)
				{
					throw new Exception("No Data Found");
				}

				// key -> row# 
				Dictionary<int, List<TextBox>> textboxes = new Dictionary<int, List<TextBox>>(); 

				string key, val;

				string[] rowColIndexes;

				foreach( var entry in table )
				{
					if (string.IsNullOrEmpty(entry.Key) || string.IsNullOrEmpty(entry.Value))
						continue;

					// split the key 
					rowColIndexes = entry.Key.Split(',');

					if (rowColIndexes == null || rowColIndexes.Length <= 1)
						continue;

					val = entry.Value;
					
					int rr = 0;
					int cc = 0;
					int.TryParse(rowColIndexes[0], out rr);
					int.TryParse(rowColIndexes[1], out cc);

					//Course	Description	Term	Grade	Units	Grd Points
					if (header[cc].Contains("Term"))
						continue;

					// Unit needs to be swapped with grade.
					if (header[cc].Contains("Unit"))
					{
						var currentRow = textboxes[rr];

						int indexOfGradeTB = currentRow.Where(tbx => tbx.Tag != null && (tbx.Tag as String).Contains("Grade"))
													 .Select(tbx => currentRow.IndexOf(tbx))
													 .FirstOrDefault();
						if (indexOfGradeTB > 0)
						{
							TextBox poppedTB = currentRow[indexOfGradeTB];

							currentRow.RemoveAt(indexOfGradeTB);

							TextBox unitTB = new TextBox()
							{
								Text = val,
								Tag = "Unit"
							};

							currentRow.Insert(indexOfGradeTB, unitTB);
							currentRow.Insert(indexOfGradeTB + 1, poppedTB);
							continue;
						}
					}

					if (val.Contains(" ") && header[cc].Contains("Course"))
					{
						//split the course abbrv from the course #.
						var split = val.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

						if (!textboxes.ContainsKey(rr))
						{
							textboxes.Add(rr, new List<TextBox>());
						}

						if ( split.Length == 2 )
						{
							TextBox tb1 = new TextBox()
							{
								Text = split[0],
								Tag = "Course"
							};
							TextBox tb2 = new TextBox()
							{
								Text = split[1],
								Tag = "Number"
							};

							textboxes[rr].Add(tb1);
							textboxes[rr].Add(tb2);
						}

						continue;
					}

					TextBox tb = new TextBox()
					{
						Text = val,
						Tag = header[cc]
					};
					if ( !textboxes.ContainsKey(rr) )
					{
						textboxes.Add( rr, new List<TextBox>(){ tb });
					}
					else
					{
						
						textboxes[rr].Add(tb);
					}					
				}

				int row = TransferCourseGrid.RowDefinitions
											.Where( r => !string.IsNullOrEmpty(r.Name) && r.Name.StartsWith("Labels"))
											.Select( r => TransferCourseGrid.RowDefinitions.IndexOf(r) )
											.FirstOrDefault() + 2;

				int colI = TransferCourseGrid.ColumnDefinitions
								.Where(cd => cd.Name == "GradeColDef")
								.Select(cd => TransferCourseGrid.ColumnDefinitions.IndexOf(cd))
								.FirstOrDefault();
				// now populate the rows
				foreach ( var tbList in textboxes.Values)
				{
					for(int i = 0; i < tbList.Count; i++)
					{
						if (tbList[i].Tag == null 
							|| (tbList[i].Tag as String).Contains("Grd") 
							|| (tbList[i].Tag as String).Contains("Reqmnt"))
						continue;

						TransferCourseGrid.Children.Add(tbList[i]);
						if( row >= TransferCourseGrid.RowDefinitions.Count )
						{
							RowDefinition rowDef = new RowDefinition();
							rowDef.Height = TransferCourseGrid.RowDefinitions.LastOrDefault().Height;
							TransferCourseGrid.RowDefinitions.Add(rowDef);
						}

						Grid.SetRow(tbList[i], row);
						// Need to place the textbox in the correct column.
						if ( (string)tbList[i].Tag == "Grade" )
						{
							Grid.SetColumn(tbList[i], colI);
						}
						else
						{
							Grid.SetColumn(tbList[i], i);
						}
					}
					row++;
				}				
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
	}
}
