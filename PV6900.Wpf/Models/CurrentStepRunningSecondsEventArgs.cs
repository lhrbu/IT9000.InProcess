using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class CurrentStepRunningSecondsEventArgs:EventArgs
    {
        public double CurrentStepRunningSeconds { get; }
        public CurrentStepRunningSecondsEventArgs(double currentStepRunningSeconds)
        { CurrentStepRunningSeconds = currentStepRunningSeconds; }
    }
}
