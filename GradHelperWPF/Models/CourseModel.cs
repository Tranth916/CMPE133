using System;
using System.Collections;
using System.Xaml.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GradHelperWPF.Interfaces;
using Prism.Mvvm;

namespace GradHelperWPF.Models
{
    public class CourseModel : BindableBase
    {
		// This dictionary is going to available to all views for now...
		private static Dictionary<string, CourseModel> _coursesDictionary;
		public static void BuildTransferCourseRow(ref CourseModel cm, ref List<ExcelCell> currentRowCells, ref Dictionary<string,CourseModel> courses)
		{
			cm.SJSUCourse = new CourseModel();
			string transferCourseVal = currentRowCells
											.Where(c => c.HeaderName == "Transfer Course")
											.Select(c => c.Value)
											.FirstOrDefault();

			string sjsuCourseVal = currentRowCells.Where(c => c.HeaderName == "SJSU Course")
												  .Select(c => c.Value)
												  .FirstOrDefault();
			// not a valid course...
			if (string.IsNullOrEmpty(transferCourseVal) || string.IsNullOrEmpty(sjsuCourseVal))
				return;

			var split = transferCourseVal.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			var split2 = sjsuCourseVal.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (split != null && split.Length == 2)
			{
				cm.CourseAbbreviation = split[0];
				cm.CourseNumber = split[1];

				cm.SJSUCourse.CourseAbbreviation = split2[0];
				cm.SJSUCourse.CourseNumber = split2[1];
			}

			// going to query like this because a simple for loop causes too much issues.
			// same column names
			var unitsFromTransferCollege = currentRowCells.Where(c => c.HeaderName == "Units").FirstOrDefault();
			var unitsAtSJSU = currentRowCells.Where(c => c.HeaderName == "Units").LastOrDefault();
			cm.CourseUnit = unitsFromTransferCollege != null ? unitsFromTransferCollege.Value : "";
			cm.SJSUCourse.CourseUnit = unitsAtSJSU != null ? unitsAtSJSU.Value : "";

			var transferGrade = currentRowCells.Where(c => c.HeaderName == "Grade").FirstOrDefault();
			var sjsuGrade = currentRowCells.Where(c => c.HeaderName == "Grade").LastOrDefault();
			cm.CourseGrade = transferGrade != null ? transferGrade.Value : "";
			cm.SJSUCourse.CourseGrade = sjsuGrade != null ? sjsuGrade.Value : "";

			var transferCourseTitle = currentRowCells.Where(c => c.HeaderName == "Transfer Course Title").FirstOrDefault();
			var sjsuCourseTitle = currentRowCells.Where(c => c.HeaderName == "SJSU Course Title").FirstOrDefault();
			cm.CourseTitle = transferCourseTitle != null ? transferCourseTitle.Value : "";
			cm.SJSUCourse.CourseTitle = sjsuCourseTitle != null ? sjsuCourseTitle.Value : "";

			//Institution
			var tranfserInstitutionName = currentRowCells.Where(c => c.HeaderName == "Transfer Institution").FirstOrDefault();
			cm.Institution = tranfserInstitutionName != null ? tranfserInstitutionName.Value : "";

			cm.SJSUCourse.IsTransferCourse = false;

			var sjsuCourseExcelCell = currentRowCells.FirstOrDefault(c => c.HeaderName == "SJSU Course");
			//assign its neighbors.
			int startingIndexOfSJSUCourse = currentRowCells.IndexOf(sjsuCourseExcelCell);
			if (startingIndexOfSJSUCourse > 0 && startingIndexOfSJSUCourse < currentRowCells.Count)
			{
				var listForCM = currentRowCells.GetRange(0, startingIndexOfSJSUCourse - 1);
				var listForSJSU = currentRowCells.Where(c => !listForCM.Contains(c)).ToList();

				cm.ExcelCellsList = listForCM;
				cm.SJSUCourse.ExcelCellsList = listForSJSU;
			}
			if (!courses.ContainsKey(cm.ToString()))
				courses.Add(cm.ToString(), cm);
			if (!courses.ContainsKey(cm.SJSUCourse.ToString()))
				courses.Add(cm.SJSUCourse.ToString(), cm.SJSUCourse);
		}
		public static void BuildSJSUCourseRow(ref CourseModel cm, ref List<ExcelCell> cells, ref Dictionary<string,CourseModel> courses)
		{

		}

