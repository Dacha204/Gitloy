using Gitloy.Services.Common.Communicator;
using Newtonsoft.Json;

namespace MessageSender.RequestResponds
{
    public class RequestRespond<TRequest, TResponse> : Sender<TRequest>
        where TRequest  : class, new()
        where TResponse : class, new()

    {
        public RequestRespond(ICommunicator communicator) 
            : base(communicator)
        {
        }
        
        public override string Send()
        {
            var response = Communicator.Bus.Request<TRequest, TResponse>(Message);
            return JsonConvert.SerializeObject(response,Formatting.Indented);
        }
    }
}