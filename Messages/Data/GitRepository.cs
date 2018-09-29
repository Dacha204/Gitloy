namespace Gitloy.BuildingBlocks.Messages.Data
{
    public class GitRepository
    {
        public string Url { get; set; }
        public string Branch { get; set; }

        public GitRepository()
        {
            Url = "url";
            Branch = "branch";
        }
    }
}