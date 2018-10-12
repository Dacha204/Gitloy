using System;

namespace Gitloy.Services.FrontPortal.BusinessLogic.Core.Model
{
    public abstract class Model
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool DeleteFlag { get; set; }

        protected Model()
        {
            Guid = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            DeleteFlag = false;
        }
    }
}