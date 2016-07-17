using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

namespace Vinesauce_ROM_Corrupter
{
    /// <summary>
    /// Interaction logic for Corruption.xaml
    /// </summary>
    public partial class CorruptionWindow : Page
    {
        public CorruptionWindow()
        {
            InitializeComponent();
        }

        private void startByteSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeTextValue(startByteTextBox, e.NewValue);
            // make sure our startbyte is after this
            if (e.NewValue >= endByteSlider.Value)
                endByteSlider.Value = e.NewValue + 1;
        }

        private void startByteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeSliderValue(startByteSlider, startByteTextBox);
        }

        private void endByteSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeTextValue(endByteTextBox, e.NewValue);
            // make sure our startbyte is before this
            if (e.NewValue <= startByteSlider.Value)
                startByteSlider.Value = e.NewValue - 1;
        }

        private void endByteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ChangeSliderValue(endByteSlider, endByteTextBox);
        }

        private void ChangeTextValue(TextBox textbox, double newValue)
        {
            var val = Nemiro.Convertion.ToBase16((long)newValue);
            textbox.Text = val.ToString();
        }

        private void ChangeSliderValue(Slider slider, TextBox textbox)
        {
            try
            {
                try
                {
                    var val = Nemiro.Convertion.FromBase16(textbox.Text);
                    if (val <= slider.Maximum && val >= slider.Minimum)
                        slider.Value = val;
                    else
                    {
                        slider.Value = slider.Maximum;
                    }
                }
                catch
                {
                    slider.Value = slider.Minimum;
                }
            }
            catch
            {

            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            endByteSlider.IsEnabled =    true;
            endByteTextBox.IsEnabled =   true;
            startByteTextBox.IsEnabled = true;
            startByteSlider.IsEnabled =  true;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            endByteSlider.IsEnabled =    false;
            endByteTextBox.IsEnabled =   false;
            startByteTextBox.IsEnabled = false;
            startByteSlider.IsEnabled =  false;
        }
    }
}
