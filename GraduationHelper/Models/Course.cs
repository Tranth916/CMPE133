using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
		public string GEDesignation
		{
			private set; get;
		}
		#endregion

		public Course(List<string> strs)
		{
			OriginalString = strs;

			if (strs.Contains("GE"))
			{
				IsGeneralEd = true;
				SetGEDesignation();
			}
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prop">
		/// Season
		/// CourseAbbreviation
		/// Year
		/// Units
		/// Course Title
		/// </param>
		/// <param name="indexes"></param>
		/// <param name="ii"></param>
		/// <returns></returns>
		public string BuildCourseFromArray(string prop, int[] indexes, int ii = -1)
		{
			StringBuilder sb = new StringBuilder();
			string ret = "";
			try
			{

				switch (prop)
				{
					case "Season":
						if (ii > 0)
							Semester = OriginalString[ii];
						ret = Semester;
						break;

					case "Course Abbreviation":
						if (ii > -1)
							CourseAbbreviation = OriginalString[ii];
						ret = CourseAbbreviation;
						break;

					case "Year":
						if (ii > -1)
							Year = int.Parse(OriginalString[ii]);
						ret = Year.ToString();
						break;

					case "Units":
						if (ii > -1)
							Units = float.Parse(OriginalString[ii]);
						ret = Units.ToString();
						break;

					case "Course Title":
						for (int i = 0; i < indexes.Length; i++)
						{
							sb.Append(OriginalString[indexes[i]] + " ");
						}
						CourseTitle = sb.ToString() ?? "";
						ret = CourseTitle;
						break;

					case "Course Number":
						if (ii > -1)
							CourseNumber = OriginalString[ii];
						ret = CourseNumber;
						break;

					case "Grade":
						if (ii > -1)
							Grade = OriginalString[ii];
						ret = Grade;
						break;

				}
			}
			catch (Exception ex)
			{
			}

			return ret;
		}
		
		public override string ToString()
		{
			return CourseAbbreviation + CourseNumber;	
		}
	}
}
