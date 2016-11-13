using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using UPPHercegovina_WebAPI.Models;

namespace UPPHercegovina_WebAPI.Controllers
{
    public class DeliveriesController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Deliveries
        public IQueryable<Delivery> GetDeliveries()
        {
            return db.Deliveries;
        }

        // GET: api/Deliveries/5
        [ResponseType(typeof(Delivery))]
        public IHttpActionResult GetDelivery(int id)
        {
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return NotFound();
            }

            return Ok(delivery);
        }

        // PUT: api/Deliveries/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDelivery(int id, Delivery delivery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != delivery.Id)
            {
                return BadRequest();
            }

            db.Entry(delivery).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeliveryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Deliveries
        [ResponseType(typeof(Delivery))]
        public IHttpActionResult PostDelivery(Delivery delivery)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Deliveries.Add(delivery);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = delivery.Id }, delivery);
        }

        // DELETE: api/Deliveries/5
        [ResponseType(typeof(Delivery))]
        public IHttpActionResult DeleteDelivery(int id)
        {
            Delivery delivery = db.Deliveries.Find(id);
            if (delivery == null)
            {
                return NotFound();
            }

            db.Deliveries.Remove(delivery);
            db.SaveChanges();

            return Ok(delivery);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DeliveryExists(int id)
        {
            return db.Deliveries.Count(e => e.Id == id) > 0;
        }
    }
}