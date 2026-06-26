using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MemberService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> CreateMemberAsync(CreateMemberViewModel member, CancellationToken ct = default)
        {
            var emailExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == member.Email , ct) ;
            var PhoneNumberexist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == member.Phone , ct);

            if(emailExist || PhoneNumberexist)
            {
                return false;
            }

            var members = _mapper.Map<Member>(member);

           _unitOfWork.GetRepository<Member>().Add(members);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;

        }

        public async Task<bool> DeleteMemberAsync(int id, CancellationToken ct = default)
        {
            var existingMember = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (existingMember == null) return false;
            var checking = await _unitOfWork.GetRepository<Booking>().AnyAsync(x => x.MemberId == id && x.session.StartDate > DateTime.Now, ct);
            if (checking) return false;
            _unitOfWork.GetRepository<Member>().Delete(existingMember);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0; 
        }


        public async Task<IEnumerable<MemberViewModels>> GetAllMemberAsync(CancellationToken ct = default)
        {
            var members = await _unitOfWork.GetRepository<Member>().GetAllAsync(ct: ct);

            if (!members.Any()) return [];

            var memberViewModel = _mapper.Map<IEnumerable<Member>, IEnumerable<MemberViewModels>>(members);
            return memberViewModel;
        }

        public async Task<HealthRecordViewModel?> GetHealthRecordDeatailsByIdAsync(int id, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepository<HealthRecord>().FirstOrDefaultAsync(x => x.MemberId == id);
            if (record == null) return null;
            else
                return _mapper.Map<HealthRecord, HealthRecordViewModel>(record);
        }

        public async Task<MemberViewModels?> GetMemberDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            var member = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id , ct);
            if (member == null) return null;

            var model = _mapper.Map<Member, MemberViewModels>(member);
            var activeMemberShip = await _unitOfWork.GetRepository<MemberShip>().FirstOrDefaultAsync(x => x.MemberId == id && x.EndDate > DateTime.Now);

            if (activeMemberShip is not null)
            {
                var activePlan = await _unitOfWork.GetRepository<Plan>().GetByIdAsync(activeMemberShip.PlanId, ct);
                model.PlaneName = activePlan?.Name;
                model.MemberShipStartDate = activeMemberShip.CreatedAt.ToString();
                model.MemberShipEndDate = activeMemberShip.EndDate.ToString();
            }
            return model;
         }

        public async Task<MemberViewModelEdit?> GetMembersEditByIdAsync(int id, CancellationToken ct = default)
        {
            var record = await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (record == null) return null;
            else
                return _mapper.Map<Member, MemberViewModelEdit>(record);
        }

        public async Task<bool> UpdateMemberIdAsync(int id, MemberViewModelEdit model, CancellationToken ct = default)
        {
            var result =await _unitOfWork.GetRepository<Member>().GetByIdAsync(id, ct);
            if (result == null) return false;
            var EmailExist =await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Email == model.Email && x.Id != id);
            var PhoneExist = await _unitOfWork.GetRepository<Member>().AnyAsync(x => x.Phone == model.Phone && x.Id != id);

            if (EmailExist || PhoneExist) return false;
            
            _mapper.Map(model , result);
            result.UpdatedAt = DateTime.Now;

            _unitOfWork.GetRepository<Member>().Update(result);
            var member = await _unitOfWork.SaveChangesAsync(ct);
            return member > 0;
        }
    }
}
