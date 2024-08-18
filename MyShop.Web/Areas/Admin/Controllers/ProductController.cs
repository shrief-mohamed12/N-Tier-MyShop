using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Myshop.DataAccess.Implimentation;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;
using MyShop.Entities.ViewModels;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            var prodects = _unitOfWork.Product.GetAll();
            return Json(new {data = prodects });
        }
        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM Productvm, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if(file!= null )
                {
                    string filename =Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Product");
                    var ext = Path.GetExtension(file.FileName);
                    using (var fileStream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    Productvm.Product.Img = @"Images\Product\" + filename + ext;
                }
                _unitOfWork.Product.Add(Productvm.Product); 
                _unitOfWork.Complate();
                TempData["Craete"] = "Item has Craeted Successefully ";
                return RedirectToAction("Index");
            }
            else
            {
                return View(Productvm.Product);
            }
        }
        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = _unitOfWork.Product.GetFirstOrDefaults(x => x.Id == Id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product Product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(Product);
                _unitOfWork.Complate();
                TempData["Update"] = "Item has Updated Successefully ";

                return RedirectToAction("Index");
            }
            else
            {
                return View(Product);
            }
        }
        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = _unitOfWork.Product.GetFirstOrDefaults(x => x.Id == Id);
            return View(item);
        }
        [HttpPost]
        public IActionResult ConfirmDelete(int? Id)
        {
            var item = _unitOfWork.Product.GetFirstOrDefaults(x => x.Id == Id);
            if (item != null)
            {
                _unitOfWork.Product.Remove(item);
                _unitOfWork.Complate();
                TempData["Delete"] = "Item has Deleted Successefully ";
                return RedirectToAction("Index");
            }
            else
            {
                return NotFound();
            }

        }
    }
}
