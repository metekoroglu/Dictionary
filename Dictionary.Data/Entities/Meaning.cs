using Dictionary.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Data.Entities
{
    public class Meaning : DBaseEntity
    {
        public Meaning()
        {
            WordTypes = new HashSet<MeaningWordType>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        public int WordId { get; set; }

        [Required]
        public string MeaningText { get; set; }

        public bool isVerb { get; set; } = false;
        
        public  ICollection<MeaningWordType> WordTypes { get; set; }

        public override DBaseEntity Copy()
        {
            return this.MemberwiseClone() as Meaning;
        }
    }
}
