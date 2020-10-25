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
        private readonly PluginTypesRepository _pluginTypesRepository;
        public DevicePanelInstanceService(PluginTypesRepository pluginTypesRepository)
        { _pluginTypesRepository = pluginTypesRepository;}

        public IDevicePanel CreateInstance(Device device)
        {
            ConcurrentDictionary<string,Type> typeModelMap = _pluginTypesRepository.ModelTypeMap;

            if(typeModelMap.ContainsKey(device.Model) && typeModelMap.TryGetValue(device.Model,out Type? deviceType))
            {
                return (Activator.CreateInstance(deviceType!) as IDevicePanel)!;
             }else{
                throw new FileNotFoundException($"Can't found plugin dll of {device.Model}");
            }
            
        }
    }
}