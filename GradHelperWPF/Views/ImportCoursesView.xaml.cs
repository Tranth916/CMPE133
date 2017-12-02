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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GradHelperWPF.Model;
using GradHelperWPF.Models;

namespace GradHelperWPF.Views
{
    /// <summary>
    /// Interaction logic for ImportCoursesView.xaml
    /// </summary>
    public partial class ImportCoursesView : Grid
    {
        public ImportCoursesView()
        {
            InitializeComponent();
			Init();
		}

		private void Init()
		{
			AllowDrop = true;
			PreviewDragOver += new DragEventHandler(Grid_PreviewDragOver);
			Drop += new DragEventHandler(Grid_Drop);
		}

		private void Grid_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
		}

		private void Grid_Drop(object sender, DragEventArgs e)
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

				foreach (var entry in table)
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

						if (split.Length == 2)
						{
							TextBox tb1 = new TextBox()
							{
								Text = split[0],
								Tag = "Course"
							};

							if (split[1].Length == 2)
								split[1] = $"0{split[1]}";

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
					if (!textboxes.ContainsKey(rr))
					{
						textboxes.Add(rr, new List<TextBox>() { tb });
					}
					else
					{

						textboxes[rr].Add(tb);
					}
				}

				int row = TransferCourseGrid.RowDefinitions
											.Where(r => !string.IsNullOrEmpty(r.Name) && r.Name.StartsWith("Labels"))
											.Select(r => TransferCourseGrid.RowDefinitions.IndexOf(r))
											.FirstOrDefault() + 2;

				int colI = TransferCourseGrid.ColumnDefinitions
								.Where(cd => cd.Name == "GradeColDef")
								.Select(cd => TransferCourseGrid.ColumnDefinitions.IndexOf(cd))
								.FirstOrDefault();

