using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using AstroPhotometry.ViewModels;

namespace AstroPhotometry
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize the splash screen and set it as the application main window
            CmdStringVM output = new CmdStringVM();
            var splashScreen = new View.Splash(output);
            this.MainWindow = splashScreen;
            splashScreen.Show();

            // In order to ensure the UI stays responsive, we need to do the work on a different thread
            Task.Factory.StartNew(() =>
            {
                string base_path = Path.GetFullPath("../../../python/");
                createPyVenv(base_path, output); // Wait to be done

                // Since we're not on the UI thread
                // once we're done we need to use the Dispatcher
                // to create and show the main window
                this.Dispatcher.Invoke(() =>
                {
                    // Initialize the main window, set it as the application main window
                    // and close the splash screen
                    var mainWindow = new MainWindow();
                    this.MainWindow = mainWindow;

                    mainWindow.Show();
                    splashScreen.Close();
                });
            });
        }

        /**
         * This function creats venv
         */
        private void createPyVenv(string python_code_folder_full_path, ViewModels.CmdStringVM output)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

            // Path and command
            string command = python_code_folder_full_path + "";
            startInfo.FileName = "\"" + python_code_folder_full_path + "\\installVenve.bat\"";
            startInfo.Arguments = "\"" + command + "\"";

            // for redireting
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            process.StartInfo = startInfo;
            process.Start();

            // For async showing th progress
            async Task Main()
            {
                output.Output = "loading...";
                await ReadCharacters();
            }

            async Task ReadCharacters()
            {
                while (!process.HasExited)
                {

                    string tmp = await process.StandardOutput.ReadLineAsync();
                    // Cleaning the input
                    if (tmp != null)
                    {
                        tmp = tmp.Replace(Path.GetFullPath(".").ToLower(), "");
                        if (tmp.Contains("python -m") || tmp.Contains("python.exe"))
                        {
                            int charPos = tmp.IndexOf("python");
                            tmp = tmp.Substring(charPos);
                        }
                        else if (tmp.Contains("set arg"))
                        {
                            int charPos = tmp.IndexOf("set arg");
                            tmp = tmp.Substring(charPos);
                        }
                    }
                    output.Output = tmp;
                    Console.WriteLine(tmp);
                }
            }
            _ = Main();

            process.WaitForExit();
        }
    }
}
