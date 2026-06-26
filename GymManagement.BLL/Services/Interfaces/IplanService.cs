using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlanViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IplanService
    {
        public  Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken ct = default);
        public Task<PlanViewModel?> GetDetailsByIdAsync(int id, CancellationToken ct = default);
        public Task<UpdatePlanViewModel?> UpdateByIdAsync(int id, CancellationToken ct = default);
        public Task<bool> EditPlanAsync(int id, UpdatePlanViewModel member, CancellationToken ct = default);
        public Task<bool> DeletePlanAsync(int id, CancellationToken ct = default);
    }
}
