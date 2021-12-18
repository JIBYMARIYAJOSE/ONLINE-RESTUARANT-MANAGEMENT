using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using firstProjectWith_ASP.Data;
using firstProjectWith_ASP.Models;
using Microsoft.AspNetCore.Authorization;

namespace firstProjectWith_ASP.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {

        private readonly applicationDbContext _db;
            public CategoryController(applicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objList = _db.category;
            //NOTE-PASS anything To the controller make sure you fetch it in the veiw 
            return View(objList);
        }
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]//built in mechanism ...built in security..used with all the post methods
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(obj);
           
        }
        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.category.Find(id);
            if (obj == null)//record not found
            {
                return NotFound();
            }

            return View(obj);
        }
        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.category.Update(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.category.Find(id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _db.category.Find(id);
            if (obj == null)
            {//if record is null
                return NotFound();
            }
            _db.category.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");


        }


    }
}
