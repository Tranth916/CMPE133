using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraduationHelper.Views
{
	public partial class InfoForm : Form
	{
		public InfoForm()
		{
			InitializeComponent();
		}

		public List<Label> LabelsList
		{
			get
			{
				return new List<Label>()
				{
					this.label8,
					this.label9,
					this.label10,
					this.label11,
					this.label12,
					this.label13,
				};
			}
		}

		public TableLayoutPanel GetTable()
		{
			TableLayoutPanel tlp = this.tableLayoutPanel1;
			Dictionary<Label, TextBox> dict = new Dictionary<Label, TextBox>();

			List<Label> labels = LabelsList;
			int currentRow, currentCol;

			for(int i = 0; i < labels.Count; i++)
			{
				labels[i].Text = "";
				currentRow = tlp.GetRow(labels[i]);
				currentCol = tlp.GetColumn(labels[i]);




			}
			tlp.Tag = dict;
			
			return tlp;
		}
	}
}
