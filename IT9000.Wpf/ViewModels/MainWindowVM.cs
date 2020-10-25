using IT9000.Wpf.Repositories;
using IT9000.Wpf.Services;
using IT9000.Wpf.Views;
using IT9000.Wpf.Shared.Services;
using IT9000.Wpf.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using Prism.Commands;
using Raccoon.DevKits.Wpf.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace IT9000.Wpf.ViewModels
{
    public class MainWindowVM
    {
        private readonly DeviceDetectService _deviceDetectService;
        private readonly DevicesRepository _devicesRepository;
        public MainWindowVM(
            DeviceDetectService deviceDetectService,
            DevicesRepository devicesRepository
        ){
            _deviceDetectService =deviceDetectService;
            _devicesRepository = devicesRepository;

            foreach(Device device in _deviceDetectService.GetDevices())
            { 
                _devicesRepository.DeviceOffline(device);
            }

            ConnectCommand = new(mainWindow =>
            {
                ConnectWindow connectWindow = Application.Current.AsDIApplication()!
                        .ServiceProvider.GetRequiredService<ConnectWindow>();
                connectWindow.Owner = mainWindow;
                connectWindow.Show();
                
            });

            MultiDevicesCommand = new(mainWindow =>
            {
                MultiDevicesPanelWindow multiDevicesPanelWindow = Application.Current.AsDIApplication()!
                    .ServiceProvider.GetRequiredService<MultiDevicesPanelWindow>();
                multiDevicesPanelWindow.ShowDialog();
            });
        }

        public ObservableCollection<Device> OnlineDevices => _devicesRepository.OnlineDevices;
        public ObservableCollection<Device> OfflineDevices => _devicesRepository.OfflineDevices;

        public DelegateCommand<Window> ConnectCommand { get;}
        public DelegateCommand<Window> MultiDevicesCommand { get; }
    }
}