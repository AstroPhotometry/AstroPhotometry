using AstroPhotometry.common;
using System.IO;
using System.Threading.Tasks;

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

        private bool output_master_bias = false;
        private bool output_master_dark = false;
        private bool output_master_flat = false;
        private bool solve_stars_plate = false;

        private string output = "";

        public ComputeEngine(string[] bias, string[] dark, string[] flat, string[] light, string output, bool output_masterdark, bool output_masterbias, bool output_masterflat, bool solve_stars_plate)
        {
            cmdString = new CmdStringVM();

            this.bias = bias;
            this.dark = dark;
            this.flat = flat;
            this.light = light;
            this.output = output;

            this.output_master_bias = output_masterbias;
            this.output_master_dark = output_masterdark;
            this.output_master_flat = output_masterflat;
            this.solve_stars_plate = solve_stars_plate;
        }

        public async void run()
        {
            cmdString.Progress = 1;
            // Create folder
            batch_num++;
            string base_folder = Global.BATCH_FOLDER + batch_num;
            Directory.CreateDirectory(base_folder);

            // Json file path
            string json_path = base_folder + @"/calibration.json";

            // Output manage
            string output_master_bias_path = output_master_bias ? this.output : "";
            string output_master_dark_path = output_master_dark ? this.output : "";
            string output_master_flat_path = output_master_flat ? this.output : "";

            // Add json to the file
            string json = JsonSerialization.computeCalibrationJson(this.bias, this.dark, this.flat, this.light, this.output, output_master_bias_path, output_master_dark_path, output_master_flat_path, solve_stars_plate);

            // Save file
            JsonSerialization.writeToFile(json_path, json);

            // Run the json
            string base_path = Path.GetFullPath(Global.PYTHON_PATH);
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
