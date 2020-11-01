using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class RunningTimeMeasuredEventArgs:EventArgs
    {
        public RunningTimeMeasuredEventArgs(TimeSpan runningTimeSpan)
        { RunningTimeSpan = runningTimeSpan; }
        public TimeSpan RunningTimeSpan { get; }
    }
}
