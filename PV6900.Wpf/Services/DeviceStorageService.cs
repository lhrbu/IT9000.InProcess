using IT9000.Wpf.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Services
{
    public class DeviceStorageService
    {
        private Device? _device;
        public Device? Get() => _device;
        public void Set(Device device)
        {
            _ = _device is null ?
                _device = device :
                throw new InvalidOperationException("Can't bind device more than once.");
        }
        public bool Empty => _device is null;
        
    }
}
