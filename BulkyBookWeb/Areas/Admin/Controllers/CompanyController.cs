using BulkBook.DataAccess.Repository.IRepository;
using BulkBook.Models;
using BulkBook.Models.ViewModels;
using BulkBook.UtilityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkBookWeb.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvirmoment;

        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvirmoment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvirmoment = hostEnvirmoment;
        }

        public IActionResult Index()
        {
            return View();
        }

        //Get
        public IActionResult Upsert(int? id) //Id ile kategori ayrıntıları alınır görüntülenir ,? işareti ile nullable olduğunu belirttik.
        {
            //Product View Modeli içinden önce getall ile çağırıp içinden istedğimiz kısımları selectlist item ile seçtik
            Company company = new();
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList=CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(company);
            }
            else
            { 
                company= _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
                return View(company);
                //update product
            }

        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid)
            {
                 
                if (obj.Id==0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["success"] = "Company Created successfuly";

                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["success"] = "Company Created successfuly";
                }
              
                _unitOfWork.Save();
               
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult GetAll()
        {
            var companyList = _unitOfWork.Company.GetAll();
            return Json(new { data = companyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);
            if (obj==null)
            {
                return Json(new {success=false,message="Error while deleting"});
            }
          
            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted successfuly" });
        }


       
    }

}
