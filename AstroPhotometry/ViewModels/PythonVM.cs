using System;
using System.Windows.Input;

namespace AstroPhotometry
{
    // TODO: connect it to real command
    public class PythonVM : ICommand
    {
        private string python_code_folder_full_path; // the position of the python moduls
        private string output_folder_relative_path;
        private string output_full_path;

        private string python_venv_relative_path; // the folder of python.exe

        public PythonVM(string python_code_folder_full_path, string output_folder_relative_path)
        {
            this.python_code_folder_full_path = python_code_folder_full_path;

            // full path of output folder - for later
            this.output_full_path = System.IO.Path.GetFullPath(output_folder_relative_path);
            this.output_folder_relative_path = output_folder_relative_path;

            this.python_venv_relative_path = ".\\astro_env\\Scripts\\python";

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

        // TODO: add all the commands
        public void Avarage(string[] fits_files, string output_file_name)
        {
            string py_file = "FitsMath.py";

            string arguments = "-A";
            foreach (string fits_file in fits_files)
            {
                arguments += " " + "\"" + fits_file + "\"";
            }

            // TODO: check if output needs folder to exist
            arguments += " " + "\"" + this.output_folder_relative_path + output_file_name + "\"";

            run(py_file, arguments);
        }

        public void FitsToPNG(string input_fits_file, string output_file_name)
        {
            string py_file = "showfits.py";

            string arguments = "\"" + input_fits_file + "\"";

            arguments += " " + "\"" + this.output_folder_relative_path + output_file_name + "\"";

            run(py_file, arguments);
        }

        public void run(string py_file, string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            // Will look like -> [path to python]\python.exe "[path to python modules][python file]" [arguments]
            startInfo.FileName = this.python_venv_relative_path;
            startInfo.Arguments = '\"' + this.python_code_folder_full_path + py_file + '\"' + " " +  arguments;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            process.StartInfo = startInfo;
            process.Start();
            
        }
    }

}
