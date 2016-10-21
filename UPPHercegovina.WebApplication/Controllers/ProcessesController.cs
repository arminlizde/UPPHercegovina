using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
        [AuthLog(Roles = "Super-Administrator, Administrator")]
    public class ProcessesController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        public ActionResult DeliveryHistory(int? page)
        {
            int pageNumber = (page ?? 1);

            var deliveries = context.Deliveries.Include(d => d.Appointment)
                .OrderByDescending(d => d.Appointment.DeliveryDate).ThenByDescending(d => d.AppointmentId)
                .Include(d => d.PersonProduct.Product)
                .Where(d => d.Status == false)
                .Where(d => d.Appointment.Status == false).Where(d => d.Appointment.Delivered == true).ToPagedList(pageNumber, _pageSize);

            return View(deliveries);
        }

        public ActionResult DeliveryDetails(int? id)
        {
            var delivery = context.Deliveries.Where(d => d.Id == id)
                .Include(d => d.Appointment).Include(d => d.Appointment.User)
                .Include(d => d.PersonProduct).Include(d => d.PersonProduct.Product)
                .Include(d => d.PersonProduct.Warehouse1).FirstOrDefault();

            return id != null ? View(delivery) : View(new Delivery());
        }

        public ActionResult Processing(int? page)
        {
            int pageNumber = (page ?? 1);

            return View(context.PersonProducts.OrderByDescending(p => p.ProductId).ThenBy(p => p.UserId)
                        .Where(p => p.Status == true)
                        .Where(p => p.Accepted == false)
                        .Include(p => p.Warehouse1)
                        .Include(p => p.User)
                        .Include(p => p.Product).ToList().ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult AcceptProcess(int? id)
        {
            ViewBag.Warehouse1Id = new SelectList(context.Warehouses1, "Id", "Name");
            ViewBag.QualityId = new SelectList(context.Qualities, "Name", "Name");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            PersonProduct personProduct = context.PersonProducts.Where(p => p.Id == id)
                .Include(p => p.User)
                .Include(p => p.Field)
                .Include(p => p.Product).FirstOrDefault();

            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AcceptProcess([Bind(Include = "Id,ProductId,HarvestDate,Neto,Bruto,UserId,Warehouse1Id,ExparationDate,FieldId,Quality,Damaged,CircaValue,Urgently,Value")] PersonProduct personProduct)
        {

            ViewBag.QualityId = new SelectList(context.Qualities, "Name", "Name");


            ViewBag.FieldId = new SelectList(context.Fields.Where(f => f.OwnerId == context.Users
                .Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id), "Id", "Name");

            personProduct.Status = true;
            personProduct.Accepted = true;

            if (ModelState.IsValid)
            {
                context.Entry(personProduct).State = EntityState.Modified;
                context.SaveChanges();
                
            }
            //return View(personProduct);
            return RedirectToAction("Processing");
        }

        public ActionResult RejectProcess(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            PersonProduct personProduct = context.PersonProducts
                .Where(p => p.Id == id)
                .Include(p => p.Field)
                .Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .FirstOrDefault();

            if (personProduct == null)
            {
                return HttpNotFound();
            }
            return View(personProduct);
        }

        [HttpPost, ActionName("RejectProcess")]
        [ValidateAntiForgeryToken]
        public ActionResult RejectProcess(int id)
        {
            var personProduct = context.PersonProducts.Find(id);

            personProduct.Accepted = false;
            personProduct.Status = false;

            if (ModelState.IsValid)
            {
                context.Entry(personProduct).State = EntityState.Modified;
                context.SaveChanges();
            }
            return RedirectToAction("Processing");

            //return View(personProduct);
        }

        public ActionResult AppointmentProcess(int? page)
        {
            //da bude historija dozvoljena
            int pageNumber = (page ?? 1);

            return View(context.Appointments.Where(t => t.Status == true).Include(a => a.User).ToList().ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult AcceptAppointment(int? id)
        {
            var appointment = context.Appointments.Find(id);
            var deliveries = context.Deliveries.Where(d => d.AppointmentId == appointment.Id)
                .Include(d => d.PersonProduct).ToList();

            appointment.Delivered = true;
            appointment.Status = false;

            context.Entry(appointment).State = EntityState.Modified;
            context.SaveChanges();

            foreach (var delivery in deliveries)
            {
                delivery.Delivered = true;
                delivery.Status = false;

                delivery.PersonProduct.InWarehouse = true;

                context.Entry(delivery.PersonProduct).State = EntityState.Modified;
                context.SaveChanges();

                context.Entry(delivery).State = EntityState.Modified;
                context.SaveChanges();
            }

            return RedirectToAction("AppointmentProcess");
        }

        public ActionResult AppointmentDetails(int? id)
        {
            var deliveries = context.Deliveries
                .Include(D => D.Appointment).Where(D => D.AppointmentId == id).Include(a => a.Appointment.User).ToList();

            ViewBag.User = deliveries[0].Appointment.User.FullName;
            ViewBag.Details = deliveries[0].Appointment.Details;
            ViewBag.DeliveryDate = deliveries[0].Appointment.DeliveryDate;
            ViewBag.Delivered = deliveries[0].Appointment.Delivered;
            ViewBag.Status = deliveries[0].Appointment.Status;
            ViewBag.Canceled = deliveries[0].Appointment.Canceled;

            return View(deliveries);
        }


    }
}