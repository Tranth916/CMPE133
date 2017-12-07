using System;
using System.Collections.Generic;
using System.IO;
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
using GradHelperWPF.Utils;
namespace GradHelperWPF.Views
{
    /// <summary>
    /// Interaction logic for ErrorWindowView.xaml
    /// </summary>
    public partial class ErrorWindowView : Window
    {
        public ErrorWindowView()
        {
            InitializeComponent();
        }

        public ErrorWindowView(string typeOfError)
        {
            InitializeComponent();
            
            if(typeOfError == "FixExcel")
                ShowFixInstruction();

            else if( typeOfError =="NotSjsuCourse")
                ShowNotSjsuCourse();

        }

        public void ShowNotSjsuCourse()
        {
            
        }

        public void ShowFixInstruction()
        {
            var fixInstructions =
                Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.pdf", SearchOption.AllDirectories);

            var file = fixInstructions.FirstOrDefault(f => f.Contains("fixExcelInstructions.pdf"));

            if( file != null )
                wb.Navigate(file);
        }
    }
}
