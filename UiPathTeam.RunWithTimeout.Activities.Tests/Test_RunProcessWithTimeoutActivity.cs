using System;
using System.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UiPathTeam.RunWithTimeout.Activities.Tests
{
    [TestClass]
    public class Test_RunProcessWithTimeoutActivity
    {
        [TestMethod]
        public void Run_PerlConfigurableDurationStdOutput_KillBeforeForExit()
        {

            var vbsInvoke = new RunProcessWithTimeoutActivity
            {
                FileName = @"c:\perl\bin\perl.exe",
                Arguments = @"..\..\TestScripts\PerlConfigurableDurationStdOutput.pls 3",
                WaitForExit = true,
                WaitForExitTimeout = 1000,
                KillAtTimeout = true,
                CaptureOutput = true
            };

            var output = WorkflowInvoker.Invoke(vbsInvoke);

            Assert.IsTrue(Convert.ToInt32(output["ProcessId"]) != 0);

            Assert.IsFalse(Convert.ToBoolean(output["Finished"]));
            Assert.IsTrue(Convert.ToInt32(output["ExitCode"]) == int.MinValue);

            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(output["Output"])));
            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(output["Error"])));
        }

        [TestMethod]
        public void Run_PerlConfigurableDurationStdOutput_WaitForExit()
        {

            var vbsInvoke = new RunProcessWithTimeoutActivity
            {
                FileName = @"c:\perl\bin\perl.exe",
                Arguments = @"..\..\TestScripts\PerlConfigurableDurationStdOutput.pls 3",
                WaitForExit = true,
                WaitForExitTimeout = 4000,
                KillAtTimeout = true,
                CaptureOutput = true
            };

            var output = WorkflowInvoker.Invoke(vbsInvoke);

            Assert.IsTrue(Convert.ToInt32(output["ProcessId"]) != 0);

            Assert.IsTrue(Convert.ToBoolean(output["Finished"]));
            Assert.IsTrue(Convert.ToInt32(output["ExitCode"]) == 0);

            Assert.IsFalse(string.IsNullOrEmpty(Convert.ToString(output["Output"])));
            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(output["Error"])));
        }

        [TestMethod]
        public void Run_PerlInvalidSyntaxErrOutput()
        {
            var vbsInvoke = new RunProcessWithTimeoutActivity
            {
                FileName = @"c:\perl\bin\perl.exe",
                Arguments = @"..\..\TestScripts\PerlInvalidSyntaxErrOutput.vbs 3",
                WaitForExit = true,
                WaitForExitTimeout = 1000,
                KillAtTimeout = true,
                CaptureOutput = true
            };

            var output = WorkflowInvoker.Invoke(vbsInvoke);

            Assert.IsTrue(Convert.ToInt32(output["ProcessId"]) != 0);

            Assert.IsTrue(Convert.ToBoolean(output["Finished"]));
            Assert.IsFalse(Convert.ToInt32(output["ExitCode"]) == 0);

            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(output["Output"])));
            Assert.IsFalse(string.IsNullOrEmpty(Convert.ToString(output["Error"])));
        }
    }
}