using System.Collections.Generic;
using System.Threading.Tasks;
using ApiBD.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using ApiBD.Infrastructure.Data;

namespace ApiBD.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.AsNoTracking().ToListAsync();
        public async Task<T?> GetByIdAsync(object id) => await _dbSet.FindAsync(id);
        public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
        public void Update(T entity) 
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                _dbSet.Attach(entity);
            }
            entry.State = EntityState.Modified;
        }
        public void Delete(T entity) => _dbSet.Remove(entity);
        public async Task<int> SaveAsync() => await _context.SaveChangesAsync();
    }
}
