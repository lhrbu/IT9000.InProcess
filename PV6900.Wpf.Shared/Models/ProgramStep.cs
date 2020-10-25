using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Models
{
    public record ProgramStep
    {
        public double Volta { get; init; } = 1.0;
        public double Ampere { get; init; } = 1.0;
        public double Duration { get; init; } = 1.0;
    }
}