				// now populate the rows
				foreach (var tbList in textboxes.Values)
				{
					for (int i = 0; i < tbList.Count; i++)
					{
						if (tbList[i].Tag == null
							|| (tbList[i].Tag as String).Contains("Grd")
							|| (tbList[i].Tag as String).Contains("Reqmnt"))
							continue;

						TransferCourseGrid.Children.Add(tbList[i]);
						if (row >= TransferCourseGrid.RowDefinitions.Count)
						{
							RowDefinition rowDef = new RowDefinition();
							rowDef.Height = TransferCourseGrid.RowDefinitions.LastOrDefault().Height;
							TransferCourseGrid.RowDefinitions.Add(rowDef);
						}

						Grid.SetRow(tbList[i], row);
						// Need to place the textbox in the correct column.
						if ((string)tbList[i].Tag == "Grade")
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

		//private void Grid_Drop(object sender, DragEventArgs e)
		//{
		//	try
		//	{
		//		if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
		//		{
		//			// Get the extension.
		//			var files = e.Data.GetData(DataFormats.FileDrop) as string[];

		//			int xls=0, xlsx=0;

		//			string file;

		//			// Get a count of how many files need to be imported.
		//			for(int i = 0; i < files.Length; i++)
		//			{
		//				file = files[i].ToLower();

		//				if (file.EndsWith(".xls"))
		//					xls++;
		//				else if (file.EndsWith(".xlsx"))
		//					xlsx++;
		//			}

		//			if (xls == 0 && xlsx == 0)
		//				return;

		//			var xlsFiles = files.Where(ff => ff.ToLower().EndsWith(".xls"));

		//			var xlsxFiles = files.Where(ff => ff.ToLower().EndsWith(".xlsx"));

		//			foreach( var s in xlsxFiles)
		//			{
		//				ExcelModel exmodel = new ExcelModel(s);

		//				var data = exmodel.DataTable;

		//				foreach(var entry in data)
		//				{
		//					var arr = entry.Value.Split(new char[] { '|'}, StringSplitOptions.RemoveEmptyEntries);

		//					List<string> list = new List<string>();

		//					int i = 0;
		//					foreach(var str in arr)
		//					{
		//						i++;
		//						list.Add(str);
		//						if (i == 4)
		//							break;
		//					}


		//					BuildGridRow(list);
		//				}
		//			}
		//			foreach (var s in xlsFiles)
		//			{
		//				ExcelModel exmodel = new ExcelModel(s);
		//				var data = exmodel.DataTable;
		//			}
		//		}
		//	}
		//	catch(Exception ex)
		//	{
		//	//	throw new Exception(ex.StackTrace);
		//	}
		//}

		private List<TextBox> BuildGridRow(List<string> data)
		{
			// Need 5 columns for 5 textboxes, only for the initial load.
			if( TransferCourseGrid != null && TransferCourseGrid.ColumnDefinitions.Count < 5)
			{
				TransferCourseGrid.ColumnDefinitions.Clear();

				// ENGR | 102 | INTRO  TO ... | UNIT | GRADE
				for(int i = 0; i < 5; i++)
				{
					string length = "";

					switch (i)
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

					ColumnDefinition colDef = new ColumnDefinition()
					{
						Width = (GridLength) (new GridLengthConverter().ConvertFromString(length)),
					};

					TransferCourseGrid.ColumnDefinitions.Add(colDef);
				}
			}

			RowDefinition rowDef = new RowDefinition()
			{
				Height = GridLength.Auto
			};

			TransferCourseGrid.RowDefinitions.Add(rowDef);

			int currentRowIndex = TransferCourseGrid.RowDefinitions.IndexOf(rowDef);

			if (currentRowIndex < 0)
				throw new Exception("Row Index is :" + currentRowIndex);

			List<TextBox> textboxes = new List<TextBox>();

			// Add a new text box for each column;
			for(int i = 0; i < data.Count; i++)
			{
				TextBox tb = new TextBox()
				{
					Text = data[i],					
				};

				TransferCourseGrid.Children.Add(tb);

				Grid.SetRow(tb, currentRowIndex);

				Grid.SetColumn(tb, i);

				textboxes.Add(tb);
			}

			return textboxes;
		}

		private void ImportBtn_OnClick(object sender, RoutedEventArgs e)
		{
			var ofd = new Microsoft.Win32.OpenFileDialog()
			{
				InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
				Filter = "*.xls | *.XLS | *.xlsx | *.XLSX",
				CheckFileExists = true,
			};

			bool? opened = ofd.ShowDialog();

			if( opened.Value )
			{
				string fileName = ofd.FileName;

				ExcelModel em = new ExcelModel(fileName);

				var data = em.DataTable;

				foreach( var row in data)
				{
					var split = row.Value.Split('|');

					List<string> firstFive = new List<string>();

					for (int i = 0; i < split.Length; i++)
					{
						if (i > 4)
							continue;

						if (split[i].Contains("Fall") || split[i].Contains("Spring"))
							continue;
						
						firstFive.Add(split[i]);
					}

					BuildGridRow(firstFive);
				}
			}			
		}


		private void TestMakeDummyRows()
		{
			Dictionary<string, List<string>> dummyData = new Dictionary<string, List<string>>();

			for(int i = 0; i < 10; i++)
			{
				string key = DateTime.Now.Second + "_time";

				List<string> list = new List<string>();
				for(int j = 0; j < 5; j++)
				{
					list.Add(" random ");
				}

				BuildGridRow(list);
			}
		}

		private void TestButton_Click(object sender, RoutedEventArgs e)
		{
			TextBox[] list = new TextBox[TransferCourseGrid.Children.Count];

			TransferCourseGrid.Children.CopyTo(list, 0);

			var grades = list.Where(l => l.Tag != null && (l.Tag as String) == "Grade");

			if (grades == null || grades.Count() == 0)
				return;

			// Load the major form doc.
			string resourceRunningPath = AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\majorform2016.docx";

			WordModel doc = new WordModel(resourceRunningPath);
			
			foreach( var tb in grades)
			{
				if (tb.Text.Contains("TCR"))
					continue;

				int i = Grid.GetRow(tb);

				var row = list.Where(tbx => Grid.GetRow(tbx) == i && !tbx.Equals(tb));

				string courseKey = "";

				foreach( var r in row)
				{
					if (r.Text == tb.Text)
						continue;

					if( !courseKey.Contains(r.Text) )
						courseKey += $" {r.Text}";
				}

				courseKey = courseKey.Trim();
				doc.WriteGradeToSJSUCourse(courseKey, tb.Text);
				//doc.WriteName("Tran");

			}

			doc.Close();
			doc.ShowDoc();
		}
	}
}
