using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using Cars.Models;

namespace Cars.Controllers
{
    public class BrandTblsController : Controller
    {
        private CarsDBEntitiesConString db = new CarsDBEntitiesConString();

        // GET: BrandTbls
        public ActionResult Index()
        {
            return View(db.BrandTbls.ToList());
        }

        // GET: BrandTbls/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTbl brandTbl = db.BrandTbls.Find(id);
            if (brandTbl == null)
            {
                return HttpNotFound();
            }   
            ViewBag.Cars = brandTbl.CarTbls.ToList();

                return View(brandTbl);
        }

        // GET: BrandTbls/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BrandTbls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BrandID,BrandName")] BrandTbl brandTbl)
        {
            if (ModelState.IsValid)
            {
                db.BrandTbls.Add(brandTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brandTbl);
        }

        // GET: BrandTbls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTbl brandTbl = db.BrandTbls.Find(id);
            if (brandTbl == null)
            {
                return HttpNotFound();
            }
            return View(brandTbl);
        }

        // POST: BrandTbls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BrandID,BrandName")] BrandTbl brandTbl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brandTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brandTbl);
        }

        // GET: BrandTbls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BrandTbl brandTbl = db.BrandTbls.Find(id);
            if (brandTbl == null)
            {
                return HttpNotFound();
            }
            return View(brandTbl);
        }

        // POST: BrandTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BrandTbl brandTbl = db.BrandTbls.Find(id);
            var cars = brandTbl.CarTbls;
            if (cars != null)
            {
                foreach (var carTbl in cars)
                {
                    db.CarTbls.Remove(carTbl);
                }
            }
        db.BrandTbls.Remove(brandTbl);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
