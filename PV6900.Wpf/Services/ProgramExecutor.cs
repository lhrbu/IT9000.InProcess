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

        
        public async Task ExecuteProgramAsync(Device device, Program program)
        {
            
             _cancellationTokenSource = new();
            foreach (int outerLoopIndex in Enumerable.Range(0,program.LoopCount))
            {
                foreach(ProgramStepWithSourceMap stepWithSourceMap in program.ProgramStepsWithSourceMap)
                {
                    if (!_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        CurrentManagedProgramStep = stepWithSourceMap.ManagedProgramStep;
                        ProgramStepExecuting?.Invoke(device,new(stepWithSourceMap.ManagedProgramStep));
                        ProgramStepAssemblyCode assemblyCode = AssemblyProgramStep(stepWithSourceMap.ProgramStep);
                        await ExecuteOneProgramStepAssemblyCodeAsync(device, assemblyCode,_cancellationTokenSource.Token);
                    }
                    else
                    {
                        return;
                    }

                }
            }
        }
        public ProgramStepAssemblyCode AssemblyProgramStep(ProgramStep programStep) =>
            new ProgramStepAssemblyCode
            {
                SetVoltaCmd = $"VOLT {programStep.Volta}",
                SetAmpereCmd = $"CURR {programStep.Ampere}",
                Duration = programStep.Duration
            };

        public async ValueTask ExecuteOneProgramStepAssemblyCodeAsync(Device device, ProgramStepAssemblyCode assemblyCode,
            CancellationToken token)
        {
            _stopwatch.Reset();
            int startTime = (int)_stopwatch.ElapsedMilliseconds;
            int durationInMS = (int)(assemblyCode.Duration * 1000);

            _iteInteropService.WaitHandle.WaitOne();
            int errNo = _iteInteropService.IteDC_WriteCmd(device.Address, assemblyCode.SetVoltaCmd);
            errNo = _iteInteropService.IteDC_WriteCmd(device.Address, assemblyCode.SetAmpereCmd);
            _iteInteropService.WaitHandle.Set();

            int endTime = (int)_stopwatch.ElapsedMilliseconds;
            int waitTime = durationInMS - (endTime - startTime);
            if (waitTime > 0)
            {
                await CheckCancelTokenDuringWaiting(waitTime, token);
            }
        }

        private async ValueTask CheckCancelTokenDuringWaiting(int waitTime,CancellationToken token)
        {
            while(!token.IsCancellationRequested && waitTime>0)
            {
                if (waitTime > 100)
                {
                    waitTime -= 100;
                    await Task.Delay(100);
                }
                else { 
                    
                    await Task.Delay(waitTime);
                    waitTime = 0;
                }
            }
        }

        public void StopProgram() => _cancellationTokenSource?.Cancel();

        
    }
}
