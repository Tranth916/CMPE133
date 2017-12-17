using System.ComponentModel;

namespace GradHelperWPF.Interfaces
{
    /// <summary>
    /// Deprecated, use the Prism MVVM framework to do property notification.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged( string propertyName )
        {
            var e = new PropertyChangedEventArgs(propertyName);
            PropertyChanged?.Invoke(this, e);
        }
    }
}