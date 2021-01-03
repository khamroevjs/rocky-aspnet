using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        private ApplicationDbContext _db;

        public ApplicationTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var collection = _db.ApplicationType;

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
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Add(obj);
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
                return NotFound();

            var obj = await _db.ApplicationType.FindAsync(id);

            if (obj == null)
                return NotFound();

            return View(obj);
        }

        // POST - Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _db.ApplicationType.Update(obj);
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
                return NotFound();

            var obj = await _db.ApplicationType.FindAsync(id);

            if (obj is null)
                return NotFound();

            return View(obj);
        }

        // POST - Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(ApplicationType obj)
        {
            if (obj is null)
                return NotFound();

            _db.ApplicationType.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
