using CodePulse.API.Data;
using CodePulse.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Repositories.Implementation
{
    public class DbContextRepository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext dbContext;
        private readonly DbSet<T> dbSet;

        public DbContextRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = dbContext.Set<T>();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await dbSet.AddAsync(entity);
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<T?> UpdateAsync(Guid id, T entity)
        {
            var existing = await dbSet.FindAsync(id);

            if (existing is null)
            {
                return null;
            }

            // Copy scalar property values from the incoming entity onto the tracked one.
            dbContext.Entry(existing).CurrentValues.SetValues(entity);
            await dbContext.SaveChangesAsync();
            return existing;
        }

        public async Task<T?> DeleteAsync(Guid id)
        {
            var existing = await dbSet.FindAsync(id);

            if (existing is null)
            {
                return null;
            }

            dbSet.Remove(existing);
            await dbContext.SaveChangesAsync();
            return existing;
        }
    }
}
