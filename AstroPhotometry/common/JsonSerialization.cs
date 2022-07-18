using Newtonsoft.Json;
using System.IO;

namespace AstroPhotometry
{
    public static class JsonSerialization
    {
        // Add data for calibration process
        public static string computeCalibrationJson(string[] bias, string[] dark, string[] flat, string[] light, string output, string output_master_bias_path, string output_master_dark_path, string output_master_flat_path)
        {
            Calibration cal = new Calibration();
            cal.calibrate(bias, dark, flat, light, output, output_master_bias_path, output_master_dark_path, output_master_flat_path);
            string json = JsonConvert.SerializeObject(cal);
            return json;
        }

        // Add data for image creation process
        public static string fitsToPNG(string input_fit_file, string output_file_name)
        {
            Calibration cal = new Calibration();
            cal.fitConvertion(input_fit_file, output_file_name);
            string json = JsonConvert.SerializeObject(cal);
            return json;
        }

        public static void writeToFile(string filename, string data)
        {
            // Create Json file
            StreamWriter file = new StreamWriter(filename);
            file.Close();

            // Write data
            File.WriteAllText(filename, data);
        }

        /**
         * Json object for output
         */
        private class Calibration
        {
            public string[] fitsToPNG = new string[0];
            public string[] bias = new string[0];
            public string[] dark = new string[0];
            public string[] flat = new string[0];
            public string[] light = new string[0];
            public string outputMasterBias = "";
            public string outputMasterDark = "";
            public string outputMasterFlat = "";
            public string outputCallibrationFile = "";
            public string outputCallibratedFolder = "";

            public Calibration() { }

            public void calibrate(string[] bias, string[] dark, string[] flat, string[] light, string output, string output_master_bias_path, string output_master_dark_path, string output_master_flat_path)
            {
                this.bias = bias;
                this.dark = dark;
                this.flat = flat;
                this.light = light;
                this.outputCallibratedFolder = output;

                this.outputMasterBias = output_master_bias_path;
                this.outputMasterDark = output_master_dark_path;
                this.outputMasterFlat = output_master_flat_path;
            }

            public void fitConvertion(string input_fit_file, string output_file_name)
            {
                string[] tmp = { input_fit_file, output_file_name };
                this.fitsToPNG = tmp;
            }

        }
    }
}
