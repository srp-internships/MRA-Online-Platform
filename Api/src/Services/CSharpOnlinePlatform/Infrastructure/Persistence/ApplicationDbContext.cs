using Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> contextOptions) : base(contextOptions)
        {
        }

        public DbSet<T> GetEntities<T>() where T : class, IEntity
        {
            return base.Set<T>();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

        void IApplicationDbContext.Add<T>(T item)
        {
            Add(item);
        }

        void IApplicationDbContext.Delete<T>(T entity)
        {
            Remove(entity);
        }
    }
}
