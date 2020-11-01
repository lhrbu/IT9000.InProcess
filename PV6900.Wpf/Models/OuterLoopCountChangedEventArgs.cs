using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class OuterLoopCountChangedEventArgs:EventArgs
    {
        public int OuterLoopCount { get; }
        public OuterLoopCountChangedEventArgs(int outerLoopCount)
        { OuterLoopCount = outerLoopCount; }
    }
}
