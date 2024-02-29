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
    public class CarTblsController : Controller
    {
        private CarsDBEntitiesConString db = new CarsDBEntitiesConString();

        // GET: CarTbls
        public ActionResult Index()
        {
            var carTbls = db.CarTbls.Include(c => c.BrandTbl);
            return View(carTbls.ToList());
        }

        //[HandleError]
        // GET: CarTbls/Details/5
        public ActionResult Details(int? id)
        {
            //have to write code to send requests to the web api and retrieve info for our cars
            if (id <= 0)
            {
                // throw an exeption of our choice
                throw new Exception("Error happened");
            }
            if (id == null)
            {
                //  return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                throw new HttpException(400, "Sergeys 400 error");
            }
            //carTbl represents a record in the CarsTbl from the Cardb
            CarTbl carTbl = db.CarTbls.Find(id);
            if (carTbl == null)
            {
                throw new HttpException(404, "Sergeys 404 error");

              //  return HttpNotFound();
            }
            // lets start getting the data from the Api

           // string theValueToDisplay;
            HttpClient client = new HttpClient();
            client.BaseAddress =new Uri( "http://localhost:49856/api/");

            var httpResponseMessage = client.GetAsync("GetInfo/" + carTbl.CarName);
            //the complete url for the web api http://localhost:49856/api/GetInfo/Octavia
            httpResponseMessage.Wait();

            var responseMessageFromApi = httpResponseMessage.Result;    
            if (responseMessageFromApi.IsSuccessStatusCode)
            {
                var taskObjectRepresentingString = responseMessageFromApi.Content.ReadAsAsync<string>(); 
                taskObjectRepresentingString.Wait();

                // theValueToDisplay= taskObjectRepresentingString.Result;
                ViewBag.InfoFromAPI = taskObjectRepresentingString.Result;
            }
            else
            {
                ViewBag.InfoFromAPI = "Error from api";
                ModelState.AddModelError(string.Empty, "No API available");

            }


            return View(carTbl);
        }

        // GET: CarTbls/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.BrandTbls, "BrandID", "BrandName");
            return View();
        }

        // POST: CarTbls/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BrandID,CarName,TechnicalCharacteristics,Properties,Image,NoStock")] CarTbl carTbl, HttpPostedFileBase CarImageFileInput)
        {
            if (ModelState.IsValid)
            {
                if (CarImageFileInput != null)
                {
                    carTbl.Image = new byte[CarImageFileInput.ContentLength];
                    CarImageFileInput.InputStream.Read(carTbl.Image, 0, CarImageFileInput.ContentLength);
                }
                db.CarTbls.Add(carTbl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.BrandTbls, "BrandID", "BrandName", carTbl.BrandID);
            return View(carTbl);
        }

        // GET: CarTbls/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarTbl carTbl = db.CarTbls.Find(id);
            if (carTbl == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.BrandTbls, "BrandID", "BrandName", carTbl.BrandID);
            return View(carTbl);
        }

        // POST: CarTbls/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarID,BrandID,CarName,TechnicalCharacteristics,Properties,NoStock")] CarTbl carTbl, HttpPostedFileBase CarImageFileInput)
        {
            if (ModelState.IsValid)
            {
                if (CarImageFileInput != null)
                {
                    carTbl.Image = new byte[CarImageFileInput.ContentLength];
                    CarImageFileInput.InputStream.Read(carTbl.Image, 0, CarImageFileInput.ContentLength);
                }
                db.Entry(carTbl).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.BrandTbls, "BrandID", "BrandName", carTbl.BrandID);
            return View(carTbl);
        }

        // GET: CarTbls/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarTbl carTbl = db.CarTbls.Find(id);
            if (carTbl == null)
            {
                return HttpNotFound();
            }
            return View(carTbl);
        }

        // POST: CarTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CarTbl carTbl = db.CarTbls.Find(id);
            db.CarTbls.Remove(carTbl);
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
