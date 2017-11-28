﻿using GradHelperWPF.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradHelperWPF.Model;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace GradHelperWPF.ViewModel
{
	public enum ExcelCourseType
	{
		SjsuCourses,
		TransferCourses,
	}

    public class CourseViewModel : ObservableObject
    {
        private CourseModel _courseModel;
        private ICommand _getCourseCommand;
        private ICommand _saveCourseCommand;
        public ICommand GetCourseCommand
        {
            get
            {
                if (_getCourseCommand == null)
                {
                    _getCourseCommand = new RelayCommand(param => GetCourse(), null);
                }
                return _getCourseCommand;
            }
        }
        public ICommand SaveCourseCommand
        {
            get
            {
                if (_saveCourseCommand == null)
                {
                    _saveCourseCommand = new RelayCommand(save => SaveCourse(), save => (Course != null));
                }
                return _saveCourseCommand;
            }
        }
        public CourseModel Course
        {
            get { return _courseModel; }
            set
            { 
                if (_courseModel == null)
                    _courseModel = value;
                else if(_courseModel != value)
                {
                    _courseModel = value;
                    OnPropertyChanged("Course");
                }
            }
        }

		ObservableCollection<CourseModel> _courses;

		public ObservableCollection<CourseModel> Courses
		{
			set
			{
				_courses = new ObservableCollection<CourseModel>();
			}
			get
			{
				return _courses;
			}
		}

        public CourseViewModel()
        {
			_courses = new ObservableCollection<CourseModel>();
            //Course = new CourseModel() { CourseTitle ="Testing MVVM" };
        }

        public bool UpdateCourse()
        {
            return true;
        }
        private void GetCourse()
        {

        }
        private void SaveCourse()
        {

        }

		
    }
}
