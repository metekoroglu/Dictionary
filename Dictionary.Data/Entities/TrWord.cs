using Dictionary.Domain.Abstracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dictionary.Data.Entities
{
    public class TrWord : DBaseEntity
    {
        public TrWord()
        {
            Meanings = new HashSet<TrMeaning>();
            Sayings = new HashSet<TrSaying>();
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

        public ICollection<TrMeaning> Meanings { get;private set; }

        public ICollection<TrSaying> Sayings { get;private set; }



        public override DBaseEntity Copy()
        {
            return this.MemberwiseClone() as TrWord;
        }
    }
}
