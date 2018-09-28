using System.Diagnostics;

namespace Gitloy.Services.JobRunner.Shell
{
    public class BashExecutor : IShellExec
    {
        public ShellCommandResult Execute(string command)
        {
            var escapedArgs = command.Replace("\"", "\\\"");

            var process = new Process()
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

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return new ShellCommandResult(command, process.ExitCode, output);
        }
    }
}