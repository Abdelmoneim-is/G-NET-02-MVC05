using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionServicr _sessionServicr;

        public SessionController(ISessionServicr sessionServicr)
        {
            _sessionServicr = sessionServicr;
        }

        public async Task<IActionResult> Index (CancellationToken ct)
        {
            var result = await _sessionServicr.GetAllAsync (ct);
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Create ()
        {
            await DropDownListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create (CreateSessionViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await DropDownListAsync();
                return View(model);
            }
            var result = await _sessionServicr.CreateSessionAsync(model, ct);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session Created";
                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                await DropDownListAsync();
                return View(model);
            }
        }

        public async Task DropDownListAsync()
        {
            ViewBag.Trainer = new SelectList(await _sessionServicr.GetTrainerAsync(), "Id", "Name");
            ViewBag.Category = new SelectList(await _sessionServicr.GetCategoryAsync(), "Id", "CategoryName");
        }

        [HttpGet]
        public async Task<IActionResult> Details (int id , CancellationToken ct)
        {
            var result = await _sessionServicr.GetSessionWithCategoryAndTrainerByIdAsync(id, ct);
            if (result.Success)
            {
                return View(result.Value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit (int id , CancellationToken ct)
        {
            var result = await _sessionServicr.GetSessionEditByIdAsync(id, ct);
            if (result.Success)
            {
                ViewBag.Trainer = new SelectList(await _sessionServicr.GetTrainerAsync(), "Id", "Name");
                return View(result.Value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit ( int id,UpdateSessionViewModel model , CancellationToken ct)
        {
            if(!ModelState.IsValid)
            {
                ViewBag.Trainer = new SelectList(await _sessionServicr.GetTrainerAsync(), "Id", "Name");
                return View(model);
            }

            var result = await _sessionServicr.UpdateSessionAsync(id, model, ct);
            if(result.Success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            } 
            else
            {
                ViewBag.Trainer = new SelectList(await _sessionServicr.GetTrainerAsync(), "Id", "Name");
                TempData["ErrorMessage"] = result.error;

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete (int id , CancellationToken ct)
        {
            var result = await _sessionServicr.GetSessionWithCategoryAndTrainerByIdAsync(id, ct);
            if(result.Success)
            {
                return View(result.Value);
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        } 

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed (int id , CancellationToken ct)
        {
            var result = await _sessionServicr.DeleteSessionAsync(id, ct);
            if(result.Success)
            {
                TempData["SuccessMessage"] = "Session Deleted";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = result.error;
                return RedirectToAction(nameof(Index));
            }
        }
    }

        

}
