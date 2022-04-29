using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroPhotometry.ViewModels
{
    public class CmdString : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string str;
        public string Output
        {
            get { return str; }
            set
            { 
                str = value;
                NotifyPropertyChanged("Output");
            }
        }

        public CmdString()
        {
            Output = "";
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
