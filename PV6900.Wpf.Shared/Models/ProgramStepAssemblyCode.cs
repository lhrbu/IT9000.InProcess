using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Models
{
    public record ProgramStepAssemblyCode
    {
        public string SetVoltaCmd { get; init; } = null!;
        public string SetAmpereCmd { get; init; } = null!;
        public double Duration { get; init; }
    }
}
