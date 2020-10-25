using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT9000.Wpf.Shared.Models
{
    public class CommandReceivedEventArgs:EventArgs
    {
        public CommandReceivedEventArgs(string command)
        { Command = command; }
        public string Command { get; }
    }
}
