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

namespace PV6900.Wpf.ViewModels
{
    public class ProgramDashboardVM:BindableBase
    {
        private readonly ManagedProgramParseService _managedProgramParseService;
        private readonly ProgramExecutor _programExecutor;
        private readonly DeviceStorageService _deviceStorageService;
        private readonly TimeSpanVoltaChartVM _timeSpanVoltaChartVM;
        private readonly TimeSpanAmpereChartVM _timeSpanAmpereChartVM;
        public ProgramDashboardVM(
            ManagedProgramParseService managedProgramParseService,
            ProgramExecutor programExecutor,
            DeviceStorageService deviceStorageService,
            TimeSpanVoltaChartVM timeSpanVoltaChartVM,
            TimeSpanAmpereChartVM timeSpanAmpereChartVM)
        {
            _managedProgramParseService = managedProgramParseService;
            _programExecutor = programExecutor;
            _deviceStorageService = deviceStorageService;
            _timeSpanVoltaChartVM = timeSpanVoltaChartVM;
            _timeSpanAmpereChartVM = timeSpanAmpereChartVM;

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
        }
        public ObservableCollection<ManagedProgramStep> ManagedProgramSteps { get; } = new() { new()};

        public DelegateCommand AddCommand { get; }
        public DelegateCommand<DataGrid> DeleteCommand { get; }
        public DelegateCommand<DataGrid> StartCommand { get; }
        public DelegateCommand StopCommand { get; }

        public bool InRunning { get => _inRunning; set => SetProperty(ref _inRunning, value); }
        private bool _inRunning =false;
        public int OuterLoopCount { get => _outerLoopCount; set => SetProperty(ref _outerLoopCount, value); }
        private int _outerLoopCount = 1;

        private Dispatcher _Dispatcher => Application.Current.Dispatcher;
        public void StartProgram(DataGrid dataGrid)
        {
            if (InRunning) { return; }

            _timeSpanVoltaChartVM.Reset();
            _timeSpanAmpereChartVM.Reset();

            _Dispatcher.Invoke(() =>
            { dataGrid.SetBinding(DataGrid.SelectedItemProperty, new Binding(nameof(CurrentManagedProgramStep)));});
            Program program = _managedProgramParseService.ParseManagedProgram(new ManagedProgram
            { LoopCount = OuterLoopCount, ManagedProgramSteps = ManagedProgramSteps.ToList() });

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

        public void StopProgram() => _programExecutor.StopProgram();

        public ManagedProgramStep? CurrentManagedProgramStep
        {
            get=>_currentManagedProgramStep;
            set=>SetProperty(ref _currentManagedProgramStep,value);
        }
        private ManagedProgramStep? _currentManagedProgramStep;

        

    }
}
