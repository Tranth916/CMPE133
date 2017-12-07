using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Mvvm;

namespace GradHelperWPF.Models
{
    public class CourseModel : BindableBase
    {
        // This dictionary is going to available to all views for now...
        private static Dictionary<string, CourseModel> _coursesDictionary;

        private string _courseAbrreviation;
        private string _courseGrade;
        private string _courseGradePoint;
        private string _courseNumber;
        private string _courseRequirementDesignation;
        private string _courseSemester;
        private string _courseTitle;
        private string _courseUnit;
        private string _courseYear;
        private string _institution;

        public static Dictionary<string, CourseModel> CoursesDictionary
        {
            set => _coursesDictionary = value;
            get
            {
                if (_coursesDictionary == null)
                    _coursesDictionary = new Dictionary<string, CourseModel>();
                return _coursesDictionary;
            }
        }

        public string CourseAbbreviation
        {
            set
            {
                if (_courseAbrreviation == null)
                {
                    _courseAbrreviation = value;
                }
                else if (_courseAbrreviation == value)
                {
                }
                else
                {
                    _courseAbrreviation = value;
                    RaisePropertyChanged("CourseAbbreviation");
                }
            }
            get => _courseAbrreviation ?? "";
        }

        public string CourseNumber
        {
            set
            {
                if (_courseNumber == null)
                {
                    _courseNumber = value;
                }
                else if (_courseNumber == value)
                {
                }
                else
                {
                    _courseNumber = value;
                    RaisePropertyChanged("CourseNumber");
                }
            }
            get => _courseNumber ?? "";
        }

        public string CourseTitle
        {
            set
            {
                var key = value;
                key = key.Replace("Engr ", "Engineering ");
                key = key.Replace("Org & Arch", "Organization and Architecture");
                key = key.Replace("Strc", "Structures");
                key = key.Replace("Progrmng", "Programming");
                key = key.Replace("Comp ", "Computer ");
                key = key.Replace("Inter ", "Interaction ");
                key = key.Replace("Struct & Alg", "Structures and Algorithms");
                key = key.Replace("Soft ", "Software ");
                key = key.Replace("SW ", "Software");
                key = key.Replace("Diff Eq ", "Differential Equations ");
                key = key.Replace("Intro ", "Introduction ");
                key = key.Replace("Orntd ", "Oriented");
                key = key.Replace("Dsgn", "Design");

                if (_courseTitle == null)
                    _courseTitle = key;
                if (value != _courseTitle)
                {
                    _courseTitle = key;
                    RaisePropertyChanged("CourseTitle");
                }
            }
            get => _courseTitle ?? "";
        }

        public string CourseSemester
        {
            set
            {
                if (_courseSemester == null)
                {
                    _courseSemester = value;
                }
                else if (_courseSemester != value)
                {
                    _courseSemester = value;
                    RaisePropertyChanged("CourseSemester");
                }
            }
            get => _courseSemester ?? "";
        }

        public string CourseYear
        {
            set
            {
                if (_courseYear == null)
                {
                    _courseYear = value;
                }
                else if (_courseYear != value)
                {
                    _courseYear = value;
                    RaisePropertyChanged("CourseYear");
                }
            }
            get => _courseYear ?? "";
        }

        public string CourseGrade
        {
            set
            {
                if (_courseGrade == null)
                {
                    _courseGrade = value;
                }
                else if (_courseGrade != value)
                {
                    _courseGrade = value;
                    RaisePropertyChanged("CourseGrade");
                }
            }
            get => _courseGrade ?? "";
        }

        public string CourseUnit
        {
            set
            {
                if (_courseUnit == null)
                {
                    _courseUnit = value;
                }
                else if (_courseUnit != value)
                {
                    _courseUnit = value;
                    RaisePropertyChanged("CourseUnit");
                }
            }
            get => _courseUnit ?? "";
        }

        public string CourseGradePoint
        {
            set
            {
                if (_courseGradePoint == null)
                {
                    _courseGradePoint = value;
                }
                else if (_courseGradePoint != value)
                {
                    _courseGradePoint = value;
                    RaisePropertyChanged("CourseGradePoint");
                }
            }
            get => _courseGradePoint ?? "";
        }

