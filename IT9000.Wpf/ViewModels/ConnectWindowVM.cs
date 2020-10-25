using IT9000.Wpf.Repositories;
using IT9000.Wpf.Services;
using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Prism.Mvvm;
using IT9000.Wpf.Views;
using Raccoon.DevKits.Wpf.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace IT9000.Wpf.ViewModels
{
    public class ConnectWindowVM:BindableBase
    {
        private readonly DevicesRepository _devicesRepository;
        private readonly DevicePanelInstanceService _devicePanelInstanceService;
        private readonly PluginLoadService _pluginLoadService;
        public ConnectWindowVM(
            DevicesRepository devicesRepository,
            DevicePanelInstanceService devicePanelInstanceService,
            PluginLoadService pluginLoadService)
        {
            _devicesRepository = devicesRepository;
            _devicePanelInstanceService = devicePanelInstanceService;
            _pluginLoadService = pluginLoadService;

            SelectionsResetCommand = new(listBox => listBox.SelectedItem = null);
            SelectionsConnectCommand = new(SelectionsConnect);
        }

        public ObservableCollection<Device> OfflineDevices=>_devicesRepository.OfflineDevices;

        public Visibility OfflineDevicesEmptyFlag =>
            OfflineDevices.Count() != 0 ? Visibility.Visible : Visibility.Hidden;
        public DelegateCommand<ListBox> SelectionsResetCommand { get; }
        public DelegateCommand<ListBox> SelectionsConnectCommand { get; }

        public void SelectionsConnect(ListBox listBox)
        {
            try
            {
                MultiDevicesPanelWindow _multiDevicesPanelWindow = Application.Current.AsDIApplication()!
                    .ServiceProvider.GetRequiredService<MultiDevicesPanelWindow>();
                IEnumerable<Device> devices = listBox.SelectedItems.Cast<Device>()!.ToList()!;
                foreach (Device device in devices)
                {
                    _pluginLoadService.Load(device);
                    IDevicePanel devicePanel = _devicePanelInstanceService.CreateInstance(device);
                    _devicesRepository.DeviceOnline(device,devicePanel);

                    devicePanel.Onclosed(()=>{
                        _devicesRepository.DeviceOffline(device);
                        if(_devicesRepository.OnlineDevices.Count()==0)
                        {
                            _multiDevicesPanelWindow.Close();
                            Application.Current.MainWindow.Focus();
                        }
                    });

                    Window devicePanelWindow = devicePanel.LaunchDevicePanelWindow(device);
                    
                    devicePanelWindow.Owner= Application.Current.MainWindow;

                    WindowLocationDistributeService distributeService = new(4,2);
                    WindowLocation windowLocation = distributeService.GetWindowLocation(int.Parse(device.Index));
                    devicePanelWindow.Left = windowLocation.Left;
                    devicePanelWindow.Top = windowLocation.Top;
                    devicePanelWindow.Show();
                    Window.GetWindow(listBox).Close();

                    _multiDevicesPanelWindow.Owner = Application.Current.MainWindow;
                    _multiDevicesPanelWindow.Topmost = true;
                    _multiDevicesPanelWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    _multiDevicesPanelWindow.Show();

                }
            }catch(Exception err)
            {
                MessageBox.Show(err.ToString(),"Can't connect device error");
            }
        }
    }
}
