using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Myshop.DataAccess.Implimentation;
using MyShop.DataAccess.Data;
using MyShop.Entities.Models;
using MyShop.Entities.Repositories;


namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var date = _unitOfWork.Category.GetAll();
            return View(date);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(category);
                _unitOfWork.Complate();
                TempData["Craete"] = "Item has Craeted Successefully ";
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }
        [HttpGet]
        public IActionResult Edit(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = _unitOfWork.Category.GetFirstOrDefaults(x => x.Id == Id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(category);
                _unitOfWork.Complate();
                TempData["Update"] = "Item has Updated Successefully ";

                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }
        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id == null || Id == 0)
            {
                return NotFound();
            }
            var item = _unitOfWork.Category.GetFirstOrDefaults(x => x.Id == Id);
            return View(item);
        }
        [HttpPost]
        public IActionResult ConfirmDelete(int? Id)
        {
            var item = _unitOfWork.Category.GetFirstOrDefaults(x => x.Id == Id);
            if (item != null)
            {
                _unitOfWork.Category.Remove(item);
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
