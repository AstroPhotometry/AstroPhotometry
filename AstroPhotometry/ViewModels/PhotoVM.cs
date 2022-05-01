using AstroPhotometry.Models;
using AstroPhotometry.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AstroPhotometry
{
    class PhotoVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private PhotoM photoM;

        public PhotoVM()
        {
            photoM = new PhotoM();
        }

        public PhotoVM(FileWatcherVM fileWatcher)
        {
            fileWatcher.Ioc(this.updateUri);
            photoM = new PhotoM();
        }

        public Uri Path
        {
            get
            {
                return photoM.Path;
            }
        }

        public string Name
        {
            get
            {
                if (Path.IsFile)
                {
                    return System.IO.Path.GetFileName(Path.LocalPath);
                }
                return "";
            }
        }

        public void updateUri(String uri)
        {
            photoM.Path = new Uri(uri);
            NotifyPropertyChanged("Path");
            NotifyPropertyChanged("Name");
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }
    }
}
