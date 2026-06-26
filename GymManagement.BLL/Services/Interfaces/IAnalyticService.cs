using GymManagement.BLL.Common;
using GymManagement.BLL.ViewModels.AnalyticViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IAnalyticService
    {
        Task<AnalyticViewModel> GetAllAsync(CancellationToken ct = default);
    }
}
