using System;
using System.Activities;
using System.IO;
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
                FileName = @"perl.exe",
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
                FileName = @"perl",
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

        [TestMethod]
        public void Run_BATFile()
        {
            var sampleText = "\"Hello world of MS DOS!\"";
            var sampleFileName = "BatOutput.txt";

            var vbsInvoke = new RunProcessWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\createTextFile.bat",
                Arguments = sampleText + " " + sampleFileName,
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

            Assert.IsTrue(File.Exists(sampleFileName));
            File.Delete(sampleFileName);
        }

        [TestMethod]
        public void Run_FilenameDoesNotExist()
        {
            var vbsInvoke = new RunProcessWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\NonExistentFile.bat",
                WaitForExit = true,
                WaitForExitTimeout = 10000,
                KillAtTimeout = true,
                CaptureOutput = true
            };

            var argumentException = Assert.ThrowsException<ArgumentException>(() => WorkflowInvoker.Invoke(vbsInvoke));
        }

        [TestMethod]
        public void Run_WorkingDirectoryDoesNotExist()
        {
            var vbsInvoke = new RunProcessWithTimeoutActivity
            {
                FileName = @"..\..\TestScripts\createTextFile.bat",
                WorkingDirectory = @"c:\NonExistentDir",
                WaitForExit = true,
                WaitForExitTimeout = 10000,
                KillAtTimeout = true,
                CaptureOutput = true
            };

            var argumentException = Assert.ThrowsException<ArgumentException>(() => WorkflowInvoker.Invoke(vbsInvoke));
        }

        [TestMethod]
        public void Run_WaitForExitWith0Timeout()
        {
            var vbsInvoke = new RunProcessWithTimeoutActivity
            {
                FileName = @"perl.exe",
                Arguments = @"..\..\TestScripts\PerlConfigurableDurationStdOutput.pls 3",
                WaitForExit = true,
                KillAtTimeout = true,
                CaptureOutput = true
            };

            var argEx = Assert.ThrowsException<ArgumentException>(() => WorkflowInvoker.Invoke(vbsInvoke));
        }
    }
}