using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModel;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerservice;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerservice = trainerService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var result = await _trainerservice.GetAllTrainer(ct);
            return View(result);
        }

        [HttpGet]

        public IActionResult Create () { return View(); }

        [HttpPost]
        public async Task<IActionResult> Create (CreateTrainerViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create));
            var result = await _trainerservice.CreatTrainerAsync(model, ct);
            if (result)
                TempData["SuccessMessage"] = "Trainer Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetDetails(int id, CancellationToken ct)
        {
            var result = await _trainerservice.GetDetailsByIdAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Failed To Get Trainer Details";
                return RedirectToAction(nameof(Index));
            }
            return View(result);

        }

        [HttpGet]
        public async Task<IActionResult> Edit (int id, CancellationToken ct)
        {
            var result = await _trainerservice.UpdateTrainerAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Not Found Trainer";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromRoute]int id, UpdateTrainerViewModel model, CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(model);
            var result = await _trainerservice.EditTrainerIdAsync(id, model, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";

            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Update Trainer";
            }
            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Delete (int id , CancellationToken ct)
        {
            var result = await _trainerservice.GetDetailsByIdAsync(id, ct);
            if (result == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed ([FromRoute]int id , CancellationToken ct)
        {
            var result = await _trainerservice.DeleteTrainerAsync(id, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
