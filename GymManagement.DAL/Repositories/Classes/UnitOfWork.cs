using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        private readonly GymDbContext _bContext;

        public UnitOfWork(GymDbContext gymDbContext , ISessionRepository sessionRepository)
        {
            _bContext = gymDbContext;
            SessionRepository = sessionRepository;
        }

        public ISessionRepository SessionRepository { get; }

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            var name = typeof(TEntity) .Name;
            if(_repositories.TryGetValue(name , out object? value))
            {
                return (IGenericRepository<TEntity>) value;
            }
            else
            {
                var repo = new GenericRepository<TEntity>(_bContext);
                _repositories[name] = repo;
                return repo;
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
           return await _bContext.SaveChangesAsync(ct);
        }
    }
}
