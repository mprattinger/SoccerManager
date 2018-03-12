using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SoccerManager.Data.DTO;
using SoccerManager.Data.DTO.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SoccerManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, UserRole, string>
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
            var user = ClaimsPrincipal.Current;
            if (user != null)
            {
                var identity = user.Identity;
                if (identity != null)
                {
                    uname = identity.Name;
                }
            }
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
    }
}
