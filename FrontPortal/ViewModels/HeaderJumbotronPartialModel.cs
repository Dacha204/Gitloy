namespace Gitloy.Services.FrontPortal.ViewModels
{
    public class HeaderJumbotronPartialModel
    {
        public string Title { get; }
        public string Description { get; }

        public HeaderJumbotronPartialModel(string title, string description)
        {
            Title = title;
            Description = description;
        }
    }
}