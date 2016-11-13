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

        #region Producer

        [HttpGet]
        [Route("api/PersonProducts/TopPersonProducts")]
        public IQueryable<PersonProductViewModel> TopPersonProducts()
        {
            var personproducts = db.PersonProducts
                .Include(p => p.AspNetUser)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .Where(p => p.Accepted == true && p.Status == false)
                .OrderBy(p => p.ProductId).ToList();

            List<PersonProductViewModel> list = new List<PersonProductViewModel>();

            foreach (var item in personproducts)
            {
                var product = new PersonProductViewModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProducerId = item.UserId,
                    Producer = String.Format("{0} {1}", item.AspNetUser.FirstName, item.AspNetUser.LastName),
                    Bruto = item.Bruto,
                    Neto = item.Neto,
                    ExpirationDate = item.ExparationDate,
                    HarvestDate = item.HarvestDate,
                    Quality = item.Quality,
                    ValuePerKg = item.Value,
                    ValueSum = item.Value * Convert.ToDecimal(item.Neto),//ovdje moze biti error u slucaju loseg decimala
                    Warehouse = item.Warehouse1.Name,
                    Damaged = item.Damaged,
                    Uregntly = item.Urgently,
                    Product = item.Product.Name,
                    Status = item.Status


                };


                list.Add(product);
            }


            var query = from item in list
                        group item by item.ProductId into grup
                        orderby grup.Sum(x => Convert.ToInt32(x.Neto)) descending
                        select grup;

            var topFive = query.Take(5);

            List<int> bruto = new List<int>();

            List<TopThreeSUm> ttslist = new List<TopThreeSUm>();
            foreach (var item in topFive)
            {
                TopThreeSUm tts = new TopThreeSUm();
                tts.pid = item.Key;
                tts.brutosum = 0;
                tts.netosum = 0;
                foreach (var product in list)
                {
                    if (product.ProductId == item.Key)
                    {
                        tts.brutosum += Convert.ToInt32(product.Bruto);
                        tts.netosum += Convert.ToInt32(product.Neto);
                        ttslist.Add(tts);
                    }
                }
            }

            List<PersonProductViewModel> list2 = new List<PersonProductViewModel>();


            foreach (var item in ttslist)
            {
                for (int i = 0; i < list.Count(); i++)
                {
                    if (list[i].ProductId == item.pid)
                    {
                        if (!list2.Contains(list[i]))
                        {
                            list[i].Bruto = item.brutosum.ToString();
                            list[i].Neto = item.netosum.ToString();

                            list2.Add(list[i]);

                        }
                    }
                }
            }
            //list.Distinct();

            var topSoldProducts = new List<PersonProductViewModel>();

            foreach (var item in list2)
            {
                if (topSoldProducts.Count() == 0 || topSoldProducts.Where(p => p.ProductId == item.ProductId).Count() == 0)
                    topSoldProducts.Add(item);
                else
                {
                   var product = topSoldProducts.Where(p => p.ProductId == item.ProductId).FirstOrDefault();
                   product.Neto = (Convert.ToDecimal(product.Neto) + Convert.ToDecimal(item.Neto)).ToString();
                    product.Bruto = (Convert.ToDecimal(product.Bruto) + Convert.ToDecimal(item.Bruto)).ToString();
                }
            }

            return topSoldProducts != null && topSoldProducts.Count() > 0 ? topSoldProducts.AsQueryable()
                : new List<PersonProductViewModel>().AsQueryable();
        }

        [HttpGet]
        [Route("api/PersonProducts/AllPersonProducts/{userID}")]
        public IQueryable<PersonProductViewModel> AllPersonProducts(string userID)
        {
            var personproducts = db.PersonProducts
                .Include(p => p.AspNetUser)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .Where(p => p.UserId == userID)
                .OrderBy(p => p.ProductId).ToList();

            List<PersonProductViewModel> list = new List<PersonProductViewModel>();

            foreach (var item in personproducts)
            {

                var product = new PersonProductViewModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProducerId = item.UserId,
                    Producer = String.Format("{0} {1}", item.AspNetUser.FirstName, item.AspNetUser.LastName),
                    Bruto = item.Bruto,
                    Neto = item.Neto,
                    ExpirationDate = item.ExparationDate,
                    HarvestDate = item.HarvestDate,
                    Quality = item.Quality,
                    ValuePerKg = item.Value,
                    ValueSum = item.Value * Convert.ToDecimal(item.Neto),//ovdje moze biti error u slucaju loseg decimala
                    Warehouse = item.Warehouse1 != null ? item.Warehouse1.Name : "Nije skladišten" ,
                    Damaged = item.Damaged,
                    Uregntly = item.Urgently,
                    Product = item.Product.Name,
                    Status = item.Status,
                    Accepted = item.Accepted,
                    InWarehouse = item.InWarehouse

                };
                list.Add(product);
            }
            return list != null && list.Count() > 0 ? list.AsQueryable()
                : new List<PersonProductViewModel>().AsQueryable();
        }

        [Route("api/PersonProducts/GetAllPersonProducts/{userID}")]
        public IQueryable<PersonProductViewModel> GetPersonProductss(string userID)
        {
            var personproducts = db.PersonProducts
                .Include(p => p.AspNetUser)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .Where(p => p.Accepted == true && p.UserId == userID)
                .OrderBy(p => p.ProductId).ToList();

            List<PersonProductViewModel> list = new List<PersonProductViewModel>();

            foreach (var item in personproducts)
            {
                var product = new PersonProductViewModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProducerId = item.UserId,
                    Producer = String.Format("{0} {1}", item.AspNetUser.FirstName, item.AspNetUser.LastName),
                    Bruto = item.Bruto,
                    Neto = item.Neto,
                    ExpirationDate = item.ExparationDate,
                    HarvestDate = item.HarvestDate,
                    Quality = item.Quality,
                    ValuePerKg = item.Value,
                    ValueSum = item.Value * Convert.ToDecimal(item.Neto),//ovdje moze biti error u slucaju loseg decimala
                    Warehouse = item.Warehouse1.Name,
                    Damaged = item.Damaged,
                    Uregntly = item.Urgently,
                    Product = item.Product.Name,
                    Status = item.Status
                };
                list.Add(product);
            }
            return list != null && list.Count() > 0 ? list.AsQueryable()
                : new List<PersonProductViewModel>().AsQueryable();
        }

        [Route("api/PersonProducts/GetAllActiveProducts")]
        public IQueryable<PersonProductViewModel> GetAllActiveProducts()
        {
            var personproducts = db.PersonProducts
                .Include(p => p.AspNetUser)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .Where(p => p.InWarehouse == true)
                .OrderBy(p => p.ProductId).ToList();

            List<PersonProductViewModel> list = new List<PersonProductViewModel>();

            foreach (var item in personproducts)
            {
                var product = new PersonProductViewModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProducerId = item.UserId,
                    Producer = String.Format("{0} {1}", item.AspNetUser.FirstName, item.AspNetUser.LastName),
                    Bruto = item.Bruto,
                    Neto = item.Neto,
                    ExpirationDate = item.ExparationDate,
                    HarvestDate = item.HarvestDate,
                    Quality = item.Quality,
                    ValuePerKg = item.Value,
                    ValueSum = item.Value * Convert.ToDecimal(item.Neto),//ovdje moze biti error u slucaju loseg decimala
                    Warehouse = item.Warehouse1.Name,
                    Damaged = item.Damaged,
                    Uregntly = item.Urgently,
                    Product = item.Product.Name,
                    Status = item.Status,
                    WarehouseId = (int)item.Warehouse1Id
                    

                };


                list.Add(product);
            }

            return list != null && list.Count() > 0 ? list.AsQueryable()
                : new List<PersonProductViewModel>().AsQueryable();
        }

        [HttpGet]
        [Route("api/PersonProducts/GetAcceptedProducts/{userId}")]
        public IHttpActionResult GetAcceptedProducts(string userID)
        {
            return Ok(db.PersonProducts
                .Where(p => p.Accepted == true && p.Status == true && p.UserId == userID)
                .OrderBy(p => p.ProductId).Count());
        }
        //zamjenjena imena slucajno
        [HttpGet]
        [Route("api/PersonProducts/GetAcceptedProductsCount/{userId}")]
        public IQueryable<PersonProductViewModel> GetAcceptedProductsCount(string userID)
        {
            var personproducts = db.PersonProducts
                .Where(p => p.Accepted == true && p.Status == true && p.UserId == userID)
                .OrderBy(p => p.ProductId).ToList();

            List<PersonProductViewModel> list = new List<PersonProductViewModel>();

            foreach (var item in personproducts)
            {
                var product = new PersonProductViewModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProducerId = item.UserId,
                    Producer = String.Format("{0} {1}", item.AspNetUser.FirstName, item.AspNetUser.LastName),
                    Bruto = item.Bruto,
                    Neto = item.Neto,
                    ExpirationDate = item.ExparationDate,
                    HarvestDate = item.HarvestDate,
                    Quality = item.Quality,
                    ValuePerKg = item.Value,
                    ValueSum = item.Value * Convert.ToDecimal(item.Neto),//ovdje moze biti error u slucaju loseg decimala
                    Warehouse = item.Warehouse1.Name,
                    Damaged = item.Damaged,
                    Uregntly = item.Urgently,
                    Product = item.Product.Name,
                    Status = item.Status
                };
                list.Add(product);
            }

            return list != null && list.Count() > 0 ? list.AsQueryable()
                : new List<PersonProductViewModel>().AsQueryable();
        }

        #endregion


        // GET: api/PersonProducts
        public IQueryable<PersonProductViewModel> GetPersonProducts()
        {
            var personproducts = db.PersonProducts
                .Include(p => p.AspNetUser)
                .Include(p => p.Product)
                .Include(p => p.Warehouse1)
                .Where(p => p.Status == true && p.InWarehouse == true)
                .OrderBy(p => p.ProductId).ToList();

            var reservedproducts = db.ReservedProducts.Where(r => r.Status == true).Include(r => r.PersonProduct).ToList();

            foreach (var item in reservedproducts)
            {
                personproducts.Remove(item.PersonProduct);
            }


            List<PersonProductViewModel> list = new List<PersonProductViewModel>();

            foreach (var item in personproducts)
            {
                var product = new PersonProductViewModel()
                {
                    Id = item.Id,
                    ProductId = item.ProductId,
                    ProducerId = item.UserId,
                    Producer = String.Format("{0} {1}", item.AspNetUser.FirstName, item.AspNetUser.LastName),
                    Bruto = item.Bruto,
                    Neto = item.Neto,
                    ExpirationDate = item.ExparationDate,
                    HarvestDate = item.HarvestDate,
                    Quality = item.Quality,
                    ValuePerKg = item.Value,
                    ValueSum = item.Value * Convert.ToDecimal(item.Neto),//ovdje moze biti error u slucaju loseg decimala
                    Warehouse = item.Warehouse1.Name,
                    Damaged = item.Damaged,
                    Uregntly = item.Urgently,
                    Product = item.Product.Name,
                    Status = item.Status

                };
                list.Add(product);
            }

            return list != null && list.Count() > 0 ? list.AsQueryable()
                : new List<PersonProductViewModel>().AsQueryable();
        }

        [HttpGet]
        [ResponseType(typeof(PriceViewModel))]
        [Route("api/PersonProducts/Pricelist")]
        public IQueryable<PriceViewModel> PriceList()
        {
            var products = db.PersonProducts.Where(p => p.InWarehouse == true).OrderBy(p => p.ProductId).ToList();
            var prices = new List<PriceViewModel>();

            foreach (var item in products)
            {
                if (prices.Where(p => p.ProductName == item.Product.Name).Count() > 0)
                    continue;

                var price = new PriceViewModel();
                price.ProductName = item.Product.Name;
                price.MinValue = item.Value.ToString();
                price.MaxValue = item.Value.ToString();

                var sameProducts = products.Where(p => p.ProductId == item.ProductId).ToList();

                foreach (var product in sameProducts)
                {
                    if (price.MinValueDec > product.Value)
                        price.MinValue = product.Value.ToString();

                    if (price.MaxValueDec < product.Value)
                        price.MaxValue = product.Value.ToString();
                }

                price.AverageValue = ((price.MinValueDec + price.MaxValueDec) / 2).ToString();
                prices.Add(price);
            }            
            return prices.AsQueryable();
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

public class TopThreeSUm
{
    public int pid { get; set; }
    public int brutosum { get; set; }
    public int netosum { get; set; }
}