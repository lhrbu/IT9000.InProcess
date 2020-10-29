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
                IEnumerable<Device> devices = listBox.SelectedItems.Cast<Device>()!.ToList()!;
                TabItem? lastTabItem=null;
                TabControl tabControl = (Application.Current.MainWindow as MainWindow)!.TabControl_DevicePanels;
                foreach (Device device in devices)
                {
                    _pluginLoadService.Load(device);
                    IDevicePanel devicePanel = _devicePanelInstanceService.CreateDevicePanelInstance(device);

                    TabItem tabItem = new TabItem()
                    {
                        Header = device.Name,
                        Content = devicePanel.CreateDevicePanelUI(device),
                        VerticalContentAlignment = VerticalAlignment.Top
                    };

                    
                    tabControl.Items.Add(tabItem);
                    //tabControl.SelectedItem = tabItem;
                    lastTabItem = tabItem;
                    _devicesRepository.DeviceOnline(device, devicePanel);
                }
                if(lastTabItem!=null){
                    tabControl.SelectedItem=lastTabItem;
                }
            }catch(Exception err)
            {
                MessageBox.Show(err.ToString(),"Can't connect device error");
            }
            finally
            {
                Window.GetWindow(listBox).Close();
            }
        }
    }
}
