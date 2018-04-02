using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerManager.Data.DTO;
using SoccerManager.Data.DTO.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerManager.Data
{
    //public class ApplicationDbContext : IdentityDbContext<User, UserRole, string>
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(t => typeof(IAuditable).IsAssignableFrom(t.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType).Property<DateTime>("CreatedAt");
                modelBuilder.Entity(entityType.ClrType).Property<DateTime>("ChangedAt");
                modelBuilder.Entity(entityType.ClrType).Property<string>("CreatedBy");
                modelBuilder.Entity(entityType.ClrType).Property<string>("ChangedBy");
            }

            //modelBuilder.Entity<Squad>().HasKey(t => new { t.TeamId, t.PlayerId });
            //modelBuilder.Entity<Squad>().HasOne(pt => pt.Team).WithMany(p => p.Squads).HasForeignKey(x => x.TeamId);
            //modelBuilder.Entity<Squad>().HasOne(pt => pt.Player).WithMany(p => p.Squads).HasForeignKey(x => x.PlayerId);
        }

        public override int SaveChanges()
        {
            auditEntities();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            auditEntities();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void auditEntities()
        {
            var uname = String.Empty;
            //var user = _principal?.FindFirst(ClaimTypes.UserData);
            //if (user != null)
            //{
            //    var identity = user.Identity;
            //    if (identity != null)
            //    {
            //        uname = identity.Name;
            //    }
            //}
            if (String.IsNullOrEmpty(uname)) uname = "Unknow";

            foreach (EntityEntry<IAuditable> entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("CreatedAt").CurrentValue = DateTime.Now;
                    entry.Property("CreatedBy").CurrentValue = uname;
                }
                entry.Property("ChangedAt").CurrentValue = DateTime.Now;
                entry.Property("ChangedBy").CurrentValue = uname;
            }
        }

        public DbSet<Club> Clubs { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Squad> Squads { get; set; }
    }
}