		public static Dictionary<string, CourseModel> CoursesDictionary
		{
			set
			{
				_coursesDictionary = value;
			}
			get
			{
				if (_coursesDictionary == null)
					_coursesDictionary = new Dictionary<string, CourseModel>();
				return _coursesDictionary;
			}
		}
		public static Dictionary<string, CourseModel> BuildCourseDictionary(List<ExcelCell> cells)
		{
			Dictionary<string, CourseModel> courses = new Dictionary<string, CourseModel>();
			int maxNumOfRows = cells.Max(c => c.Row);
			for (int i = 0; i < maxNumOfRows; i++)
			{
				// group columns by their row #.
				var currentRowCells = cells.Where(c => c.Row == i).OrderBy(c => c.Column).ToList();
				if (currentRowCells.Count == 0)
					continue;
				CourseModel cm = new CourseModel();
				cm.ExcelCellsList = currentRowCells;
				// need to distinguish between transfer & non-transfer course excel files & handle them separately.
				cm.IsTransferCourse = currentRowCells.Where(cell => cell.HeaderName.Contains("Transfer")).Count() > 0;
				if (cm.IsTransferCourse)
				{
					BuildTransferCourseRow(ref cm, ref currentRowCells, ref courses);
				}
				else
				{
					BuildSJSUCourseRow(ref cm, ref currentRowCells, ref courses);
				}
			}
			foreach (var entry in courses)
			{
				if (CoursesDictionary.ContainsKey(entry.Key))
					CoursesDictionary[entry.Key] = entry.Value;
				else
					CoursesDictionary.Add(entry.Key, entry.Value);
			}
			return courses;
		}

		private String _courseAbrreviation;
        private String _courseNumber                ;
        private String _courseTitle                 ;
        private String _courseSemester              ;
        private String _courseYear                  ;
        private String _courseGrade                 ;
        private String _courseUnit                  ;
        private String _courseGradePoint            ;
        private String _courseRequirementDesignation;
		private String _institution					;
		private bool _isTransferCourse				;

		public String CourseAbbreviation
        {
            set
            {
                if (_courseAbrreviation == null)
                    _courseAbrreviation = value;
                else if (_courseAbrreviation == value)
                    return;
                else
                {
                    _courseAbrreviation = value;
                    RaisePropertyChanged("CourseAbbreviation");
                }
            }
            get { return _courseAbrreviation ?? ""; }
        }
        public String CourseNumber
        {
            set {
                if (_courseNumber == null)
                    _courseNumber = value;
                else if (_courseNumber == value)
                    return;
                else
                {
                    _courseNumber = value;
                    OnPropertyChanged("CourseNumber");
                }
            }
            get { return _courseNumber ?? ""; }
        }
        public String CourseTitle
        {
            set
            {
                if (_courseTitle == null)
                {
                    _courseTitle = value;
                }
                if(value != _courseTitle)
                {
                    _courseTitle = value;
                    OnPropertyChanged("CourseTitle");
                }
            }
            get { return _courseTitle ?? ""; }
        }
        public String CourseSemester
        {
            set
            {
                if (_courseSemester == null)
                    _courseSemester = value;
                else if(_courseSemester != value)
                {
                    _courseSemester = value;
                    OnPropertyChanged("CourseSemester");
                }
            }
            get { return _courseSemester ?? ""; }
        }
        public String CourseYear
        {
            set
            {
                if (_courseYear == null)
                    _courseYear = value;
                else if(_courseYear != value)
                {
                    _courseYear = value;
                    OnPropertyChanged("CourseYear");
                }
            }
            get { return _courseYear ?? ""; }
        }
        public String CourseGrade
        {
            set
            {
                if (_courseGrade == null)
                    _courseGrade = value;
                else if(_courseGrade != value)
                {
                    _courseGrade = value;
                    OnPropertyChanged("CourseGrade");
                }
            }
            get { return _courseGrade ?? ""; }
        }
        public String CourseUnit
        {
            set {
                if (_courseUnit == null) _courseUnit = value;
                else if(_courseUnit != value)
                {
                    _courseUnit = value;
                    OnPropertyChanged("CourseUnit");
                }
            }
            get { return _courseUnit ?? ""; }
        }
        public String CourseGradePoint
        {
            set
            {
                if (_courseGradePoint == null) _courseGradePoint = value;
                else if(_courseGradePoint != value)
                {
                    _courseGradePoint = value;
                    OnPropertyChanged("CourseGradePoint");
                }
            }
            get { return _courseGradePoint ?? ""; }
        }
        public String CourseRequirementDesignation
        {
            set
            {
                if (_courseRequirementDesignation == null) _courseRequirementDesignation = value;
                else if( _courseRequirementDesignation != value)
                {
                    _courseRequirementDesignation = value;
                    OnPropertyChanged("CourseRequirementDesignation");
                }
            }
            get { return _courseRequirementDesignation ?? ""; }
        }

