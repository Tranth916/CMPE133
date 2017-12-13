using System.Diagnostics.CodeAnalysis;
using GradHelperWPF.Models;
using GradHelperWPF.Utils;
using Prism.Commands;
using Prism.Mvvm;
using System.Text;

namespace GradHelperWPF.ViewModel
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class GradAppViewModel : BindableBase
    {
        public StringBuilder strBuilder = new StringBuilder();
        private readonly GradAppModel _gradAppForm;
       
        public GradAppViewModel( )
        {
            _gradAppForm = GradAppModel.GetInstance( );

            UpdateCommand = new DelegateCommand( Execute, CanExecute )
                .ObservesProperty( ( ) => FirstName )
                .ObservesProperty( ( ) => LastName );
        }

        public void ExportToPDF(string output)
        {
            _gradAppForm?.ExportToPdf(output);
        }

		public string ApartmentNumber
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.apartmentNumber, value);
                if ( updated ) _gradAppForm.WriteField( "Apartment number", value );
            }
            get => _gradAppForm.apartmentNumber;
        }

        public string City
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.city, StringUtils.CapFirstLetterRemoveNums(value));
                if ( updated ) _gradAppForm.WriteField( "City", value );
            }
            get => _gradAppForm.city;
        }

        public string CurrentEnrolledCourse1
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[0], value);
                if ( updated ) _gradAppForm.WriteField( "current SJSU course 1", value );
            }
            get => _gradAppForm.currentEnrolledCourses[0];
        }

        public string CurrentEnrolledCourse2
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[1], value);
                if ( updated ) _gradAppForm.WriteField( "current SJSU course 2", value );
            }
            get => _gradAppForm.currentEnrolledCourses[1];
        }

        public string CurrentEnrolledCourse3
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[2], value);
                if ( updated ) _gradAppForm.WriteField( "current SJSU course 3", value );
            }
            get => _gradAppForm.currentEnrolledCourses[2];
        }

        public string CurrentEnrolledCourse4
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[3], value);
                if ( updated ) _gradAppForm.WriteField( "current SJSU course 4", value );
            }
            get => _gradAppForm.currentEnrolledCourses[3];
        }

        public string CurrentEnrolledCourse5
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[4], value);
                if ( updated ) _gradAppForm.WriteField( "current SJSU course 5", value );
            }
            get => _gradAppForm.currentEnrolledCourses[4];
        }

        public string CurrentEnrolledCourse6
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[5], value);
                if ( updated ) _gradAppForm.WriteField( "current SJSU course 6", value );
            }
            get => _gradAppForm.currentEnrolledCourses[5];
        }

        public string CurrentEnrolledCourse7
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[6], value);
                if ( updated ) _gradAppForm.WriteField( "current SJSU course 7", value );
            }
            get => _gradAppForm.currentEnrolledCourses[6];
        }

        public string CurrentEnrolledCourse8
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.currentEnrolledCourses[7], value);
                if ( updated ) _gradAppForm.WriteField( "current SJSU course 8", value );
            }
            get => _gradAppForm.currentEnrolledCourses[7];
        }

        public string DegreeObjective
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.degreeObjective, value);
                if ( updated )
                    _gradAppForm.WriteField( "", value );
            }
            get => _gradAppForm.degreeObjective;
        }

        public string Email
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.email, value);
                if ( updated )
                    _gradAppForm.WriteField( "E-mail address", value );
            }
            get => _gradAppForm.email;
        }

        public string FirstName
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.firstName, StringUtils.CapFirstLetterRemoveNums(value));
                if ( updated )
                    _gradAppForm.WriteField( "First name", value );
            }
            get => _gradAppForm.firstName;
        }

        // Grad Semester must be checked before the grad year can be written.
        public string GradSemester
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.gradSemester, value);
                if ( updated )
                    _gradAppForm.WriteField( "Semester", value );
            }
            get => _gradAppForm.gradSemester;
        }

        public string GradYear
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.gradYear, value);
                if ( updated )
                    _gradAppForm.WriteField( "Year", value );
            }
            get => _gradAppForm.gradYear;
        }

        public string LastName
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.lastName, StringUtils.CapFirstLetterRemoveNums(value));
                if ( updated )
                    _gradAppForm.WriteField( "Last name", value );
            }
            get => _gradAppForm.lastName;
        }

        public string MajorName
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.majorName, value);
                if ( updated )
                    _gradAppForm.WriteField( "Major" );
            }
            get => _gradAppForm.majorName;
        }

        public string MiddleName
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.middleName, StringUtils.CapFirstLetterRemoveNums(value));
                if ( updated )
                    _gradAppForm.WriteField( "Middle name", value );
            }
            get => _gradAppForm.middleName;
        }

        public string NonSJSUCourseCompleted1
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[0], value);
                if ( updated ) _gradAppForm.WriteField( "Non SJSU course 1", value );
            }
            get => _gradAppForm.nonSJSUNotCompleted[0];
        }

        public string NonSJSUCourseCompleted2
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[1], value);
                if ( updated ) _gradAppForm.WriteField( "Non SJSU course 2", value );
            }
            get => _gradAppForm.nonSJSUNotCompleted[1];
        }

        public string NonSJSUCourseCompleted3
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[2], value);
                if ( updated ) _gradAppForm.WriteField( "Non SJSU course 3", value );
            }
            get => _gradAppForm.nonSJSUNotCompleted[2];
        }

        public string NonSJSUCourseCompleted4
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[3], value);
                if ( updated ) _gradAppForm.WriteField( "Non SJSU course 4", value );
            }
            get => _gradAppForm.nonSJSUNotCompleted[3];
        }

        public string NonSJSUCourseCompleted5
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[4], value);
                if ( updated ) _gradAppForm.WriteField( "Non SJSU course 5", value );
            }
            get => _gradAppForm.nonSJSUNotCompleted[4];
        }

        public string NonSJSUCourseCompleted6
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[5], value);
                if ( updated ) _gradAppForm.WriteField( "Non SJSU course 6", value );
            }
            get => _gradAppForm.nonSJSUNotCompleted[5];
        }

        public string NonSJSUCourseCompleted7
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[6], value);
                if ( updated ) _gradAppForm.WriteField( "Non SJSU course 7", value );
            }
            get => _gradAppForm.nonSJSUNotCompleted[6];
        }

        public string NonSJSUCourseCompleted8
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.nonSJSUNotCompleted[7], value);
                if ( updated ) _gradAppForm.WriteField( "Non SJSU course 8", value );
            }
            get => _gradAppForm.nonSJSUNotCompleted[7];
        }

        public string PhoneNumber
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.phoneNumber, StringUtils.FormatPhoneNumber(value));
                if ( updated )
                    _gradAppForm.WriteField( "Home phone number", value );
            }
            get => _gradAppForm.phoneNumber;
        }

        public string State
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.state, StringUtils.CapFirstLetterRemoveNums(value));
                if ( updated ) _gradAppForm.WriteField( "State", value );
            }
            get => _gradAppForm.state;
        }

        public string StreetName
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.streetName, StringUtils.CapFirstLetterRemoveNums(value));
                if ( updated )
                    _gradAppForm.WriteField( "Street name", value );
            }
            get => _gradAppForm.streetName;
        }

        public string StreetNumber
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.streetNumber, value);
                if ( updated )
                    _gradAppForm.WriteField( "Street number", value );
            }
            get => _gradAppForm.streetNumber;
        }

        public string StudentID
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.studentID, StringUtils.RemoveAllLetters(value));
                if ( updated )
                    _gradAppForm.WriteField( "SJSU ID", value );
            }
            get => _gradAppForm.studentID;
        }

        public DelegateCommand UpdateCommand { set; get; }
        public string Zipcode
        {
            set
            {
                var updated = SetProperty(ref _gradAppForm.zipcode, value);
                if ( updated ) _gradAppForm.WriteField( "Zip code", value );
            }
            get => _gradAppForm.zipcode;
        }
        public void CloseAndShowPDF( )
        {
            _gradAppForm.Close( );
        }

        private bool CanExecute( )
        {
            return true;
        }

        private void Execute( )
        {
            //System.Windows.MessageBox.Show("Excuted!");
        }
    }
}