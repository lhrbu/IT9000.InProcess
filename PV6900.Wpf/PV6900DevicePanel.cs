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

namespace PV6900.Wpf
{
    public class PV6900DevicePanel:IDevicePanel
    {
        private DevicePanelWindowsRepository _devicePanelWindowsRepository=null!;
        public IServiceProvider ServiceProvider=>_serviceProvider;
        public IConfiguration Configuration => _configuration;
        public Window LaunchDevicePanelWindow(Device device)
        {
            Initialize();
            _devicePanelWindowsRepository = ServiceProvider.GetRequiredService<DevicePanelWindowsRepository>();
            using IServiceScope scope = ServiceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<DeviceStorageService>().Set(device);
            PV6900Window window = scope.ServiceProvider.GetRequiredService<PV6900Window>();

            while(!_devicePanelWindowsRepository.DevicePanelWindowMap.TryAdd(device, window));
            window.Closed += (sender, e) =>
            {
                  _onClosedAction?.Invoke();
                while(!_devicePanelWindowsRepository.DevicePanelWindowMap.TryRemove(device, out _));
            };
            return window;
        }
        public void Onclosed(Action callback)=>_onClosedAction = callback;
        private Action? _onClosedAction;


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<PV6900Window>();
            //services.AddScoped<TimeSpanChartsWindow>();

            services.AddScoped<PV6900WindowVM>();
            services.AddScoped<TimeSpanVoltaChartVM>();
            services.AddScoped<TimeSpanAmpereChartVM>();
            services.AddScoped<TimeSpanVoltaChartVM>();
            services.AddScoped<TimeSpanAmpereChartVM>();
            services.AddScoped<TimeSpanGaugesVM>();
            services.AddScoped<ProgramDashboardVM>();

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
        private static IServiceProvider _serviceProvider=null!;
        private static IConfiguration _configuration=null!;
        private static bool _initializedFlag = false;
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
                _serviceProvider = services.BuildServiceProvider();
                _initializedFlag = true;
            }
        }

        public void StartRunProgram(Device device)
        {
            PV6900Window? devicePanelWindow = _devicePanelWindowsRepository
                .DevicePanelWindowMap.GetValueOrDefault(device);
            if (devicePanelWindow is not null) {
                devicePanelWindow.Button_StartRunProgram
                    .Command.Execute(devicePanelWindow.DataGrid_ProgramEditor);
            }
        }

        public bool CanRunProgram(Device device)
        {
            PV6900Window? devicePanelWindow = _devicePanelWindowsRepository
                .DevicePanelWindowMap.GetValueOrDefault(device);
            if(devicePanelWindow is not null)
            {
                return devicePanelWindow.Button_StartRunProgram.IsEnabled;
            }
            else { return false; }
        }

        public void StopRunProgram(Device device)
        {
            PV6900Window? devicePanelWindow = _devicePanelWindowsRepository
                            .DevicePanelWindowMap.GetValueOrDefault(device);
            if (devicePanelWindow is not null)
            {
                devicePanelWindow.Button_StopRunProgram
                    .Command.Execute(devicePanelWindow.DataGrid_ProgramEditor);
            }
        }
    }
}