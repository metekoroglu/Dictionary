using Dictionary.Domain.Abstracts;
using System;

namespace Dictionary.Data.Entities
{
    public class MeaningWordType : DBaseEntity
    {
        public Int64 Id { get; set; }

        public Int64 MeaningId { get; set; }

        public byte WordTypeId { get; set; }

        public virtual WordType WordType { get; set; }

        public virtual Meaning Meaning { get; set; }

        public override DBaseEntity Copy()
        {
            return this.MemberwiseClone() as MeaningWordType;
        }
    }
}
