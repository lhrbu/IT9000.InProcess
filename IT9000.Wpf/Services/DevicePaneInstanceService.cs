using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using IT9000.Wpf.Repositories;
using System.Reflection;
using System.IO;
using System.Windows;
using System.Collections.Concurrent;

namespace IT9000.Wpf.Services
{
    public class DevicePanelInstanceService
    {
        private readonly PluginFactoriessRepository _pluginFactoriesRepository;
        public DevicePanelInstanceService(PluginFactoriessRepository pluginFactoriesRepository)
        { _pluginFactoriesRepository = pluginFactoriesRepository;}

        public IDevicePanel CreateDevicePanelInstance(Device device)
        {
            ConcurrentDictionary<string,IDevicePanelFactory> modelFactoriesMap = _pluginFactoriesRepository.ModelFactoriesMap;

            if(modelFactoriesMap.ContainsKey(device.Model) && modelFactoriesMap.TryGetValue(device.Model,out IDevicePanelFactory? devicePanelFactory))
            {
                return devicePanelFactory!.CreateDevicePanel(device);
             }else{
                throw new FileNotFoundException($"Can't found plugin dll of {device.Model}");
            }
            
        }
    }
}