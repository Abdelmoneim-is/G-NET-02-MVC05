using AutoMapper;
using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModel;
using  GymManagement.DAL.Data.Models;
using GymManagement.DAL.Data.Models.ENum;
using GymManagement.DAL.Repositories.Interfaces;


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class SessionService : ISessionServicr
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result> CreateSessionAsync(CreateSessionViewModel model, CancellationToken ct = default)
        {
            if (model.EndDate <= model.StartDate) return Result.Validation("End Date Must Be Greater Than StartDate");
            if(model.StartDate <= DateTime.Now) return Result.Validation("Start Date Must Be Greater Than Time Now");
            if(model.Capacity < 1 || model.Capacity > 25) return Result.Validation("Capcity Must Be Between 1 And 25");
            
            var trainer =await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category =await _unitOfWork.GetRepository<Category>().GetByIdAsync(model.CategoryId);
            if(category == null) return Result.NotFound("Category Not Found");

            var isValid = Enum.TryParse<Speciality>(category.CategoryName, true, out var categorySpeciality);
            if (!isValid || trainer.speciality != categorySpeciality) return Result.Validation("Can Not Create This Session To This Trainer");

            var result = _mapper.Map<CreateSessionViewModel, Session>(model);

            _unitOfWork.GetRepository<Session>().Add(result);
            var session = await _unitOfWork.SaveChangesAsync(ct);
            return session > 0 ? Result.Ok() : Result.Fail("Fail To Create This Session");
        }

        public async Task<Result> DeleteSessionAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id);
            if (session == null) return Result.NotFound("Session Not Found");
            if (session.EndDate >= DateTime.Now) return Result.Fail("Can Not Delete Session That Has Not Ended Yet");

            var Booking = await _unitOfWork.SessionRepository.GetCountBookedOfSlots(id , ct);
            if (Booking > 0) return Result.Fail("Can Not Delete Session Has Booked");
            _unitOfWork.GetRepository<Session>().Delete(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Can Not Delete This Session");
        }

        public async Task<IEnumerable<SessionViewModel>?> GetAllAsync(CancellationToken ct = default)
        {

            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithCategortAndTrainer(ct : ct);
            if (sessions == null || !sessions.Any()) return null;
            var mappedSessions = sessions.Select(m => new SessionViewModel
            {
                Id = m.Id,
                Capacity = m.Capacity,
                CategoryName = m.category.CategoryName,
                TrainerName = m.trainer.Name,
                Description = m.Description,
                EndDate = m.EndDate,
                StartDate = m.StartDate,

            });

            foreach (var session in mappedSessions)
            {
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountBookedOfSlots(session.Id, ct);
            }
            return mappedSessions;
 

        }

        public async Task<IEnumerable<GetCategoryBySelectViewModel>> GetCategoryAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Category>().GetAllAsync(ct : ct);
            return _mapper.Map<IEnumerable<GetCategoryBySelectViewModel>>(result);
        }

        public async Task<Result<UpdateSessionViewModel>> GetSessionEditByIdAsync(int id, CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id , ct);
            if (session == null) return Result<UpdateSessionViewModel>.NotFound("Session Not Found");
            if (session.StartDate <= DateTime.Now) return Result<UpdateSessionViewModel>.Fail("No Update Session Has Already Started");
            var booking = await _unitOfWork.SessionRepository.GetCountBookedOfSlots(id, ct);
            if (booking > 0) return Result<UpdateSessionViewModel>.Fail("Can Not Update Session Has Already Booking");
            var mapping = _mapper.Map<Session, UpdateSessionViewModel>(session);
            return Result<UpdateSessionViewModel>.Ok(mapping);
        }

        public async Task<Result<SessionViewModel>> GetSessionWithCategoryAndTrainerByIdAsync(int id, CancellationToken ct = default)
        {
            var result = await _unitOfWork.SessionRepository.GetSessionWithCategoryAndTrainerById(id, ct);
            if(result == null)
            {
                return Result<SessionViewModel>.NotFound("Session Not Fount");
            }
            else
            {
                var mappedSession = _mapper.Map<SessionViewModel>(result);
                mappedSession.AvailableSlots = mappedSession.Capacity - await _unitOfWork.SessionRepository.GetCountBookedOfSlots(id , ct);
                return Result<SessionViewModel>.Ok(mappedSession);
            }
        }

        public async Task<IEnumerable<GetTrainerBySelectViewModel>> GetTrainerAsync(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct: ct);
            return _mapper.Map<IEnumerable<GetTrainerBySelectViewModel>>(result);
        }

        public async Task<Result> UpdateSessionAsync(int id,UpdateSessionViewModel model , CancellationToken ct = default)
        {
            var session = await _unitOfWork.GetRepository<Session>().GetByIdAsync(id, ct);
            if (session == null) return Result.NotFound("Session Not Found");
            if (session.StartDate <= DateTime.Now) return Result.Fail("No Update Session Has Already Started");
            if (model.StartDate >= model.EndDate) return Result.Validation("Start Date Must Be Before End Date");
            if (model.StartDate <= DateTime.Now) return Result.Validation("Start Date Must Be In Future");

            var booking = await _unitOfWork.SessionRepository.GetCountBookedOfSlots(id, ct);
            if (booking > 0) return Result.Fail("Can Not Update Session Has Already Booking");

            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(model.TrainerId);
            if (trainer == null) return Result.NotFound("Trainer Not Found");

            var category = await _unitOfWork.GetRepository<Category>().GetByIdAsync(session.CategoryId);

            var isValid = Enum.TryParse<Speciality>(category?.CategoryName, true, out var categorySpeciality);
            if (!isValid || trainer.speciality != categorySpeciality) return Result.Validation("Can Not Create This Session To This Trainer");

            _mapper.Map(model, session);
            session.UpdatedAt =   DateTime.Now;
            _unitOfWork.GetRepository<Session>().Update(session);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0 ? Result.Ok() : Result.Fail("Failed To Update Session");
        }
    }
}
