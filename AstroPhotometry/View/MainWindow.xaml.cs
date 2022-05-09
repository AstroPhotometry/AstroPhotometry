using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AstroPhotometry.ShellClasses;
using System.ComponentModel;
using AstroPhotometry.ViewModels;

namespace AstroPhotometry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhotoVM photo;

        public MainWindow()
        {
            InitializeComponent();

            // watcher:
            var watcher = new ViewModels.FileWatcherVM("./tmp/", "*.png");
            photo = new PhotoVM(watcher);
            DataContext = photo;

            // TODO: find the pyhon folder no metter what (maybe copy the content to bin)
            // string base_path = Path.GetFullPath("../../../python/");
            // CmdStringVM cmdString = new CmdStringVM();

            // PythonVM py_runner = new PythonVM(base_path, @".\tmp\", cmdString);
            // string[] files = { @"C:\Users\ישי טרטנר\Desktop\astro_photometry\data_test\flats\Cal-0002flat6.fit",
            //                     @"C:\Users\ישי טרטנר\Desktop\astro_photometry\data_test\light\Light-0034EVLac.fit"};
            //py_runner.FitsToPNG(files[0], @"test.png");
            //py_runner.run("helloworld.py", "qwer");

        }
    }

}
