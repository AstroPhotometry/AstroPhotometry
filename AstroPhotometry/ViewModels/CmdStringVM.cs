using System.ComponentModel;

namespace AstroPhotometry.ViewModels
{
    public class CmdStringVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string str;
        int progress;

        string last_str;
        int last_progress;

        public CmdStringVM()
        {
            Output = "";
            progress = 0;
        }

        public string Output
        {
            get { return str; }

            set
            {
                if (null == value)
                {
                    last_str = str;
                    str = "";
                }
                else
                {
                    last_str = str;
                    str = value;
                }

                NotifyPropertyChanged("Output");
            }
        }

        public int Progress
        {
            get { return progress; }
            set
            {
                last_progress = progress;
                progress = value;
                NotifyPropertyChanged("Progress");
            }
        }
        
        public int LastProgress
        {
            get { return last_progress; }
        }

        public string LastStr
        {
            get { return last_str; }
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
