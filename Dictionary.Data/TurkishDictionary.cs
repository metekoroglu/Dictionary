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

        public DbSet<TrWord> Words { get; set; }

        public DbSet<TrMeaning> Meanings { get; set; }

        public DbSet<TrSaying> Sayings { get; set; }

        public DbSet<TrWordType> WordTypes { get; set; }

        public DbSet<TrMeaningWordType> MeaningWordTypes { get; set; }
        

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
            modelBuilder.Entity<TrWordType>().HasData(new TrWordType
            {
                Id = 1,
                TdkText = "isim",
                Text = "Noun"
            }, new TrWordType
            {
                Id = 2,
                TdkText = "sıfat",
                Text = "Adjective"
            }, new TrWordType
            {
                Id = 3,
                TdkText = "zarf",
                Text = "Adverb"
            }, new TrWordType
            {
                Id = 4,
                TdkText = "zamir",
                Text = "Pronoun"
            }, new TrWordType
            {
                Id = 5,
                TdkText = "fiil",
                Text = "Verb"
            }, new TrWordType
            {
                Id = 6,
                TdkText = "mecaz",
                Text = "Metaphor"
            }, new TrWordType
            {
                Id = 7,
                TdkText = "hukuk",
                Text = "Law"
            }, new TrWordType
            {
                Id = 8,
                TdkText = "dil bilgisi",
                Text = "Language Information"
            }, new TrWordType
            {
                Id = 9,
                TdkText = "edat",
                Text = "Language Information"
            });

            modelBuilder.Entity<TrWord>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);
            modelBuilder.Entity<TrMeaning>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);
            modelBuilder.Entity<TrMeaningWordType>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);
            modelBuilder.Entity<TrSaying>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);
            modelBuilder.Entity<TrWordType>().HasQueryFilter(x => x.IsActive && !x.IsDeleted);

            modelBuilder.Entity<TrWordType>().Property(p => p.CreateDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<TrMeaning>().Property(p => p.CreateDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<TrMeaningWordType>().Property(p => p.CreateDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<TrSaying>().Property(p => p.CreateDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<TrWord>().Property(p => p.CreateDate).HasColumnType("smalldatetime");


            modelBuilder.Entity<TrWordType>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<TrMeaning>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<TrMeaningWordType>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<TrSaying>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");
            modelBuilder.Entity<TrWord>().Property(p => p.LastModifiedDate).HasColumnType("smalldatetime");

            base.OnModelCreating(modelBuilder);
        }
    }
  
}
