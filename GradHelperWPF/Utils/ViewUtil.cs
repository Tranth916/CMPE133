using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GradHelperWPF.Models;

namespace GradHelperWPF.Utils
{
	public class ViewUtil
	{
		public static bool AddRowToGrid(ref Grid grid, RowDefinition toClone = null, string height = null)
		{
			GridLength rowHeight = new GridLength();
			
			if( toClone != null)
			{
				rowHeight = toClone.Height;
			}
			else if( !string.IsNullOrEmpty(height) )
			{
				var converter = new GridLengthConverter();
				rowHeight = (GridLength) converter.ConvertFromString(height);
			}

			ColumnDefinition colDef = new ColumnDefinition();

			grid.ColumnDefinitions.Add(colDef);

			return false;
		}
		public static bool AddExcelCellToGrid(ref Grid grid, List<ExcelCell> cells)
		{
			int processed = 0;
			foreach( ExcelCell cell in cells)
			{
				AddExcelCellToGrid(ref grid, cell);
				processed++;
			}
			return processed > 0;
		}		
		/// <summary>
		/// Creates a new textbox and add it into the grid.
		/// </summary>
		/// <param name="grid"></param>
		/// <param name="cell"></param>
		/// <returns></returns>
		public static bool AddExcelCellToGrid(ref Grid grid, ExcelCell cell)
		{
			TextBox tb = new TextBox()
			{
				Text = cell.Value,
				Width = 50f,
				Tag = cell,
			};

			grid.Children.Add(tb);			
			if (cell.Column < grid.ColumnDefinitions.Count)
				Grid.SetColumn(tb, cell.Column);
			
			return true;
		}

		public static bool AddCourseRowToGrid(ref Grid grid, List<CourseModel> data)
		{
			int processed = 0;
			UIElement[] children = new UIElement[grid.Children.Count];
			// save the existing rows first.
			grid.Children.CopyTo(children, 0);
			grid.Children.Clear();

			var headerLabels = children.Where(c => (c is Label) ).Select( c=> c as Label );
			foreach( var header in headerLabels )
			{
				if (string.IsNullOrEmpty(header.Name) || !header.Name.Contains("Header"))
					continue;
				string colIndex = header.Name.LastOrDefault().ToString();
				int colInt;

				if( int.TryParse(colIndex, out colInt))
				{
					grid.Children.Add(header);
					Grid.SetColumn(header, colInt);
				}
			}
			
			var rowDefz = grid.RowDefinitions.ToList();
			var colDefz = grid.ColumnDefinitions.ToList();		
			string text = "";
			int row = 1;

			foreach (var course in data)
			{
				++row;
				if (row > grid.RowDefinitions.Count)
					grid.RowDefinitions.Add(new RowDefinition() { });
				TextBox[] tbs = new TextBox[colDefz.Count];
				for (int i = 0; i < tbs.Length; i++)
				{
					tbs[i] = new TextBox();					
					switch (i)
					{
						case 0:
							text = course.CourseAbbreviation;
							break;
						case 1:
							text = course.CourseNumber;
							break;
						case 2:
							text = course.CourseTitle;
							break;
						case 3:
							text = course.CourseUnit;
							break;
						case 4:
							text = course.CourseGrade;
							break;
						case 5:
							text = course.Institution;
							break;
					}

					tbs[i].Text = text;
					tbs[i].Tag = course;
					grid.Children.Add(tbs[i]);
					Grid.SetColumn(tbs[i], i);
					Grid.SetRow(tbs[i], row);
					processed++;
				}				
			}

			return processed > 0;
		}
	}
}
