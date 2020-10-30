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
    public class PV6900DevicePanelFactory:IDevicePanelFactory
    {
        public IDevicePanel CreateDevicePanel(Device device)
        { 
            Initialize();
            IServiceScope scope = GlobalServiceProvider.CreateScope();
            scope.ServiceProvider.GetRequiredService<DeviceStorageService>().Set(device);
            PV6900DevicePanel devicePanel = scope.ServiceProvider.GetRequiredService<PV6900DevicePanel>();
            devicePanel.Scope = scope;
            return devicePanel;
        }

        public IServiceProvider GlobalServiceProvider {get;private set;}=null!;
        private static bool _initializeFlag = false;
        private void Initialize()
        {
            if(!_initializeFlag)
            {
                Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                "MzM2ODE5QDMxMzgyZTMzMmUzMGsydzdtUHFwdmVkKzgwUUQ2Y3pXSzdRcHpIUHRPSzJjUUw0M01qcldYb1U9");

                IServiceCollection services = new ServiceCollection();
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings", true, true)
                    .Build();
                services.AddSingleton(configuration);

                ConfigureServices(services);
                GlobalServiceProvider = services.BuildServiceProvider();
                _initializeFlag = true;
            }
        }

         public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<PV6900DevicePanel>();

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

    }
}