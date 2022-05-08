using AstroPhotometry.ViewModels;
using Microsoft.Win32;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

        private const int show_max = 2; // Max files to show

        public NodeEditor()
        {
            InitializeComponent();
        }

        private void Button_Click_Bias(object sender, RoutedEventArgs e)
        {
            bias = fileDialog();

            if (bias != null)
            {
                string tmp = Path.GetFileName(bias[0]);
                for (int i = 1; i < bias.Length && i < show_max; i++)
                {
                    tmp += ", " + Path.GetFileName(bias[i]);
                }
                if (bias.Length > show_max)
                {
                    tmp += " ...";
                }

                bias_text.Text = tmp;
            }
        }

        private void Button_Click_Dark(object sender, RoutedEventArgs e)
        {
            dark = fileDialog();

            if (dark != null)
            {
                string tmp = Path.GetFileName(dark[0]);
                for (int i = 1; i < dark.Length && i < show_max; i++)
                {
                    tmp += ", " + Path.GetFileName(dark[i]);
                }
                if (dark.Length > show_max)
                {
                    tmp += " ...";
                }

                dark_text.Text = tmp;
            }
        }

        private void Button_Click_Flat(object sender, RoutedEventArgs e)
        {
            flat = fileDialog();

            if (flat != null)
            {
                string tmp = Path.GetFileName(flat[0]);
                for (int i = 1; i < flat.Length && i < show_max; i++)
                {
                    tmp += ", " + Path.GetFileName(flat[i]);
                }
                if (flat.Length > show_max)
                {
                    tmp += " ...";
                }

                flat_text.Text = tmp;
            }
        }

        private void Button_Click_Light(object sender, RoutedEventArgs e)
        {
            light = fileDialog();

            if (light != null)
            {
                string tmp = Path.GetFileName(light[0]);
                for (int i = 1; i < light.Length && i < show_max; i++)
                {
                    tmp += ", " + Path.GetFileName(light[i]);
                }
                if (light.Length > show_max)
                {
                    tmp += " ...";
                }

                light_text.Text = tmp;
            }
        }

        private void Button_Click_Run(object sender, RoutedEventArgs e)
        {
            ComputeEngine compute = new ComputeEngine(bias, dark, flat, light);
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
            openFileDialog.Title = "Open fits files";

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileNames;
            }
            else
            {
                return null;
            }
        }
    }
}
