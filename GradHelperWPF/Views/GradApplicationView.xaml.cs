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

namespace GradHelperWPF.Views
{
    /// <summary>
    /// Interaction logic for MajorFormView.xaml
    /// </summary>
    public partial class GradApplicationView : Grid
    {
        private List<TextBox> _textboxes;
        private List<TextBox> TextBoxes
        {
            set { _textboxes = value; }
            get
            {
                if( _textboxes == null)
                {
                    _textboxes = new List<TextBox>();

                    TextBox tb;
                    try
                    {
                        foreach (var c in EntryGrid.Children)
                        {
                            try
                            {
                                if (c is TextBox && (tb = c as TextBox) != null)
                                {
                                    if (tb.Name == null)
                                        throw new Exception($" THIS TEXTBOX HAS NO NAME! : {tb.GetHashCode()}");

                                    _textboxes.Add(tb);
                                }
                                else if (c is Grid)
                                {
                                    var g = c as Grid;
                                    foreach (var grandKid in g.Children)
                                    {
                                        if (grandKid is TextBox && (tb = grandKid as TextBox) != null)
                                        {
                                            if (tb.Name == null)
                                                throw new Exception($" THIS GRANDKID TEXTBOX HAS NO NAME! {tb.GetHashCode()}");
                                            _textboxes.Add(tb);
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.StackTrace);
                            }
                        }
                    }catch(Exception exx)
                    {
                        throw new Exception(exx.StackTrace);
                    }
                }
                return _textboxes;
            }
        }

        public GradApplicationView()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void ExportToPDf_OnClick(object sender, RoutedEventArgs e)
        {

        }
    
        public Dictionary<string,string> GetAllEntries()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
         
            try
            {
                foreach(var tb in TextBoxes)
                {
                    if (string.IsNullOrEmpty(tb.Name) || data.ContainsKey(tb.Name))
                        continue;
                    data.Add(tb.Name, tb.Text);
                }
            }
            catch(Exception exx)
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
            catch(Exception ex)
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
                        break;
                    case "summerCheckBox":
                        springCheckBox.IsChecked = false;
                        fallCheckBox.IsChecked = false;
                        break;
                    case "fallCheckBox":
                        springCheckBox.IsChecked = false;
                        summerCheckBox.IsChecked = false;
                        break;
                }
            }
        }

        private void LoadSaveResetBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!(sender is Button))
                return;

            Button btn = sender as Button;

            if( btn.Name.StartsWith("Save"))
            {

            }
            else if(btn.Name.StartsWith("Load"))
            {

            }
            else if(btn.Name.StartsWith("Reset"))
            {
                var mb = MessageBox.Show(" Clear all current entries? ", "Reset", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if(mb == MessageBoxResult.Yes)
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
