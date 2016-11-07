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
    public class PersonProductsController : ApiController
    {
        private Entities db = new Entities();


        // GET: api/PersonProducts
        //kad hoceda t i povuce sve, samo pokrenes getResponse
        public IQueryable<PersonProduct> GetPersonProducts()
        {
            return db.PersonProducts;
        }
        [HttpGet]
        [Route("api/PersonProducts/HazaKralj")]
        public List<JsonTest> NekiString()
        {
            JsonTest test = new JsonTest();
            test.Id = "Zivio tito";

            JsonTest test2 = new JsonTest();
            test2.Id = "Zivio2 tito";

            List<JsonTest> list = new List<JsonTest>();
            list.Add(test);
            list.Add(test2);


            return list;
        }

        // GET: api/PersonProducts/5
        [ResponseType(typeof(PersonProduct))]
        public IHttpActionResult GetPersonProduct(int id)
        {
            PersonProduct personProduct = db.PersonProducts.Find(id);
            if (personProduct == null)
            {
                return NotFound();
            }

            return Ok(personProduct);
        }

        // PUT: api/PersonProducts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPersonProduct(int id, PersonProduct personProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != personProduct.Id)
            {
                return BadRequest();
            }

            db.Entry(personProduct).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonProductExists(id))
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

        // POST: api/PersonProducts
        [ResponseType(typeof(PersonProduct))]
        public IHttpActionResult PostPersonProduct(PersonProduct personProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PersonProducts.Add(personProduct);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = personProduct.Id }, personProduct);
        }

        // DELETE: api/PersonProducts/5
        [ResponseType(typeof(PersonProduct))]
        public IHttpActionResult DeletePersonProduct(int id)
        {
            PersonProduct personProduct = db.PersonProducts.Find(id);
            if (personProduct == null)
            {
                return NotFound();
            }

            db.PersonProducts.Remove(personProduct);
            db.SaveChanges();

            return Ok(personProduct);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonProductExists(int id)
        {
            return db.PersonProducts.Count(e => e.Id == id) > 0;
        }
    }
}