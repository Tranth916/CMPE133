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
using System.Windows.Navigation;
using System.Windows.Shapes;
using GradHelperWPF.Utils;
using iTextSharp.text.pdf;
using System.IO;
using GradHelperWPF.ViewModel;

namespace GradHelperWPF.Views
{
	/// <summary>
	/// Interaction logic for MajorFormView.xaml
	/// </summary>
	public partial class GradApplicationView : StackPanel
	{
        /// <summary>
        /// Student's data in GradApplicationView
        /// </summary>
        public static GradAppViewModel gradAppViewModelStatic = new GradAppViewModel();

		private List<TextBox> _textboxes;
		private List<TextBox> TextBoxes
		{set;get;
			//set { _textboxes = value; }
			//get
			//{
			//	if (_textboxes == null)
			//	{
			//		_textboxes = new List<TextBox>();

			//		TextBox tb;
			//		try
			//		{
			//			foreach (var c in EntryGrid.Children)
			//			{
			//				try
			//				{
			//					if (c is TextBox && (tb = c as TextBox) != null)
			//					{
			//						if (tb.Name == null)
			//							throw new Exception($" THIS TEXTBOX HAS NO NAME! : {tb.GetHashCode()}");

			//						_textboxes.Add(tb);
			//					}
			//					else if (c is Grid)
			//					{
			//						var g = c as Grid;
			//						foreach (var grandKid in g.Children)
			//						{
			//							if (grandKid is TextBox && (tb = grandKid as TextBox) != null)
			//							{
			//								if (tb.Name == null)
			//									throw new Exception($" THIS GRANDKID TEXTBOX HAS NO NAME! {tb.GetHashCode()}");
			//								_textboxes.Add(tb);
			//							}
			//						}
			//					}
			//				}
			//				catch (Exception ex)
			//				{
			//					throw new Exception(ex.StackTrace);
			//				}
			//			}
			//		}
			//		catch (Exception exx)
			//		{
			//			throw new Exception(exx.StackTrace);
			//		}
			//	}
			//	return _textboxes;
			//}
		}

		public GradApplicationView()
		{
			InitializeComponent();
			DataContext = gradAppViewModelStatic;


		}


		private void ExportToPDf_OnClick(object sender, RoutedEventArgs e)
		{
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog()
            {
                InitialDirectory = System.IO.Directory.GetCurrentDirectory(),
                Filter = "*.pdf | *.PDF",                
            };

            var result = sfd.ShowDialog();
            if( result.Value )
            {
                StampDataToPDF(sfd.FileName, GetAllEntries());
            }
        }

