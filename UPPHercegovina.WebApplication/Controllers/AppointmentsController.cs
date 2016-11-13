using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    [AuthLog(Roles = "Korisnik")]
    public class AppointmentsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        public ActionResult Index(int? page)
        {
            var appointments = context.Appointments
                .Where(a => a.UserId == context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id)
                .Include(a => a.User)
                .OrderByDescending(ap => ap.Status).ThenBy(apo => apo.Id);

            int pageNumber = (page ?? 1);

            return View(appointments.ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult Create()
        {
            foreach (var item in GlobalData.Instance.forDelivery)
            {
                item.Product = context.Products.Find(item.ProductId);
            }
            ViewBag.PersonProducts = GlobalData.Instance.forDelivery;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Details,DeliveryDate")] Appointment appointment)
        {
            appointment.UserId = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
            appointment.Status = true;
            appointment.Canceled = false;
            appointment.Delivered = false;

            if (ModelState.IsValid && GlobalData.Instance.forDelivery.Count() > 0)
            {
                context.Appointments.Add(appointment);
                context.SaveChanges();

                foreach (var item in GlobalData.Instance.forDelivery)
                {
                    Delivery delivery = new Delivery();
                    delivery.AppointmentId = appointment.Id;
                    delivery.Status = true;
                    delivery.PersonProductId = item.Id;
                    delivery.Delivered = false;
                    context.Deliveries.Add(delivery);
                    context.SaveChanges();
                }

                GlobalData.Instance.forDelivery.Clear();

                return RedirectToAction("Index");
            }

            return View(appointment);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = context.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Details,DeliveryDate,UserId,Delivered,Canceled,Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                context.Entry(appointment).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appointment);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appointment appointment = context.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
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
