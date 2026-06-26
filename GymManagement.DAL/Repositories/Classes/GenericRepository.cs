using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbcontext;
        private readonly DbSet<TEntity> _set;
        public GenericRepository(GymDbContext dbContext)
        {
            _dbcontext = dbContext;
            _set = dbContext.Set<TEntity>();

        }

        public void Add(TEntity entity)
        {
            _set.Add(entity);
        }



        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicat, CancellationToken ct = default)
        {
            return _set.AsNoTracking() .AnyAsync(predicat, ct);
        }

        public void Delete(TEntity entity )
        {
            _set.Remove(entity);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicat, bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query = tracking ? _set : _set.AsNoTracking();
            return await query.FirstOrDefaultAsync(predicat);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            IQueryable<TEntity> query  = tracking ? _set : _set.AsNoTracking();
            return await query.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _set.FindAsync(id, ct);
        }

        public async Task<int> GetCountAsync(Expression<Func<TEntity, bool>>? condition = null, CancellationToken ct = default)
        {
            return condition == null ? await _set.AsNoTracking().CountAsync(ct) : await _set.AsNoTracking().CountAsync(condition, ct);
        }

        public void  Update(TEntity entity)
        {
            _set.Update(entity);
        }
    }
}
