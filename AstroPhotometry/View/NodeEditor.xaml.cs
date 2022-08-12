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
        private string[] bias = new string[0];
        private string[] dark = new string[0];
        private string[] flat = new string[0];
        private string[] light = new string[0];

        private bool output_master_bias = false;
        private bool output_master_dark = false;
        private bool output_master_flat = false;
        private bool solve_stars_plate = false;
        private string output = "";


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
            else
            {
                bias = new string[0];
            }
        }

        private void Button_Click_Remove_Bias(object sender, RoutedEventArgs e)
        {
            bias = new string[0];
            bias_text.Text = "no files selected";
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
            else
            {
                bias = new string[0];
            }
        }

        private void Button_Click_Remove_Dark(object sender, RoutedEventArgs e)
        {
            dark = new string[0];
            dark_text.Text = "no files selected";
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
            else
            {
                bias = new string[0];
            }
        }

        private void Button_Click_Remove_Flat(object sender, RoutedEventArgs e)
        {
            flat = new string[0];
            flat_text.Text = "no files selected";
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
            else
            {
                bias = new string[0];
            }
        }

        private void Button_Click_Remove_Light(object sender, RoutedEventArgs e)
        {
            light = new string[0];
            light_text.Text = "no files selected";
        }

        private void showMessegeAndClearBar(string message)
        {
            this.DataContext = new CmdStringVM();
            MessageBox.Show(message);
        }


        private void Button_Click_Run(object sender, RoutedEventArgs e)
        {
            if (bias.Length == 0 && dark.Length == 0 && flat.Length == 0 && light.Length == 0)
            {
                showMessegeAndClearBar("Please select files");
                return;
            }
            if (bias.Length == 0 && this.output_master_bias)
            {
                showMessegeAndClearBar("Choose bias files or dont choose output of master bias");
                return;
            }
            if (dark.Length == 0 && this.output_master_dark)
            {
                showMessegeAndClearBar("Choose dark files or dont choose output of master dark");
                return;
            }
            if (flat.Length == 0 && this.output_master_flat)
            {
                showMessegeAndClearBar("Choose flat files or dont choose output of master flat");
                return;
            }
            if (output.Length == 0)
            {
                showMessegeAndClearBar("Choose output folder");
                return;
            }

            ComputeEngine compute = new ComputeEngine(bias, dark, flat, light, output, this.output_master_dark, this.output_master_bias, this.output_master_flat, this.solve_stars_plate);

            compute.run();
            this.DataContext = compute.cmdString;
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

        private string folderDialog()
        {
            System.Windows.Forms.FolderBrowserDialog openFolderDialog = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = openFolderDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(openFolderDialog.SelectedPath))
            {
                return openFolderDialog.SelectedPath;
            }
            else
            {
                return null;
            }
        }

        private void Button_Click_Out(object sender, RoutedEventArgs e)
        {
            string tmp = output;
            this.output = folderDialog();
            if (this.output == null)
            {
                this.output = tmp;
            }
            else
            {
                output_text.Text = output;
            }
        }

        private void checkbox(object sender, RoutedEventArgs e)
        {
            this.output_master_bias = (bool)output_master_bias_check.IsChecked;
            this.output_master_dark = (bool)output_master_dark_check.IsChecked;
            this.output_master_flat = (bool)output_master_flat_check.IsChecked;
            this.solve_stars_plate = (bool)solve_stars_plate_check.IsChecked;
        }
    }
}
