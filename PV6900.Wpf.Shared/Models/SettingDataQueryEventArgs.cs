using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Models
{
    public class SettingDataQueryEventArgs : EventArgs
    {
        public double SettingVolta { get; init; }
        public double SettingAmpere { get; init; }
    }
}
