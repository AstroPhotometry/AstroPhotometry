﻿using AstroPhotometry.Models;
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
                if (Path != null && Path.IsAbsoluteUri && Path.IsFile) // TODO: find a way to get rid of IsAbsoluteUri
                {
                    return Utils.RemoveFromSubstring(System.IO.Path.GetFileName(Path.LocalPath), ".fit");
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

        /**
         * replace the name of the uri to one with the mode - /path/to/file_linear.png -> /path/to/file_{mode}.png
         */
        public void changeMode(string mode)
        {
            photoM.changeMode(mode);
            NotifyPropertyChanged("Path");
            NotifyPropertyChanged("Name");
        }
    }
}
