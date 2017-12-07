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
            _gradAppForm = GradAppModel.GetInstance();

			UpdateCommand = new DelegateCommand( Execute, CanExecute )
								.ObservesProperty( () => FirstName )
								.ObservesProperty( () => LastName );

		}
		public void CloseAndShowPDF()
		{
			_gradAppForm.Close();
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
			set;
			get;
		}

        public String FirstName
		{
			set
			{
				bool updated = SetProperty(ref _gradAppForm.firstName, StringUtils.CapFirstLetterRemoveNums(value));
				if (updated)
					_gradAppForm.WriteField("First name", value);
			}
			get { return _gradAppForm.firstName; }
		}

		public String LastName
		{
			set
			{
				bool updated = SetProperty(ref _gradAppForm.lastName, StringUtils.CapFirstLetterRemoveNums(value));
				if (updated)
					_gradAppForm.WriteField("Last name", value);
			}
			get { return _gradAppForm.lastName; }
		}

		public String MiddleName
		{
			set
			{
				bool updated = SetProperty(ref _gradAppForm.middleName, StringUtils.CapFirstLetterRemoveNums(value));
				if (updated)
					_gradAppForm.WriteField("Middle name", value);
			}
			get { return _gradAppForm.middleName; }
		}

		public String StudentID
		{
			set
			{
				bool updated = SetProperty(ref _gradAppForm.studentID, StringUtils.RemoveAllLetters(value));
				if (updated)
					_gradAppForm.WriteField("SJSU ID", value);
			}
			get { return _gradAppForm.studentID; }
		}

		public String Email
		{
			set
			{
				bool updated = SetProperty(ref _gradAppForm.email, value);
				if (updated)
					_gradAppForm.WriteField("E-mail address", value);
			}
			get { return _gradAppForm.email; }
		}

        public String PhoneNumber
        {
			set
			{
				bool updated = SetProperty(ref _gradAppForm.phoneNumber, StringUtils.FormatPhoneNumber(value));
				if (updated)
					_gradAppForm.WriteField("Home phone number", value);
			}
			get { return _gradAppForm.phoneNumber; }
        }

        public String MajorName
        {
			set
			{
				bool updated = SetProperty(ref _gradAppForm.majorName, value);
				if (updated)
					_gradAppForm.WriteField("Major");
			}
			get { return _gradAppForm.majorName; }
        }

        public String GradYear
        {
			set
			{
				bool updated = SetProperty(ref _gradAppForm.gradYear, value);
				if (updated)
					_gradAppForm.WriteField("Year", value);
			}
			get { return _gradAppForm.gradYear; }
		}

		// Grad Semester must be checked before the grad year can be written.
        public String GradSemester
        {
			set
			{
				bool updated = SetProperty(ref _gradAppForm.gradSemester, value);
				if (updated)
				{
					_gradAppForm.WriteField("Semester", value);
				}
			}
			get { return _gradAppForm.gradSemester; }
		}

        public String StreetNumber
        {
			set
			{
				bool updated = SetProperty(ref _gradAppForm.streetNumber, value);
				if (updated)
					_gradAppForm.WriteField("Street number", value);
			}
			get { return _gradAppForm.streetNumber; }
		}
        public String StreetName
        {
			set
			{
				bool updated = SetProperty(ref _gradAppForm.streetName, StringUtils.CapFirstLetterRemoveNums(value));
				if (updated)
					_gradAppForm.WriteField("Street name", value);
			}
			get { return _gradAppForm.streetName; }
        }
        public String ApartmentNumber
        {
            set
			{
				bool updated = SetProperty(ref _gradAppForm.apartmentNumber, value);
				if (updated) _gradAppForm.WriteField("Apartment number", value);
			}
            get { return _gradAppForm.apartmentNumber; }
        }
        public String City
        {
            set
			{
				bool updated = SetProperty(ref _gradAppForm.city, StringUtils.CapFirstLetterRemoveNums(value));
				if (updated) _gradAppForm.WriteField("City", value);
			}
            get { return _gradAppForm.city; }
        }
        public String State
        {
            set
			{
				bool updated = SetProperty(ref _gradAppForm.state, StringUtils.CapFirstLetterRemoveNums(value));
				if (updated) _gradAppForm.WriteField("State", value);
			}
            get { return _gradAppForm.state; }
        }
        public String Zipcode
        {
            set { bool updated = SetProperty(ref _gradAppForm.zipcode, value); if (updated) _gradAppForm.WriteField("Zip code", value); }
            get { return _gradAppForm.zipcode; }
        }

        public String DegreeObjective
        {
			set
			{
				bool updated = SetProperty(ref _gradAppForm.degreeObjective, value);
				if (updated)
					_gradAppForm.WriteField("", value);
			}
			get { return _gradAppForm.degreeObjective; }
		}

        public String NonSJSUCourseCompleted1
        {
            set { bool updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[0], value); if (updated) _gradAppForm.WriteField("Non SJSU course 1",value);  }
            get { return _gradAppForm.nonSJSUNotCompleted[0]; }
        }
        public String NonSJSUCourseCompleted2
        {
            set { bool updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[1], value); if (updated) _gradAppForm.WriteField("Non SJSU course 2",value);  }
            get { return _gradAppForm.nonSJSUNotCompleted[1]; }
        }
        public String NonSJSUCourseCompleted3
        {
            set { bool updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[2], value); if (updated) _gradAppForm.WriteField("Non SJSU course 3",value);  }
            get { return _gradAppForm.nonSJSUNotCompleted[2]; }
        }
        public String NonSJSUCourseCompleted4
        {
            set { bool updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[3], value); if (updated) _gradAppForm.WriteField("Non SJSU course 4",value);  }
            get { return _gradAppForm.nonSJSUNotCompleted[3]; }
        }
        public String NonSJSUCourseCompleted5
        {
            set { bool updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[4], value); if (updated) _gradAppForm.WriteField("Non SJSU course 5",value);  }
            get { return _gradAppForm.nonSJSUNotCompleted[4]; }
        }
        public String NonSJSUCourseCompleted6
        {
            set { bool updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[5], value); if (updated) _gradAppForm.WriteField("Non SJSU course 6",value);  }
            get { return _gradAppForm.nonSJSUNotCompleted[5]; }
        }
        public String NonSJSUCourseCompleted7
        {
            set { bool updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[6], value); if (updated) _gradAppForm.WriteField("Non SJSU course 7",value);  }
            get { return _gradAppForm.nonSJSUNotCompleted[6]; }
        }
        public String NonSJSUCourseCompleted8
        {
            set { bool updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[7], value); if (updated) _gradAppForm.WriteField("Non SJSU course 8",value);  }
            get { return _gradAppForm.nonSJSUNotCompleted[7]; }
        }

        public String CurrentEnrolledCourse1
        {
            set { bool updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[0], value); if (updated) _gradAppForm.WriteField("current SJSU course 1",value);  }
            get { return _gradAppForm.currentEnrolledCourses[0]; }
        }
        public String CurrentEnrolledCourse2
        {
            set { bool updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[1], value); if (updated) _gradAppForm.WriteField("current SJSU course 2",value);  }
            get { return _gradAppForm.currentEnrolledCourses[1]; }
        }
        public String CurrentEnrolledCourse3
        {
            set { bool updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[2], value); if (updated) _gradAppForm.WriteField("current SJSU course 3",value);  }
            get { return _gradAppForm.currentEnrolledCourses[2]; }
        }
        public String CurrentEnrolledCourse4
        {
            set { bool updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[3], value); if (updated) _gradAppForm.WriteField("current SJSU course 4",value);  }
            get { return _gradAppForm.currentEnrolledCourses[3]; }
        }
        public String CurrentEnrolledCourse5
        {
            set { bool updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[4], value); if (updated) _gradAppForm.WriteField("current SJSU course 5",value);  }
            get { return _gradAppForm.currentEnrolledCourses[4]; }
        }
        public String CurrentEnrolledCourse6
        {
            set { bool updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[5], value); if (updated) _gradAppForm.WriteField("current SJSU course 6",value);  }
            get { return _gradAppForm.currentEnrolledCourses[5]; }
        }
        public String CurrentEnrolledCourse7
        {
            set { bool updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[6], value); if (updated) _gradAppForm.WriteField("current SJSU course 7",value);  }
            get { return _gradAppForm.currentEnrolledCourses[6]; }
        }
        public String CurrentEnrolledCourse8
        {
            set { bool updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[7], value); if (updated) _gradAppForm.WriteField("current SJSU course 8",value);  }
            get { return _gradAppForm.currentEnrolledCourses[7]; }
        }

    }
}

