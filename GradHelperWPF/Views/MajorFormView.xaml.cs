using GradHelperWPF.ViewModel;

namespace GradHelperWPF.Views
{
    /** OUT OF COMMISSION
     *  Do not use this view class yet.
     *  Need to create the ViewModel for this View to be functional.
     */

    public partial class MajorFormView
    {
        public static MajorFormViewModel MajorFormModelStatic = new MajorFormViewModel();

        public MajorFormView( )
        {
            InitializeComponent( );
            DataContext = new
            {
                MajorFormVM = MajorFormModelStatic,
                GradAppVM = GradApplicationView.GradAppViewModelStatic
            };
        }
    }
}