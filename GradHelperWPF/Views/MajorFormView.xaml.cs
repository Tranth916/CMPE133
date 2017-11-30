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
	public partial class MajorFormView : Grid
	{
		public MajorFormView()
		{
			InitializeComponent();
		}

		private void ExportBtn_OnClick(object sender, RoutedEventArgs e)
		{
			var ofd = new Microsoft.Win32.SaveFileDialog()
			{

				Filter = ".docx"
			};

			bool? result = ofd.ShowDialog();

			if( result.Value )
			{

			}

		}
	}
}
