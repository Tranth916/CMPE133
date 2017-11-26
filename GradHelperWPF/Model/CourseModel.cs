using System;
using System.Collections;
using System.Xaml.Permissions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GradHelperWPF.Interfaces;

namespace GradHelperWPF.Model
{
    public class CourseModel : ObservableObject
    {
        private String _courseAbrreviation;
        private String _courseNumber                ;
        private String _courseTitle                 ;
        private String _courseSemester              ;
        private String _courseYear                  ;
        private String _courseGrade                 ;
        private String _courseUnit                  ;
        private String _courseGradePoint            ;
        private String _courseRequirementDesignation;

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
                    OnPropertyChanged("CourseAbbreviation");
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

        private List<string> _data;

        public CourseModel() { }
        public CourseModel(List<string> data)
        {
            _data = data;
        }


        public void StoreAllData(List<string> data)
        {

        }
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

    }
}
