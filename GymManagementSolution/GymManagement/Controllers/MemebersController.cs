using GymManagement.BLL.Services.Classes;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.DAL.Data.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace GymManagement.PL.Controllers
{
    public class MemebersController : Controller
    {
        private readonly IMemberService _memberService;

        public MemebersController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var members = await _memberService.GetAllMemberAsync(ct);

            return View(members);
        }

        #region Create Member
        //Get BaseUrl/Members/Create
        //Create -Show Empty Form

        [HttpGet]
        public IActionResult Create() => View();

        //Post BaseURL/Members/Create {Member}
        //CreateMember - Submit Form

        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel models, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(nameof(Create), models);
            var result = await _memberService.CreateMemberAsync(models, ct);

            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Create Member";

            return RedirectToAction(nameof(Index));
        }

        //Member Details
        public async Task<IActionResult> MemeberDetails(int id, CancellationToken ct)
        {
            //Get Member By Id
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
            //{
            //    //Get HealthRecord By Id
            //    //Check Is Null => Return Index With Message
            //    //Check Is Not Null => Return View Data
            //}

            #endregion


            
        }

        //Health Record Deatails
        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var result = await _memberService.GetHealthRecordDeatailsByIdAsync(id, ct);
            if(result == null)
            {
                TempData["ErrorMessage"] = "Health Record Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }

        #region EditMember
        [HttpGet]
        public async Task<IActionResult> MembersEdit(int id , CancellationToken ct)
        {
            var result =await _memberService.GetMembersEditByIdAsync(id, ct);
            if(result == null)
            {
                TempData["ErrorMessage"] = "Member Is Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(result);
        }


        [HttpPost]

        public async Task<IActionResult> MembersEdit([FromRoute]int id , MemberViewModelEdit model , CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await _memberService.UpdateMemberIdAsync(id, model,  ct);
            if (result)
                TempData["SuccessMessage"] = "Member Update Successfully";
            else
                TempData["ErrorMessage"] = "Member Failed";
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region DeleteMember
        [HttpGet]
        public async Task<IActionResult> DeleteMember(int id , CancellationToken ct)
        {
            var result = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if(result == null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed ([FromRoute] int id , CancellationToken ct)
        {
            var result = await _memberService.DeleteMemberAsync(id, ct);
            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully";
            else
                TempData["ErrorMessage"] = "Failed To Delete Member";
            return RedirectToAction(nameof(Index));

        }
        #endregion

    }
};
