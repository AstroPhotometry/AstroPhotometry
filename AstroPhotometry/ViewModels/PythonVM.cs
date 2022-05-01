using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace AstroPhotometry
{
    // TODO: connect it to real command
    public class PythonVM : ICommand
    {
        private string python_code_folder_full_path;
        private string output_folder_relative_path;
        private string output_full_path;

        public PythonVM(string python_code_folder_full_path, string output_folder_relative_path)
        {
            this.python_code_folder_full_path = python_code_folder_full_path;

            // full path of output folder - for later
            this.output_full_path = System.IO.Path.GetFullPath(output_folder_relative_path);
            this.output_folder_relative_path = output_folder_relative_path;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
        public void MathActions(string dir_path, string output_file_name, string action)
        {
            string argument = " ";
            if (action.Equals("Addition"))
            {
                argument += "-a";
            }
            else if (action.Equals("Avarage"))
            {
                argument += "-A";
            }
            else if (action.Equals("Minus"))
            {
                argument += "-m";
            }
            else if (action.Equals("Multiplication"))
            {
                argument += "-M";
            }
            else if (action.Equals("Division"))
            {
                argument += "-d";
            }

            string py_file = "FitsMath.py";


            // TODO: check if output needs folder to exist
            argument = " -folder " +"\""+ dir_path+ "\"" + " -f " + "\"" + this.output_folder_relative_path + output_file_name + "\"" + argument;
            MessageBox.Show(argument);
            run(py_file, argument);
        }



        public void MathActions(string[] fits_files, string output_file_name, string action)
        {
            string argument = "FitsMath.py";
            if (action.Equals("Addition"))
            {
                argument += "-a";
            }else if (action.Equals("Avarage"))
            {
                argument += "-A";
            }
            else if (action.Equals("Minus"))
            {
                argument += "-m";
            }
            else if (action.Equals("Multiplication"))
            {
                argument += "-M";
            }
            else if (action.Equals("Division"))
            {
                argument += "-d";
            }

            string py_file = "FitsMath.py";

            foreach (string fits_file in fits_files)
            {
                argument += " " + "\"" + fits_file + "\"";
            }

            // TODO: check if output needs folder to exist
            argument += " " + "\"" + this.output_folder_relative_path + output_file_name + ".py\"";

            run(py_file, argument);
        }

        public void FitsToPNG(string input_fits_file, string output_file_name)
        {

            if (File.Exists("tmp\\" + output_file_name))
            {
                // TODO: show the existing picture 
                MessageBox.Show("exist");
              
            }
            else
            {
                string py_file = "FitsToPNG.py";
                string arguments = "-f " + "\"" + input_fits_file + "\"";
                string tmp_loc = " -o \"tmp\\";
                arguments += tmp_loc + output_file_name;
                run(py_file, arguments);
            }
        }

        public void run(string py_file, string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            startInfo.FileName = this.python_code_folder_full_path + py_file;
            startInfo.Arguments = arguments;

            process.StartInfo = startInfo;
            process.Start();

            process.WaitForExit();
            if(process.ExitCode != 0)
            {
                MessageBox.Show("Procces exit code is "+ process.ExitCode);
            }
        }
    }

}
