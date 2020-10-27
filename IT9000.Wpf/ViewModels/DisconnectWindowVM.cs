using IT9000.Wpf.Repositories;
using IT9000.Wpf.Shared.Models;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace IT9000.Wpf.ViewModels
{
    public class DisconnectWindowVM
    {
        private readonly DevicesRepository _devicesRepository;
        public DisconnectWindowVM(DevicesRepository devicesRepository)
        { 
            _devicesRepository = devicesRepository;
            SelectionsResetCommand = new(listBox => listBox.SelectedItem = null);
            SelectionsDisconnectCommand = new(SelectionsDisconnect);
        }
        public Visibility OnlineDevicesEmptyFlag =>
            OnlineDevices.Count() != 0 ? Visibility.Visible : Visibility.Hidden;
        public ObservableCollection<Device> OnlineDevices => _devicesRepository.OnlineDevices;
        public DelegateCommand<ListBox> SelectionsResetCommand { get; }
        public DelegateCommand<ListBox> SelectionsDisconnectCommand { get; }

        public void SelectionsDisconnect(ListBox listBox)
        {
            try
            {
                IEnumerable<Device> devices = listBox.SelectedItems.Cast<Device>()!.ToList()!;
                foreach (Device device in devices)
                {
                    TabControl tabControl = (Application.Current.MainWindow as MainWindow)!.TabControl_DevicePanels;
                    IEnumerable<TabItem> tabItems = tabControl.Items.Cast<TabItem>();
                    TabItem? tabItem = tabItems.FirstOrDefault(item => item.Header as string == device.Name);
                    if (tabItem is not null)
                    {
                        tabControl.Items.Remove(tabItem);
                        _devicesRepository.DeviceOffline(device);
                    }
                }
            }catch(Exception e) { MessageBox.Show(e.ToString()); }
            finally { Window.GetWindow(listBox).Close(); }
        }

    }
}
