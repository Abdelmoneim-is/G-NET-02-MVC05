using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.SessionViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ISessionServicr
    {
        Task<IEnumerable<SessionViewModel>?> GetAllAsync(CancellationToken ct = default);
        Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default);

        Task<IEnumerable<GetTrainerBySelectViewModel>> GetTrainerAsync(CancellationToken ct = default);
        Task<IEnumerable<GetCategoryBySelectViewModel>> GetCategoryAsync(CancellationToken ct = default);
        Task<Result<SessionViewModel>> GetSessionWithCategoryAndTrainerByIdAsync(int id, CancellationToken ct = default);
        Task<Result<UpdateSessionViewModel>> GetSessionEditByIdAsync(int id, CancellationToken ct = default);
        Task<Result> UpdateSessionAsync(int id, UpdateSessionViewModel model ,CancellationToken ct = default);

        Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default);

    }
}
