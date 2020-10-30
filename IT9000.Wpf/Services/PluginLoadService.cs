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
using System.Runtime.Loader;
using System.Runtime.CompilerServices;

namespace IT9000.Wpf.Services
{
    public class PluginLoadService
    {
        private readonly PluginFactoriessRepository _pluginFactoriesRepository;
        public PluginLoadService(
            PluginFactoriessRepository pluginFactoriesRepository)
        { _pluginFactoriesRepository=pluginFactoriesRepository;}
        private string PluginsDirectory => Path.Combine(
            Environment.CurrentDirectory, "Plugins");
        private string GetPluginDllPath(string deviceModel) => Path.Combine(
            PluginsDirectory, $"{deviceModel}.Wpf.dll");

        private string GetPluginDependencyDirectory(string deviceModel)=>Path
            .Combine(PluginsDirectory,$"{deviceModel}.Refs");

        public void Load(Device device)
        {
            if(!_pluginFactoriesRepository.ModelFactoriesMap.ContainsKey(device.Model))
            {
                
                foreach(Type type in Assembly.LoadFrom(GetPluginDllPath(device.Model)).GetTypes())
                {
                    Type? idevicePanelFactoryType = type.GetInterface(nameof(IDevicePanelFactory));
                    if(idevicePanelFactoryType is not null)
                    { 
                        IDevicePanelFactory devicePanelFactory= 
                            (Activator.CreateInstance(type) as IDevicePanelFactory)!;
                        _pluginFactoriesRepository.ModelFactoriesMap.TryAdd(device.Model,devicePanelFactory);
                    }
                }
            }
        }

        public void LoadDependency(Device device)
        {
            if (!_pluginFactoriesRepository.ModelFactoriesMap.ContainsKey(device.Model))
            {
                IEnumerable<string> dllPaths = Directory.GetFiles(GetPluginDependencyDirectory(device.Model)).Where(item => item.EndsWith(".dll"));
                foreach (string dllPath in dllPaths)
                { AssemblyLoadContext.Default.LoadFromAssemblyPath(dllPath); }
            }
        }
    }
}