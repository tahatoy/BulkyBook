
using BulkBook.DataAccess;
using BulkBook.DataAccess.Repository.IRepository;
using BulkBook.Models;
using BulkBook.UtilityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkBookWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList=_unitOfWork.CoverType.GetAll();
            return View(objCoverTypeList);
        } 
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType obj)
        {
      

            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Add(obj);//Veritabanına kayıt
                _unitOfWork.Save();//Değişikleri kaydet
                TempData["success"] = "Cover Type created successfuly";
                return RedirectToAction("Index");
            }

            return View(obj); 

        }


        //GET
        public IActionResult? Edit(int? id)
        {
            if (id==null || id==0)
            {
                return NotFound();
            }

        
            var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u=>u.Id==id);
           
            if (coverTypeFromDbFirst==null)
            {
                return NotFound();
            }
            return View(coverTypeFromDbFirst);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType obj)
        {
           

            if (ModelState.IsValid)
            {
                _unitOfWork.CoverType.Update(obj);
                _unitOfWork.Save();
                TempData["success"] = "Cover Type updated successfuly";
                return RedirectToAction("Index");
            }

            return View(obj);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u=>u.Id==id);
         
            if (coverTypeFromDbFirst == null)
            {
                return NotFound();
            }
            return View(coverTypeFromDbFirst);
        }


        //POST
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
          var obj= _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);
            if (obj==null)
          {
              return NotFound();
          }
          _unitOfWork.CoverType.Remove(obj);
          _unitOfWork.Save();
          TempData["success"] = "Cover Type deleted successfuly";
            return RedirectToAction("Index");

        }


    }
}
