using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraduationHelper.Utils
{
	public class ViewHelper
	{
		public static TableLayoutPanel BuildTablePanel(string[] data, int width = -1, int height = -1)
		{
			int sizeWidth = width > 0 ? width : 500;
			int sizeHeight = height > 0 ? height : 500;

			TableLayoutPanel tlp = new TableLayoutPanel()
			{
				ColumnCount = 2,
				Padding = new Padding(5, 5, 5, 5),
				RowCount = data.Length,
				Size = new System.Drawing.Size(sizeWidth, sizeHeight),
			};
			
			for(int i = 0; i < data.Length; i++)
			{
				Label rowLabel = new Label()
				{
					Text = data[i],
					TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
					Name = data[i] + "Label",
				};

				TextBox rowTextBox = new TextBox()
				{
					Name = data[i] + "TextBox",
				};

				tlp.Controls.Add(rowLabel, 0, i);
				tlp.Controls.Add(rowTextBox, 1, i);

				tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50f));
				tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
			}
			return tlp;
		}











	}
}
