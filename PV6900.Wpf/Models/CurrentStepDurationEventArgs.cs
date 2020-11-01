using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class CurrentStepDurationEventArgs
    {
        public double CurrentStepDuration { get; }
        public CurrentStepDurationEventArgs(double currentStepDuration)
        { CurrentStepDuration = currentStepDuration; }
    }
}
