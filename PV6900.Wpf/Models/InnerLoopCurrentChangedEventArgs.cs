using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class InnerLoopCurrentChangedEventArgs
    {
        public int InnerLoopCurrent { get; }
        public InnerLoopCurrentChangedEventArgs(int innerLoopCurrent)
        { InnerLoopCurrent = innerLoopCurrent; }
    }
}
