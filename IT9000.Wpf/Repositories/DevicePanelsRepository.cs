using IT9000.Wpf.Shared.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT9000.Wpf.Repositories
{
    public class DevicePanelsRepository
    {
        private readonly ConcurrentDictionary<Device, IDevicePanel> _deviceDevicePanelMap = new();

        public bool TryRegisterDevicePanel(Device device, IDevicePanel devicePanel) =>
            _deviceDevicePanelMap.TryAdd(device, devicePanel);
        public bool TryDisposeDevicePanel(Device device) =>
            _deviceDevicePanelMap.TryRemove(device, out _);

        public IDevicePanel? FindDevicePanel(Device device)
        {
            if(_deviceDevicePanelMap.TryGetValue(device, out IDevicePanel? devicePanel))
            {
                return devicePanel ?? null;
            }
            else { return null; }
        }
    }
}
