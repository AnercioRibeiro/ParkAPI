using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModels;
using ParkyWeb.Repository.IRepository;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public TrailsController(INationalParkRepository npRepo, ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View(new Trail() { });
        }
        public async Task<IActionResult> Upsert(int? Id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

            TrailsViewModel objTrailVM = new TrailsViewModel()
            {
                NationParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trail = new Trail()
        };
            //this will be true for Insert/Create
            if (Id == null)
            {
                return View(objTrailVM);
            }
            //Flow will come here for update
            objTrailVM.Trail = await _trailRepo.GetAsync(SD.TrailAPIPath, Id.GetValueOrDefault());
            if (objTrailVM.Trail == null)
            {
                return NotFound();
            }
            return View(objTrailVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(TrailsViewModel trailVM)
        {
            if (ModelState.IsValid)
            {
                
                if (trailVM.Trail.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailAPIPath, trailVM.Trail);
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailAPIPath+trailVM.Trail.Id, trailVM.Trail);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);

                TrailsViewModel objTrailVM = new TrailsViewModel()
                {
                    NationParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trail = trailVM.Trail
                };
                return View(objTrailVM);
            }
        }

        public async Task<IActionResult> GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIPath) });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailAPIPath, Id);
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }



    }
}
