using GymManagement.BLL.ViewModels.TrainerViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        Task<IEnumerable<TrainerViewModel>> GetAllTrainer(CancellationToken ct = default);
        Task<bool> CreatTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default);
        Task<TrainerViewModel?> GetDetailsByIdAsync(int id, CancellationToken ct = default);
        Task<UpdateTrainerViewModel?> UpdateTrainerAsync(int id, CancellationToken ct = default);
        Task<bool> EditTrainerIdAsync(int id, UpdateTrainerViewModel model, CancellationToken ct = default);
        Task<bool> DeleteTrainerAsync(int id, CancellationToken ct = default);
    }
}
