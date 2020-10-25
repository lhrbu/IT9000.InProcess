using IT9000.Wpf.Repositories;
using IT9000.Wpf.Services;
using IT9000.Wpf.ViewModels;
using IT9000.Wpf.Views;
using IT9000.Wpf.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Raccoon.DevKits.Wpf.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Runtime.Loader;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace IT9000.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application,IDIApplication
    {
        public IServiceProvider ServiceProvider { get; set; } = null!;
        public IConfiguration Configuration { get; set; } = null!;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
            services.AddTransient<ConnectWindow>();
            //services.AddSingleton<MultiDevicesPanelWindow>();

            services.AddTransient<MainWindowVM>();
            services.AddTransient<ConnectWindowVM>();
            //services.AddSingleton<MultiDevicesPanelWindowVM>();

            services.AddSingleton<DevicesRepository>();
            services.AddSingleton<PluginTypesRepository>();

            services.AddTransient<PluginLoadService>();
            services.AddTransient<DeviceDetectService>();
            services.AddTransient<CommandSendService>();
            services.AddTransient<DevicePanelInstanceService>();

#if DEBUG
            services.AddSingleton<IIteInteropService, MockIteInteropService>();
#else
            services.AddSingleton<IIteInteropService, IT9000IteInteropService>();
#endif
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                base.OnStartup(e);
                this.OnStartupProxy<MainWindow>();
                MultiDevicesPanelWindow multiDevicesPanelWindow = ServiceProvider
                    .GetRequiredService<MultiDevicesPanelWindow>();
                multiDevicesPanelWindow.Closing += (sender, e) =>
                {
                    multiDevicesPanelWindow.Visibility = Visibility.Hidden;
                    e.Cancel = true;
                };
                Application.Current.MainWindow.Closed += (sender,e)=>Application.Current.Shutdown();
            }catch(Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Error:");
            }
        }
    }
}
