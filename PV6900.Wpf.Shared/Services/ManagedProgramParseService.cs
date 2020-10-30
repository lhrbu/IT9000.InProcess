using PV6900.Wpf.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PV6900.Wpf.Shared.Services
{
    public class ManagedProgramParseService
    {
        private int GetProgramStepsCount(ManagedProgram managedProgram)
        {
            int count = 0;
            int innerLoopCount = 0;
            int innerLoopStepsCount = 0;
            bool intoInnerLoop = false;
            foreach(ManagedProgramStep managedStep in managedProgram.ManagedProgramSteps)
            {
                switch (managedStep.InnerLoopFlag)
                {
                    case InnerLoopFlag.None:
                        if(!intoInnerLoop){++count;}
                        else{++innerLoopStepsCount;}
                        break;
                    case InnerLoopFlag.On:
                        innerLoopCount = managedStep.InnerLoopCount;
                        ++innerLoopStepsCount;
                        intoInnerLoop = true;
                        break;
                    case InnerLoopFlag.Off:
                        ++innerLoopStepsCount;
                        count+=(innerLoopCount*innerLoopStepsCount);
                        innerLoopCount = 0;
                        innerLoopStepsCount = 0;
                        intoInnerLoop = false;
                        break;
                }
            }
            return count;
        }
        public Program ParseManagedProgram(ManagedProgram managedProgram)
        {
            int programStepsCount = GetProgramStepsCount(managedProgram);
            System.Windows.MessageBox.Show($"Steps count:{programStepsCount}");
            List<ProgramStepWithSourceMap> programSteps = new(programStepsCount);

            List<ManagedProgramStep> innerLoopStepsBuffer = new();
            bool intoInnerLoop = false;
            foreach(ManagedProgramStep managedStep in managedProgram.ManagedProgramSteps)
            {
                switch (managedStep.InnerLoopFlag)
                {
                    case InnerLoopFlag.None:
                        if (!intoInnerLoop) {programSteps.Add(ParseNoneLoopStep(managedStep));}
                        else { innerLoopStepsBuffer.Add(managedStep); }
                        break;
                    case InnerLoopFlag.On:
                        innerLoopStepsBuffer.Add(managedStep);
                        intoInnerLoop = true;
                        break;
                    case InnerLoopFlag.Off:
                        innerLoopStepsBuffer.Add(managedStep);
                        programSteps.AddRange(ParseInnerLoopSteps(innerLoopStepsBuffer));
                        innerLoopStepsBuffer.Clear();
                        intoInnerLoop = false;
                        break;
                }
            }

            if (intoInnerLoop)
            {throw new ArgumentException("The program is invalid", nameof(managedProgram));}

            return new Program
            {
                LoopCount = managedProgram.LoopCount,
                ProgramStepsWithSourceMap = programSteps
            };
        }

        public IEnumerable<ProgramStepWithSourceMap> ParseInnerLoopSteps(IEnumerable<ManagedProgramStep> managedProgramSteps)
        {
            if(!ValidateInnerLoopSteps(managedProgramSteps))
            { throw new ArgumentException("Input steps are not a inner loop", nameof(managedProgramSteps)); }
            IEnumerable<ProgramStepWithSourceMap> loopBatchSteps = managedProgramSteps.Select(managedStep => ParseNoneLoopStep(managedStep));
            int innerLoopCount = managedProgramSteps.First().InnerLoopCount;

            List<ProgramStepWithSourceMap> loopSteps = new(managedProgramSteps.Count() * innerLoopCount);
            Enumerable.Range(0, innerLoopCount).ToList().ForEach(index => loopSteps.AddRange(loopBatchSteps));
            return loopSteps;
        }

        private bool ValidateInnerLoopSteps(IEnumerable<ManagedProgramStep> managedProgramSteps)
        {
            int stepsCount = managedProgramSteps.Count();
            if(stepsCount<2 ||
                managedProgramSteps.First().InnerLoopFlag is not InnerLoopFlag.On ||
                managedProgramSteps.Last().InnerLoopFlag is not InnerLoopFlag.Off ||
                managedProgramSteps.Skip(1).Take(stepsCount-2).Any(step=>step.InnerLoopFlag is not InnerLoopFlag.None))
            {
                return false;
            }
            else { return true; }
        }
        public ProgramStepWithSourceMap ParseNoneLoopStep(ManagedProgramStep managedProgramStep) =>
            new(new ProgramStep
                        {
                            Volta = managedProgramStep.Volta,
                            Ampere = managedProgramStep.Ampere,
                            Duration = managedProgramStep.Duration
                        },
            managedProgramStep);
    }
}
