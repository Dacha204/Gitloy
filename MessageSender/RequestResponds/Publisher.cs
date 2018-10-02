using Gitloy.Services.Common.Communicator;

namespace MessageSender.RequestResponds
{
    public class Publisher<TMessage> : Sender<TMessage>
        where TMessage : class, new()
    {
        public Publisher(ICommunicator communicator) 
            : base(communicator)
        {
        }

        public override string Send()
        {
            Communicator.Bus.Publish(Message);
            return "Published";
        }
    }
}