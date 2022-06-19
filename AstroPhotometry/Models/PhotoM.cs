using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;


namespace AstroPhotometry.Models
{
    internal class PhotoM
    {
        private Uri uri;
        public PhotoM()
        {
            uri = new Uri("/Assets/choose_file.png", UriKind.Relative);
        }
        public Uri Path
        {
            get { return uri; }
            set { uri = value; }
        }
    }
}
