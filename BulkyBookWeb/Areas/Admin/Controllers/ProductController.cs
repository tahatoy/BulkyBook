using BulkBook.DataAccess.Repository.IRepository;
using BulkBook.Models.ViewModels;
using BulkBook.UtilityLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkBookWeb.Controllers
{

    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvirmoment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvirmoment)
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
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
            };
            if (id == null || id == 0)
            {
                //create product
                //ViewBag.CategoryList=CategoryList;
                //ViewData["CoverTypeList"] = CoverTypeList;
                return View(productVM);
            }
            else
            { 
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
                return View(productVM);
                //update product
            }

        }
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile file) //IFormFile ile image vb. meedyaları yükleyebilme imkanı elde ettik.
        {
            if (ModelState.IsValid)
            {
                 string wwwrootPath = _hostEnvirmoment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwrootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl!=null)
                    {
                        var oldImagePath=Path.Combine(wwwrootPath,obj.Product.ImageUrl.TrimStart('\\') );
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    //Burada Product View Modeli altındaki Product classına ardından onun içindeki ImageURL kısmına atamamızı yaptık.
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;

                }

               
                if (obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product); //Veri tabanına kayıt
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                }
              
                _unitOfWork.Save();
                TempData["success"] = "Product Created successfuly";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            if (obj==null)
            {
                return Json(new {success=false,message="Error while deleting"});
            }
            var oldImagesPath=Path.Combine(_hostEnvirmoment.WebRootPath,obj.ImageUrl.Trim('\\'));
            if (System.IO.File.Exists(oldImagesPath))
            {
                System.IO.File.Delete(oldImagesPath);
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted successfuly" });
        }


        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = productList });
        }

        #endregion
    }

}
