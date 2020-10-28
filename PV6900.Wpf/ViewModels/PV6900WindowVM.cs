using IT9000.Wpf.Shared.Models;
using Prism.Commands;
using Prism.Mvvm;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Raccoon.DevKits.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace PV6900.Wpf.ViewModels
{
    public class PV6900WindowVM:BindableBase
    {
        public TimeSpanGaugesVM TimeSpanGaugesVM { get; }
        public ProgramDashboardVM ProgramDashboardVM { get; }
        public MonitorMenuVM MonitorMenuVM { get; }
        public TimeSpanVoltaChartVM TimeSpanVoltaChartVM{get;}
        public TimeSpanAmpereChartVM TimeSpanAmpereChartVM {get;}
        public PV6900WindowVM(
            TimeSpanGaugesVM timeSpanGaugesVM,
            ProgramDashboardVM programDashboardVM,
            MonitorMenuVM monitorMenuVM,
            TimeSpanVoltaChartVM timeSpanVoltaChartVM,
            TimeSpanAmpereChartVM timeSpanAmpereChartVM
            )
        { 
            TimeSpanGaugesVM = timeSpanGaugesVM;
            ProgramDashboardVM = programDashboardVM;
            MonitorMenuVM = monitorMenuVM;
            TimeSpanVoltaChartVM =timeSpanVoltaChartVM;
            TimeSpanAmpereChartVM = timeSpanAmpereChartVM;
        }

        
        
    }
}
