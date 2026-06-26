using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.PlanViewModel;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Classes;
using GymManagement.DAL.Repositories.Interfaces;
//using GymManagement.DbContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GymManagement.Controllers
{
    public class PlanController : Controller
    {
        //private readonly GymDbContext dbContext;

        //public PlanController()
        //{
        //    dbContext = new GymDbContext();
        //}

        private readonly IplanService _planService;
        public PlanController(IplanService planservice)
        {
            _planService = planservice;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planService.GetAllAsync(ct : ct);
            return View(plans);
        }

        //public async Task<IActionResult> Detail (int id , CancellationToken ct)
        //{
        //    var result = await _planService.GetDetailsByIdAsync(id , ct);
        //    if (result is null)
        //    {
        //        TempData["SuccessMessage"] = "Plan Not Found";
        //        return RedirectToAction(nameof(Index));

        //    }
        //    return View(result);
        //}
        public async Task<IActionResult> Detail(int id, CancellationToken ct)
        {
            var result = await _planService.GetDetailsByIdAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));

            }
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _planService.UpdateByIdAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "Plan Can Not Be Edit";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Edit ([FromRoute]int id , UpdatePlanViewModel model  , CancellationToken ct)
        {
            if(!ModelState.IsValid) return View(model);
            var result = await _planService.EditPlanAsync(id , model , ct);
            if (result)
                TempData["SuccessMessage"] = "Plan Updated Successfully";
            else
                TempData["ErrorMessage"] = "Plan Failed To Update";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost] 
        public async Task<IActionResult> Activate (int id , CancellationToken ct)
        {
            var result =await _planService.DeletePlanAsync(id , ct);
            if (result)
                TempData["SuccessMessage"] = "Plan Status Changed";
            else
                TempData["ErrorMessage"] = "Failed To Toggle Plan Status";
            return RedirectToAction(nameof(Index));
        }
    }
}
