using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class OuterLoopCurrentChangedEventArgs : EventArgs
    {
        public int OuterLoopCurrent { get; }
        public OuterLoopCurrentChangedEventArgs(int outerLoopCurrent)
        { OuterLoopCurrent = outerLoopCurrent; }
    }
}
