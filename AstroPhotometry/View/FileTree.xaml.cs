using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

using AstroPhotometry.ShellClasses;
using AstroPhotometry.ViewModels;

namespace AstroPhotometry.View
{
    /// <summary>
    /// Interaction logic for FileTree.xaml
    /// This is showing the file tree
    /// </summary>
    public partial class FileTree : UserControl
    {
        private PythonVM py_runner;
        private CmdStringVM cmd_string;
        private static int running_number = 0;
        public FileTree()
        {
            // TODO: find the pyhon folder no metter what (maybe copy the content to bin)
            string base_path = Path.GetFullPath("../../../python/");
            cmd_string = new CmdStringVM(); // TODO: get that from outside for showing progress bar
            Directory.CreateDirectory(@".\tmp\pics\");
            this.py_runner = new PythonVM(base_path, @".\tmp\pics\", cmd_string);
            this.DataContext = cmd_string;

            InitializeComponent();
            InitializeFileSystemObjects();
        }

        #region Events

        private void FileSystemObject_AfterExplore(object sender, System.EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void FileSystemObject_BeforeExplore(object sender, System.EventArgs e)
        {
            Cursor = Cursors.Wait;
        }

        #endregion

        #region Methods

        /**
        * Initialize and add all the drives to the treeview object 
        */
        private void InitializeFileSystemObjects()
        {
            var desktop_path = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
            var desktop = new FileSystemObjectInfo(desktop_path);
            //desktop.IsExpanded = true;
            treeView.Items.Add(desktop);

            var drives = DriveInfo.GetDrives();
            DriveInfo
                .GetDrives()
                .ToList()
                .ForEach(drive =>
                {
                    var fileSystemObject = new FileSystemObjectInfo(drive);
                    fileSystemObject.BeforeExplore += FileSystemObject_BeforeExplore;
                    fileSystemObject.AfterExplore += FileSystemObject_AfterExplore;
                    treeView.Items.Add(fileSystemObject);
                });

            // start in specific folder 
            //PreSelect(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
        }

        /**
        * Pre select the folder that passed into it - 
        *   Open all the path to the folder 
        */
        private void PreSelect(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            var driveFileSystemObjectInfo = GetDriveFileSystemObjectInfo(path);
            driveFileSystemObjectInfo.IsExpanded = true;
            PreSelect(driveFileSystemObjectInfo, path);
        }

        private void PreSelect(FileSystemObjectInfo fileSystemObjectInfo, string path)
        {
            foreach (var childFileSystemObjectInfo in fileSystemObjectInfo.Children)
            {
                var isParentPath = IsParentPath(path, childFileSystemObjectInfo.FileSystemInfo.FullName);
                if (isParentPath)
                {
                    if (string.Equals(childFileSystemObjectInfo.FileSystemInfo.FullName, path))
                    {
                        /* We found the item for pre-selection */
                        childFileSystemObjectInfo.IsExpanded = true;

                    }
                    else
                    {
                        childFileSystemObjectInfo.IsExpanded = true;
                        PreSelect(childFileSystemObjectInfo, path);
                    }
                }
            }
        }

        #endregion

        #region Helpers

        private FileSystemObjectInfo GetDriveFileSystemObjectInfo(string path)
        {
            var directory = new DirectoryInfo(path);
            var drive = DriveInfo
                .GetDrives()
                .Where(d => d.RootDirectory.FullName == directory.Root.FullName)
                .FirstOrDefault();
            return GetDriveFileSystemObjectInfo(drive);
        }

        private FileSystemObjectInfo GetDriveFileSystemObjectInfo(DriveInfo drive)
        {
            foreach (var fso in treeView.Items.OfType<FileSystemObjectInfo>())
            {
                if (fso.FileSystemInfo.FullName == drive.RootDirectory.FullName)
                {
                    return fso;
                }
            }
            return null;
        }

        private bool IsParentPath(string path,
            string targetPath)
        {
            return path.StartsWith(targetPath);
        }

        #endregion

        // TODO: add class that construct with path object and use this function as command 
        private void StackPanel_MouseHandler(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) // double click
            {
                FileSystemInfo file = (FileSystemInfo)((TextBlock)((StackPanel)sender).Children[1]).Tag;
                if ((file is DirectoryInfo))
                {
                    // This is a part to make fit file with math action and convert it to png
                    /*
                    py_runner.MathActions(file.FullName, "uriya2.fit", "Addition");
                    py_runner.FitsToPNG(".\\tmp\\uriya2.fit", @"uriya2.png");*/
                    return;
                }
                string png_file = file.Name.Substring(0, file.Name.IndexOf('.'));
                cmd_string.clear();
                cmd_string.Message = "converting to image";
                cmd_string.Progress = 1;

                // make the json
                string out_path_png = Path.GetFullPath(@".\tmp\pics\");
                string json = JsonSerialization.fitsToPNG(file.FullName, out_path_png);

                string json_path = @".\tmp\pics\fitToPic.json";
                JsonSerialization.writeToFile(json_path, json);

                py_runner.runByJsonPath(json_path);
            }
        }
    }
}
