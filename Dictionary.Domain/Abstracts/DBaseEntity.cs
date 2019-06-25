using Dictionary.Domain.Interfaces;
using System;

namespace Dictionary.Domain.Abstracts
{
    public abstract class DBaseEntity : IDictionaryBase
    {
        public DateTime CreateDate { get; set; }
        public Guid CreateUserID { get; set; } 

        public DateTime LastModifiedDate { get; set; } 
        public Guid LastModifiedUser { get; set; } 
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;

        public abstract DBaseEntity Copy();

        protected DBaseEntity()
        {
            IsActive = true;
            IsDeleted = false;
        }
    }
}
