using System;
using System.Linq;

namespace GradHelperWPF.Models
{
    public class ExcelCell
    {
        /// <summary>
        /// Constructor for ExcelCell class
        /// </summary>
        /// <param name="rowCol"> "row number, column number" </param>
        /// <param name="val">value at [row,col]</param>
        /// <param name="header">Name of the column header</param>
        public ExcelCell( string rowCol, string val, string header )
        {
            var arr = SplitRowCol(rowCol);
            Row = arr[0];
            Column = arr[1];
            Value = val;
            HeaderName = header;
        }

        public ExcelCell( int row, int col, string val, string header )
        {
            Row = row;
            Column = col;
            Value = val;
            HeaderName = header;
        }

        /// <summary>
        ///     Column number from the excel file.
        /// </summary>
        public int Column { set; get; }

        public int ColumnInGrid { set; get; }

        /// <summary>
        ///     Name of the column header under which the value was retrieved.
        /// </summary>
        public string HeaderName { set; get; }

        /// <summary>
        ///     Row number from the excel file.
        /// </summary>
        public int Row { set; get; }

        public int RowInGrid { set; get; }

        /// <summary>
        ///     Value from the cell at (row,column)
        /// </summary>
        public string Value { set; get; }

        public int[] SplitRowCol( string str )
        {
            var arr = new int[2];

            try
            {
                var indexOfNonDigit = str.Where(c => !char.IsDigit(c))
                    .Select(c => str.IndexOf(c))
                    .FirstOrDefault();

                char[] nonDigitChar = {str[indexOfNonDigit]};

                var split = str.Split(nonDigitChar, StringSplitOptions.RemoveEmptyEntries);

                if ( split.Length == arr.Length )
                {
                    int.TryParse( split[0], out arr[0] );
                    int.TryParse( split[1], out arr[1] );
                }
            }
            catch ( Exception ex )
            {
                throw new Exception( ex.StackTrace );
            }
            return arr;
        }
    }
}