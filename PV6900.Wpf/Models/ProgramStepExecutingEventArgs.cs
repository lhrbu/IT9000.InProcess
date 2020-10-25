using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV6900.Wpf.Shared.Models;

namespace PV6900.Wpf.Models
{
    public class ProgramStepExecutingEventArgs: EventArgs
    {
        public ProgramStepExecutingEventArgs(ManagedProgramStep managedProgramStep)
        {CurrentManagedProgramStep=managedProgramStep;}
        public ManagedProgramStep CurrentManagedProgramStep{get;}
    }
}