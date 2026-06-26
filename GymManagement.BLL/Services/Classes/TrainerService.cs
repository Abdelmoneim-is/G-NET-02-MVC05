using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModel;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CreatTrainerAsync(CreateTrainerViewModel model, CancellationToken ct = default)
        {
            var phonExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.Phone, ct);
            var EmailExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email, ct);
            if (phonExist || EmailExist) return false;
            var trainer = new Trainer()
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email,
                DateOfBirth = model.DateOfBirth,
                gender = model.Gender,

                Address = new Address()
                {
                    BuildingNumber = model.BuildingNumber,
                    Street = model.Street,
                    City = model.City,
                },
                speciality = model.Speciality,

            };
            _unitOfWork.GetRepository<Trainer>().Add(trainer);
            var result = await _unitOfWork.SaveChangesAsync(ct);
            return result > 0;
        }

        public async Task<bool> DeleteTrainerAsync(int id, CancellationToken ct = default)
        {
            var result =await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            if(result is null) return false;
            var CheckingBooking = await _unitOfWork.GetRepository<Booking>().AnyAsync(x => x.SessionId == id && x.CreatedAt > DateTime.Now , ct);
            if (CheckingBooking) return false;
            _unitOfWork.GetRepository<Trainer>().Delete( result);
            var trainer = await _unitOfWork.SaveChangesAsync(ct);
            return trainer > 0;
        }

        public async Task<bool> EditTrainerIdAsync(int id, UpdateTrainerViewModel model, CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            if (result == null) return false;
            var emailExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Phone == model.PhoneNumber && x.Id != id);
            var phoneExist = await _unitOfWork.GetRepository<Trainer>().AnyAsync(x => x.Email == model.Email && x.Id != id);
            if (emailExist || phoneExist) return false;

            result.Email = model.Email;
            result.Name = model.Name;
            result.Phone = model.PhoneNumber;
            result.Address.BuildingNumber = model.BuildingNumber;
            result.Address.Street = model.Street;
            result.Address.City = model.City;
            result.speciality = model.Specialitize;

            _unitOfWork.GetRepository<Trainer>().Update(result);
            var trainer = await _unitOfWork.SaveChangesAsync(ct);
            return trainer > 0;

        }
        

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainer(CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetAllAsync(ct : ct);
            if (!result.Any()) return [];

            var trainer = result.Select(m => new TrainerViewModel()
            {
                Id = m.Id,
                Name = m.Name,
                Email = m.Email,
                Phone = m.Phone,
                Specialization = m.speciality.ToString()
            });
            return trainer;

        }

        public async Task<TrainerViewModel?> GetDetailsByIdAsync(int id, CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id, ct);
            if (result is null) return null;
            var trainer = new TrainerViewModel()
            {
                Name = result.Name,
                Specialization = result.speciality.ToString(),
                Email = result.Email,
                Phone = result.Phone,
                DateOfBirth = result.DateOfBirth.ToString(),
                Address = $"{result.Address.BuildingNumber} - {result.Address.Street} - {result.Address.City}",
                Gender = result.gender.ToString(),
                
            };
            return trainer;
        }

        public async Task<UpdateTrainerViewModel?> UpdateTrainerAsync(int id, CancellationToken ct = default)
        {
            var result = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id ,ct);
            if (result is null) return null;
            var trainer = new UpdateTrainerViewModel()
            {
                Name = result.Name,
                Email = result.Email,
                PhoneNumber = result.Phone,
                BuildingNumber = result.Address.BuildingNumber,
                City = result.Address.City.ToString(),
                Street = result.Address.Street.ToString(),
                Specialitize = result.speciality

            };
            return trainer;
        }
    }
}
