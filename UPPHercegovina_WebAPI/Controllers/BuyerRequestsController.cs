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
    public class BuyerRequestsController : ApiController
    {
        private Entities db = new Entities();

        [HttpGet]
        [Route("api/BuyerRequests/User/{UserId}")]
        public IQueryable<BuyerRequestViewModel> GetBuyerRequests(string UserId)
        {
            var requests = db.BuyerRequests.Include(r => r.ReservedProducts.Select(rp => rp.PersonProduct))
                .Where(r => r.BuyerId == UserId).OrderByDescending(r => r.Status).ThenBy(r => r.DateOfPickUp).ToList();

            var list = new List<BuyerRequestViewModel>();

            foreach (var item in requests)
            {
                var request = new BuyerRequestViewModel();
                request.Id = item.Id;
                request.Accepted = item.Accepted;
                request.BuyerId = item.BuyerId;
                request.DateOfCreation = item.Date;
                request.Details = item.Details;
                request.PickedUp = item.PickedUp;
                request.PickUpDate = item.DateOfPickUp;
                request.Status = item.Status;
                request.Products = new List<PersonProductViewModel>();

                foreach (var product in item.ReservedProducts)
                {
                    var productViewModel = new PersonProductViewModel()
                    {
                        Id = product.Id,
                        Producer = String.Format("{0} {1}", product.PersonProduct.AspNetUser.FirstName, product.PersonProduct.AspNetUser.LastName),
                        Bruto = product.PersonProduct.Bruto,
                        Neto = product.PersonProduct.Neto,
                        ExpirationDate = product.PersonProduct.ExparationDate,
                        HarvestDate = product.PersonProduct.HarvestDate,
                        Quality = product.PersonProduct.Quality,
                        ValuePerKg = product.PersonProduct.Value,
                        ValueSum = product.PersonProduct.Value * Convert.ToDecimal(product.PersonProduct.Neto),//ovdje moze biti error u slucaju loseg decimala
                        Warehouse = product.PersonProduct.Warehouse1.Name,
                        Damaged = product.PersonProduct.Damaged,
                        Uregntly = product.PersonProduct.Urgently,
                        Product = product.PersonProduct.Product.Name
                    };

                    request.Products.Add(productViewModel);
                }

                list.Add(request);
            }

            return list.AsQueryable();
        }


        [HttpGet]
        [ResponseType(typeof(int))]
        [Route("api/BuyerRequests/NumberOfAcceptedReservations/{UserId}")]
        public IHttpActionResult GetNumberOfAcceptedReservations(string UserId)
        {
            var requests = db.BuyerRequests
                .Where(r => r.BuyerId == UserId && r.Status == true && r.Accepted == true).Count();

            return Ok(requests);
        }


        // GET: api/BuyerRequests/5
        [ResponseType(typeof(BuyerRequest))]
        public IHttpActionResult GetBuyerRequest(int id)
        {
            BuyerRequest buyerRequest = db.BuyerRequests.Find(id);
            if (buyerRequest == null)
            {
                return NotFound();
            }

            return Ok(buyerRequest);
        }

        // PUT: api/BuyerRequests/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBuyerRequest(int id, BuyerRequest buyerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != buyerRequest.Id)
            {
                return BadRequest();
            }

            db.Entry(buyerRequest).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BuyerRequestExists(id))
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

        // POST: api/BuyerRequests
        [ResponseType(typeof(BuyerRequest))]
        public IHttpActionResult PostBuyerRequest(BuyerRequestViewModel request)
        {
            BuyerRequest buyerRequest = new BuyerRequest();
            buyerRequest.BuyerId = request.BuyerId;
            buyerRequest.DateOfPickUp = request.PickUpDate;
            buyerRequest.Date = request.DateOfCreation;
            buyerRequest.Details = request.Details;
            buyerRequest.Accepted = request.Accepted;
            buyerRequest.Status = request.Status;
            buyerRequest.PickedUp = request.PickedUp;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BuyerRequests.Add(buyerRequest);
            db.SaveChanges();

            foreach (var item in request.Products)
            {
                ReservedProduct product = new ReservedProduct();
                product.BuyerRequestId = buyerRequest.Id;
                product.PersonProductId = item.Id;
                product.Status = true;
                product.Canceled = false;
                product.Details = buyerRequest.Details;

                db.ReservedProducts.Add(product);
                db.SaveChanges();
            }

            return CreatedAtRoute("DefaultApi", new { id = buyerRequest.Id }, buyerRequest);
        }

        // DELETE: api/BuyerRequests/5
        [ResponseType(typeof(BuyerRequest))]
        public IHttpActionResult DeleteBuyerRequest(int id)
        {
            BuyerRequest buyerRequest = db.BuyerRequests.Find(id);
            if (buyerRequest == null)
            {
                return NotFound();
            }

            db.BuyerRequests.Remove(buyerRequest);
            db.SaveChanges();

            return Ok(buyerRequest);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BuyerRequestExists(int id)
        {
            return db.BuyerRequests.Count(e => e.Id == id) > 0;
        }
    }
}