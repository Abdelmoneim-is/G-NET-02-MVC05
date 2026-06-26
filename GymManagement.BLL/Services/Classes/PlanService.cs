using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.PlanViewModel;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class PlanService : IplanService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PlanService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> DeletePlanAsync(int id, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id , ct);
            if (plan == null) return false;
            if (plan.IsActive && await GetMemberShipWithPlan(id , ct))
                return false;
            plan.IsActive = !plan.IsActive;
            plan.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> EditPlanAsync(int id, UpdatePlanViewModel member, CancellationToken ct = default)
        {
            var plan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (plan is null) return false;
            if (await GetMemberShipWithPlan(id, ct)) return false;

            plan.Name = member.Name;
            plan.Description = member.Description;
            plan.Price = member.Price;
            plan.UpdatedAt = DateTime.Now;
            _unitOfWork.GetRepository<Plan>().Update(plan);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }

        public async Task<IEnumerable<PlanViewModel>> GetAllAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Plan>().GetAllAsync(ct: ct);
            if (!result.Any() ) return [];
            var plan = result.Select(x => new PlanViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Description = x.Description,
                DurationDays = x.DurationDays,
                IsActive = x.IsActive,

            });
            return plan;
        }

        public async Task<PlanViewModel?> GetDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (result is null) return null;
            else
                return new PlanViewModel()
                {
                    Name = result.Name,
                    Price = result.Price,
                    Description = result.Description,
                    DurationDays = result.DurationDays,
                    IsActive = result.IsActive,
                };

        }

        public async Task<UpdatePlanViewModel?> UpdateByIdAsync(int id, CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(id, ct);
            if (result == null || !result.IsActive) return null;
            if (await GetMemberShipWithPlan(id, ct))
                return null;
            else return new UpdatePlanViewModel()
            {
                Name = result.Name,
                Price = result.Price,
                DurationDays = result.DurationDays,
                Description = result.Description,
            };
            
        }

        private async Task<bool> GetMemberShipWithPlan (int planid , CancellationToken ct = default)
        {
            return await _unitOfWork.GetRepository<MemberShip>().AnyAsync(x => x.PlanId == planid && x.EndDate > DateTime.Now, ct);
        }
    }
}
