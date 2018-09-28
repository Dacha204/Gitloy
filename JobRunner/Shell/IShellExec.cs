namespace Gitloy.Services.JobRunner.Shell
{
    public interface IShellExec
    {
        ShellCommandResult Execute(string command);
    }
}