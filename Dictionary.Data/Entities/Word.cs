using Dictionary.Domain.Abstracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Data.Entities
{
    public class Word : DBaseEntity
    {
        public Word()
        {
            Meanings = new HashSet<Meaning>();
            Sayings = new HashSet<Saying>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TdkId { get; set; }

        public int MeaningNumber { get; set; }

        public bool isPlural { get; set; } = false;

        public bool isPrivate { get; set; } = false;

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string TextSimple { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Text { get; set; }

        public ICollection<Meaning> Meanings { get;private set; }

        public ICollection<Saying> Sayings { get;private set; }



        public override DBaseEntity Copy()
        {
            return this.MemberwiseClone() as Word;
        }
    }
}