        //Move this out
        public void StampDataToPDF(string path, Dictionary<string, string> data)
        {
            var files = Directory.EnumerateFiles(Directory.GetCurrentDirectory(), "*.pdf", SearchOption.AllDirectories);

            if (files == null)
                throw new Exception();

            var pdfFile = files.FirstOrDefault(ff => ff.Contains("gradapp"));

            var reader = new PdfReader(pdfFile);

            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            var stamper = new PdfStamper(reader, fs);

            var fields = stamper.AcroFields.Fields;

            Dictionary<string, string> map = new Dictionary<string, string>()
            {
                    { "firstNameTextBox"    ,"First name"},
                    { "middleNameTextBox"   ,"Middle name"},
                    { "lastNameTextBox"     ,"Last name"},
                    { "emailTextBox"        ,"E-mail address"},
                    { "phoneTextBox"        ,"Home phone number"},
                    { "sidNameTextBox"      ,"SJSU ID"},
                    { "majorTextBox"        ,"Major"},
                    { "streetNumberTextBox" ,"Street number" },
                    { "streetNameTextBox"   ,"Street name"},
                    { "apartmentTextBox"    ,"Apartment number"},
                    { "cityTextBox"         ,"City"},
                    { "stateTextBox"        ,"State"},
                    { "zipcodeTextBox"      ,"Zip code"},
                    { "uncompletedTextBox1" ,"Non SJSU course 1"},
                    { "uncompletedTextBox2" ,"Non SJSU course 2"},
                    { "uncompletedTextBox3" ,"Non SJSU course 3"},
                    { "uncompletedTextBox4" ,"Non SJSU course 4"},
                    { "uncompletedTextBox5" ,"Non SJSU course 5"},
                    { "uncompletedTextBox6" ,"Non SJSU course 6"},
                    { "uncompletedTextBox7" ,"Non SJSU course 7"},
                    { "uncompletedTextBox8" ,"Non SJSU course 8"},
                    { "currentEnrolled1"    ,"current SJSU course 1" } ,
                    { "currentEnrolled2"    ,"current SJSU course 2" } ,
                    { "currentEnrolled3"    ,"current SJSU course 3" } ,
                    { "currentEnrolled4"    ,"current SJSU course 4" } ,
                    { "currentEnrolled5"    ,"current SJSU course 5" } ,
                    { "currentEnrolled6"    ,"current SJSU course 6" } ,
                    { "currentEnrolled7"    ,"current SJSU course 7" } ,
                    { "currentEnrolled8"    ,"current SJSU course 8" } ,
            };

            string fieldName;
            foreach( var f in fields)
            {
                fieldName = f.Key;

                var textBoxName = map.Where(m => m.Value == fieldName).Select(m => m.Key).FirstOrDefault();

                if (textBoxName == null || !data.ContainsKey(textBoxName))
                    continue;

                stamper.AcroFields.SetField(fieldName, data[textBoxName] , true);
            }

            stamper.Close();
            fs.Close();
            reader.Close();

            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        public Dictionary<string, string> GetAllEntries()
		{
			Dictionary<string, string> data = new Dictionary<string, string>();

			try
			{
				foreach (var tb in TextBoxes)
				{
					if (string.IsNullOrEmpty(tb.Name) || data.ContainsKey(tb.Name))
						continue;
					data.Add(tb.Name, tb.Text);
				}
			}
			catch (Exception exx)
			{
				throw new Exception(exx.StackTrace);
			}
			try
			{
				if (degreeObjectiveCBox != null && degreeObjectiveCBox.SelectedItem != null)
				{
					if (degreeObjectiveCBox.SelectedItem is ComboBoxItem)
					{
						ComboBoxItem citem = degreeObjectiveCBox.SelectedItem as ComboBoxItem;

						if (!data.ContainsKey(degreeObjectiveCBox.Name))
						{
							data.Add(degreeObjectiveCBox.Name, citem.Content.ToString());
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.StackTrace);
			}
			try
			{
				if (springCheckBox.IsChecked.Value)
				{
					data.Add(springCheckBox.Name, "Checked");
				}
				else if (summerCheckBox.IsChecked.Value)
				{
					data.Add(summerCheckBox.Name, "Checked");
				}
				else if (fallCheckBox.IsChecked.Value)
				{
					data.Add(fallCheckBox.Name, "Checked");
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.StackTrace);
			}

			return data;
		}

		private void Checkbox_OnCheck(object sender, RoutedEventArgs e)
		{
			if (sender is CheckBox)
			{
				CheckBox cb = sender as CheckBox;
				switch (cb.Name)
				{
					case "springCheckBox":
						summerCheckBox.IsChecked = false;
						fallCheckBox.IsChecked = false;
						gradAppViewModelStatic.GradSemester = "Spring";
						break;
					case "summerCheckBox":
						springCheckBox.IsChecked = false;
						fallCheckBox.IsChecked = false;
						gradAppViewModelStatic.GradSemester = "Summer";
						break;
					case "fallCheckBox":
						springCheckBox.IsChecked = false;
						summerCheckBox.IsChecked = false;
						gradAppViewModelStatic.GradSemester = "Fall";
						break;
				}
			}
		}

		private void LoadSaveResetBtn_OnClick(object sender, RoutedEventArgs e)
		{
			if (!(sender is Button))
				return;

			Button btn = sender as Button;

			if (btn.Name.StartsWith("Save"))
			{
                Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog()
                {
                    InitialDirectory = Directory.GetCurrentDirectory(),
                    FileName = firstNameTextBox.Text ?? "",
                    Filter = "*.xml | *.XML"
                };

                var okayToSave = sfd.ShowDialog();

                if( okayToSave.Value )
                {
                    var resultFile =  XmlUtil.SaveXMLConfiguration(sfd.FileName, GetAllEntries());

                    if (!string.IsNullOrEmpty(resultFile))
                    {
                        System.Diagnostics.Process.Start("explorer.exe", resultFile);
                    }
                }
			}
			else if (btn.Name.StartsWith("Load"))
			{

			}
			else if (btn.Name.StartsWith("Reset"))
			{
				var mb = MessageBox.Show(" Clear all current entries? ", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
				if (mb == MessageBoxResult.Yes)
				{
					foreach (var tb in TextBoxes)
					{
						tb.Text = "";
					}
				}
			}
		}
	}
}
