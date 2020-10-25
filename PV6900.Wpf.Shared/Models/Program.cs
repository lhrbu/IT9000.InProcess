using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Models
{
    public record Program
    {
        public int LoopCount { get; init; } = 1;
        public List<ProgramStepWithSourceMap> ProgramStepsWithSourceMap { get; init; } = null!;
    }
}
