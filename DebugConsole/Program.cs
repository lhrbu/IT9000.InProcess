using PV6900.Wpf.Shared.Models;
using PV6900.Wpf.Shared.Services;
using System;
using System.Collections.Generic;

namespace DebugConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TestProgramParser();
            Console.WriteLine("Hello World!");
        }

        static void TestProgramParser()
        {
            List<ManagedProgramStep> steps = new()
            {
                new ManagedProgramStep{InnerLoopFlag=InnerLoopFlag.None},
                new ManagedProgramStep{InnerLoopFlag=InnerLoopFlag.On,InnerLoopCount=2},
                new ManagedProgramStep {InnerLoopFlag=InnerLoopFlag.None},
                new ManagedProgramStep{InnerLoopFlag=InnerLoopFlag.Off},
                new ManagedProgramStep{InnerLoopFlag=InnerLoopFlag.On,InnerLoopCount=2},
                new ManagedProgramStep {InnerLoopFlag=InnerLoopFlag.None},
                new ManagedProgramStep{InnerLoopFlag=InnerLoopFlag.Off},
            };
            ManagedProgram managedProgram = new () { OuterLoopCount = 1, ManagedProgramSteps = steps };

            ManagedProgramParseService parser = new();
            var program= parser.ParseManagedProgram(managedProgram);
        }
    }
}
