using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraduationHelper.Utils;
namespace GraduationHelper.Models
{
	public class Course
	{
		#region Properties
		public string OriginalStringWord
		{
			set;get;
		}
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
		#endregion

		public Course(Dictionary<string,string> info)
		{
			if(info != null)
				SetCourseInfo(info);
		}
		public Course(List<string> strs)
		{
			OriginalString = strs;

			if (strs.Contains("GE"))
			{
				IsGeneralEd = true;
				SetGEDesignation();
			}
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

		private void SetGEDesignation()
		{
			foreach(string str in OriginalString)
			{
				OriginalStringWord += str + " ";
			}
			OriginalStringWord = OriginalStringWord.Trim();

			if (OriginalStringWord.Contains("A1: Oral Communication"))
				GEDesignation = "areaTB_A1";
			if (OriginalStringWord.Contains("A2: Written Comm"))
				GEDesignation = "areaTB_A2";
			if (OriginalStringWord.Contains("B1 + B3:  Physical + Lab Sci"))
				GEDesignation = "areaTB_B1";
			if (OriginalStringWord.Contains("B1 + B3:  Physical + Lab Sci"))
				GEDesignation = "areaTB_B3";
			if (OriginalStringWord.Contains("B2: Life Science"))
				GEDesignation = "areaTB_B2";
			if (OriginalStringWord.Contains("B4: Mathematical Concepts"))
				GEDesignation = "areaTB_B4";
			if (OriginalStringWord.Contains("GE C1: Arts"))
				GEDesignation = "areaTB_C1";
			if (OriginalStringWord.Contains("C2: Letters"))
				GEDesignation = "areaTB_C2";
			if (OriginalStringWord.Contains("GE: D"))
				GEDesignation = "areaTB_D1";
			if (OriginalStringWord.Contains("E: Human Understndng & Devl"))
				GEDesignation = "areaTB_E1";
		}

		public string GetDictionaryStr()
		{
			return CourseAbbreviation ?? "" + CourseNumber ?? "";
		}
		
		public override string ToString()
		{
			return CourseAbbreviation + CourseNumber + " " + CourseTitle + " " + Semester + " " + YearStr;
		}
	}
}
