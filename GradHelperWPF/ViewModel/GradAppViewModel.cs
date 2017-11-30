using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradHelperWPF.Model;
using System.Collections.ObjectModel;
using Prism.Mvvm;
using Prism.Commands;
using GradHelperWPF.Utils;

namespace GradHelperWPF.ViewModel
{
    public class GradAppViewModel : BindableBase
    {
		public StringBuilder strBuilder = new StringBuilder();

		private GradAppFormModel _gradformModel;

		public GradAppViewModel()
        {
			UpdateCommand = new DelegateCommand( Execute, CanExecute )
								.ObservesProperty( () => FirstName )
								.ObservesProperty( () => LastName );


		}

		private bool CanExecute()
		{
			//System.Windows.MessageBox.Show("CanExcuted!");
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

		private string _middleName;
		private string _lastName;
		private string _studentID;
		private string _email;

		private string _firstName;

		public String FirstName
		{
			set
			{
				SetProperty(ref _firstName, StringUtils.CapFirstLetterRemoveNums(value));
			}
			get { return _firstName; }
		}

		public String LastName
		{
			set { SetProperty(ref _lastName, value); }
			get { return _lastName; }
		}

		public String MiddleName
		{
			set { SetProperty(ref _middleName, value); }
			get { return _middleName; }
		}


		public String StudentID
		{
			set { SetProperty(ref _studentID, StringUtils.RemoveAllLetters(value)); }
			get { return _studentID; }
		}

		public String Email
		{
			set { SetProperty(ref _email, value); }
			get { return _email; }
		}
		
	}

	// move this class out
	class GradAppFormModel
	{
		public GradAppFormModel()
		{

		}






	}
}

