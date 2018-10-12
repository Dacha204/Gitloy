using System;

namespace Gitloy.Services.FrontPortal.ViewModels
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        
        public Exception Exception { get; set; }
    }
}