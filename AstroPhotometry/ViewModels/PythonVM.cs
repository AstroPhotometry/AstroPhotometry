using AstroPhotometry.ViewModels;
using Newtonsoft.Json;
using System;
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
        private bool running;

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
            this.running = false;
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

        public bool Running
        {
            get { return running; }
        }

        // Send to calibrate
        public void runByJsonPath(string json_path)
        {
            string py_file = "main.py";
            string arguments = "-j \"" + json_path + "\"";

            MessageBox.Show(arguments);
            run(py_file, arguments);
        }

        public void run(string py_file, string arguments)
        {
            running = true;
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();

            // Will look like -> [path to python in venv]\python.exe "[path to python modules][python file]" [arguments]
            startInfo.FileName = this.python_venv_relative_path;
            startInfo.Arguments = '\"' + this.python_code_folder_full_path + py_file + '\"' + " " + arguments;
            MessageBox.Show(startInfo.Arguments);
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden; // Normal for debug

            // for redireting output
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = false;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true; // false for debug

            // Connecting the end and the readline functions
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += (sender, args) => ReadCharacters(args.Data);
            process.Exited += new EventHandler(Process_Exited);

            // Start the process
            process.StartInfo = startInfo;
            process.Start();

            process.BeginOutputReadLine();
        }

        // Handle Exited event and display process information.
        private void Process_Exited(object sender, System.EventArgs e)
        {
            // Check for errors
            if (((System.Diagnostics.Process)sender).ExitCode != 0)
            {
                MessageBox.Show("Python exit code is " + ((System.Diagnostics.Process)sender).ExitCode);
            }
            running = false;
        }

        // convert string to messege
        private void ReadCharacters(string data)
        {
            if (data == null)
            {
                return;
            }
            try
            {
                Progress progress = JsonConvert.DeserializeObject<Progress>(data);
                cmdString.Message = progress.message;
                cmdString.Progress = progress.progress;
            }
            catch
            {
                string err = "ERROR: could not Deserialize Object";
                cmdString.Message = err;
                Console.WriteLine(err);
            }

            // For debug
            Console.WriteLine(data);
        }

        // The parsed json from the python process
        public class Progress
        {
            public string module_name { get; set; }
            public string message { get; set; }
            public float progress { get; set; }
        }
    }
}
