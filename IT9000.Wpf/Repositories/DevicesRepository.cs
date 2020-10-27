using IT9000.Wpf.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace IT9000.Wpf.Repositories
{
    public class DevicesRepository
    {
        private readonly DevicePanelsRepository _devicePanelsRepository;
        public DevicesRepository(DevicePanelsRepository devicePanelsRepository)
        { _devicePanelsRepository = devicePanelsRepository; }
        private Dispatcher _Dispatcher => Application.Current.Dispatcher;
        public void DeviceOnline(Device device,IDevicePanel devicePanel)
        {
            if(OfflineDevices.Contains(device))
            { _Dispatcher.Invoke(()=>OfflineDevices.Remove(device));}

            if (!OnlineDevices.Contains(device))
            { _Dispatcher.Invoke(() => OnlineDevices.Add(device)); }

            SortDevices(OfflineDevices);
            SortDevices(OnlineDevices);

            _devicePanelsRepository.TryRegisterDevicePanel(device, devicePanel);
        }
        public void DeviceOffline(Device device)
        {
            
            if(OnlineDevices.Contains(device))
            { _Dispatcher.Invoke(()=>OnlineDevices.Remove(device));}
            if (!OfflineDevices.Contains(device))
            { _Dispatcher.Invoke(() => OfflineDevices.Add(device)); }

            SortDevices(OfflineDevices);
            SortDevices(OnlineDevices);
            _devicePanelsRepository.TryDisposeDevicePanel(device);
        }
        
        public ObservableCollection<Device> OnlineDevices { get;} = new();
        public ObservableCollection<Device> OfflineDevices { get;} = new();
        private void SortDevices(ObservableCollection<Device> devices)
        {
            List<Device> sortedDevices = devices.OrderBy(item => item.Index).ToList();
            Application.Current.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < sortedDevices.Count(); ++i)
                { devices.Move(devices.IndexOf(sortedDevices[i]), i); }
            });
        }
    }
}
