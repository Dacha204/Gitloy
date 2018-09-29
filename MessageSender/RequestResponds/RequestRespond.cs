using System;
using System.Runtime.InteropServices;
using Gitloy.Services.Common.Communicator;
using Newtonsoft.Json;

namespace MessageSender.RequestResponds
{
    public interface IRequestRespond
    {
        string Name { get; }
        string JsonExample { get; }
        bool JsonRequest(string jsonData);
        string SendRequest();
    }
    
    public class RequestRespond<TRequest, TResponse> : IRequestRespond
        where TRequest  : class, new()
        where TResponse : class, new()

    {
        private readonly ICommunicator _communicator;
        private TRequest _request;
        public string Name => typeof(TRequest).Name;
        public string JsonExample { get; } = JsonConvert.SerializeObject(new TRequest());

        public bool JsonRequest(string jsonData)
        {
            try
            {
                _request = JsonConvert.DeserializeObject<TRequest>(jsonData);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Can't parse data. Reason:" + ex.Message);
                return false;
            }
        }

        public string SendRequest()
        {
            var response = _communicator.Bus.Request<TRequest, TResponse>(_request);
            return JsonConvert.SerializeObject(response,Formatting.Indented);
        }

        public RequestRespond(ICommunicator communicator)
        {
            _communicator = communicator;
        }
    }
}