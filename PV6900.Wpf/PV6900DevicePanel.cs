using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using IT9000.Wpf.Shared.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IT9000.Wpf.Shared.Services;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Models;
using PV6900.Wpf.Shared.Services;
using PV6900.Wpf.ViewModels;
using PV6900.Wpf.Repositories;
using PV6900.Wpf.Controls;
using System.Windows.Controls;
using System.Collections.Concurrent;

namespace PV6900.Wpf
{
    public class PV6900DevicePanel:IDevicePanel
    {
        private DevicePanelWindowsRepository _devicePanelWindowsRepository=null!;
        public IServiceScope Scope {get;set;} = null!;
        public Device Device {get;}
        private readonly ProgramDashboardVM _programDashboardVM;
        public PV6900DevicePanel(
            DeviceStorageService deviceStorageService,
            ProgramDashboardVM programDashboardVM
        )
        { 
            Device = deviceStorageService.Get()!;
            _programDashboardVM = programDashboardVM;
        }

        public UIElement CreateDevicePanelUI()
        {
            PV6900Window devicePanelWindow = Scope.ServiceProvider.GetRequiredService<PV6900Window>();
            return devicePanelWindow;
        }
       
       public void Disconnect() => Scope.Dispose();        
       
        public void StartRunProgram()
        {
            PV6900Window? devicePanelWindow = Scope.ServiceProvider.GetRequiredService<PV6900Window>();
            if (devicePanelWindow is not null) {
                devicePanelWindow.Button_StartRunProgram.Command.Execute(devicePanelWindow.DataGrid_ProgramEditor);
            }
        }
        public bool CanRunProgram()
        {
            PV6900Window? devicePanelWindow = Scope.ServiceProvider.GetRequiredService<PV6900Window>();
            if(devicePanelWindow is not null)
            { return devicePanelWindow.Button_StartRunProgram.IsEnabled;}
            else { return false; }
        }

        public void StopRunProgram()
        {
            PV6900Window? devicePanelWindow = Scope.ServiceProvider.GetRequiredService<PV6900Window>();
            if (devicePanelWindow is not null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                devicePanelWindow.Button_StopRunProgram
                    .Command.Execute(devicePanelWindow.DataGrid_ProgramEditor));
            }
        }

        public bool CanStopProgram()
        {
            PV6900Window? devicePanelWindow = Scope.ServiceProvider.GetRequiredService<PV6900Window>();
            if(devicePanelWindow is not null)
            {
                return devicePanelWindow.Button_StopRunProgram.IsEnabled;
            }
            else { return false; }
        }
    }
}