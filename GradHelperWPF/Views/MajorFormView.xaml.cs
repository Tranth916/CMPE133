using GradHelperWPF.ViewModel;
using System.Windows.Controls;

namespace GradHelperWPF.Views
{
    /// <summary>
    ///     Interaction logic for MajorFormView.xaml
    /// </summary>
    public partial class MajorFormView : StackPanel
    {
        public static MajorFormViewModel MajorFormModelStatic = new MajorFormViewModel();

        public MajorFormView( )
        {
            InitializeComponent( );
            DataContext = new
            {
                MajorFormVM = MajorFormModelStatic,
                GradAppVM = GradApplicationView.gradAppViewModelStatic
            };
        }
    }
}