using System;
using System.Activities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UiPathTeam.RunWithTimeout.Activities.Tests
{
    [TestClass]
    public class Test_RunScriptWithTimeoutActivity
    {
        [TestMethod]
        public void Run_VBSConfigurableDurationStdOutput_KillBeforeForExit()
        {

            var vbsInvoke = new RunScriptWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\VBSConfigurableDurationStdOutput.vbs",
                Arguments = "3000",
                WaitForExit = true,
                WaitForExitTimeout = 1000,
                KillAtTimeout = true,
                CaptureOutput = true
            };

            var output = WorkflowInvoker.Invoke(vbsInvoke);

            Assert.IsTrue(Convert.ToInt32(output["ProcessId"]) != 0);

            Assert.IsFalse(Convert.ToBoolean(output["Finished"]));
            Assert.IsTrue(Convert.ToInt32(output["ExitCode"]) == int.MinValue);

            Assert.IsFalse(string.IsNullOrEmpty(Convert.ToString(output["Output"])));
            Assert.IsTrue(string.IsNullOrEmpty(Convert.ToString(output["Error"])));
        }

        [TestMethod]
        public void Run_VBSConfigurableDurationStdOutput_WaitForExit()
        {

            var vbsInvoke = new RunScriptWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\VBSConfigurableDurationStdOutput.vbs",
                Arguments = "3000",
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
        public void Run_VBSInvalidSyntaxErrOutput()
        {
            var vbsInvoke = new RunScriptWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\VBSInvalidSyntaxErrOutput.vbs",
                Arguments = "3000",
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

        [TestMethod]
        public void Run_VBSWithMsgBoxAndStdOutput()
        {
            var vbsInvoke = new RunScriptWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\VBSWithMsgBoxAndStdOutput.vbs",
                Arguments = "1 2 3",
                WaitForExit = true,
                WaitForExitTimeout = 10000,
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
        public void Run_VBSWithSpacesInFilePath()
        {
            var vbsInvoke = new RunScriptWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\VBS With Spaces In Name.vbs",
                WaitForExit = true,
                WaitForExitTimeout = 10000,
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
        public void Run_UnsupportedScript()
        {
            var vbsInvoke = new RunScriptWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\UnsupportedScript.wtf",
                WaitForExit = true,
                WaitForExitTimeout = 10000,
                KillAtTimeout = true,
                CaptureOutput = true
            };

            var output = WorkflowInvoker.Invoke(vbsInvoke);

            Assert.IsTrue(Convert.ToInt32(output["ProcessId"]) != 0);

            Assert.IsTrue(Convert.ToBoolean(output["Finished"]));
            Assert.IsTrue(Convert.ToInt32(output["ExitCode"]) != 0);

            Assert.IsTrue(Convert.ToString(output["Output"]).Contains("There is no script engine for file extension"));
        }
    }
}
