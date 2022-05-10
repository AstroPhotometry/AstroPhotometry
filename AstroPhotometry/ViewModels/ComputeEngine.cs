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

        public ComputeEngine(string[] bias, string[] dark, string[] flat, string[] light)
        {
            cmdString = new CmdStringVM();

            this.bias = bias;
            this.dark = dark;
            this.flat = flat;
            this.light = light;
            run();
        }

        private async void run()
        {
            batch_num++;
            string base_folder = @".\tmp\batch" + batch_num;
            Directory.CreateDirectory(base_folder);

            string base_path = Path.GetFullPath("../../../python/");

            // integrate (stack) the bias frames to create a master bias.
            Directory.CreateDirectory(base_folder + @"\masterBias");
            string masterBias = Path.GetFullPath(base_folder + @"\masterBias");
            py_runner = new PythonVM(base_path, base_folder + @"\masterBias" + @"\", cmdString);
            AvgMean(bias);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

            // subtract the master bias from the dark frames.
            Directory.CreateDirectory(base_folder + @"\darkAndBias");
            string darkAndBias = Path.GetFullPath(base_folder + @"\darkAndBias");
            py_runner = new PythonVM(base_path, base_folder + @"\darkAndBias" + @"\", cmdString);
            Minus(dark, masterBias);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

            // integrate (stack) the bias subtracted dark frames to create a master dark.
            Directory.CreateDirectory(base_folder + @"\masterDark");
            string masterDark = Path.GetFullPath(base_folder + @"\masterDark");
            py_runner = new PythonVM(base_path, base_folder + @"\masterDark" + @"\", cmdString);
            AvgMeanFolder(darkAndBias);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

            // subtract the master bias from the flat frames.
            Directory.CreateDirectory(base_folder + @"\flatAndMasterBias");
            string flatAndMasterBias = Path.GetFullPath(base_folder + @"\flatAndMasterBias");
            py_runner = new PythonVM(base_path, base_folder + @"\flatAndMasterBias" + @"\", cmdString);
            Minus(flat, masterDark);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

            // integrate(stack) the bias subtracted flat frames to create a master flat.
            Directory.CreateDirectory(base_folder + @"\masterFlat");
            string masterFlat = Path.GetFullPath(base_folder + @"\masterFlat");
            py_runner = new PythonVM(base_path, base_folder + @"\masterFlat" + @"\", cmdString);
            AvgMeanFolder(flatAndMasterBias);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

            // subtract the master bias and the master dark from the light frames
            // 1.
            Directory.CreateDirectory(base_folder + @"\masterBiasPlusMasterDark");
            string masterBiasPlusMasterDark = Path.GetFullPath(base_folder + @"\masterBiasPlusMasterDark");
            py_runner = new PythonVM(base_path, base_folder + @"\masterBiasPlusMasterDark" + @"\", cmdString);
            Addition(masterDark, masterBias);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }
            // 2.
            Directory.CreateDirectory(base_folder + @"\masterBiasPlusMasterDarkMinusLight");
            string masterBiasPlusMasterDarkMinusLight = Path.GetFullPath(base_folder + @"\masterBiasPlusMasterDarkMinusLight");
            py_runner = new PythonVM(base_path, base_folder + @"\masterBiasPlusMasterDarkMinusLight" + @"\", cmdString);
            Minus(light, masterBiasPlusMasterDark);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

            // divide the result by the master flat
            Directory.CreateDirectory(base_folder + @"\calibration");
            string calibration = Path.GetFullPath(base_folder + @"\calibration");
            py_runner = new PythonVM(base_path, base_folder + @"\calibration" + @"\", cmdString);
            Divide(masterBiasPlusMasterDarkMinusLight, masterFlat);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }
        }

        private bool Divide(string divide, string divider)
        {
            string[] tmp = { divide, divider };

            py_runner.MathActions(tmp, "1", "Division");
            return true;
        }

        private bool Addition(string file1, string file2)
        {
            string[] tmp = { file1, file2 };

            py_runner.MathActions(tmp, "1", "Addition");
            return true;
        }

        private bool AvgMeanFolder(string folder)
        {
            py_runner.MathActions(folder, "1", "Avarage");
            return true;
        }

        private bool AvgMean(string[] photos)
        {
            py_runner.MathActions(photos, "1", "Avarage");
            return true;
        }

        private bool Minus(string[] photos, string master_photo)
        {
            int count = 0;
            foreach (var photo in photos)
            {
                string[] tmp = { photo, master_photo };

                py_runner.MathActions(tmp, (count++).ToString(), "Minus");
            }
            return true;

        }
        // func to compute all the nodes
    }
}
