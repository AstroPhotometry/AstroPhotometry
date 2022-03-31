﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AstroPhotometry
{
    // TODO: connect it to real command
    public class PythonVM : ICommand
    {
        private string python_code_folder_full_path;
        private string output_folder_relative_path;
        
        public PythonVM(string python_code_folder_full_path, string output_folder_relative_path)
        { 
            this.python_code_folder_full_path = python_code_folder_full_path;

            // full path of output folder
            string output_full_path = System.IO.Path.GetFullPath(output_folder_relative_path);
            this.output_folder_relative_path = output_full_path;
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

        public void Avarage(string[] fits_files, string output_file_name)
        {
            string py_file = "FitsMath.py";

            string arguments = "-A -d";
            foreach (string fits_file in fits_files)
            {
                arguments += " " + fits_file;
            }

            arguments += " " + output_file_name;

            run(py_file, arguments);
        }

        public void run(string py_file, string arguments)
        {
            MessageBox.Show(py_file);
            MessageBox.Show(arguments);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = this.python_code_folder_full_path + py_file;
            startInfo.Arguments = arguments;
        
            process.StartInfo = startInfo;
            process.Start();
        }
    }
    
}
