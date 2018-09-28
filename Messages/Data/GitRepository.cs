namespace Gitloy.BuildingBlocks.Messages.Data
{
    public class GitRepository
    {
        public string Url { get; private set; }
        public string Branch { get; private set; }
        
        public GitRepository(string url, string branch)
        {
            Url = url;
            Branch = branch;
        }
    }
}