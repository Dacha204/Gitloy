using System;
using System.Diagnostics;

namespace Gitloy.Services.JobRunner.Shell
{
    public class BashExecutor : IShellExec, IDisposable
    {
        private Process _process { get; set; }
        
        public ShellCommandResult Execute(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");

            _process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            
            _process.Start();
            string output = _process.StandardOutput.ReadToEnd();
            _process.WaitForExit();

            return new ShellCommandResult(command, _process.ExitCode, output);
        }

        public void Dispose()
        {
            _process?.Dispose();
        }
    }
}