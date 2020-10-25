using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Models
{
    public class TimeSpanVoltaPoint : BindableBase
    {
        public long TimeSpan { get => _timeSpan; set => SetProperty(ref _timeSpan, value); }
        private long _timeSpan;
        public double Volta { get => _volta; set => SetProperty(ref _volta, value); }
        private double _volta;
    }
}
