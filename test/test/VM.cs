using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class VM
    {
        public string Title { get; set; }
        public Mycommand ShowCommand { get; set; }

        public VM()
        {
            ShowCommand = new Mycommand();
            Title = "ariel";
        }

    }
}
