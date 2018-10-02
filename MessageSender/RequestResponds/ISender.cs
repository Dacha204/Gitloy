namespace MessageSender.RequestResponds
{
    public interface ISender
    {
        string Name { get; }
        string JsonExample { get; }
        bool JsonMessage(string jsonData);
        string Send();
    }
}