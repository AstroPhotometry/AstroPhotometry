using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AstroPhotometry.ShellClasses;

namespace AstroPhotometry
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PhotoVM photo;
        public MainWindow()
        {
            InitializeComponent();
            InitializeFileSystemObjects();

            photo = new PhotoVM();
            DataContext = photo;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // String filePos = textBox_file.Text.Replace("\\", "/");

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Fits files (*.fits)|*.fit;*.fits|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                //txtEditor.Text = openFileDialog.FileName;
                photo.updateUri(openFileDialog.FileName);
            }
            //txtEditor.Text = File.ReadAllText(openFileDialog.FileName);


            InitializeFileSystemObjects();
        }

        //public Image getPicture()
        //{

        //}
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
            // TODO: make it start from desktop (one layer in)
            PreSelect(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));
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

        private void StackPanel_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            FileSystemInfo file = (FileSystemInfo)((TextBlock)((StackPanel)sender).Children[1]).Tag;
            if ((file is DirectoryInfo))
            {
                return;
            }

            MessageBox.Show(file.FullName);
            //TODO: connect it to image and show image (command to python)


        }
    }

}
