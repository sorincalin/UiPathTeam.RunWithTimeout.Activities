using System;
using System.Diagnostics;

namespace UiPathTeam.RunWithTimeout.Activities
{
    class RunProcessWrapper
    {
        public string FileName { get; set; }
        public string Arguments { get; set; }
        public string WorkindDirectory { get; set; }

        public bool WaitForExit { get; set; }
        public int WaitForExitTimeoutMs { get; set; }
        public bool KillAfterTimeout { get; set; }
        public bool CaptureOutput { get; set; }

        public string StandardOutput { get; private set; }
        public string StandardError { get; private set; }
        public int ProcessId { get; private set; }
        public bool Finished { get; private set; }
        public int ExitCode { get; private set; }

        public RunProcessWrapper(string filename)
        {
            FileName = filename;
        }

        public void StartProcess()
        {
            StandardOutput = string.Empty;
            StandardError = string.Empty;
            ExitCode = int.MinValue;

            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = this.FileName,
                    Arguments = this.Arguments,
                    WorkingDirectory = this.WorkindDirectory,
                    UseShellExecute = !this.CaptureOutput,
                    RedirectStandardOutput = this.CaptureOutput,
                    RedirectStandardError = this.CaptureOutput,
                }
            };

            if (CaptureOutput)
            {
                KillAfterTimeout = true;
                process.OutputDataReceived += (sender, args) => ReadOutput(args.Data, false);
                process.ErrorDataReceived += (sender, args) => ReadOutput(args.Data, true);
            }


            process.Start();
            ProcessId = process.Id;

            if (WaitForExit)
            {
                if (CaptureOutput)
                {
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                }

                Finished = process.WaitForExit(WaitForExitTimeoutMs);

                if (CaptureOutput)
                {
                    process.CancelErrorRead();
                    process.CancelOutputRead();
                }

                if (Finished)
                {
                    ExitCode = process.ExitCode;
                }
                else if (KillAfterTimeout)
                {
                    process.Kill();
                }
            }
        }

        private void ReadOutput(string output, bool isError)
        {
            if (isError)
                StandardError += (string.IsNullOrEmpty(StandardError) ? string.Empty : Environment.NewLine) + output;
            else
                StandardOutput += (string.IsNullOrEmpty(StandardOutput) ? string.Empty : Environment.NewLine) + output;
        }
    }
}
