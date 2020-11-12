using Prism.Commands;
using Prism.Mvvm;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Models;
using PV6900.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading;
using PV6900.Wpf.Models;
using System.Runtime.CompilerServices;
using System.Windows.Threading;
using System.IO;
using System.Text.Json;
using Microsoft.Win32;
using Syncfusion.Windows.Tools.Controls;
using Prism.Services.Dialogs;

namespace PV6900.Wpf.ViewModels
{
    public class ProgramDashboardVM:BindableBase,IDisposable
    {
        private readonly ManagedProgramParseService _managedProgramParseService;
        private readonly ProgramExecutor _programExecutor;
        private readonly DeviceStorageService _deviceStorageService;
        private readonly TimeSpanVoltaChartVM _timeSpanVoltaChartVM;
        private readonly TimeSpanAmpereChartVM _timeSpanAmpereChartVM;
        private readonly MonitorMenuVM _monitorMenuVM;
        public ProgramRunningStatusPanelVM ProgramRunningStatusPanelVM { get; }
        public ProgramDashboardVM(
            ManagedProgramParseService managedProgramParseService,
            ProgramExecutor programExecutor,
            DeviceStorageService deviceStorageService,
            MonitorMenuVM monitorMenuVM,
            TimeSpanVoltaChartVM timeSpanVoltaChartVM,
            TimeSpanAmpereChartVM timeSpanAmpereChartVM,
            ProgramRunningStatusPanelVM programRunningStatusPanelVM)
        {
            _managedProgramParseService = managedProgramParseService;
            _programExecutor = programExecutor;
            _deviceStorageService = deviceStorageService;
            _timeSpanVoltaChartVM = timeSpanVoltaChartVM;
            _timeSpanAmpereChartVM = timeSpanAmpereChartVM;
            _monitorMenuVM = monitorMenuVM;
            ProgramRunningStatusPanelVM = programRunningStatusPanelVM;

            AddCommand = new(() =>ManagedProgramSteps.Add(new()));
            DeleteCommand = new((dataGrid => ManagedProgramSteps
                .Remove((dataGrid.SelectedItem as ManagedProgramStep)!)));

            StartCommand = new(StartProgram);
            StopCommand = new(StopProgram);

             _programExecutor.ProgramStepExecuting+=(sender,e)=>
            {
                CurrentManagedProgramStep = null;
                CurrentManagedProgramStep=e.CurrentManagedProgramStep;
            };

            ExportProgramCommand = new(async()=>await ExportProgramAsync());
            ImportProgramCommand = new(async () => await ImportProgramAsync());

        }
        public ObservableCollection<ManagedProgramStep> ManagedProgramSteps { get; } = new() { new()};

        public DelegateCommand AddCommand { get; }
        public DelegateCommand<DataGrid> DeleteCommand { get; }
        public DelegateCommand<DataGrid> StartCommand { get; }
        public DelegateCommand StopCommand { get; }
        public DelegateCommand ExportProgramCommand { get; }
        public DelegateCommand ImportProgramCommand { get; }

        public bool InRunning { get => _inRunning; set => SetProperty(ref _inRunning, value); }
        private bool _inRunning =false;
        public int OuterLoopCount { get => _outerLoopCount; set => SetProperty(ref _outerLoopCount, value); }
        private int _outerLoopCount = 1;

        private Dispatcher _Dispatcher => Application.Current.Dispatcher;
        private void StartProgram(DataGrid dataGrid)
        {
            if (InRunning) { return; }

            _timeSpanVoltaChartVM.Reset();
            _timeSpanAmpereChartVM.Reset();
            
            if(!_monitorMenuVM.InMonitor)
            { _Dispatcher.Invoke(() => _monitorMenuVM.StartMonitorCommand.Execute()); }

            _Dispatcher.Invoke(() =>
            { dataGrid.SetBinding(DataGrid.SelectedItemProperty, new Binding(nameof(CurrentManagedProgramStep)));});
            Program program = _managedProgramParseService.ParseManagedProgram(new ManagedProgram
            { OuterLoopCount = OuterLoopCount, ManagedProgramSteps = ManagedProgramSteps.ToList() });

            _programExecutor.ExecuteProgramAsync(
                _deviceStorageService.Get()!, program!).ContinueWith(task=>{
                    _Dispatcher.Invoke(()=>
                        {
                            BindingOperations.ClearBinding(dataGrid,DataGrid.SelectedItemProperty);
                            dataGrid.SelectedItem=null;
                            GC.Collect();
                            InRunning=false;
                        });
                }).ConfigureAwait(false);
            InRunning = true;
        }

        private void StopProgram() => _programExecutor.StopProgram();

        public ManagedProgramStep? CurrentManagedProgramStep
        {
            get=>_currentManagedProgramStep;
            set=>SetProperty(ref _currentManagedProgramStep,value);
        }
        private ManagedProgramStep? _currentManagedProgramStep;

        private bool _disposed = false;
        public void Dispose()
        {
            if(!_disposed)
            {
                ManagedProgramSteps.Clear();
                StopProgram();
                CurrentManagedProgramStep = null;
                OuterLoopCount = 1;
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }


        private async ValueTask ExportProgramAsync()
        {
            SaveFileDialog saveFileDialog = new() { Title = "Export Program File:" };
            saveFileDialog.DefaultExt = ".json";
            saveFileDialog.AddExtension = true;
            saveFileDialog.ShowDialog();
            string? filePath = saveFileDialog.FileName;

            if (filePath is not null)
            {
                ManagedProgram program = new()
                {
                    OuterLoopCount = OuterLoopCount,
                    ManagedProgramSteps = ManagedProgramSteps.ToList()
                };
                using Stream stream = File.OpenWrite(filePath);
                await JsonSerializer.SerializeAsync(stream, program,new() { WriteIndented=true});
            }
        }
        private async ValueTask ImportProgramAsync()
        {
            OpenFileDialog openFileDialog = new() { Title="Select Program File:" };
            openFileDialog.ShowDialog();
            string filePath = openFileDialog.FileName;

            if (File.Exists(filePath))
            {
                using Stream stream = File.OpenRead(filePath);
                ManagedProgram managedProgram = (await JsonSerializer.DeserializeAsync<ManagedProgram>(stream))!;
                OuterLoopCount = managedProgram.OuterLoopCount;
                ManagedProgramSteps.Clear();
                ManagedProgramSteps.AddRange(managedProgram.ManagedProgramSteps);
            }

        }
    }
}
