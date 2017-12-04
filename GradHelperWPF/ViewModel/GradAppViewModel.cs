using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradHelperWPF.Models;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Commands;
using GradHelperWPF.Utils;

namespace GradHelperWPF.ViewModel
{
    public class GradAppViewModel : BindableBase
    {
		public StringBuilder strBuilder = new StringBuilder();

		private GradAppModel _gradAppForm;

		public GradAppViewModel()
        {
            _gradAppForm = new GradAppModel();

			UpdateCommand = new DelegateCommand( Execute, CanExecute )
								.ObservesProperty( () => FirstName )
								.ObservesProperty( () => LastName );

		}

		private bool CanExecute()
		{
			return true;
		}

		private void Execute()
		{
			//System.Windows.MessageBox.Show("Excuted!");
		}

		public DelegateCommand UpdateCommand
		{
			set; get;
		}


        public String FirstName
		{
			set
			{
				SetProperty(ref _gradAppForm.firstName, StringUtils.CapFirstLetterRemoveNums(value));

			}
			get { return _gradAppForm.firstName; }
		}

		public String LastName
		{
			set { SetProperty(ref _gradAppForm.lastName, StringUtils.CapFirstLetterRemoveNums(value)); }
			get { return _gradAppForm.lastName; }
		}

		public String MiddleName
		{
			set { SetProperty(ref _gradAppForm.middleName, StringUtils.CapFirstLetterRemoveNums(value)); }
			get { return _gradAppForm.middleName; }
		}

		public String StudentID
		{
			set { SetProperty(ref _gradAppForm.studentID, StringUtils.RemoveAllLetters(value)); }
			get { return _gradAppForm.studentID; }
		}

		public String Email
		{
			set { SetProperty(ref _gradAppForm.email, value); }
			get { return _gradAppForm.email; }
		}

        public String PhoneNumber
        {
            set { SetProperty(ref _gradAppForm.phoneNumber, StringUtils.FormatPhoneNumber(value)) ; }
            get { return _gradAppForm.phoneNumber; }
        }

        public String MajorName
        {
            set { SetProperty(ref _gradAppForm.majorName, value); }
            get { return _gradAppForm.majorName; }
        }

        public String GradYear
        {
            set { SetProperty(ref _gradAppForm.gradYear, value); }
            get { return _gradAppForm.gradYear; }
        }

        public String GradSemester
        {
            set { SetProperty(ref _gradAppForm.gradSemester, value); }
            get { return _gradAppForm.gradSemester; }
        }

        public String StreetNumber
        {
            set { SetProperty(ref _gradAppForm.streetNumber, value); }
            get { return _gradAppForm.streetNumber; }
        }

        public String StreetName
        {
            set { SetProperty(ref _gradAppForm.streetName, StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.streetName; }
        }

        public String ApartmentNumber
        {
            set { SetProperty(ref _gradAppForm.apartmentNumber, value); }
            get { return _gradAppForm.apartmentNumber; }
        }

        public String City
        {
            set { SetProperty(ref _gradAppForm.city, StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.city; }
        }

        public String State
        {
            set { SetProperty(ref _gradAppForm.state, StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.state; }
        }

        public String Zipcode
        {
            set { SetProperty(ref _gradAppForm.zipcode, value); }
            get { return _gradAppForm.zipcode; }
        }

        public String DegreeObjective
        {
            set { SetProperty(ref _gradAppForm.degreeObjective, value); }
            get { return _gradAppForm.degreeObjective; }
        }

        public String NonSJSUCourseCompleted1
        {
            set { SetProperty(ref _gradAppForm.nonSJSUNotCompleted[0], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.nonSJSUNotCompleted[0]; }
        }
        public String NonSJSUCourseCompleted2
        {
            set { SetProperty(ref _gradAppForm.nonSJSUNotCompleted[1], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.nonSJSUNotCompleted[1]; }
        }
        public String NonSJSUCourseCompleted3
        {
            set { SetProperty(ref _gradAppForm.nonSJSUNotCompleted[2], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.nonSJSUNotCompleted[2]; }
        }
        public String NonSJSUCourseCompleted4
        {
            set { SetProperty(ref _gradAppForm.nonSJSUNotCompleted[3], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.nonSJSUNotCompleted[3]; }
        }
        public String NonSJSUCourseCompleted5
        {
            set { SetProperty(ref _gradAppForm.nonSJSUNotCompleted[4], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.nonSJSUNotCompleted[4]; }
        }
        public String NonSJSUCourseCompleted6
        {
            set { SetProperty(ref _gradAppForm.nonSJSUNotCompleted[5], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.nonSJSUNotCompleted[5]; }
        }
        public String NonSJSUCourseCompleted7
        {
            set { SetProperty(ref _gradAppForm.nonSJSUNotCompleted[6], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.nonSJSUNotCompleted[6]; }
        }
        public String NonSJSUCourseCompleted8
        {
            set { SetProperty(ref _gradAppForm.nonSJSUNotCompleted[7], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.nonSJSUNotCompleted[7]; }
        }

        public String CurrentEnrolledCourse1
        {
            set { SetProperty(ref _gradAppForm.currentEnrolledCourses[0], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.currentEnrolledCourses[0]; }
        }
        public String CurrentEnrolledCourse2
        {
            set { SetProperty(ref _gradAppForm.currentEnrolledCourses[1], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.currentEnrolledCourses[1]; }
        }
        public String CurrentEnrolledCourse3
        {
            set { SetProperty(ref _gradAppForm.currentEnrolledCourses[2], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.currentEnrolledCourses[2]; }
        }
        public String CurrentEnrolledCourse4
        {
            set { SetProperty(ref _gradAppForm.currentEnrolledCourses[3], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.currentEnrolledCourses[3]; }
        }
        public String CurrentEnrolledCourse5
        {
            set { SetProperty(ref _gradAppForm.currentEnrolledCourses[4], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.currentEnrolledCourses[4]; }
        }
        public String CurrentEnrolledCourse6
        {
            set { SetProperty(ref _gradAppForm.currentEnrolledCourses[5], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.currentEnrolledCourses[5]; }
        }
        public String CurrentEnrolledCourse7
        {
            set { SetProperty(ref _gradAppForm.currentEnrolledCourses[6], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.currentEnrolledCourses[6]; }
        }
        public String CurrentEnrolledCourse8
        {
            set { SetProperty(ref _gradAppForm.currentEnrolledCourses[7], StringUtils.CapFirstLetterRemoveNums(value)); }
            get { return _gradAppForm.currentEnrolledCourses[7]; }
        }

    }
}

