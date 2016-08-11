using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Project.Service.Models;
using Project.Service.DAL;
using PagedList;

namespace Project.MVC.Controllers
{
    public class VehicleModelController : Controller
    {
        private VehicleContext db = new VehicleContext();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            //StreamWriter tw = new StreamWriter("F:\\log.txt");
            //tw.Write(sortOrder);
            //tw.Close();
            string tempSortOrder = sortOrder;
            if (tempSortOrder == "abrv_ord" || tempSortOrder == "madeby_ord")
                tempSortOrder = "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_ord" : "";
            ViewBag.AbrvSortParm = String.IsNullOrEmpty(tempSortOrder) ? "abrv_ord" : "";
            ViewBag.MadeSortParm = String.IsNullOrEmpty(tempSortOrder) ? "madeby_ord" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var vehicleModels = from vmd in db.VehicleModels select vmd;

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleModels = vehicleModels.Where(vmd => vmd.Name.ToUpper().Contains(searchString.ToUpper()) || vmd.Abrv.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_ord":
                    vehicleModels = vehicleModels.OrderBy(vmd => vmd.Name);
                    break;
                case "abrv_ord":
                    vehicleModels = vehicleModels.OrderBy(vmd => vmd.Abrv);
                    break;
                case "madeby_ord":
                    vehicleModels = vehicleModels.OrderBy(vmd => vmd.VehicleMakeID);
                    break;
                default:
                    vehicleModels = vehicleModels.OrderBy(vmd => vmd.Name);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(vehicleModels.ToPagedList(pageNumber, pageSize));
        }

        // GET: /VehicleModel/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehiclemodel = db.VehicleModels.Find(id);
            if (vehiclemodel == null)
            {
                return HttpNotFound();
            }
            return View(vehiclemodel);
        }

        // GET: /VehicleModel/Create
        public ActionResult Create()
        {
            ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "Name");
            return View();
        }

        // POST: /VehicleModel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,VehicleMakeID,Name,Abrv")] VehicleModel vehiclemodel)
        {
            if (ModelState.IsValid)
            {
                db.VehicleModels.Add(vehiclemodel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "Name", vehiclemodel.VehicleMakeID);
            return View(vehiclemodel);
        }

        // GET: /VehicleModel/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehiclemodel = db.VehicleModels.Find(id);
            if (vehiclemodel == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "Name", vehiclemodel.VehicleMakeID);
            return View(vehiclemodel);
        }

        // POST: /VehicleModel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,VehicleMakeID,Name,Abrv")] VehicleModel vehiclemodel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehiclemodel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VehicleMakeID = new SelectList(db.VehicleMakes, "ID", "Name", vehiclemodel.VehicleMakeID);
            return View(vehiclemodel);
        }

        // GET: /VehicleModel/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehiclemodel = db.VehicleModels.Find(id);
            if (vehiclemodel == null)
            {
                return HttpNotFound();
            }
            return View(vehiclemodel);
        }

        // POST: /VehicleModel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleModel vehiclemodel = db.VehicleModels.Find(id);
            db.VehicleModels.Remove(vehiclemodel);
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
