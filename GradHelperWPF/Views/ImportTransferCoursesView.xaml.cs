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
					
					if (val.Contains(" ") && header[cc].Contains("Course"))
					{
						//split the course abbrv from the course #.
						var split = val.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

						if (!textboxes.ContainsKey(rr))
							textboxes.Add(rr, new List<TextBox>());

						foreach (var str in split)
						{
							TextBox tbx = new TextBox()
							{
								Text = str
							};
						}

						continue;
					}

					TextBox tb = new TextBox()
					{
						Text = val,
						Tag = new int[] { rr, cc }
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

				// now populate the rows
				foreach( var tbList in textboxes.Values)
				{
					for(int i = 0; i < tbList.Count; i++)
					{
						TransferCourseGrid.Children.Add(tbList[i]);

						if( row >= TransferCourseGrid.RowDefinitions.Count)
						{
							RowDefinition rowDef = new RowDefinition();
							rowDef.Height = TransferCourseGrid.RowDefinitions.LastOrDefault().Height;
							TransferCourseGrid.RowDefinitions.Add(rowDef);
						}

						Grid.SetRow(tbList[i], row);
						Grid.SetColumn(tbList[i], i);
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
