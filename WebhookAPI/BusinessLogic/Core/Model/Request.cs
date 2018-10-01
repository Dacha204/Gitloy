using System;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model
{
    public enum RequestStatus
    {
        Requested,
        Finished
    }

    public enum RequestResultStatus
    {
        Pending,
        Successful,
        Failed
    }
    
    public class Request
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime Date { get; set; }
        
        public virtual Integration Integration { get; set; }
        public RequestStatus Status { get; set; }
        public RequestResultStatus ResultStatus { get; set; }
        public string ResultMessage { get; set; }
        public string ResultDetails { get; set; }

        public Request()
        {
            Status = RequestStatus.Requested;
            ResultStatus = RequestResultStatus.Pending;
            Guid = Guid.NewGuid();
            Date = DateTime.Now;
        }
    }
}