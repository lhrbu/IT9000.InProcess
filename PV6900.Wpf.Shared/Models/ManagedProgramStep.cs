using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Models
{
    public class ManagedProgramStep:BindableBase
    {
        public double Volta { get=>_volta; set=>SetProperty(ref _volta,value); }
        private double _volta;
        public double Ampere { get=>_ampere; set=>SetProperty(ref _ampere,value); }
        private double _ampere;
        public double Duration { get=>_duration; set=>SetProperty(ref _duration,value); }
        private double _duration;
        public InnerLoopFlag InnerLoopFlag { get=>_innerLoopFlag; set=>SetProperty(ref _innerLoopFlag,value); }
        private InnerLoopFlag _innerLoopFlag;
        public int InnerLoopCount { get=> _innerLoopCount; set=>SetProperty(ref _innerLoopCount,value); }
        private int _innerLoopCount;
    }
}
