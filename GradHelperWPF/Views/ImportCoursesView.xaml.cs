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
			try
			{
				if (e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop))
				{
					// Get the extension.
					var files = e.Data.GetData(DataFormats.FileDrop) as string[];

					int xls=0, xlsx=0;

					string file;

					// Get a count of how many files need to be imported.
					for(int i = 0; i < files.Length; i++)
					{
						file = files[i].ToLower();

						if (file.EndsWith(".xls"))
							xls++;
						else if (file.EndsWith(".xlsx"))
							xlsx++;
					}

					if (xls == 0 && xlsx == 0)
						return;

					var xlsFiles = files.Where(ff => ff.ToLower().EndsWith(".xls"));

					var xlsxFiles = files.Where(ff => ff.ToLower().EndsWith(".xlsx"));

					foreach( var s in xlsxFiles)
					{
						ExcelModel exmodel = new ExcelModel(s);

						var data = exmodel.DataTable;

						foreach(var entry in data)
						{
							var arr = entry.Value.Split(new char[] { '|'}, StringSplitOptions.RemoveEmptyEntries);

							List<string> list = new List<string>();

							int i = 0;
							foreach(var str in arr)
							{
								i++;
								list.Add(str);
								if (i == 4)
									break;
							}


							BuildGridRow(list);
						}
					}
					foreach (var s in xlsFiles)
					{
						ExcelModel exmodel = new ExcelModel(s);
						var data = exmodel.DataTable;
					}
				}
			}
			catch(Exception ex)
			{
			//	throw new Exception(ex.StackTrace);
			}
		}

		private List<TextBox> BuildGridRow(List<string> data)
		{
			// Need 5 columns for 5 textboxes, only for the initial load.
			if( TableGrid != null && TableGrid.ColumnDefinitions.Count < 5)
			{
				TableGrid.ColumnDefinitions.Clear();

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

					TableGrid.ColumnDefinitions.Add(colDef);
				}
			}

			RowDefinition rowDef = new RowDefinition()
			{
				Height = GridLength.Auto
			};

			TableGrid.RowDefinitions.Add(rowDef);

			int currentRowIndex = TableGrid.RowDefinitions.IndexOf(rowDef);

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

				TableGrid.Children.Add(tb);

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
	}
}
