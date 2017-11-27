using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GradHelperWPF.Model;
using System.Collections.ObjectModel;

namespace GradHelperWPF.ViewModel
{
    public class GradAppViewModel : INotifyPropertyChanged
    {
        public GradAppViewModel()
        {
            LoadStudents();
        }

        public void LoadStudents()
        {
            ObservableCollection<GradAppModel> gradapp = new ObservableCollection<GradAppModel>();
            gradapp.Add(new GradAppModel() { FirstName="Thao", LastName="Tran"});
            GradApp = gradapp;
        }
        
        public ObservableCollection<GradAppModel> GradApp
        {
            set;get;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
