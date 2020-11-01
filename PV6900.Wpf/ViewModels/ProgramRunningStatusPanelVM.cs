using IT9000.Wpf.Shared.Models;
using Prism.Mvvm;
using PV6900.Wpf.Services;
using PV6900.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.ViewModels
{
    public class ProgramRunningStatusPanelVM : BindableBase
    {
        public ProgramRunningStatusPanelVM(
            DeviceStorageService deviceStorageService,
            ProgramExecutor programExecutor)
        {
            Device device = deviceStorageService.Get()!;
            programExecutor.RunningTimeMeasured += (sender, e) =>
              {RunningTimeSpan = e.RunningTimeSpan;};
            programExecutor.OuterLoopCurrentChanged += (sender, e) =>
              { OuterLoopCurrent = e.OuterLoopCurrent; };
            programExecutor.CurrentStepRunningSecondsMeasured += (sender, e) =>
                  CurrentStepRunningSeconds = e.CurrentStepRunningSeconds;
            programExecutor.InnerLoopCurrentChanged += (sender, e) =>
                 InnerLoopCurrent = e.InnerLoopCurrent;
            programExecutor.CurrentStepDurationChanged += (sender, e) =>
                 CurrentStepDuration = e.CurrentStepDuration;
            programExecutor.InnerLoopCountChanged += (sender, e) =>
                InnerLoopCount = e.InnerLoopCount;
            programExecutor.OuterLoopCountChanged += (sender, e) =>
                  OuterLoopCount = e.OuterLoopCount;
            programExecutor.ProgramDurationChanged += (sender, e) =>
                  ProgramDuration = e.ProgramDuration;


        }
        public TimeSpan RunningTimeSpan
        { get => _runningTimeSpan; set => SetProperty(ref _runningTimeSpan, value); }
        private TimeSpan _runningTimeSpan;
        public TimeSpan ProgramDuration
        { get => _programDuration; set => SetProperty(ref _programDuration, value); }
        private TimeSpan _programDuration;
        public int InnerLoopCurrent 
        { get => _innerLoopCurrent; set => SetProperty(ref _innerLoopCurrent, value); }
        private int _innerLoopCurrent = 0;
        public int InnerLoopCount
        { get => _innerLoopCount; set => SetProperty(ref _innerLoopCount, value); }
        private int _innerLoopCount = 0;
        public int OuterLoopCurrent 
        { get => _outerLoopCurrent;set => SetProperty(ref _outerLoopCurrent, value);}
        private int _outerLoopCurrent = 0;
        public int OuterLoopCount
        { get => _outerLoopCount; set => SetProperty(ref _outerLoopCount, value); }
        private int _outerLoopCount = 0;
        public double CurrentStepRunningSeconds 
        { get=>_currentStepRunningSeconds; set=>SetProperty(ref _currentStepRunningSeconds,value); }
        private double _currentStepRunningSeconds = 0;
        public double CurrentStepDuration
        { get => _currentStepDuration; set => SetProperty(ref _currentStepDuration, value); }
        private double _currentStepDuration = 0;
    }
}
