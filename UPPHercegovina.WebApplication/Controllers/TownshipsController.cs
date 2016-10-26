using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class TownshipsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index(string sortOrder)
        {
            var townships = from t in context.PlaceOfResidences
                           select t;

            return townships != null ? View(townships.ToList()) : View(new List<Township>());      
        }

        public ActionResult Details_old(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var township = context.PlaceOfResidences.Find(id);
            if (township == null)
            {
                return HttpNotFound();
            }
            return View(township);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var township = context.PlaceOfResidences.Find(id);
            if (township == null)
            {
                return HttpNotFound();
            }
            @ViewBag.lat = township.GeographicPosition.Latitude;
            @ViewBag.lng = township.GeographicPosition.Longitude;
            return View(township);
        }

        public ActionResult Create()
        {
            SetGoogleMap();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Entity,GeographicPosition")] Township township)
        {
            if (ModelState.IsValid)
            {
                context.PlaceOfResidences.Add(township);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(township);
        }

        private void SetGoogleMap()
        {
            var user = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            //This works only if we allow only registered users to visit 
            ViewBag.Location = context.PlaceOfResidences.Find(user.TownshipId);
            ViewBag.Markers = context.Warehouses1.ToList();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create_old([Bind(Include = "Id,Name,Entity,GeographicPosition")] Township township)
        {
            if (ModelState.IsValid)
            {
                context.PlaceOfResidences.Add(township);
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(township);
        }

        public ActionResult Edit_old(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var township = context.PlaceOfResidences.Find(id);
            if (township == null)
            {
                return HttpNotFound();
            }
            return View(township);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Entity,GeographicPosition")] Township township)
        {
            if (ModelState.IsValid)
            {
                context.Entry(township).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(township);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var township = context.PlaceOfResidences.Find(id);
            if (township == null)
            {
                return HttpNotFound();
            }
            @ViewBag.lat = township.GeographicPosition.Latitude;
            @ViewBag.lng = township.GeographicPosition.Longitude;
            return View(township);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var township = context.PlaceOfResidences.Find(id);
            if (township == null)
            {
                return HttpNotFound();
            }
            return View(township);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var township = context.PlaceOfResidences.Find(id);
            context.PlaceOfResidences.Remove(township);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
