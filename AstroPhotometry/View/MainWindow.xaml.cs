using Microsoft.Win32;
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

namespace AstroPhotometry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Photo photo;
        public MainWindow()
        {
            InitializeComponent();
            var photo = new PhotoVM();
            DataContext = photo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // String filePos = textBox_file.Text.Replace("\\", "/");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fits files (*.fits)|*.fit;*.fits|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
                txtEditor.Text = openFileDialog.FileName;
                //txtEditor.Text = File.ReadAllText(openFileDialog.FileName);
        }

        //public Image getPicture()
        //{

        //}
    }
}
