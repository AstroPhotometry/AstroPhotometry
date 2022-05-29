using Newtonsoft.Json;

namespace AstroPhotometry
{
    public static class JsonSerialization
    {
        // Add data for calibration process
        public static string computeCalibrationJson(string[] bias, string[] dark, string[] flat, string[] light, string output)
        {
            Calibration cal = new Calibration();
            cal.calibrate(bias, dark, flat, light, output);
            string json = JsonConvert.SerializeObject(cal);
            return json;
        }

        // Add data for image creation process
        public static string fitsToPNG(string fit_file)
        {
            Calibration cal = new Calibration();
            cal.fitConvertion(fit_file);
            string json = JsonConvert.SerializeObject(cal);
            return json;
        }

        /**
         * Json object for output
         */
        private class Calibration
        {
            public string fitsToPNG = "";
            public string[] bias;
            public string[] dark;
            public string[] flat;
            public string[] light;
            public string outputMasterBias = "";
            public string outputMasterDark = "";
            public string outputMasterFlat = "";
            public string outputCallibrationFile = "";
            public string outputCallibratedFolder = "";

            public Calibration() { }

            public void calibrate(string[] bias, string[] dark, string[] flat, string[] light, string output)
            {
                this.bias = bias;
                this.dark = dark;
                this.flat = flat;
                this.light = light;
                this.outputCallibratedFolder = output;
            }

            public void fitConvertion(string fit_file)
            {
                this.fitsToPNG = fit_file;
            }

        }
    }
}
