using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    internal class PythonCmdBE
    {
        private string files_pos;

        public PythonCmdBE(string files_pos, string temp_folder_pos)
        {
            this.files_pos = files_pos;
            TempFolderPos = temp_folder_pos;
        }

        public string TempFolderPos { get; set; }

        public void cleanTempFolder()
        {
            //TODO: fill it
        }

        public string run(string python_with_args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "Python.exe";
            startInfo.Arguments = python_with_args;
            process.StartInfo = startInfo;
            process.Start();
            //process. // TODO: get the output
            return "hi";
        }
    }
}
