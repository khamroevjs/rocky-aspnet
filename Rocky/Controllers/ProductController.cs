using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var collection = _db.Product;

            foreach (var item in collection)
            {
                item.Category = _db.Category.FirstOrDefault(x => x.Id == item.CategoryId);
                item.ApplicationType = _db.ApplicationType.FirstOrDefault(x => x.Id == item.ApplicationTypeId);
            }

            return View(collection);
        }

        //GET - Upsert
        public IActionResult Upsert(int? id)
        {
            var productVM = new ProductVM
            {
                Product = new Product(),
                CategorySelectList = _db.Category.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                ApplicationTypeSelectList = _db.ApplicationType.Select(x=>new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            // Create
            if (id is null)
            {
                return View(productVM);
            }

            // Edit
            productVM.Product = _db.Product.Find(id);
            if (productVM.Product is null)
                return NotFound();

            return View(productVM);
        }


        //POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    // Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _db.Product.Add(productVM.Product);
                }
                else
                {
                    // Editing
                    var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    // var objFromDb = _db.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
                    if (files.Any())
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                            System.IO.File.Delete(oldFile);

                        using var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create);
                        files[0].CopyTo(fileStream);

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }

                    _db.Product.Update(productVM.Product);
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _db.Category.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            productVM.ApplicationTypeSelectList = _db.ApplicationType.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View(productVM);
        }

        // GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id is null or 0)
                return NotFound();

            var product = _db.Product.Include(x => x.Category).Include(x=>x.ApplicationType).FirstOrDefault(x => x.Id == id);

            if (product is null)
                return NotFound();

            return View(product);
        }

        // POST - Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var product = _db.Product.Find(id);

            if (product is null)
                return NotFound();

            var image = Path.Combine(_webHostEnvironment.WebRootPath + WC.ImagePath, product.Image);
            if (System.IO.File.Exists(image))
                System.IO.File.Delete(image);


            _db.Product.Remove(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
