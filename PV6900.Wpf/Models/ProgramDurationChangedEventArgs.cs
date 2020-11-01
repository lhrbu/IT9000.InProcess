using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class ProgramDurationChangedEventArgs
    {
        public TimeSpan ProgramDuration { get; }
        public ProgramDurationChangedEventArgs(TimeSpan programDuration)
        { ProgramDuration = programDuration; }
    }
}
