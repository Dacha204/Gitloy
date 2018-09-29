using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly List<IRequestRespond> _requests;
        private readonly CancellationToken _cancellationToken;
        
        public MessageSender(ICommunicator communicator, ILogger<MessageSender> logger, IApplicationLifetime applicationLifetime)
        {
            _communicator = communicator;
            _logger = logger;
            _cancellationToken = applicationLifetime.ApplicationStopping;
            _requests = new List<IRequestRespond>();
        }
        
        private void Setup()
        {
            _requests.Add(new RequestRespond<WorkerJobRequest, WorkerJobResponse>(_communicator));
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

                var request = _requests[index];
                ProcessRequest(request);
            }
        }
        private void PrintAvailableMessages()
        {
            Console.WriteLine("== Message Types ==");

            foreach (var request in _requests)
            {
                Console.WriteLine($"{_requests.IndexOf(request)}. {request.Name}");
            }
            
            Console.WriteLine();
        }
        
        private int ChooseIndex()
        {
            Console.WriteLine("Choose: ");
            
            var index = Console.ReadLine();
            bool isSuccessful = int.TryParse(index, out int indexInt);
            
            if (isSuccessful && (0 <= indexInt && indexInt < _requests.Count)) 
                return indexInt;
            
            Console.WriteLine($"Wrong input. Please enter number between 0-{_requests.Count-1}");
            return -1;
        }
        
        private void ProcessRequest(IRequestRespond request)
        {
            Console.WriteLine("Example:");
            Console.WriteLine(request.JsonExample);
            Console.WriteLine();
            Console.WriteLine("Enter json:");
            var isSuccessful = request.JsonRequest(Console.ReadLine());

            if (!isSuccessful)
                return;
            
            var response = request.SendRequest();
            Console.WriteLine("Response: ");
            Console.WriteLine(response);
        }


    }
}