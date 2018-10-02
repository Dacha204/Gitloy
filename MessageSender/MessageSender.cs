using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Gitloy.BuildingBlocks.Messages.IntegrationEvents;
using Gitloy.BuildingBlocks.Messages.WorkerJob;
using Gitloy.Services.Common.Communicator;
using MessageSender.RequestResponds;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MessageSender
{
    public class MessageSender
    {
        private readonly ICommunicator _communicator;
        private readonly ILogger<MessageSender> _logger;
        private readonly List<ISender> _messageTypes;
        private readonly CancellationToken _cancellationToken;
        
        public MessageSender(ICommunicator communicator, ILogger<MessageSender> logger, IApplicationLifetime applicationLifetime)
        {
            _communicator = communicator;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
            _messageTypes = new List<ISender>();
        }
        
        private void Setup()
        {
            _messageTypes.Add(new RequestRespond<WorkerJobRequest, WorkerJobResponse>(_communicator));
            _messageTypes.Add(new Publisher<IntegrationCreateEvent>(_communicator));
            _messageTypes.Add(new Publisher<IntegrationDeleteEvent>(_communicator));
            _messageTypes.Add(new Publisher<IntegrationPingEvent>(_communicator));
            _messageTypes.Add(new Publisher<IntegrationPushEvent>(_communicator));
            _messageTypes.Add(new Publisher<IntegrationUpdateEvent>(_communicator));
        }

        public void Start()
        {
            Setup();
            Task.Run(() => MainLoop());
            _logger.LogInformation("Message sender started");
        }
        
        
        public void MainLoop()
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Press any key to send message.");
                Console.ReadKey();
                Console.Clear();
                
                PrintAvailableMessages();
                
                var index = ChooseIndex();
                if (index == -1)
                    continue;

                var request = _messageTypes[index];
                ProcessRequest(request);
            }
        }
        private void PrintAvailableMessages()
        {
            Console.WriteLine("== Message Types ==");

            foreach (var request in _messageTypes)
            {
                Console.WriteLine($"{_messageTypes.IndexOf(request)}. {request.Name}");
            }
            
            Console.WriteLine();
        }
        
        private int ChooseIndex()
        {
            Console.WriteLine("Choose: ");
            
            var index = Console.ReadLine();
            bool isSuccessful = int.TryParse(index, out int indexInt);
            
            if (isSuccessful && (0 <= indexInt && indexInt < _messageTypes.Count)) 
                return indexInt;
            
            Console.WriteLine($"Wrong input. Please enter number between 0-{_messageTypes.Count-1}");
            return -1;
        }
        
        private void ProcessRequest(ISender request)
        {
            Console.WriteLine("Example:");
            Console.WriteLine(request.JsonExample);
            Console.WriteLine();
            Console.WriteLine("Enter json:");
            var isSuccessful = request.JsonMessage(Console.ReadLine());

            if (!isSuccessful)
                return;
            
            var response = request.Send();
            Console.WriteLine("Response: ");
            Console.WriteLine(response);
        }


    }
}