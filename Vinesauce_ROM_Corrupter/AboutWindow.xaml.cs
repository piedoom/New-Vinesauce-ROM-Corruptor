using Newtonsoft.Json;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vinesauce_ROM_Corrupter.Changelog;
using System.Reflection;

/* 
 * doomy
 * 
 * Hi!  I'm the owner of these new cool WPF files.  VRC is great, but unforutnately it is licensed under
 * the very restrictive GPL.  However, these new items I have committed that are not part of the original
 * corruption source created by Rikerz are licensed under the BSD 3-Clause.
 */


namespace Vinesauce_ROM_Corrupter
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Page
    {
        bool loaded;

        public AboutWindow()
        {
            InitializeComponent();
            loaded = false;
            this.Loaded += AboutWindow_Loaded;
        }

        private void AboutWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (loaded == false)
            {
                // parse our changelog
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = "Vinesauce_ROM_Corrupter.Changelog.changelog.json";
                Stream stream = assembly.GetManifestResourceStream(resourceName);
                StreamReader reader = new StreamReader(stream);
                string json = reader.ReadToEnd();
                List<ChangeItem> changelog = JsonConvert.DeserializeObject<List<ChangeItem>>(json);
                changelog.Reverse();

                // add these items to our UI.  They never change dynamically, so we don't need databinding.
                foreach (ChangeItem item in changelog)
                {
                    changelogListBox.Items.Add(item);
                }

                reader.Close();
                stream.Close();
                loaded = true;
            }
        }
    }
}
