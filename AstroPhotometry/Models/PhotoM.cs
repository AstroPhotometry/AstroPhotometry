using System;

namespace AstroPhotometry.Models
{
    internal class PhotoM
    {
        private Uri uri;
        private string base_name;
        private string mode = "linear";

        public PhotoM()
        {
            uri = new Uri("/Assets/choose_file.png", UriKind.Relative);
        }

        public Uri Path
        {
            get { return uri; }
            set
            {
                base_name = pathToBase(value);

                uri = changeModeFun(value, mode);
                if (uri == null)
                {
                    uri = value;
                }
            }
        }

        /**
         * gets uri and pull the base name of it - /path/to/file_linear.png -> file_
         */
        private string pathToBase(Uri uri)
        {
            string tmp_base = System.IO.Path.GetFileNameWithoutExtension(uri.AbsolutePath);
            tmp_base = Utils.RemoveFromEnd(tmp_base, "linear");
            tmp_base = Utils.RemoveFromEnd(tmp_base, "logarithm");
            tmp_base = Utils.RemoveFromEnd(tmp_base, "exponential");
            return tmp_base;
        }

        /**
         * replace the name of the uri to one with the mode - /path/to/file_linear.png -> /path/to/file_{mode}.png
         */
        public void changeMode(string mode)
        {
            if (base_name == null)
            {
                return;
            }
            this.mode = mode;
            Path = changeModeFun(uri, mode);
        }

        /**
        * replace the name of the uri to one with the mode - /path/to/file_linear.png -> /path/to/file_{mode}.png
        */
        private Uri changeModeFun(Uri uri, string mode)
        {
            if (base_name == null)
            {
                return null;
            }
            string full_path = uri.AbsoluteUri;
            string extension = System.IO.Path.GetExtension(full_path);
            string fileName = System.IO.Path.GetFileNameWithoutExtension(full_path);

            full_path = full_path.Replace(string.Format("{0}{1}", fileName, extension), string.Format("{0}{1}{2}", base_name, mode, extension));
            Uri new_uri = new Uri(full_path, UriKind.Absolute);
            return new_uri;
        }
    }
}
