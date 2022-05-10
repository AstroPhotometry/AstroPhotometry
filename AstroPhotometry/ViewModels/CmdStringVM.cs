using System;
using System.ComponentModel;

namespace AstroPhotometry.ViewModels
{
    // This class is for bridging the command output
    public class CmdStringVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string str;
        float progress;

        string last_str;
        float last_progress;

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
                    last_str = str;
                    str = "";
                }
                else
                {
                    last_str = str;
                    str = value;
                }

                NotifyPropertyChanged("Message");
            }
        }

        public float Progress
        {
            get { return progress; }
            set
            {
                last_progress = progress;
                progress = value;
                NotifyPropertyChanged("Progress");
            }
        }
        
        public float LastProgress
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

        public void clear()
        {
            str = "";
            progress = 0;
            Progress = 0;
            Message = "";
        }
    }
}
