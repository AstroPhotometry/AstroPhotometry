using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.IO;

using AstroPhotometry.ShellClasses;

namespace AstroPhotometry.View
{
    /// <summary>
    /// Interaction logic for FileTree.xaml
    /// </summary>
    public partial class FileTree : UserControl
    {
        private PythonVM py_runner;

        public FileTree()
        {
            // TODO: find the pyhon folder no metter what (maybe copy the content to bin)
            string base_path = Path.GetFullPath("../../../python/");
            this.py_runner = new PythonVM(base_path, @".\tmp\");

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
                    return;
                }
                py_runner.FitsToPNG(file.FullName, file.Name + @".png");

                //photo.updateUri(file.FullName);
            }
        }
    }
}
