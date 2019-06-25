﻿using Dictionary.Domain.Abstracts;

namespace Dictionary.Data.Entities
{
    public class TrWordType : DBaseEntity
    {
        public byte Id { get; set; }

        public string TdkText { get; set; }

        public string Text { get; set; }

        public override DBaseEntity Copy()
        {
            return this.MemberwiseClone() as TrWordType;
        }
    }
}
