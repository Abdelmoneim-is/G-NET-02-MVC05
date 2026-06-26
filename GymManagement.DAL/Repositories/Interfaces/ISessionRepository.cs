using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface ISessionRepository
    {
        Task<IEnumerable<Session?>> GetAllSessionsWithCategortAndTrainer(CancellationToken ct = default);
        Task<int> GetCountBookedOfSlots(int sessionid, CancellationToken ct = default);
        Task<Session?> GetSessionWithCategoryAndTrainerById(int id, CancellationToken ct = default);
    }
}
