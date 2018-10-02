using System;
using Gitloy.Services.Common.Communicator;
using Newtonsoft.Json;

namespace MessageSender.RequestResponds
{
    public abstract class Sender<TMessage> : ISender
        where TMessage : class, new()
    {
        public string Name => typeof(TMessage).Name;
        public string JsonExample { get; } = JsonConvert.SerializeObject(new TMessage());
        
        protected readonly ICommunicator Communicator;
        protected TMessage Message;

        protected Sender(ICommunicator communicator)
        {
            Communicator = communicator;
        }

        public bool JsonMessage(string jsonData)
        {
            try
            {
                Message = JsonConvert.DeserializeObject<TMessage>(jsonData);
                return true;
            }
            catch (Exception ex)
            {  
                Console.WriteLine("Can't parse data. Reason:" + ex.Message);
                return false;
            }
        }

        public abstract string Send();
    }
}