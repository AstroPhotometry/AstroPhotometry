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
            Directory.CreateDirectory(@".\tmp\batch" + batch_num);

            string base_path = Path.GetFullPath("../../../python/");
            Directory.CreateDirectory(@".\tmp\batch" + batch_num + @"\masterBias");
            py_runner = new PythonVM(base_path, @".\tmp\batch" + batch_num + @"\masterBias" + @"\", cmdString);
            AvgMean(bias); // TODO: mean avg
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

            // TODO: fix error in math python
            Directory.CreateDirectory(@".\tmp\batch" + batch_num + @"\darkAndBias");
            string master_bias = Path.GetFullPath(@".\tmp\batch" + batch_num + @"\masterBias");
            py_runner = new PythonVM(base_path, @".\tmp\batch" + batch_num + @"\darkAndBias" + @"\", cmdString);
            Minus(dark, master_bias);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

            Directory.CreateDirectory(@".\tmp\batch" + batch_num + @"\masterDark");
            string darkAndBias = Path.GetFullPath(@".\tmp\batch" + batch_num + @"\darkAndBias");
            py_runner = new PythonVM(base_path, @".\tmp\batch" + batch_num + @"\masterDark" + @"\", cmdString);
            AvgMean(dark);
            while (py_runner.Running)
            {
                //Code waits until bool is set to false
                await Task.Delay(100);
            }

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
