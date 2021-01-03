using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class CategoryController : Controller
    {
        private ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var collection = _db.Category;

            return View(collection);
        }

        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }


        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }
    }
}
