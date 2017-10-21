using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using GraduationHelper.Utils;
using System.Windows.Forms;

namespace GraduationHelper.Views
{
	public enum DataGridType
	{
		CourseTranscript,
	}
	public partial class ViewDataGrid : Control
	{
		private System.Windows.Forms.Timer _timer;
		public DataGridView DataTable
		{
			get { return _dataGridView; }
		}
		private Dictionary<string, string[]> _coursesData;
		private Control _parentForm;
		private List<DataGridViewColumn> _columns;
		private Dictionary<string, DataGridViewCell> _cellTemplatesTable;

		private int _timerInterval = 5000;
		private int _gradeColumnIndex;
		private int _rowIndex;
		private int _colIndex;


		private static string[] _columnHeaderNames = new string[] 
		{
			"Course",
			"Title",
			"Units",
			"Semester",
			"Year",
			"Grade"
		};
		
		public ViewDataGrid(Control parent, Dictionary<string, string[]> courses)
		{
			InitializeComponent();

			_dataGridView.ScrollBars = ScrollBars.Vertical;
		
			if (parent != null)
			{
				_parentForm = parent;
				SetSizeBindingToParent(_parentForm);
			}

			if(courses != null && courses.Count > 0)
			{
				_coursesData = courses;
				SetAndCreateTable(_coursesData);
			}

			if(parent.Size != null)
			{
				_dataGridView.Size = parent.Size;
				
				_dataGridView.CellMouseMove += CellMouse_OnMouseMove;
			}
		}

		private void CellMouse_OnMouseMove(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (e.ColumnIndex != _gradeColumnIndex)
				return;

			if (e.ColumnIndex > _columns.Count || e.RowIndex > _dataGridView.RowCount)
				return;
			
			if (_timer == null)
			{
				_timer = new System.Windows.Forms.Timer()
				{
					Interval = _timerInterval,
				};
			}

			_colIndex = e.ColumnIndex;
			_rowIndex = e.RowIndex;

	//		var cell = _dataGridView[_colIndex, _rowIndex];

//			cell.Style.BackColor = Color.White;
			
		}
		
		public void MouseOverShowGrade(object o, EventArgs e)
		{

		}

		public void SetSizeBindingToParent(Control p)
		{
			_parentForm = p;
			_dataGridView.SizeChanged += new EventHandler(ViewDataGrid_SizeChanged);

			if(p != null)
			{
				// Lamba to resize this control when parent resizes.
				p.SizeChanged += (o, e) => 
				{
					if(_dataGridView.Size != p.Size)
					{
						_dataGridView.Size = p.Size - new Size(50, 50);
					}
				};
			}
		}

		private void ViewDataGrid_SizeChanged(object sender, EventArgs e)
		{
			UpdateColumnWidths();
		}

		public void UpdateColumnWidths()
		{


		}
		
		public void BuildColumnHeaders(DataGridType t)
		{
			if( t == DataGridType.CourseTranscript)
			{
				DataGridViewColumn dgvc;
				DataGridViewCell dgc;

				for (int i = 0; i < _columnHeaderNames.Length; i++)
				{
					dgc = new DataGridViewTextBoxCell();
					dgc.Style.BackColor = Color.White;
					dgc.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
					
					dgvc = new DataGridViewColumn()
					{
						Name = _columnHeaderNames[i],
						HeaderText = _columnHeaderNames[i],
						ValueType = typeof(String),
						CellTemplate = dgc,
						AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells
					};
					
					if (_cellTemplatesTable == null)
						_cellTemplatesTable = new Dictionary<string, DataGridViewCell>();

					if (_columns == null)
						_columns = new List<DataGridViewColumn>();

					_cellTemplatesTable.Add(_columnHeaderNames[i], dgc);
					_dataGridView.Columns.Add(dgvc);
					_columns.Add(dgvc);
				}
			}
		}

		public void SetAndCreateTable(Dictionary<string, string[]> c)
		{
			string typeOfTable = "";

			if (c.ContainsKey("TableType"))
				typeOfTable = c["TableType"].FirstOrDefault();
			
			if(typeOfTable == DataGridType.CourseTranscript.ToString())
			{
				BuildColumnHeaders(DataGridType.CourseTranscript);
				BuildCourseTranscriptTable(c);
			}		
		}
		
		public void BuildCourseTranscriptTable(Dictionary<string,string[]> c)
		{
			int rowIndex;
			string[] rowData;

			_gradeColumnIndex  = _columns
								.Where(yy => yy.Name == "Grade")
								.Select(yy => yy.Index)
								.FirstOrDefault();

			foreach(var entry in c)
			{
				if (entry.Key == "TableType")
					continue;

				rowData = entry.Value;
				rowIndex = _dataGridView.Rows.Add();

				_dataGridView.Rows[rowIndex].Tag = entry.Key;

				// Need to set the Grade Cell Initialially hidden.
				for (int i = 0; i < rowData.Length; i++)
				{
					_dataGridView.Rows[rowIndex].Cells[i].Value = rowData[i];

					if (i == _gradeColumnIndex)
						_dataGridView.Rows[rowIndex].Cells[i].Style.BackColor = Color.Black;
				}
			}
			int yearColn = _columns.Where(yy => yy.Name == "Year")
									.Select(yy => yy.Index)
									.FirstOrDefault();

			var sortCol = _dataGridView.Columns[yearColn > -1 ? yearColn : 0];

			_dataGridView.Sort(sortCol, ListSortDirection.Ascending);
			
		}
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			base.OnPaint(pe);
		}
	}
}
