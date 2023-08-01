using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<T> GetEntities<T>() where T : class, IEntity;
        void Add<T>(T item) where T : class, IEntity;
        void Delete<T>(T entity) where T : class, IEntity;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
