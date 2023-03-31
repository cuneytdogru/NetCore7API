using Microsoft.EntityFrameworkCore;
using NetCore7API.Domain.Models;
using NetCore7API.Domain.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NetCore7API.EFCore.Context
{
    public class BlogContext : DbContext
    {
        public BlogContext(DbContextOptions<BlogContext> options) : base(options)
        {
        }

        public DbSet<Domain.Models.Post> Posts { get; set; }

        public DbSet<Domain.Models.Comment> Comments { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditedEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                    case EntityState.Deleted:
                        entry.Entity.ModifiedDate = DateTime.UtcNow;
                        break;
                }
            }

            foreach (var entry in ChangeTracker.Entries<ISoftDeletedEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.Deleted = true;
                        break;
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}