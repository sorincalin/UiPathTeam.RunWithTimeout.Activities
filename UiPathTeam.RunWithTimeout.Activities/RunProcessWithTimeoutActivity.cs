using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UiPathTeam.RunWithTimeout.Design;

namespace UiPathTeam.RunWithTimeout.Activities
{
    [Designer(typeof(RunProcessWithTimeoutActivityDesigner))]
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
        [DisplayName("Wait for exit")]
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

        protected void ValidateWorkingDirectory(CodeActivityContext context)
        {
            var workingDirectory = WorkingDirectory.Get(context);
            if (!string.IsNullOrEmpty(workingDirectory) && !Directory.Exists(workingDirectory))
            {
                throw new ArgumentException("Working directory does not exist.");
            }
        }

        protected virtual void ValidateFilename(CodeActivityContext context, bool lookForFilesInPATH)
        {
            var workingDirectory = WorkingDirectory.Get(context);
            if (string.IsNullOrEmpty(workingDirectory))
            {
                workingDirectory = Directory.GetCurrentDirectory();
            }

            var filename = FileName.Get(context);
            if (Path.IsPathRooted(filename) && !File.Exists(filename))
            {
                throw new ArgumentException("Filename with absolute path could not be found.");
            }
            else
            {
                if (Path.GetFileName(filename) == filename)
                {
                    if (lookForFilesInPATH && !Environment.GetEnvironmentVariable("PATH").Split(';').
                        Any(dir => (string.IsNullOrEmpty(Path.GetExtension(filename)) && // The given filename does not have an extension but matches one file from the directories in PATH
                                    Directory.GetFiles(dir).Any(file => Path.GetFileNameWithoutExtension(file) == filename)) ||
                                    File.Exists(Path.Combine(dir, filename)))) // Filename is present in one of the directories from PATH 
                    {
                        throw new ArgumentException("Filename could not be found in the working directory or in any directory from the PATH Environment Variable.");
                    }
                }
                else
                {
                    if (!File.Exists(Path.Combine(workingDirectory, filename)))
                    {
                        throw new ArgumentException("Filename with relative path could not be found in the current working directory.");
                    }
                }
            }
        }

        protected void ValidateInputArguments(CodeActivityContext context) 
        {
            ValidateWorkingDirectory(context);
            ValidateFilename(context, true); // For processes we want to look in directories from PATH

            if (WaitForExit.Get(context) && WaitForExitTimeout.Get(context) == 0)
            {
                throw new ArgumentException("WaitForExit is set to True but the WaitForExitTimeout is 0.");
            }
        }

        protected override void Execute(CodeActivityContext context)
        {
            ValidateInputArguments(context);

            var processInvokeWrapper = new RunProcessWrapper(FileName.Get(context),
                                                            Arguments.Get(context),
                                                            WorkingDirectory.Get(context),
                                                            CaptureOutput.Get(context),
                                                            WaitForExit.Get(context),
                                                            WaitForExitTimeout.Get(context),
                                                            KillAtTimeout.Get(context));

            processInvokeWrapper.StartProcess();

            ProcessId.Set(context, processInvokeWrapper.ProcessId);
            Finished.Set(context, processInvokeWrapper.Finished);
            Output.Set(context, processInvokeWrapper.StandardOutput);
            Error.Set(context, processInvokeWrapper.StandardError);
            ExitCode.Set(context, processInvokeWrapper.ExitCode);
        }
    }
}
