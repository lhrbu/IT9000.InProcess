using IT9000.Wpf.Shared.Models;
using IT9000.Wpf.Shared.Services;
using PV6900.Wpf.Shared.Models;
using PV6900.Wpf.Shared.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PV6900.Wpf.Models;
using System.Threading;

namespace PV6900.Wpf.Services
{
    public class ProgramExecutor
    {
        private readonly IIteInteropService _iteInteropService;

        private Stopwatch _stopwatch = Stopwatch.StartNew();
        public ProgramExecutor(
            IIteInteropService iteInteropService)
        { 
            _iteInteropService = iteInteropService;
        }
        public ManagedProgramStep? CurrentManagedProgramStep { get; private set; }

        private CancellationTokenSource? _cancellationTokenSource;

        public EventHandler<ProgramStepExecutingEventArgs>? ProgramStepExecuting;
        public EventHandler<RunningTimeMeasuredEventArgs>? RunningTimeMeasured;
        public EventHandler<CurrentStepRunningSecondsEventArgs>? CurrentStepRunningSecondsMeasured;
        public EventHandler<OuterLoopCurrentChangedEventArgs>? OuterLoopCurrentChanged;
        public EventHandler<InnerLoopCurrentChangedEventArgs>? InnerLoopCurrentChanged;
        public EventHandler<CurrentStepDurationEventArgs>? CurrentStepDurationChanged;
        public EventHandler<InnerLoopCountChangedEventArgs>? InnerLoopCountChanged;
        public EventHandler<OuterLoopCountChangedEventArgs>? OuterLoopCountChanged;
        public EventHandler<ProgramDurationChangedEventArgs>? ProgramDurationChanged;

        private DateTimeOffset _startTimeOffset;
        private DateTimeOffset _currentStepStartOffset;

        public async Task ExecuteProgramAsync(Device device, Program program)
        {
             _cancellationTokenSource = new();
            _startTimeOffset = DateTimeOffset.Now;
            _currentStepStartOffset = DateTimeOffset.Now;
            _ =MonitorRunningStatusAsync(_cancellationTokenSource.Token)
                .ConfigureAwait(false);

            OuterLoopCountChanged?.Invoke(device, new(program.LoopCount));
            ProgramDurationChanged?.Invoke(device, new(TimeSpan.FromSeconds(
                program.ProgramStepsWithSourceMap.Sum(item => item.ProgramStep.Duration))));

            bool intoInnerLoop = false;
            
            foreach (int outerLoopIndex in Enumerable.Range(0,program.LoopCount))
            {
                OuterLoopCurrentChanged?.Invoke(device, new(outerLoopIndex + 1));

                int innerLoopCount = 1;
                int innerLoopCurrent = 1;
                foreach (ProgramStepWithSourceMap stepWithSourceMap in program.ProgramStepsWithSourceMap)
                {
                    _stopwatch.Reset();
                    _currentStepStartOffset = DateTimeOffset.Now;

                    // Record current step duration
                    CurrentStepDurationChanged?.Invoke(device, new(stepWithSourceMap.ProgramStep.Duration));

                    // Start record inner loop current
                    switch(stepWithSourceMap.ManagedProgramStep.InnerLoopFlag)
                    {
                        case InnerLoopFlag.On:
                            if (!intoInnerLoop)
                            {
                                intoInnerLoop = true;
                                innerLoopCount = stepWithSourceMap.ManagedProgramStep.InnerLoopCount;
                                innerLoopCurrent = 1;
                                InnerLoopCurrentChanged?.Invoke(device, new(innerLoopCurrent));
                            }
                            else
                            {
                                InnerLoopCurrentChanged?.Invoke(device, new(++innerLoopCurrent));
                            }
                        break;
                        case InnerLoopFlag.Off:
                            if (innerLoopCurrent == innerLoopCount)
                            {
                                innerLoopCurrent = 1;
                                intoInnerLoop = false;
                                innerLoopCount = 1;
                            }
                        break;
                        case InnerLoopFlag.None:
                            if (!intoInnerLoop) {
                                innerLoopCurrent = 1;
                                innerLoopCount = 1;
                                InnerLoopCurrentChanged?.Invoke(device, new(innerLoopCurrent));
                            }
                         break;
                    }

                    // Record inner loop count changed
                    InnerLoopCountChanged?.Invoke(device, new(innerLoopCount));

                    CurrentManagedProgramStep = stepWithSourceMap.ManagedProgramStep;
                    ProgramStepExecuting?.Invoke(device, new(stepWithSourceMap.ManagedProgramStep));

                    ProgramStepAssemblyCode assemblyCode = AssemblyProgramStep(stepWithSourceMap.ProgramStep);
                    ExecuteOneProgramStepAssemblyCode(device, assemblyCode);

                    int durationInMS = (int)(assemblyCode.Duration * 1000);
                    int endTime = (int)_stopwatch.ElapsedMilliseconds;
                    int waitTime = durationInMS - endTime;
                    if (waitTime > 0)
                    { await Task.Delay(waitTime, _cancellationTokenSource.Token);}
                }
            }
            if (!_cancellationTokenSource.IsCancellationRequested) { _cancellationTokenSource.Cancel(); }
        }
        public ProgramStepAssemblyCode AssemblyProgramStep(ProgramStep programStep) =>
            new ProgramStepAssemblyCode
            {
                SetVoltaCmd = $"VOLT {programStep.Volta}",
                SetAmpereCmd = $"CURR {programStep.Ampere}",
                Duration = programStep.Duration
            };

        private void ExecuteOneProgramStepAssemblyCode(Device device, ProgramStepAssemblyCode assemblyCode)
        {
            _iteInteropService.WaitHandle.WaitOne();
            int errNo = _iteInteropService.IteDC_WriteCmd(device.Address, assemblyCode.SetVoltaCmd);
            errNo = _iteInteropService.IteDC_WriteCmd(device.Address, assemblyCode.SetAmpereCmd);
            _iteInteropService.WaitHandle.Set();
        }

        private async Task MonitorRunningStatusAsync(CancellationToken token)
        {
            while (true)
            {
                TimeSpan runningTime = DateTimeOffset.Now - _startTimeOffset;
                RunningTimeMeasured?.Invoke(this, new(runningTime));
                double currentStepRunningSeconds = (DateTimeOffset.Now - _currentStepStartOffset).TotalSeconds;
                CurrentStepRunningSecondsMeasured?.Invoke(this, new(currentStepRunningSeconds));
                await Task.Delay(65,token);
            }
        }


        //private async ValueTask CheckCancelTokenDuringWaiting(int waitTime,CancellationToken token)
        //{
           
        //    while(!token.IsCancellationRequested && waitTime>0)
        //    {
        //        _stopwatch.Reset();

        //        TimeSpan runningTime = DateTimeOffset.Now - _startTimeOffset;
        //        AfterRunningTimeMeasured?.Invoke(this, new(runningTime));
        //        double currentStepRunningSeconds = (DateTimeOffset.Now - _currentStepStartOffset).TotalSeconds;
        //        AfterCurrentStepRunningSecondsMeasured?.Invoke(this, new(currentStepRunningSeconds));
        //        int runningMS = (int)_stopwatch.ElapsedMilliseconds;

        //        int tickTime = 100 + runningMS;
        //        if (waitTime > tickTime)
        //        {
        //            waitTime -= (tickTime);
        //            await Task.Delay(tickTime);
        //        }
        //        else { 
        //            await Task.Delay(waitTime);
        //            waitTime = 0;
        //        }
        //    }
        //}

        public void StopProgram() => _cancellationTokenSource?.Cancel();

        
    }
}
