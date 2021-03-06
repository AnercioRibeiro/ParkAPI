using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using System.IO;
using System.Threading.Tasks;

namespace ParkyWeb.Controllers
{
    [Authorize]
    public class NationalParksController : Controller
    {
        private readonly INationalParkRepository _npRepo;

        public NationalParksController(INationalParkRepository npRepo)
        {
            _npRepo = npRepo;
        }
        public IActionResult Index()
        {
            return View(new NationalPark() { });
        }
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Upsert(int? Id)
        {
            NationalPark objNationalPark = new NationalPark();
            //this will be true for Insert/Create
            if (Id == null)
            {
                return View(objNationalPark);
            }
            //Flow will come here for update
            objNationalPark = await _npRepo.GetAsync(SD.NationalParkAPIPath, Id.GetValueOrDefault(), HttpContext.Session.GetString("JWToken"));
            if (objNationalPark == null)
            {
                return NotFound();
            }
            return View(objNationalPark);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(NationalPark nationalPark)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[] p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using (var ms1 = new MemoryStream())
                        {
                            fs1.CopyTo(ms1);
                            p1 = ms1.ToArray();
                        }
                    }
                    nationalPark.Picture = p1;
                }
                else
                {
                    var objFromDB = await _npRepo.GetAsync(SD.NationalParkAPIPath, nationalPark.Id, HttpContext.Session.GetString("JWToken"));
                    nationalPark.Picture = objFromDB.Picture;
                }
                if (nationalPark.Id == 0)
                {
                    await _npRepo.CreateAsync(SD.NationalParkAPIPath, nationalPark, HttpContext.Session.GetString("JWToken"));
                }
                else
                {
                    await _npRepo.UpdateAsync(SD.NationalParkAPIPath+nationalPark.Id, nationalPark, HttpContext.Session.GetString("JWToken"));
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(nationalPark);
            }
        }

        public async Task<IActionResult> GetAllNationalPark()
        {
            return Json(new { data = await _npRepo.GetAllAsync(SD.NationalParkAPIPath, HttpContext.Session.GetString("JWToken")) });
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int Id)
        {
            var status = await _npRepo.DeleteAsync(SD.NationalParkAPIPath, Id, HttpContext.Session.GetString("JWToken"));
            if (status)
            {
                return Json(new { success = true, message = "Delete Successful" });
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }



    }
}
