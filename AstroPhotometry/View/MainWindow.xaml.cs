using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AstroPhotometry.ShellClasses;
using System.ComponentModel;

namespace AstroPhotometry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhotoVM photo;
        ViewModels.FileWatcherVM watcher;

        public MainWindow()
        {
            InitializeComponent();

            // wather:
            watcher = new ViewModels.FileWatcherVM("tmp", "*.png");
            
            photo = new PhotoVM(watcher);
            DataContext = photo;

            // TODO: find the pyhon folder no metter what (maybe copy the content to bin)
            string base_path = System.IO.Path.GetFullPath("../../../python/");

            PythonVM py_runner = new PythonVM(base_path, @".\tmp\");
            string[] files = { @"C:\Users\ישי טרטנר\Desktop\astro_photometry\data_test\flats\Cal-0002flat6.fit",
                                @"C:\Users\ישי טרטנר\Desktop\astro_photometry\data_test\light\Light-0034EVLac.fit"};
            //py_runner.FitsToPNG(files[0], @"test.png");
            //py_runner.run("helloworld.py", "qwer");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // String filePos = textBox_file.Text.Replace("\\", "/");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fits files (*.fits)|*.fit;*.fits|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                //txtEditor.Text = openFileDialog.FileName;
                photo.updateUri(openFileDialog.FileName);
            }
            //txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }
        private void Window_Closing(object sender, CancelEventArgs e)
        {
           e.Cancel = true;
           String path = "tmp";
           if (Directory.Exists(path))
           {
                // This path is a file
                try
                {
                    Directory.Delete(path, true);
                }
                catch(Exception exception){ 
                  MessageBox.Show(exception.ToString());
                }
           }
           e.Cancel = false;
        }
    }

}
