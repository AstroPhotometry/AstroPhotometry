using AstroPhotometry.ViewModels;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AstroPhotometry
{
    // TODO: connect it to real command
    public class PythonVM : ICommand
    {
        private string python_code_folder_full_path; // The position of the python modules
        private string output_folder_relative_path;
        private string output_full_path;

        private string python_venv_relative_path; // The folder of python.exe

        private CmdStringVM cmdString;
        public PythonVM(string python_code_folder_full_path, string output_folder_relative_path, CmdStringVM cmdString)
        {
            this.python_code_folder_full_path = python_code_folder_full_path;

            // full path of output folder - for later
            this.output_full_path = System.IO.Path.GetFullPath(output_folder_relative_path);
            this.output_folder_relative_path = output_folder_relative_path;

            this.python_venv_relative_path = ".\\astro_env\\Scripts\\python";

            this.cmdString = cmdString; // What bridge out the output of the pythons

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
            argument = " -folder " + "\"" + dir_path + "\"" + " -f " + "\"" + this.output_folder_relative_path + output_file_name + "\"" + argument;
            MessageBox.Show(argument);
            run(py_file, argument);
        }



        public void MathActions(string[] fits_files, string output_file_name, string action)
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

            // Add files to the args
            if(fits_files.Length != 0)
            {
                argument += " -i ";
            }
            foreach (string fits_file in fits_files)
            {
                argument += " " + "\"" + fits_file + "\"";
            }

            // TODO: check if output needs folder to exist
            argument += " -f " + "\"" + this.output_folder_relative_path + output_file_name + ".fit\"";

            string py_file = "FitsMath.py";

            run(py_file, argument);
        }

        public void FitsToPNG(string input_fits_file, string output_file_name)
        {

            /*if (File.Exists(this.output_folder_relative_path + output_file_name))
            {
                // TODO: show the existing picture 
                MessageBox.Show("exist");
            }
            else*/
            {
                string py_file = "FitsToPNG.py";
                string arguments = "-f \"" + input_fits_file + "\"";
                arguments += " " + "-o \"" + this.output_folder_relative_path + output_file_name + "\"";
                run(py_file, arguments);
            }
        }

        public void run(string py_file, string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            // Will look like -> [path to python in venv]\python.exe "[path to python modules][python file]" [arguments]
            startInfo.FileName = this.python_venv_relative_path;
            startInfo.Arguments = '\"' + this.python_code_folder_full_path + py_file + '\"' + " " + arguments;
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            // for redireting output
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            process.StartInfo = startInfo;
            process.Start();

            // For async showing th progress
            async Task Main()
            {
                await ReadCharacters();
            }

            async Task ReadCharacters()
            {
                while (!process.HasExited)
                {
                    string tmp = await process.StandardOutput.ReadLineAsync();
                    cmdString.Output = tmp; // TODO: parse and push message and progress

                    // For debug
                    //MessageBox.Show(tmp);
                    //Console.WriteLine(tmp);
                }
            }
            _ = Main();

            process.WaitForExit();
            if (process.ExitCode != 0)
            {
                MessageBox.Show("Procces exit code is " + process.ExitCode);
            }
        }
    }

}