		/// <summary>
		/// If TransferCourse is true, then have the CourseModel obj own a SJSU<CourseModel>.
		/// </summary>
		public bool IsTransferCourse
		{
			set
			{
				_isTransferCourse = value;
			}
			get { return _isTransferCourse; }
		}
		public String Institution
		{
			set
			{
				if (_institution == null)
					_institution = value;
				else if (_institution != value)
				{
					_institution = value;
					OnPropertyChanged("Institution");
				}
			}
			get { return _institution ?? ""; }
		}
		public CourseModel SJSUCourse
		{
			set;get;
		}
		/// <summary>
		/// The list of ExcelCells that made up of this Course object.
		/// </summary>
		public List<ExcelCell> ExcelCellsList { set; get; }
        public CourseModel() { }        
        /// <summary>
        /// Assign the value to the best matching string properties.
        /// </summary>
        /// <param name="data"></param>
        public void AssignData( KeyValuePair<string, string> data )
        {
            string key = data.Key.ToLower();

            if (key.Contains("abbr"))
                key = "CourseAbbreviation";
            else if (key.Contains("number") || key.Contains("no"))
                key = "CourseNumber";
            else if (key.Contains("title") || key.Contains("description"))
                key = "CourseTitle";
            else if (key.Contains("semester") || key.Contains("fall") || key.Contains("spring") || key.Contains("summer"))
                key = "CourseSemester";
            else if (key.Contains("year") || (key.StartsWith("20") && key.Length == 4))
                key = "CourseYear";
            else if (key.Contains("unit") || (key.Contains(".") && float.TryParse(key, out var f)))
                key = "CourseUnit";
            else if (key.Contains("grade"))
                key = "CourseGrade";
            else if (key.Contains("req"))
                key = "CourseRequirementDesignation";

            switch ( key )
            {
                case "CourseAbbreviation":
                    CourseAbbreviation = data.Value;
                    break;
                case "CourseNumber":
                    CourseNumber = data.Value;
                    break;
                case "CourseTitle":
                    CourseTitle = data.Value;
                    break;
                case "CourseSemester":
                    CourseSemester = data.Value;
                    break;
                case "CourseYear":
                    CourseYear = data.Value;
                    break;
                case "CourseGrade":
                    CourseGrade = data.Value;
                    break;
                case "CourseUnit":
                    CourseUnit = data.Value;
                    break;
                case "CourseGradePoint":
                    CourseGradePoint = data.Value;
                    break;
                case "CourseRequirementDesignation":
                    CourseRequirementDesignation = data.Value;
                    break;

                default:
                    Console.WriteLine("Could not find where to assign value: "+key);
                    break;
            }
        }
		public override string ToString()
		{
			return CourseAbbreviation + " " + CourseNumber + " " + CourseTitle;
		}
	}
}
