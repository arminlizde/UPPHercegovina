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
    public class WarehousesController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Warehouses
        [HttpGet]
        [Route("api/Warehouses/GetAll")]
        public IQueryable<WarehouseViewModel> GetAll()
        {
            var list =  db.Warehouse1.Where(x=>x.Status == true).ToList();
            List<WarehouseViewModel> warehouses = new List<WarehouseViewModel>();


            foreach (var item in list)
            {
                var warehouse = new WarehouseViewModel()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                warehouses.Add(warehouse);
            }

            return warehouses != null && warehouses.Count() > 0 ? warehouses.AsQueryable()
                : new List<WarehouseViewModel>().AsQueryable();
        }

        // GET: api/Warehouses/5
        [ResponseType(typeof(Warehouse1))]
        public IHttpActionResult GetWarehouse1(int id)
        {
            Warehouse1 warehouse1 = db.Warehouse1.Find(id);
            if (warehouse1 == null)
            {
                return NotFound();
            }

            return Ok(warehouse1);
        }

        // PUT: api/Warehouses/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutWarehouse1(int id, Warehouse1 warehouse1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != warehouse1.Id)
            {
                return BadRequest();
            }

            db.Entry(warehouse1).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Warehouse1Exists(id))
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

        // POST: api/Warehouses
        [ResponseType(typeof(Warehouse1))]
        public IHttpActionResult PostWarehouse1(Warehouse1 warehouse1)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Warehouse1.Add(warehouse1);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = warehouse1.Id }, warehouse1);
        }

        // DELETE: api/Warehouses/5
        [ResponseType(typeof(Warehouse1))]
        public IHttpActionResult DeleteWarehouse1(int id)
        {
            Warehouse1 warehouse1 = db.Warehouse1.Find(id);
            if (warehouse1 == null)
            {
                return NotFound();
            }

            db.Warehouse1.Remove(warehouse1);
            db.SaveChanges();

            return Ok(warehouse1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool Warehouse1Exists(int id)
        {
            return db.Warehouse1.Count(e => e.Id == id) > 0;
        }
    }
}