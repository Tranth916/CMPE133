using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationHelper.Models
{
	public class Student
	{
		public Student()
		{

		}

		#region Personal Info
		public string FirstName
		{
			set; get;
		}
		public string MiddleName
		{
			set;
			get;
		}
		public string MiddleInitial
		{
			set; get;
		}

		public string LastName
		{
			set; get;
		}
		public int StudentID
		{
			set; get;
		}
		public string Major
		{
			set; get;
		}
		#endregion




	}
}
