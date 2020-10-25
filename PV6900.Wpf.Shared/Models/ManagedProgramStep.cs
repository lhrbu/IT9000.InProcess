﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Models
{
    public class ManagedProgramStep:BindableBase
    {
        public double Volta { get; set; }
        public double Ampere { get; set; }
        public double Duration { get; set; }
        public InnerLoopFlag InnerLoopFlag { get=>_innerLoopFlag; set=>SetProperty(ref _innerLoopFlag,value); }
        private InnerLoopFlag _innerLoopFlag = InnerLoopFlag.None;
        public int InnerLoopCount { get; set; }
    }
}
