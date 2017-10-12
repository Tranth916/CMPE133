using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationHelper.Models
{
	public class Course
	{
		public string Description
		{
			set;
			get;
		}
		public string Grade
		{
			set;
			get;
		}
		public string Semester
		{
			set;
			get;
		}
		public int Year
		{
			set;
			get;
		}
		public float Units
		{
			set;
			get;
		}

		public Course()
		{

		}

		public override string ToString()
		{
			return base.ToString();
		}
	}
}
