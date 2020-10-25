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
    public class MultiDevicesPanelWindowVM
    {
        private readonly DevicesRepository _devicesRepository;
        public MultiDevicesPanelWindowVM(DevicesRepository devicesRepository)
        { 
            _devicesRepository = devicesRepository;
            SelectionsResetCommand = new(listBox => listBox.SelectedItem = null);
            SelectionsRunCommand = new(listBox=> 
            {
                var selectedDevices = listBox.SelectedItems.Cast<Device>();
                foreach(var devicePanelPair in _devicesRepository.DevicePanelMap)
                {
                    if(selectedDevices.Contains(devicePanelPair.Key))
                    {
                        devicePanelPair.Value.StartRunProgram(devicePanelPair.Key);
                    }
                }
            });
        }

        public ObservableCollection<Device> OnlineDevices => _devicesRepository.OnlineDevices;
        public Visibility OnlineDevicesEmptyFlag =>
            OnlineDevices.Count() != 0 ? Visibility.Visible : Visibility.Hidden;

        public DelegateCommand<ListBox> SelectionsResetCommand { get; }
        public DelegateCommand<ListBox> SelectionsRunCommand { get; }
    }
}
