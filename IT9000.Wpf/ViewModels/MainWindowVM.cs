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
            { _devicesRepository.DeviceOffline(device);}

            ShowConnectWindowCommand = new(ShowConnectWindow);
            ShowDisconnectWindowCommand = new(ShowDisconnectWindow);
            ShowMultiDevicesPanelWindowCommand = new(ShowMultiDevicesPanelWindow);
        }

        public ObservableCollection<Device> OnlineDevices => _devicesRepository.OnlineDevices;
        public ObservableCollection<Device> OfflineDevices => _devicesRepository.OfflineDevices;
        public DelegateCommand ShowConnectWindowCommand { get; }
        public DelegateCommand ShowDisconnectWindowCommand { get; }
        public DelegateCommand ShowMultiDevicesPanelWindowCommand { get; }


        public void ShowConnectWindow() => Application.Current.AsDIApplication()!
            .GetWindow<ConnectWindow>().ShowDialog();
        public void ShowDisconnectWindow() => Application.Current.AsDIApplication()!
            .GetWindow<DisconnectWindow>().ShowDialog();
        public void ShowMultiDevicesPanelWindow() => Application.Current.AsDIApplication()!
            .GetWindow<MultiDevicesPanelWindow>().ShowDialog();
    }
}