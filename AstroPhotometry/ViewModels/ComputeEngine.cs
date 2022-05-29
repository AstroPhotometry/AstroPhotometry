using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace AstroPhotometry.ViewModels
{
    // This class is for computing the output fit by recipe
    public class ComputeEngine
    {
        public CmdStringVM cmdString;

        private PythonVM py_runner;
        private static int batch_num = 0; // Running name of folders

        private string[] bias;
        private string[] dark;
        private string[] flat;
        private string[] light;
        private string output = "";

        public ComputeEngine(string[] bias, string[] dark, string[] flat, string[] light, string output)
        {
            cmdString = new CmdStringVM();

            this.bias = bias;
            this.dark = dark;
            this.flat = flat;
            this.light = light;
            this.output = output;
            run();
        }

        private async void run()
        {
            // Create folder
            batch_num++;
            string base_folder = @".\tmp\batch" + batch_num;
            Directory.CreateDirectory(base_folder);

            // Json file path
            string json_path = base_folder + @"/calibration.json";

            // Add json to the file
            string json = JsonSerialization.computeCalibrationJson(this.bias, this.dark, this.flat, this.light, this.output);
            
            // Save file
            JsonSerialization.writeToFile(json_path, json);

            // Run the json
            string base_path = Path.GetFullPath("../../../python/");
            py_runner = new PythonVM(base_path, base_folder, cmdString);
            string json_full_path = Path.GetFullPath(json_path);
            py_runner.runByJsonPath(json_full_path);

            // Wait
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

        }
    }
}
