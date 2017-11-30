using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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








	}
}
