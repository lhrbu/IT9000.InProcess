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

namespace PV6900.Wpf
{
    public class PV6900DevicePanel:IDevicePanel
    {
        private DevicePanelWindowsRepository _devicePanelWindowsRepository=null!;
        public IServiceProvider ServiceProvider=>_serviceProvider;
        public IConfiguration Configuration => _configuration;
        public Device Device { get; private set; } = null!;

        private IServiceScope _serviceScope=null!;

        public UIElement CreateDevicePanelUI(Device device)
        {
            Initialize();
            Device = device;

            _devicePanelWindowsRepository = ServiceProvider.GetRequiredService<DevicePanelWindowsRepository>();
            
            TimeSpanCharts proxyInstance = ServiceProvider.GetRequiredService<TimeSpanCharts>();

            _serviceScope = ServiceProvider.CreateScope();
            _serviceScope.ServiceProvider.GetRequiredService<DeviceStorageService>().Set(device);
            PV6900Window window = _serviceScope.ServiceProvider.GetRequiredService<PV6900Window>();

            _devicePanelWindowsRepository.DevicePanelWindowMap.TryAdd(device, window);
            return window;
        }
       
        public void Onclosed(Action callback)=>_onClosedAction = callback;
        private Action? _onClosedAction;


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<PV6900Window>();
            services.AddTransient<TimeSpanCharts>();

            services.AddTransient<PV6900WindowVM>();
            services.AddScoped<TimeSpanVoltaChartVM>();
            services.AddScoped<TimeSpanAmpereChartVM>();
            services.AddScoped<TimeSpanGaugesVM>();
            services.AddTransient<ProgramDashboardVM>();
            services.AddTransient<MonitorMenuVM>();

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
        private static IServiceProvider _serviceProvider=null!;
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