using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface IMemberService
    {
        Task<IEnumerable<MemberViewModels>> GetAllMemberAsync(CancellationToken ct = default);
        Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default);

        Task<MemberViewModels?> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default);
        Task<HealthRecordViewModel?> GetHealthRecordDeatailsByIdAsync(int id, CancellationToken ct = default);
        Task<MemberViewModelEdit?> GetMembersEditByIdAsync(int id, CancellationToken ct = default);
        Task<bool> UpdateMemberIdAsync(int id, MemberViewModelEdit model, CancellationToken ct = default);
        Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default);
    }
}
