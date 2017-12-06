using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GradHelperWPF.Models
{
	public class ExcelCell
	{
		/// <summary>
		/// Row number from the excel file.
		/// </summary>
		public int Row { set; get; }

		/// <summary>
		/// Column number from the excel file.
		/// </summary>
		public int Column { set; get; }

		/// <summary>
		/// Value from the cell at (row,column)
		/// </summary>
		public String Value { set; get; }

		/// <summary>
		/// Name of the column header under which the value was retrieved.
		/// </summary>
		public String HeaderName { set; get; }
		
		public int RowInGrid { set; get; }
		public int ColumnInGrid { set; get; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rowCol"> row, col </param>
		/// <param name="val"></param>
		public ExcelCell(string rowCol, string val, string header)
		{
			int[] arr = SplitRowCol(rowCol);
			Row = arr[0];
			Column = arr[1];
			Value = val;
			HeaderName = header;
		}
		public ExcelCell(int row, int col, string val, string header)
		{
			Row = row;
			Column = col;
			Value = val;
			HeaderName = header;
		}
		public int[] SplitRowCol(string str)
		{
			int[] arr = new int[2];

			try
			{
				int indexOfNonDigit = str.Where(c => !Char.IsDigit(c))
						 .Select(c => str.IndexOf(c))
						 .FirstOrDefault();

				Char[] nonDigitChar = new Char[] { str[indexOfNonDigit] };

				string[] split = str.Split(nonDigitChar, StringSplitOptions.RemoveEmptyEntries);

				if (split.Length == arr.Length)
				{
					int.TryParse(split[0], out arr[0]);
					int.TryParse(split[1], out arr[1]);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.StackTrace);
			}
			return arr;
		}
	}


}

