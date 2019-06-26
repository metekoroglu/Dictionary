using Dictionary.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dictionary.Data
{
    public class TurkishDictionary : DbContext
    {
        public static string ConnectionString
        {
            get;
            set;
        }

        private readonly IHttpContextAccessor _contextAccessor;


        public TurkishDictionary(DbContextOptions options, IHttpContextAccessor contextAccessor) : base(options)
        {
            //Database.SetCommandTimeout(0);
            _contextAccessor = contextAccessor;
        }

        public TurkishDictionary(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public DbSet<Word> Words { get; set; }

        public DbSet<Meaning> Meanings { get; set; }

        public DbSet<Saying> Sayings { get; set; }

        public DbSet<WordType> WordTypes { get; set; }

        public DbSet<MeaningWordType> MeaningWordTypes { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            Save();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Save();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            Save();
            return base.SaveChanges();
        }

        private void Save()
        {
            var AddedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Added).ToList();


            AddedEntities.ForEach(E =>
            {
                E.Property("CreateDate").CurrentValue = DateTime.Now;
                E.Property("LastModifiedDate").CurrentValue = DateTime.Now;
                E.Property("IsDeleted").CurrentValue = false;
                E.Property("IsActive").CurrentValue = true;

            });

            var EditedEntities = ChangeTracker.Entries().Where(E => E.State == EntityState.Modified).ToList();

            EditedEntities.ForEach(E =>
            {
                E.Property("LastModifiedDate").CurrentValue = DateTime.Now;
            });
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WordType>().HasData(new WordType
            {
                Id = 1,
                TdkText = "isim",
                Text = "Noun"
            }, new WordType
            {
                Id = 2,
                TdkText = "sıfat",
                Text = "Adjective"
            }, new WordType
            {
                Id = 3,
                TdkText = "zarf",
                Text = "Adverb"
            }, new WordType
            {
                Id = 4,
                TdkText = "zamir",
                Text = "Pronoun"
            }, new WordType
            {
                Id = 5,
                TdkText = "fiil",
                Text = "Verb"
            }, new WordType
            {
                Id = 6,
                TdkText = "mecaz",
                Text = "Metaphor"
            }, new WordType
            {
                Id = 7,
                TdkText = "hukuk",
                Text = "Law"
            }, new WordType
            {
                Id = 8,
                TdkText = "dil bilgisi",
                Text = "Language Information"
            }, new WordType
            {
                Id = 9,
                TdkText = "edat",
                Text = "Language Information"
            });

            modelBuilder.Entity<Word>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);
            modelBuilder.Entity<Meaning>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);
            modelBuilder.Entity<MeaningWordType>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);
            modelBuilder.Entity<Saying>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);
            modelBuilder.Entity<WordType>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);

            modelBuilder.Entity<WordType>().Property(p => p.CreateDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<Meaning>().Property(p => p.CreateDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<MeaningWordType>().Property(p => p.CreateDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<Saying>().Property(p => p.CreateDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<Word>().Property(p => p.CreateDate).HasColumnType("smalldatetime");


            modelBuilder.Entity<WordType>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<Meaning>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<MeaningWordType>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<Saying>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<Word>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");

            base.OnModelCreating(modelBuilder);
        }
    }
  
}
