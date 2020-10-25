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
        
        private bool _isRequestCancellation = false;
        public EventHandler<ProgramStepExecutingEventArgs>? ProgramStepExecuting;
        public async Task ExecuteProgramAsync(Device device, Program program)
        {
            _isRequestCancellation=false;
            foreach(int outerLoopIndex in Enumerable.Range(0,program.LoopCount))
            {
                foreach(ProgramStepWithSourceMap stepWithSourceMap in program.ProgramStepsWithSourceMap)
                {
                    if (!_isRequestCancellation)
                    {
                        CurrentManagedProgramStep = stepWithSourceMap.ManagedProgramStep;
                        ProgramStepExecuting?.Invoke(device,new(stepWithSourceMap.ManagedProgramStep));
                        ProgramStepAssemblyCode assemblyCode = AssemblyProgramStep(stepWithSourceMap.ProgramStep);
                        await ExecuteOneProgramStepAssemblyCodeAsync(device, assemblyCode);
                    }
                    else
                    {
                        _isRequestCancellation = false;
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

        public async ValueTask ExecuteOneProgramStepAssemblyCodeAsync(Device device, ProgramStepAssemblyCode assemblyCode)
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
            if (waitTime > 0){await Task.Delay(waitTime);}
        }

        public void StopProgram()=>_isRequestCancellation=true;

        
    }
}
