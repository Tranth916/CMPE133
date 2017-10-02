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
		public static TableLayoutPanel BuildTablePanel(string[] data)
		{
			RowStyle rs = new RowStyle(SizeType.Absolute, 50F);

			TableLayoutPanel tlp = new TableLayoutPanel()
			{
				ColumnCount = 2,
				Padding = new Padding(5,5,5,5),
				RowCount = data.Length,
			};
			
			for(int i = 0; i < data.Length; i++)
			{
				Label l = new Label()
				{
					Text = data[i],
				};
				TextBox t = new TextBox()
				{
					Name = data[i] + "TextBox",
				};

				tlp.Controls.Add(l, 0, i);
				tlp.Controls.Add(t, 1, i);

				tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
			}

			
			return tlp;
		}











	}
}