        public string CourseRequirementDesignation
        {
            set
            {
                if (_courseRequirementDesignation == null)
                {
                    _courseRequirementDesignation = value;
                }
                else if (_courseRequirementDesignation != value)
                {
                    _courseRequirementDesignation = value;
                    RaisePropertyChanged("CourseRequirementDesignation");
                }
            }
            get => _courseRequirementDesignation ?? "";
        }

        /// <summary>
        ///     If TransferCourse is true, then have the CourseModel obj own a SJSU<CourseModel>.
        /// </summary>
        public bool IsTransferCourse { set; get; }

        public string Institution
        {
            set
            {
                if (_institution == null)
                {
                    _institution = value;
                }
                else if (_institution != value)
                {
                    _institution = value;
                    RaisePropertyChanged("Institution");
                }
            }
            get => _institution ?? "";
        }

        public CourseModel SjsuCourse { set; get; }

        /// <summary>
        ///     The list of ExcelCells that made up of this Course object.
        /// </summary>
        public List<ExcelCell> ExcelCellsList { set; get; }

        public static void BuildTransferCourseRow(ref CourseModel cm, ref List<ExcelCell> currentRowCells,
            ref Dictionary<string, CourseModel> courses)
        {
            cm.SjsuCourse = new CourseModel();
            var transferCourseVal = currentRowCells
                .Where(c => c.HeaderName == "Transfer Course")
                .Select(c => c.Value)
                .FirstOrDefault();

            var sjsuCourseVal = currentRowCells.Where(c => c.HeaderName == "SJSU Course")
                .Select(c => c.Value)
                .FirstOrDefault();
            // not a valid course...
            if (string.IsNullOrEmpty(transferCourseVal) || string.IsNullOrEmpty(sjsuCourseVal))
                return;

            var split = transferCourseVal.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            var split2 = sjsuCourseVal.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 2)
            {
                cm.CourseAbbreviation = split[0];
                cm.CourseNumber = split[1];

                cm.SjsuCourse.CourseAbbreviation = split2[0];
                cm.SjsuCourse.CourseNumber = split2[1];
            }

            // going to query like this because a simple for loop causes too much issues.
            // same column names
            var unitsFromTransferCollege = currentRowCells.FirstOrDefault(c => c.HeaderName == "Units");
            var unitsAtSJSU = currentRowCells.LastOrDefault(c => c.HeaderName == "Units");
            cm.CourseUnit = unitsFromTransferCollege != null ? unitsFromTransferCollege.Value : "";
            cm.SjsuCourse.CourseUnit = unitsAtSJSU != null ? unitsAtSJSU.Value : "";

            var transferGrade = currentRowCells.FirstOrDefault(c => c.HeaderName == "Grade");
            var sjsuGrade = currentRowCells.LastOrDefault(c => c.HeaderName == "Grade");
            cm.CourseGrade = transferGrade != null ? transferGrade.Value : "";
            cm.SjsuCourse.CourseGrade = sjsuGrade != null ? sjsuGrade.Value : "";

            var transferCourseTitle =
                currentRowCells.FirstOrDefault(c => c.HeaderName == "Transfer Course Title");
            var sjsuCourseTitle = currentRowCells.FirstOrDefault(c => c.HeaderName == "SJSU Course Title");
            cm.CourseTitle = transferCourseTitle != null ? transferCourseTitle.Value : "";
            cm.SjsuCourse.CourseTitle = sjsuCourseTitle != null ? sjsuCourseTitle.Value : "";

            //Institution
            var tranfserInstitutionName =
                currentRowCells.FirstOrDefault(c => c.HeaderName == "Transfer Institution");
            cm.Institution = tranfserInstitutionName != null ? tranfserInstitutionName.Value : "";

            cm.SjsuCourse.IsTransferCourse = false;

