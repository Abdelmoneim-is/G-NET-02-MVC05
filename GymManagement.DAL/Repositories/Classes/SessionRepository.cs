using GymManagement.DAL.Data.DbContexts;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Classes
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Session?>> GetAllSessionsWithCategortAndTrainer(CancellationToken ct = default)
        {
            var result = _dbContext.Sessions.AsNoTracking().Include(x => x.category).Include(x => x.trainer);
            return await result.ToListAsync(ct);
        }

        public async Task<int> GetCountBookedOfSlots(int sessionid, CancellationToken ct = default)
        {
            return await _dbContext.Bookings.CountAsync(x => x.SessionId == sessionid);
        }

        public Task<Session?> GetSessionWithCategoryAndTrainerById(int id, CancellationToken ct = default)
        {
            var result = _dbContext.Sessions.AsNoTracking().Include(x => x.category).Include(x => x.trainer).FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }
    }
}
