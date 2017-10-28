using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
	public class Course
	{
		#region Properties
		public List<string> OriginalString
		{
			set; get;
		}
		public string CourseTitle
		{
			set;
			get;
		}
		public string CourseNumber
		{
			set; get;
		}
		public string CourseAbbreviation
		{
			set;
			get;
		}
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
		public bool IsGeneralEd
		{
			set; get;
		}
		public string YearStr { set; get; }
		public string GEDesignation
		{
			private set; get;
		}
		public string CourseUnits { set; get; }
		public string ETC { set; get; }
		public string EntryKey
		{
			get { return CourseAbbreviation + CourseNumber + CourseTitle; }
		}
		public string FullName
		{
			get
			{
				return CourseAbbreviation + " " +
						CourseNumber + " " +
						CourseTitle + " " +
						CourseUnits + " " +
						YearStr + " " +
						Grade;
			}
		}
		public string[] DataArray
		{
			get
			{
				return new string[]
				{
					CourseAbbreviation + CourseNumber,
					CourseTitle,
					CourseUnits,
					Semester,
					YearStr,
					Grade
				};
			}
		}
		#endregion

		public Course(Dictionary<string, string> info)
		{
			if (info != null)
				SetCourseInfo(info);
		}
		public Course(List<string> strs)
		{
			OriginalString = strs;
		}
		private void SetCourseInfo(Dictionary<string, string> info)
		{
			CourseAbbreviation = info[CourseDictionaryContent.CourseAbbreviation.ToString()];
			CourseNumber = info[CourseDictionaryContent.CourseNumber.ToString()];
			Semester = info[CourseDictionaryContent.Season.ToString()];
			YearStr = info[CourseDictionaryContent.Year.ToString()];
			Grade = info[CourseDictionaryContent.Grade.ToString()];
			CourseUnits = info[CourseDictionaryContent.Unit.ToString()];
			CourseTitle = info[CourseDictionaryContent.CourseTitle.ToString()];
			ETC = info[CourseDictionaryContent.ETC.ToString()];
		}
		public override string ToString()
		{
			return CourseAbbreviation + CourseNumber + " " + CourseTitle + " " + Semester + " " + YearStr;
		}
	}
}
