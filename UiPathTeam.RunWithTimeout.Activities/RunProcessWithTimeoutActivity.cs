using System.Activities;
using System.ComponentModel;

namespace UiPathTeam.RunWithTimeout.Activities
{
    [DisplayName("Run Process With Timeout")]
    [Description("Runs a process using System.Diagnostics.Process.")]
    public class RunProcessWithTimeoutActivity: CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Filename")]
        public InArgument<string> FileName { get; set; }

        [Category("Input")]
        [DisplayName("Working directory")]
        public InArgument<string> WorkingDirectory { get; set; }

        [Category("Input")]
        [DisplayName("Arguments")]
        public InArgument<string> Arguments { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Capture output")]
        [Description("Indicates if the activity should capture the ouput of the process. If this is set to true, the process will be killed when the timeout is exceeded.")]
        public InArgument<bool> CaptureOutput { get; set; }

        [Category("Input")]
        [RequiredArgument]
        [DisplayName("Wait for exit flag")]
        [Description("Indicates if the activity should wait for the process to finish.")]
        public InArgument<bool> WaitForExit { get; set; }

        [Category("Input")]
        [DisplayName("Wait for exit timeout in ms")]
        [Description("Number of miliseconds to wait for the process to finish.")]
        public InArgument<int> WaitForExitTimeout { get; set; }

        [Category("Input")]
        [DisplayName("Kill at timeout")]
        [Description("Indicates if the process should be killed after the wait timeout is over. If Capture ouput option is selected, it will default to true.")]
        public InArgument<bool> KillAtTimeout { get; set; }

        [Category("Output")]
        [DisplayName("Standard output")]
        [Description("Standard output stream content of the process.")]
        public OutArgument<string> Output { get; set; }

        [Category("Output")]
        [DisplayName("Standard error")]
        [Description("Standard error stream content of the process.")]
        public OutArgument<string> Error { get; set; }

        [Category("Output")]
        [DisplayName("Process Id")]
        [Description("Id of the started process")]
        public OutArgument<int> ProcessId { get; set; }

        [Category("Output")]
        [DisplayName("Finished")]
        [Description("Indicates it the process has finished in the allocated time.")]
        public OutArgument<bool> Finished { get; set; }


        [Category("Output")]
        [DisplayName("Exit code")]
        [Description("Exit Code of the process. If the process did not finish in the allocated time will be set to Int.MinValue.")]
        public OutArgument<int> ExitCode { get; set; }



        protected override void Execute(CodeActivityContext context)
        {
            var processInvokeWrapper = new RunProcessWrapper(FileName.Get(context))
            {
                Arguments = Arguments.Get(context),
                WorkindDirectory = WorkingDirectory.Get(context),
                CaptureOutput = CaptureOutput.Get(context),
                WaitForExit = WaitForExit.Get(context),
                WaitForExitTimeoutMs = WaitForExitTimeout.Get(context),
                KillAfterTimeout = KillAtTimeout.Get(context)
            };

            processInvokeWrapper.StartProcess();

            ProcessId.Set(context, processInvokeWrapper.ProcessId);
            Finished.Set(context, processInvokeWrapper.Finished);
            Output.Set(context, processInvokeWrapper.StandardOutput);
            Error.Set(context, processInvokeWrapper.StandardError);
            ExitCode.Set(context, processInvokeWrapper.ExitCode);
        }
    }
}
