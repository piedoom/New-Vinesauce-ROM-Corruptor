using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Vinesauce_ROM_Corruptor;

namespace Vinesauce_ROM_Corrupter
{
    /// <summary>
    /// Interaction logic for FileWindow.xaml
    /// </summary>
    public partial class FileWindow : Page
    {
        public ROM.RomID ROM;

        public FileWindow()
        {
            InitializeComponent();
            romList.SelectionChanged += RomList_SelectionChanged;
        }

        private void RomList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // automatically set the save file 
            var filePath = ((File)romList.SelectedItem).FullPath;
            fileNameText.Text = File.ConvertFileNameToCorruptedName(filePath);

            Console.WriteLine(filePath);

            // load our ROM as a byte array
            ROM = new ROM.RomID(filePath);
            //Corruption.Client.ROM = ROMArray;

            // set the range of our sliders in the corruption view
            MainWindow.CorruptionWindowPage.startByteSlider.Maximum = ROM.FileLength;
            MainWindow.CorruptionWindowPage.endByteSlider.Maximum = ROM.FileLength;
        }

        private void openFolder_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            // change the displayed path text
            folderName.Text = dialog.SelectedPath;

            // populate our file field with rom names
            PopulateFileList(dialog.SelectedPath);
        }

        private void PopulateFileList(string path)
        {
            if (path.Length > 0)
            {
                string[] fileNames = Directory.GetFiles(path, "*", SearchOption.TopDirectoryOnly);
                if (fileNames.Count() > 0)
                {
                    List<File> fileList = File.FileListBuilder(fileNames);
                    romList.DataContext = fileList;
                    romList.ItemsSource = fileList;

                    // select the first item if any
                    if (romList.Items.Count > 0)
                    {
                        romList.SelectedItem = romList.Items[0];
                    }
                }
            }
        }

        private void saveAsFile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
