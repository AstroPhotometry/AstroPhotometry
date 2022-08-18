using System.ComponentModel;

namespace AstroPhotometry.ViewModels
{
    // This class is for bridging the command output from the outside to the WPF
    public class CmdStringVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string str;
        float progress;

        string last_error;

        public CmdStringVM()
        {
            Message = "";
            progress = 0;
        }

        public string Message
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
                if (progress == -1)
                {
                    last_error = str;
                }

                NotifyPropertyChanged("Message");
            }
        }

        public float Progress
        {
            get { return progress; }
            set
            {
                progress = value;
                if (progress == -1)
                {
                    last_error = str;
                }
                NotifyPropertyChanged("Progress");
            }
        }

        public string LastError
        {
            get { return last_error; }
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        public void clear()
        {
            str = "";
            progress = 0;
            Progress = 0;
            Message = "";
        }
    }
}
