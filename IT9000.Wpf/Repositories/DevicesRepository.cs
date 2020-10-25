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
        public void DeviceOnline(Device device,IDevicePanel devicePanel)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if (!OnlineDevices.Contains(device))
            {
                dispatcher.Invoke(()=>
                    OnlineDevices.Add(device));
            }
            if(OfflineDevices.Contains(device))
            {
                dispatcher.Invoke(()=>
                    OfflineDevices.Remove(device));
            }
            SortOfflineDeivces();
            SortOnlineDevices();
            while(!DevicePanelMap.TryAdd(device, devicePanel));
        }
        public void DeviceOffline(Device device)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            if(!OfflineDevices.Contains(device))
            {
                dispatcher.Invoke(()=>OfflineDevices.Add(device));
            }
            if(OnlineDevices.Contains(device))
            {
                dispatcher.Invoke(()=>OnlineDevices.Remove(device));
            }
            SortOfflineDeivces();
            SortOnlineDevices();
            while(!DevicePanelMap.TryRemove(device,out _));
        }
        public void SortOfflineDeivces()
        {
            Application.Current.Dispatcher.Invoke(()=>{
                List<Device> sortedList = OfflineDevices.OrderBy(item=>item.Index).ToList(); 
                for(int i=0;i<sortedList.Count();++i)
                { OfflineDevices.Move(OfflineDevices.IndexOf(sortedList[i]),i);}
            });
        }
        public void SortOnlineDevices()
        {
            Application.Current.Dispatcher.Invoke(()=>{
                List<Device> sortedList = OnlineDevices.OrderBy(item=>item.Index).ToList(); 
                for(int i=0;i<sortedList.Count();++i)
                { OnlineDevices.Move(OnlineDevices.IndexOf(sortedList[i]),i);}
            });
        }
        
        public ObservableCollection<Device> OnlineDevices { get;} = new();
        public ObservableCollection<Device> OfflineDevices { get;} = new();
        public ConcurrentDictionary<Device, IDevicePanel> DevicePanelMap { get; } = new();
    }
}
