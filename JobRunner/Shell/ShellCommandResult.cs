namespace JobRunner.Shell
{
    public class ShellCommandResult
    {
        public string Command { get; }
        public bool Successful => ExitCode == 0;
        public int ExitCode { get; }
        public string Output { get; }
        
        public ShellCommandResult(string command, int exitCode, string output)
        {
            Command = command;
            ExitCode = exitCode;
            Output = output;
        }
    }
}