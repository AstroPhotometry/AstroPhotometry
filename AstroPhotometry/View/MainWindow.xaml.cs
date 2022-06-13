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
        }
    }

}
