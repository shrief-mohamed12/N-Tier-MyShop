﻿using Microsoft.AspNetCore.Mvc;
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
            var prodects = _unitOfWork.Product.GetAll(IncludeWord:"Category");
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
            ProductVM productVM = new ProductVM()
            {
                Product = _unitOfWork.Product.GetFirstOrDefaults(x => x.Id == Id),
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
        public IActionResult Edit(ProductVM Productvm, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Product");
                    var ext = Path.GetExtension(file.FileName);
                    if(Productvm.Product.Img != null)
                    {
                        var oldImg = Path.Combine(RootPath,Productvm.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImg))
                        {
                            System.IO.File.Delete(oldImg);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    Productvm.Product.Img = @"Images\Product\" + filename + ext;

                }

                _unitOfWork.Product.Update(Productvm.Product);
                _unitOfWork.Complate();
                TempData["Update"] = "Item has Updated Successefully ";
                return RedirectToAction("Index");
            }
            else
            {
                return View(Productvm.Product);
            }
        }
     
        [HttpDelete]
        public IActionResult ConfirmDelete(int? Id)
        {
            var item = _unitOfWork.Product.GetFirstOrDefaults(x => x.Id == Id);
            if (item != null)
            {
                var oldImg = Path.Combine(_webHostEnvironment.WebRootPath, item.Img.TrimStart('\\'));
                if (System.IO.File.Exists(oldImg))
                {
                    System.IO.File.Delete(oldImg);
                }
                _unitOfWork.Product.Remove(item);
                _unitOfWork.Complate();
                return Json(new { success = true, message = " The item has been deleted " });
            }
          
              return Json(new { success = false, message = " Error while deleting " });
        }
    }
}