            var sjsuCourseExcelCell = currentRowCells.FirstOrDefault(c => c.HeaderName == "SJSU Course");
            //assign its neighbors.
            var startingIndexOfSjsuCourse = currentRowCells.IndexOf(sjsuCourseExcelCell);
            if (startingIndexOfSjsuCourse > 0 && startingIndexOfSjsuCourse < currentRowCells.Count)
            {
                var listForCM = currentRowCells.GetRange(0, startingIndexOfSjsuCourse - 1);
                var listForSJSU = currentRowCells.Where(c => !listForCM.Contains(c)).ToList();

                cm.ExcelCellsList = listForCM;
                cm.SjsuCourse.ExcelCellsList = listForSJSU;
            }
            if (!courses.ContainsKey(cm.ToString()))
                courses.Add(cm.ToString(), cm);
            if (!courses.ContainsKey(cm.SjsuCourse.ToString()))
                courses.Add(cm.SjsuCourse.ToString(), cm.SjsuCourse);
        }

        public static void BuildSjsuCourseRow(ref CourseModel cm, ref List<ExcelCell> cells,
            ref Dictionary<string, CourseModel> courses)
        {
            //Course	Description	Term	Grade	Units	Grd Points	Repeat Code	Reqmnt Desig	Status	Transcript Note
            var courseNameNum = cells.Where(c => c.HeaderName == "Course").Select(c => c.Value).FirstOrDefault()?.Split(' ');

            if (courseNameNum != null)
            {
                cm.CourseAbbreviation = courseNameNum[0];
                cm.CourseNumber = courseNameNum[1];
            }

            cm.CourseTitle = cells.Where(c => c.HeaderName == "Description").Select(c => c.Value).FirstOrDefault();
            cm.CourseGrade = cells.Where(c => c.HeaderName == "Grade").Select(c => c.Value).FirstOrDefault();
            cm.CourseUnit = cells.Where(c => c.HeaderName == "Units").Select(c => c.Value).FirstOrDefault();
            cm.IsTransferCourse = false;
            cm.Institution = "SJSU";

            if (!courses.ContainsKey(cm.ToString()))
                courses.Add(cm.ToString(), cm);
        }

        public static Dictionary<string, CourseModel> BuildCourseDictionary(List<ExcelCell> cells)
        {
            var courses = new Dictionary<string, CourseModel>();
            var maxNumOfRows = cells.Max(c => c.Row);
            for (var i = 0; i < maxNumOfRows; i++)
            {
                // group columns by their row #.
                var currentRowCells = cells.Where(c => c.Row == i).OrderBy(c => c.Column).ToList();
                if (currentRowCells.Count == 0)
                    continue;
                var cm = new CourseModel
                {
                    ExcelCellsList = currentRowCells,
                    IsTransferCourse = currentRowCells.Any(cell => cell.HeaderName.Contains("Transfer"))
                };
                // need to distinguish between transfer & non-transfer course excel files & handle them separately.
                if (cm.IsTransferCourse)
                    BuildTransferCourseRow(ref cm, ref currentRowCells, ref courses);
                else
                    BuildSjsuCourseRow(ref cm, ref currentRowCells, ref courses);
            }
            foreach (var entry in courses)
                if (CoursesDictionary.ContainsKey(entry.Key))
                    CoursesDictionary[entry.Key] = entry.Value;
                else
                    CoursesDictionary.Add(entry.Key, entry.Value);
            return courses;
        }

        /// <summary>
        ///     Assign the value to the best matching string properties.
        /// </summary>
        /// <param name="data"></param>
        public void AssignData(KeyValuePair<string, string> data)
        {
            var key = data.Key.ToLower();

            if (key.Contains("abbr"))
                key = "CourseAbbreviation";
            else if (key.Contains("number") || key.Contains("no"))
                key = "CourseNumber";
            else if (key.Contains("title") || key.Contains("description"))
                key = "CourseTitle";
            else if (key.Contains("semester") || key.Contains("fall") || key.Contains("spring") ||
                     key.Contains("summer"))
                key = "CourseSemester";
            else if (key.Contains("year") || key.StartsWith("20") && key.Length == 4)
                key = "CourseYear";
            else if (key.Contains("unit") || key.Contains(".") && float.TryParse(key, out var f))
                key = "CourseUnit";
            else if (key.Contains("grade"))
                key = "CourseGrade";
            else if (key.Contains("req"))
                key = "CourseRequirementDesignation";

            switch (key)
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
                    //Console.WriteLine("Could not find where to assign value: " + key);
                    break;
            }
        }

        public override string ToString()
        {
            return CourseAbbreviation + " " + CourseNumber + " " + CourseTitle;
        }
    }
}