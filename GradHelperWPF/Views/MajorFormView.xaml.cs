using System.Windows.Controls;
using GradHelperWPF.ViewModel;

namespace GradHelperWPF.Views
{
    /// <summary>
    ///     Interaction logic for MajorFormView.xaml
    /// </summary>
    public partial class MajorFormView : StackPanel
    {
        public static MajorFormViewModel MajorFormModelStatic = new MajorFormViewModel();

        public MajorFormView()
        {
            InitializeComponent();
            DataContext = new
            {
                MajorFormVM = MajorFormModelStatic,
                GradAppVM = GradApplicationView.gradAppViewModelStatic
            };
        }
    }
}