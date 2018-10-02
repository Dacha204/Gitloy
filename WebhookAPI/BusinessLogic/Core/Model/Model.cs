using System;

namespace Gitloy.Services.WebhookAPI.BusinessLogic.Core.Model
{
    public abstract class Model
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }

        public bool DeleteFlag { get; set; }

        protected Model()
        {
            Guid = Guid.NewGuid();
            DeleteFlag = false;
        }
    }
}