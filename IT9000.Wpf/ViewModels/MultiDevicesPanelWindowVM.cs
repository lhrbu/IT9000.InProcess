﻿using IT9000.Wpf.Repositories;
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
        private readonly DevicePanelsRepository _devicePanelsRepository;
        public MultiDevicesPanelWindowVM(
            DevicesRepository devicesRepository,
            DevicePanelsRepository devicePanelsRepository)
        { 
            _devicesRepository = devicesRepository;
            _devicePanelsRepository = devicePanelsRepository;

            SelectionsResetCommand = new(listBox => listBox.SelectedItem = null);
            SelectionsRunCommand = new(SelectionsRun);
            SelectionsStopCommand = new(SelectionsStop);
            SelectionsStopMonitorCommand = new(SelectionsStopMonitor);
        }

        public ObservableCollection<Device> OnlineDevices => _devicesRepository.OnlineDevices;
        public Visibility OnlineDevicesEmptyFlag =>
            OnlineDevices.Count() != 0 ? Visibility.Visible : Visibility.Hidden;

        public DelegateCommand<ListBox> SelectionsResetCommand { get; }
        public DelegateCommand<ListBox> SelectionsRunCommand { get; }
        public DelegateCommand<ListBox> SelectionsStopCommand { get; }
        public DelegateCommand<ListBox> SelectionsStopMonitorCommand { get; }

        public void SelectionsRun(ListBox listBox)
        {
            try
            {
                IEnumerable<Device> devices = listBox.SelectedItems.Cast<Device>()!;
                //IEnumerable<IDevicePanel?> devicePanels = devices.Select(_devicePanelsRepository.FindDevicePanel);
                foreach (Device device in devices)
                {
                    IDevicePanel? devicePanel = _devicePanelsRepository.FindDevicePanel(device);
                    if (devicePanel is null || (!devicePanel.CanRunProgram()))
                    {
                        MessageBox.Show($"{device.Name} can't run program now.", "Error:");
                        return;
                    }
                }
                foreach (Device device in devices)
                {
                    IDevicePanel devicePanel = _devicePanelsRepository.FindDevicePanel(device)!;
                    devicePanel.StartRunProgram();
                }
            }catch(Exception e) { MessageBox.Show(e.ToString()); }
            finally { Window.GetWindow(listBox).Close(); }
        }

        public void SelectionsStop(ListBox listBox)
        {
            IEnumerable<Device> devices = listBox.SelectedItems.Cast<Device>()!;
            //IEnumerable<IDevicePanel?> devicePanels = devices.Select(_devicePanelsRepository.FindDevicePanel);
            foreach (Device device in devices)
            {
                IDevicePanel? devicePanel = _devicePanelsRepository.FindDevicePanel(device);
                if (devicePanel is null || (!devicePanel.CanStopProgram()))
                {
                    MessageBox.Show($"{device.Name} can't stop program now.", "Error:");
                    return;
                }
            }
            foreach (Device device in devices)
            {
                IDevicePanel devicePanel = _devicePanelsRepository.FindDevicePanel(device)!;
                devicePanel.StopRunProgram();
            }
            Window.GetWindow(listBox).Close();
        }

        public void SelectionsStopMonitor(ListBox listBox)
        {
            IEnumerable<Device> devices = listBox.SelectedItems.Cast<Device>()!;
            foreach (Device device in devices)
            {
                IDevicePanel? devicePanel = _devicePanelsRepository.FindDevicePanel(device);
                if (devicePanel is null || (!devicePanel.CanStopMonitor()))
                {
                    MessageBox.Show($"{device.Name} can't stop monitor now.", "Error:");
                    return;
                }
            }
            foreach(Device device in devices)
            {
                IDevicePanel devicePanel = _devicePanelsRepository.FindDevicePanel(device)!;
                devicePanel.StopMonitor();
            }
            Window.GetWindow(listBox).Close();
        }
    }
}
