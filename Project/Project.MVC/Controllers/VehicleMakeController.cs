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
    public class VehicleMakeController : Controller
    {
        private VehicleContext db = new VehicleContext();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            //StreamWriter tw = new StreamWriter("F:\\log.txt");
            //tw.Write(sortOrder);
            //tw.Close();
            //temSortOrder dodan zbog sortiranja po Abrv
            //ako se ne koristi u ovo slucaju
            //na dva uzastopna klika na oznaku Abrv sortirati ce jednom po Abrv jednom po Name
            string tempSortOrder = sortOrder;
            if (tempSortOrder == "abrv_ord")
                tempSortOrder = "";
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_ord" : "";
            ViewBag.AbrvSortParm = String.IsNullOrEmpty(tempSortOrder) ? "abrv_ord" : "";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var vehicleMakes = from vmk in db.VehicleMakes select vmk;

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicleMakes = vehicleMakes.Where(vmk => vmk.Name.ToUpper().Contains(searchString.ToUpper()) || vmk.Abrv.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder)
            {
                case "name_ord":
                    vehicleMakes = vehicleMakes.OrderBy(vmk => vmk.Name);
                    break;
                case "abrv_ord":
                    vehicleMakes = vehicleMakes.OrderBy(vmk => vmk.Abrv);
                    break;
                default:
                    vehicleMakes = vehicleMakes.OrderBy(vmk => vmk.Name);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            sortOrder = null;
            return View(vehicleMakes.ToPagedList(pageNumber, pageSize));
        }

        // GET: /VehicleMake/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleMake vehiclemake = db.VehicleMakes.Find(id);
            if (vehiclemake == null)
            {
                return HttpNotFound();
            }
            return View(vehiclemake);
        }

        // GET: /VehicleMake/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /VehicleMake/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,Abrv")] VehicleMake vehiclemake)
        {
            if (ModelState.IsValid)
            {
                db.VehicleMakes.Add(vehiclemake);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vehiclemake);
        }

        // GET: /VehicleMake/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleMake vehiclemake = db.VehicleMakes.Find(id);
            if (vehiclemake == null)
            {
                return HttpNotFound();
            }
            return View(vehiclemake);
        }

        // POST: /VehicleMake/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,Abrv")] VehicleMake vehiclemake)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehiclemake).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vehiclemake);
        }

        // GET: /VehicleMake/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleMake vehiclemake = db.VehicleMakes.Find(id);
            if (vehiclemake == null)
            {
                return HttpNotFound();
            }
            return View(vehiclemake);
        }

        // POST: /VehicleMake/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VehicleMake vehiclemake = db.VehicleMakes.Find(id);
            db.VehicleMakes.Remove(vehiclemake);
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
