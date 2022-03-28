using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroPhotometry
{
    public class PythonVM
    {
        private string python_code_folder_full_path;
        private string output_folder_relative_path;
        
        public PythonVM(string python_code_folder_full_path, string output_folder_relative_path)
        {
            this.python_code_folder_full_path = python_code_folder_full_path;
            this.output_folder_relative_path = output_folder_relative_path;
        }

        public void run(string py_file, string arguments)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = this.python_code_folder_full_path + py_file;
            startInfo.Arguments = arguments;
        
            //startInfo.Arguments = "/C copy /b Image1.jpg + Archive.rar Image2.jpg";
            process.StartInfo = startInfo;
            process.Start();
        }
    }
    
}
