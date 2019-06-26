using Dictionary.Domain.Abstracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Data.Entities
{
    public class Saying : DBaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        public int WordId { get; set; }

        [Required]
        public string Text { get; set; }
        
        public override DBaseEntity Copy()
        {
            return this.MemberwiseClone() as Saying;
        }
    }
}
