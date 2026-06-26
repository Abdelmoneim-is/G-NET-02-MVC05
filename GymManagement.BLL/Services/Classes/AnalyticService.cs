using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AnalyticViewModel;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class AnalyticService : IAnalyticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AnalyticService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AnalyticViewModel> GetAllAsync(CancellationToken ct = default)
        {
            var now = DateTime.Now;
            var totalMember = await _unitOfWork.GetRepository<Member>().GetCountAsync(ct : ct);
            var totalTrainers = await _unitOfWork.GetRepository<Trainer>().GetCountAsync(ct : ct);
            var upComingSessions = await _unitOfWork.GetRepository<Session>().GetCountAsync(s => s.StartDate > now);
            var ongoingSessions = await _unitOfWork.GetRepository<Session>().GetCountAsync(s => s.StartDate <= now && s.EndDate >= now);
            var completedSessions = await _unitOfWork.GetRepository<Session>().GetCountAsync(s => s.EndDate < now);
            var activeMemberShip = await _unitOfWork.GetRepository<MemberShip>().GetCountAsync(m => m.EndDate > now , ct) ;

            return new AnalyticViewModel()
            {
                TotalMembers = totalMember,
                Trainers = totalTrainers,
                UpComingSessions = upComingSessions,
                CompletedSessions = completedSessions,
                ActiveMembers = activeMemberShip,
                OngoingSessions = ongoingSessions
            };
        }
    }
}
