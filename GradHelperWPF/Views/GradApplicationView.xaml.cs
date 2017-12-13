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
    public partial class GradApplicationView
    {
        /// <summary>
        ///     Student's data in GradApplicationView
        /// </summary>
        public static GradAppViewModel GradAppViewModelStatic = new GradAppViewModel();

        public GradApplicationView( )
        {
            InitializeComponent( );
            DataContext = GradAppViewModelStatic;
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
                    new TextBox {Name = "DegreeObjective", Text = GradAppViewModelStatic.DegreeObjective ?? ""},
                    new TextBox {Name = "GradSemester", Text = GradAppViewModelStatic.GradSemester ?? ""}
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
                GradAppViewModelStatic.ExportToPDF(sfd.FileName);
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
                if ( springCheckBox.IsChecked != null && springCheckBox.IsChecked.Value )
                    data.Add( springCheckBox.Name, "Checked" );
                else if ( summerCheckBox.IsChecked != null && summerCheckBox.IsChecked.Value )
                    data.Add( summerCheckBox.Name, "Checked" );
                else if ( fallCheckBox.IsChecked != null && fallCheckBox.IsChecked.Value )
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
                        GradAppViewModelStatic.GradSemester = "Spring";
                        break;

                    case "summerCheckBox":
                        springCheckBox.IsChecked = false;
                        fallCheckBox.IsChecked = false;
                        GradAppViewModelStatic.GradSemester = "Summer";
                        break;

                    case "fallCheckBox":
                        springCheckBox.IsChecked = false;
                        summerCheckBox.IsChecked = false;
                        GradAppViewModelStatic.GradSemester = "Fall";
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
							GradAppViewModelStatic.FirstName = entry.Value;
							break;
						case "middleNameTextBox":
							GradAppViewModelStatic.MiddleName = entry.Value;
							break;
						case "lastNameTextBox":
							GradAppViewModelStatic.LastName = entry.Value;
							break;
						case "emailTextBox":
							GradAppViewModelStatic.Email = entry.Value;
							break;
						case "phoneTextBox":
							GradAppViewModelStatic.PhoneNumber = entry.Value;
							break;
						case "sidNameTextBox":
							GradAppViewModelStatic.StudentID = entry.Value;
							break;
						case "streetNumberTextBox":
							GradAppViewModelStatic.StreetNumber = entry.Value;
							break;
						case "streetNameTextBox":
							GradAppViewModelStatic.StreetName = entry.Value;
							break;
						case "apartmentTextBox":
							GradAppViewModelStatic.ApartmentNumber = entry.Value;
							break;
						case "cityTextBox":
							GradAppViewModelStatic.City = entry.Value;
							break;
						case "stateTextBox":
							GradAppViewModelStatic.State = entry.Value;
							break;
						case "zipcodeTextBox":
							GradAppViewModelStatic.Zipcode = entry.Value;
							break;
						case "majorTextBox":
							GradAppViewModelStatic.MajorName = entry.Value;
							break;
						case "yearTextBox":
							GradAppViewModelStatic.GradYear = entry.Value;
							break;
						case "uncompletedTextBox1":
							GradAppViewModelStatic.NonSJSUCourseCompleted1 = entry.Value;
							break;
						case "uncompletedTextBox2":
							GradAppViewModelStatic.NonSJSUCourseCompleted2 = entry.Value;
							break;
						case "uncompletedTextBox3":
							GradAppViewModelStatic.NonSJSUCourseCompleted3 = entry.Value;
							break;
						case "uncompletedTextBox4":
							GradAppViewModelStatic.NonSJSUCourseCompleted4 = entry.Value;
							break;
						case "uncompletedTextBox5":
							GradAppViewModelStatic.NonSJSUCourseCompleted5 = entry.Value;
							break;
						case "uncompletedTextBox6":
							GradAppViewModelStatic.NonSJSUCourseCompleted6 = entry.Value;
							break;
						case "uncompletedTextBox7":
							GradAppViewModelStatic.NonSJSUCourseCompleted7 = entry.Value;
							break;
						case "uncompletedTextBox8":
							GradAppViewModelStatic.NonSJSUCourseCompleted8 = entry.Value;
							break;
						case "currentEnrolled1":
							GradAppViewModelStatic.CurrentEnrolledCourse1 = entry.Value;
							break;
						case "currentEnrolled2":
							GradAppViewModelStatic.CurrentEnrolledCourse2 = entry.Value;
							break;
						case "currentEnrolled3":
							GradAppViewModelStatic.CurrentEnrolledCourse3 = entry.Value;
							break;
						case "currentEnrolled4":
							GradAppViewModelStatic.CurrentEnrolledCourse4 = entry.Value;
							break;
						case "currentEnrolled5":
							GradAppViewModelStatic.CurrentEnrolledCourse5 = entry.Value;
							break;
						case "currentEnrolled6":
							GradAppViewModelStatic.CurrentEnrolledCourse6 = entry.Value;
							break;
						case "currentEnrolled7":
							GradAppViewModelStatic.CurrentEnrolledCourse7 = entry.Value;
							break;
						case "currentEnrolled8":
							GradAppViewModelStatic.CurrentEnrolledCourse8 = entry.Value;
							break;
						case "DegreeObjective":
							SelectDegreeObjective(entry.Value);
							GradAppViewModelStatic.DegreeObjective = entry.Value;
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

                    if ( okayToSave != null && okayToSave.Value )
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

                    if ( opened != null && (opened.Value && File.Exists( ofd.FileName )) )
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
                MessageBox.Show("Exception while saving XML Config. " + ex.StackTrace);
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

                var xmlFile = files?.Where(f => f.ToLower().Contains(".xml")).FirstOrDefault();
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
                    MessageBox.Show("Exception thrown while loading XML Config" + ex.StackTrace);
                }
            }
        }
    }
}