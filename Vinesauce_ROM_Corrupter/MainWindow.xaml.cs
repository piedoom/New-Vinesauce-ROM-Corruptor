using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using Vinesauce_ROM_Corrupter;
using Vinesauce_ROM_Corrupter.Corruption;

namespace Vinesauce_ROM_Corruptor
{
    public enum HotkeyActions
    {
        AddStart,
        AddEnd,
        AddRange,
        SubStart,
        SubEnd,
        SubRange
    }

    public partial class MainWindow : Window
    {
        // initialize all of our pages
        public static EmulatorWindow EmulatorWindowPage = new EmulatorWindow();
        public static FileWindow FileWindowPage = new FileWindow();
        public static CorruptionWindow CorruptionWindowPage = new CorruptionWindow();
        public static AboutWindow AboutWindowPage = new AboutWindow();

        public MainWindow()
        {
            InitializeComponent();
            // init our corruption client
            Client.Setup(0,1,null,null,null,false,false,ByteCorruptionOptions.AddXToByte, 1, 0, 0, 0, 0, false, false, false, false, null, null);
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(FileWindowPage);
        }

        private void romButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(FileWindowPage);
        }

        private void corruptionButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(CorruptionWindowPage);
        }

        private void emulatorButton_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(EmulatorWindowPage);
        }

        private void aboutButton_Click(object sender, RoutedEventArgs e)
        {
           mainFrame.Navigate(AboutWindowPage);
        }

        private async void runCorruptorButton_Click(object sender, RoutedEventArgs e)
        {
            // only run if we have a rom already loaded
                // get the following values from our forms
                // the sane thing to do here would be to do databinding on a background object instead of
                // picking apart the UI, but I'm lazy.
                Client.ByteCorruptionEnable = CorruptionWindowPage.byteCorruptionEnabledUI.IsChecked.Value;
                // Client.ByteCorruptionOption = 
                Client.ColorReplacementEnable = false; //TODO: implement
                Client.ColorUseByteCorruptionRange = false; //TODO: implement
                Client.EnableNESCPUJamProtection = false; //TODO: implement
                Client.EndByte = (long)CorruptionWindowPage.endByteSlider.Value;
                Client.EveryNthByte = 10; //TODO: implement
                Client.ROM = System.IO.File.ReadAllBytes(FileWindowPage.ROM.FilePath);
                Client.StartByte = (long)CorruptionWindowPage.startByteSlider.Value;
                Client.ShiftRightXBytes = 0; // TODO: implement
                Client.TextReplacementEnable = false; // TODO: implement
                Client.TextUseByteCorruptionRange = false; // TODO: implement
                Client.ByteCorruptionOption = ByteCorruptionOptions.AddXToByte; // TODO: implement
                Client.AddXtoByte = 1; // TODO: implement

                byte[] corruptedRom = await Client.CorruptAsync();

                // local stuff
                var saveFilePath = FileWindowPage.fileNameText.Text;

                // save to file
                System.IO.File.WriteAllBytes(saveFilePath, corruptedRom);
        }
    }
}
