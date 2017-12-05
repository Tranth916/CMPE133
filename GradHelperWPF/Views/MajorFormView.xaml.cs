using GradHelperWPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GradHelperWPF.Views
{
	/// <summary>
	/// Interaction logic for MajorFormView.xaml
	/// </summary>
	public partial class MajorFormView : StackPanel
	{
        public static MajorFormViewModel majorFormModelStatic = new MajorFormViewModel();

		public MajorFormView()
		{
			InitializeComponent();
            DataContext = new
            {
                MajorFormVM = majorFormModelStatic,
                GradAppVM = GradApplicationView.gradAppViewModelStatic,
            };
        }



		private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
		{
			var ofd = new Microsoft.Win32.SaveFileDialog()
			{

			};
		}
	}
}
