using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Models
{
    public class DataMeasureEventArgs : EventArgs
    {
        public double Volta { get; init; }
        public double Ampere { get; init; }
    }
}
