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
        public IServiceProvider ServiceProvider=> _scope.ServiceProvider;
        public IConfiguration Configuration => _configuration;

        //private IServiceScope _serviceScope=null!;
        private Device _device = null!;

        private IServiceScope _scope = null!;

        private static ConcurrentDictionary<string, IServiceScope> _deviceScopesMap = new();

        public UIElement CreateDevicePanelUI(Device device)
        {
            _device = device;
            Initialize();
            _devicePanelWindowsRepository = _globalServiceProvider.GetRequiredService<DevicePanelWindowsRepository>();
            
            if(_deviceScopesMap.TryGetValue(device.Name,out IServiceScope? scope))
            {
                if(scope is null) { throw new InvalidOperationException(); }
                return scope.ServiceProvider.GetRequiredService<PV6900Window>();
            }
            else
            {
                IServiceScope deviceScope = _globalServiceProvider.CreateScope();
                deviceScope.ServiceProvider.GetRequiredService<DeviceStorageService>().Set(device);
                PV6900Window devicePanel = deviceScope.ServiceProvider.GetRequiredService<PV6900Window>();
                _devicePanelWindowsRepository.DevicePanelWindowMap.TryAdd(device.Name, devicePanel);
                return devicePanel;
            }
        }
       
       public void Disconnect()
       {
            if(_devicePanelWindowsRepository.DevicePanelWindowMap.ContainsKey(_device.Name))
            {
                var programVM = ServiceProvider.GetRequiredService<ProgramDashboardVM>();
                programVM.StopProgram();
                programVM.ManagedProgramSteps.Clear();
                programVM.OuterLoopCount = 1;
                programVM.CurrentManagedProgramStep = null;
                programVM.InRunning = false;
            }
       }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<PV6900Window>();
            services.AddScoped<TimeSpanCharts>();

            services.AddScoped<PV6900WindowVM>();
            services.AddScoped<TimeSpanVoltaChartVM>();
            services.AddScoped<TimeSpanAmpereChartVM>();
            services.AddScoped<TimeSpanGaugesVM>();
            services.AddScoped<ProgramDashboardVM>();
            services.AddScoped<MonitorMenuVM>();

            services.AddTransient<ManagedProgramParseService>();
           
            services.AddTransient<DeviceLinkService>();
            services.AddTransient<DeviceLimitsQueryService>();
            services.AddTransient<ProgramExecutor>();

            services.AddTransient<DeviceSettingDataQueryService>();
            services.AddTransient<DeviceDataMeasureService>();


            services.AddScoped<DeviceStorageService>();
            services.AddScoped<DeviceMonitorService>();

            services.AddSingleton<DevicePanelWindowsRepository>();

#if DEBUG
            services.AddSingleton<IIteInteropService, MockIteInteropService>();
#else
            services.AddSingleton<IIteInteropService, PV6900IteInteropService>();
#endif
        }


        private static bool _initializedFlag = false;
        private static IServiceProvider _globalServiceProvider=null!;
        private static IConfiguration _configuration=null!;
       
        private void Initialize()
        {
            if (!_initializedFlag)
            {
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
         "MzM2ODE5QDMxMzgyZTMzMmUzMGsydzdtUHFwdmVkKzgwUUQ2Y3pXSzdRcHpIUHRPSzJjUUw0M01qcldYb1U9");
                IServiceCollection services = new ServiceCollection();
                _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings", true, true)
                    .Build();
                services.AddSingleton(_configuration);
                ConfigureServices(services);
                _globalServiceProvider = services.BuildServiceProvider();
                _initializedFlag = true;
            }
        }

        public void StartRunProgram(Device device)
        {
            PV6900Window? devicePanelWindow = _devicePanelWindowsRepository
                .DevicePanelWindowMap.GetValueOrDefault(device.Name);
            if (devicePanelWindow is not null) {
                devicePanelWindow.Button_StartRunProgram.Command.Execute(devicePanelWindow.DataGrid_ProgramEditor);
            }
        }

        public bool CanRunProgram(Device device)
        {
            PV6900Window? devicePanelWindow = _devicePanelWindowsRepository
                .DevicePanelWindowMap.GetValueOrDefault(device.Name);
            if(devicePanelWindow is not null)
            {
                return devicePanelWindow.Button_StartRunProgram.IsEnabled;
            }
            else { return false; }
        }

        public void StopRunProgram(Device device)
        {
            PV6900Window? devicePanelWindow = _devicePanelWindowsRepository
                            .DevicePanelWindowMap.GetValueOrDefault(device.Name);
            if (devicePanelWindow is not null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                devicePanelWindow.Button_StopRunProgram
                    .Command.Execute(devicePanelWindow.DataGrid_ProgramEditor));
            }
        }

        public bool CanStopProgram(Device device)
        {
            PV6900Window? devicePanelWindow = _devicePanelWindowsRepository
               .DevicePanelWindowMap.GetValueOrDefault(device.Name);
            if(devicePanelWindow is not null)
            {
                return devicePanelWindow.Button_StopRunProgram.IsEnabled;
            }
            else { return false; }
        }
    }
}