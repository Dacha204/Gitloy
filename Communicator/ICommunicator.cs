using System;
using EasyNetQ;

namespace Gitloy.Services.Common.Communicator
{
    public interface ICommunicator : IDisposable
    {
        IBus Bus { get; }
        void Connect();
        void Disconnect();
        bool IsConnected { get; }
    }
}