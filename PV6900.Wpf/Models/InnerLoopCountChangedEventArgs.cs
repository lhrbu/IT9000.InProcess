using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class InnerLoopCountChangedEventArgs
    {
        public int InnerLoopCount { get; }
        public InnerLoopCountChangedEventArgs(int innerLoopCount)
        { InnerLoopCount = innerLoopCount; }
    }
}
