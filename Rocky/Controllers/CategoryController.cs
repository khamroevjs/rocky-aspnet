using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
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

        #region Edit

        // GET - Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = await _db.Category.FindAsync(id);

            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        // POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Update(obj);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        #endregion

        // GET - Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null or 0)
            {
                return NotFound();
            }

            var obj = await _db.Category.FindAsync(id);

            if (obj is null)
                return NotFound();

            return View(obj);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var category = await _db.Category.FindAsync(id);

            if (category is null)
                return NotFound();

            _db.Category.Remove(category);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
