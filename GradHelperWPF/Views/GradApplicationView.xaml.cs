using GradHelperWPF.Utils;
using GradHelperWPF.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GradHelperWPF.Views
{
    /// <summary>
    ///     Interaction logic for MajorFormView.xaml
    /// </summary>
    public partial class GradApplicationView : StackPanel
    {
        /// <summary>
        ///     Student's data in GradApplicationView
        /// </summary>
        public static GradAppViewModel gradAppViewModelStatic = new GradAppViewModel();

        public GradApplicationView( )
        {
            InitializeComponent( );
            DataContext = gradAppViewModelStatic;
        }

        public List<TextBox> TextBoxes
        {
            get
            {
                var textboxes = new List<TextBox>
                {
                    firstNameTextBox,
                    middleNameTextBox,
                    lastNameTextBox,
                    emailTextBox,
                    phoneTextBox,
                    sidNameTextBox,
                    streetNumberTextBox,
                    streetNameTextBox,
                    apartmentTextBox,
                    cityTextBox,
                    stateTextBox,
                    zipcodeTextBox,
                    majorTextBox,
                    yearTextBox,
                    uncompletedTextBox1,
                    uncompletedTextBox2,
                    uncompletedTextBox3,
                    uncompletedTextBox4,
                    uncompletedTextBox5,
                    uncompletedTextBox6,
                    uncompletedTextBox7,
                    uncompletedTextBox8,
                    currentEnrolled1,
                    currentEnrolled2,
                    currentEnrolled3,
                    currentEnrolled4,
                    currentEnrolled5,
                    currentEnrolled6,
                    currentEnrolled7,
                    currentEnrolled8,
                    new TextBox {Name = "DegreeObjective", Text = gradAppViewModelStatic.DegreeObjective ?? ""},
                    new TextBox {Name = "GradSemester", Text = gradAppViewModelStatic.GradSemester ?? ""}
                };
                return textboxes;
            }
        }

        private void ExportToPDf_OnClick( object sender, RoutedEventArgs e )
        {
            var sfd = new SaveFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                Filter = "*.pdf | *.PDF"
            };

            var result = sfd.ShowDialog();
            if ( result != null && result.Value )
            {
                gradAppViewModelStatic.ExportToPDF(sfd.FileName);
            }
        }

        public Dictionary<string, string> GetAllEntries( )
        {
            var data = new Dictionary<string, string>();

            try
            {
                foreach ( var tb in TextBoxes )
                {
                    if ( string.IsNullOrEmpty( tb.Name ) || data.ContainsKey( tb.Name ) )
                        continue;
                    data.Add( tb.Name, tb.Text );
                }
            }
            catch ( Exception exx )
            {
                throw new Exception( exx.StackTrace );
            }
            try
            {
                if ( degreeObjectiveCBox != null && degreeObjectiveCBox.SelectedItem != null )
                    if ( degreeObjectiveCBox.SelectedItem is ComboBoxItem )
                    {
                        var citem = degreeObjectiveCBox.SelectedItem as ComboBoxItem;

                        if ( !data.ContainsKey( degreeObjectiveCBox.Name ) )
                            data.Add( degreeObjectiveCBox.Name, citem.Content.ToString( ) );
                    }
            }
            catch ( Exception ex )
            {
                throw new Exception( ex.StackTrace );
            }
            try
            {
                if ( springCheckBox.IsChecked.Value )
                    data.Add( springCheckBox.Name, "Checked" );
                else if ( summerCheckBox.IsChecked.Value )
                    data.Add( summerCheckBox.Name, "Checked" );
                else if ( fallCheckBox.IsChecked.Value )
                    data.Add( fallCheckBox.Name, "Checked" );
            }
            catch ( Exception ex )
            {
                throw new Exception( ex.StackTrace );
            }

            return data;
        }

        private void Checkbox_OnCheck( object sender, RoutedEventArgs e )
        {
            if ( sender is CheckBox )
            {
                var cb = sender as CheckBox;
                switch ( cb.Name )
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


				if(cb.IsChecked != null && !cb.IsChecked.Value)
				{
					cb.IsChecked = true;					
				}
			}
        }

        private void PopulateTextBoxes( Dictionary<string, string> data )
        {
            var tbs = TextBoxes.ToDictionary(k => k.Name);

            foreach ( var entry in data )
			{
				if (tbs.ContainsKey(entry.Key))
				{
					switch (entry.Key)
					{
						//gradAppViewModelStatic
						case "firstNameTextBox":
							gradAppViewModelStatic.FirstName = entry.Value;
							break;
						case "middleNameTextBox":
							gradAppViewModelStatic.MiddleName = entry.Value;
							break;
						case "lastNameTextBox":
							gradAppViewModelStatic.LastName = entry.Value;
							break;
						case "emailTextBox":
							gradAppViewModelStatic.Email = entry.Value;
							break;
						case "phoneTextBox":
							gradAppViewModelStatic.PhoneNumber = entry.Value;
							break;
						case "sidNameTextBox":
							gradAppViewModelStatic.StudentID = entry.Value;
							break;
						case "streetNumberTextBox":
							gradAppViewModelStatic.StreetNumber = entry.Value;
							break;
						case "streetNameTextBox":
							gradAppViewModelStatic.StreetName = entry.Value;
							break;
						case "apartmentTextBox":
							gradAppViewModelStatic.ApartmentNumber = entry.Value;
							break;
						case "cityTextBox":
							gradAppViewModelStatic.City = entry.Value;
							break;
						case "stateTextBox":
							gradAppViewModelStatic.State = entry.Value;
							break;
						case "zipcodeTextBox":
							gradAppViewModelStatic.Zipcode = entry.Value;
							break;
						case "majorTextBox":
							gradAppViewModelStatic.MajorName = entry.Value;
							break;
						case "yearTextBox":
							gradAppViewModelStatic.GradYear = entry.Value;
							break;
						case "uncompletedTextBox1":
							gradAppViewModelStatic.NonSJSUCourseCompleted1 = entry.Value;
							break;
						case "uncompletedTextBox2":
							gradAppViewModelStatic.NonSJSUCourseCompleted2 = entry.Value;
							break;
						case "uncompletedTextBox3":
							gradAppViewModelStatic.NonSJSUCourseCompleted3 = entry.Value;
							break;
						case "uncompletedTextBox4":
							gradAppViewModelStatic.NonSJSUCourseCompleted4 = entry.Value;
							break;
						case "uncompletedTextBox5":
							gradAppViewModelStatic.NonSJSUCourseCompleted5 = entry.Value;
							break;
						case "uncompletedTextBox6":
							gradAppViewModelStatic.NonSJSUCourseCompleted6 = entry.Value;
							break;
						case "uncompletedTextBox7":
							gradAppViewModelStatic.NonSJSUCourseCompleted7 = entry.Value;
							break;
						case "uncompletedTextBox8":
							gradAppViewModelStatic.NonSJSUCourseCompleted8 = entry.Value;
							break;
						case "currentEnrolled1":
							gradAppViewModelStatic.CurrentEnrolledCourse1 = entry.Value;
							break;
						case "currentEnrolled2":
							gradAppViewModelStatic.CurrentEnrolledCourse2 = entry.Value;
							break;
						case "currentEnrolled3":
							gradAppViewModelStatic.CurrentEnrolledCourse3 = entry.Value;
							break;
						case "currentEnrolled4":
							gradAppViewModelStatic.CurrentEnrolledCourse4 = entry.Value;
							break;
						case "currentEnrolled5":
							gradAppViewModelStatic.CurrentEnrolledCourse5 = entry.Value;
							break;
						case "currentEnrolled6":
							gradAppViewModelStatic.CurrentEnrolledCourse6 = entry.Value;
							break;
						case "currentEnrolled7":
							gradAppViewModelStatic.CurrentEnrolledCourse7 = entry.Value;
							break;
						case "currentEnrolled8":
							gradAppViewModelStatic.CurrentEnrolledCourse8 = entry.Value;
							break;
						case "DegreeObjective":
							SelectDegreeObjective(entry.Value);
							gradAppViewModelStatic.DegreeObjective = entry.Value;
							break;

						case "GradSemester":
							if (entry.Value == "Spring")
							{
								Checkbox_OnCheck(springCheckBox, new RoutedEventArgs() { });
							}
							else if (entry.Value == "Summer")
							{
								Checkbox_OnCheck(summerCheckBox, new RoutedEventArgs() { });
							}
							else if (entry.Value == "Fall")
							{
								Checkbox_OnCheck(fallCheckBox, new RoutedEventArgs() { });
							}
							break;
					}
				}
			}			
        }


		private void SelectDegreeObjective(string key)
		{
			var cmBox = degreeObjectiveCBox;

			int selectedIndex = -1;
			for (int i = 0; i < cmBox.Items.Count; i++)
			{
				if (cmBox.Items[i].ToString().Contains(key))
					selectedIndex = i;
			}

			if( selectedIndex >= 0)
			{
				cmBox.SelectedIndex = selectedIndex;
			}
		}

        private void LoadSaveResetBtn_OnClick( object sender, RoutedEventArgs e )
        {
            if ( !( sender is Button ) )
                return;

            var btn = sender as Button;
            try
            {
                if ( btn.Name.StartsWith( "Save" ) )
                {
                    var sfd = new SaveFileDialog
                    {
                        InitialDirectory = Directory.GetCurrentDirectory(),
                        FileName = firstNameTextBox.Text ?? "",
                        Filter = "*.xml | *.XML"
                    };

                    var okayToSave = sfd.ShowDialog();

                    if ( okayToSave.Value )
                    {
                        var resultFile = XmlUtil.SaveXmlConfiguration(sfd.FileName, GetAllEntries());

                        if ( !string.IsNullOrEmpty( resultFile ) )
                            Process.Start( "explorer.exe", resultFile );
                    }
                }
                else if ( btn.Name.StartsWith( "Load" ) )
                {
                    var ofd = new OpenFileDialog
                    {
                        InitialDirectory = Directory.GetCurrentDirectory(),
                        FileName = firstNameTextBox.Text ?? "",
                        Filter = "*.xml | *.XML"
                    };

                    var opened = ofd.ShowDialog();

                    if ( opened.Value && File.Exists( ofd.FileName ) )
                    {
                        var data = XmlUtil.LoadXmlConfiguration(ofd.FileName);
                        PopulateTextBoxes( data );
                    }
                }
                else if ( btn.Name.StartsWith( "Reset" ) )
                {
                    var mb = MessageBox.Show(" Clear all current entries? ", "Reset", MessageBoxButton.YesNo,
                        MessageBoxImage.Question, MessageBoxResult.No);
                    if ( mb == MessageBoxResult.Yes )
					{
						Dictionary<string, string> emptyData = new Dictionary<string, string>();
						var textBoxNames = TextBoxes.Select(tb => tb.Name).ToList();
						
						foreach(var tb in textBoxNames)
						{
							if (!emptyData.ContainsKey(tb))
								emptyData.Add(tb, "");
						}

						PopulateTextBoxes(emptyData);
					}
                }
            }
            catch ( Exception ex )
            {
            }
        }

        private void ButtonsGroupBox_PreviewDragOver( object sender, DragEventArgs e )
        {
            e.Handled = true;
        }

        private void ButtonsGroupBox_Drop( object sender, DragEventArgs e )
        {
            var hasData = e != null && e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop);
            if ( hasData )
            {
                var files = e.Data.GetData(DataFormats.FileDrop) as string[];
                var xmlFile = files != null ? files.Where(f => f.ToLower().Contains(".xml")).FirstOrDefault() : null;
                try
                {
                    if ( xmlFile != null )
                    {
                        var data = XmlUtil.LoadXmlConfiguration(xmlFile);
                        PopulateTextBoxes( data );
                    }
                }
                catch ( Exception ex )
                {
                    throw new Exception( ex.StackTrace );
                }
            }
        }
    }
}