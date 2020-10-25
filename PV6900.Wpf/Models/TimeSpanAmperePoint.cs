using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class TimeSpanAmperePoint:BindableBase
    {
        public long TimeSpan { get => _timeSpan; set => SetProperty(ref _timeSpan, value); }
        private long _timeSpan;
        public double Ampere { get => _ampere; set => SetProperty(ref _ampere, value); }
        private double _ampere;
    }
}
