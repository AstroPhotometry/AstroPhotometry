using AstroPhotometry.Models;
using Microsoft.Win32;
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
            var photo = new Photo();
            DataContext = photo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Fits files (*.fits)|*.fit;*.fits|All files (*.*)|*.*";
            ////openFileDialog.Multiselect = true;


            //if (openFileDialog.ShowDialog() == DialogResult.OK)
            //    ImageFrame.Image = Image.FromFile(openFileDialog.FileName);
            //Console.WriteLine(e.GetType().ToString());
            //// String filePos = textBox_file.Text.Replace("\\", "/");
        }
        
        //public Image getPicture()
        //{

        //}
    }
}
