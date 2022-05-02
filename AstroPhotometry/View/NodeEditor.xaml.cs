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

namespace AstroPhotometry.View
{
    /// <summary>
    /// Interaction logic for NodeEditor.xaml
    /// </summary>
    public partial class NodeEditor : UserControl
    {
        private string[] bias;
        private string[] dark;
        private string[] flat;
        private string[] light;
        
        public NodeEditor()
        {
            InitializeComponent();
        }

        private void Button_Click_Bias(object sender, RoutedEventArgs e)
        {
            bias = fileDialog();
        }

        private void Button_Click_Dark(object sender, RoutedEventArgs e)
        {
            dark = fileDialog();

        }

        private void Button_Click_Flat(object sender, RoutedEventArgs e)
        {
            flat = fileDialog();

        }

        private void Button_Click_Light(object sender, RoutedEventArgs e)
        {
            light = fileDialog();

        }

        private void Button_Click_Run(object sender, RoutedEventArgs e)
        {
            if (bias != null && dark != null && flat != null && light != null)
            {
                MessageBox.Show("running");
            }
            else
            {
                MessageBox.Show("somthing missing");
            }
        }

        private string[] fileDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Fits files (*.fits)|*.fit;*.fits|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                // debug
                string txt = "";
                foreach (string file in openFileDialog.FileNames)
                {
                    txt += " " + file;
                }
                MessageBox.Show(txt);
                //
                return openFileDialog.FileNames;
            }
            else
            {
                return null;
            }
        }
    }
}
