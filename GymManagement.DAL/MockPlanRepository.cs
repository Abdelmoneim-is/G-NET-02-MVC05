using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL
{
    public class MockPlanRepository : IPlanRepository
    {
        public Task<int> AddAsync(Plan plan, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteAsync(Plan plan, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Plan>> GetAllAsync(bool tracking = false, CancellationToken ct = default)
        {
            List<Plan> plans = new List<Plan>()
            {
                new ()
                {
                    Name = "Abdo"
                }
            };
            return plans;
        }

        public Task<Plan?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateAsync(Plan plan, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
