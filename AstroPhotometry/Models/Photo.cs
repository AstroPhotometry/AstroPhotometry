﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AstroPhotometry.Models
{
    internal class Photo
    {
        private Uri uri;
        private BitmapImage image;
        public Photo()
        {
            uri = new Uri("https://www.extremetech.com/wp-content/uploads/2019/12/SONATA-hero-option1-764A5360-edit-640x354.jpg");
            image = new BitmapImage(uri);
        }
        public Uri Path { 
            get { return uri; } 
            set { uri = value; } 
        }
        public BitmapImage ImageUri
        {
            get { return image; }
        }
    }
}
