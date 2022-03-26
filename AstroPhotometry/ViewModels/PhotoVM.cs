﻿using AstroPhotometry.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Uri Path
        {
            get
            {
                return photoM.Path;
            }
        }
        public void updateUri(String uri)
        {
            photoM.Path = new Uri(uri);
            NotifyPropertyChanged("Path");

        }
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }
}