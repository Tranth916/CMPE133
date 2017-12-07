using GradHelperWPF.Views;

namespace GradHelperWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {		

        // All Views here.
        public MainWindow()
        {
            InitializeComponent();

			Closing += (o, e) =>
			{
				GradApplicationView.gradAppViewModelStatic.CloseAndShowPDF();
			};

		}		
	}
}

