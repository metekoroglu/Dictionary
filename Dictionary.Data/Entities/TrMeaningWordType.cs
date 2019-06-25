using Dictionary.Domain.Abstracts;
using System;

namespace Dictionary.Data.Entities
{
    public class TrMeaningWordType : DBaseEntity
    {
        public Int64 Id { get; set; }

        public Int64 MeaningId { get; set; }

        public byte WordTypeId { get; set; }

        public virtual TrWordType WordType { get; set; }

        public virtual TrMeaning Meaning { get; set; }

        public override DBaseEntity Copy()
        {
            return this.MemberwiseClone() as TrMeaningWordType;
        }
    }
}
