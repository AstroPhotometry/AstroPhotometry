using System.ComponentModel;

namespace AstroPhotometry.ViewModels
{
    public class CmdStringVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string str;

        public CmdStringVM()
        {
            Output = "";
        }

        public string Output
        {
            get { return str; }

            set
            {
                if (null == value)
                {
                    str = "";
                }
                else
                {
                    str = value;
                }

                NotifyPropertyChanged("Output");
            }
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
