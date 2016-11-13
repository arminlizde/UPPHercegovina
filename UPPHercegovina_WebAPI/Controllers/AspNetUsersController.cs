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
    public class AspNetUsersController : ApiController
    {
        private Entities db = new Entities();

        [HttpGet]
        [Route("api/AspNetUsers/GetAverageMark/{UserId}")]
        public IHttpActionResult GetAverageMark(string UserId)
        {
            var list = db.PersonProducts.Where(p => p.Accepted == true)
                .Where(p => p.UserId == UserId)
                .Where(p => p.Rating != 0).ToList();

            var countRated = list.Count();

            float averageMark = 0;

            foreach (var item in list)
            {
                averageMark += item.Rating;
            }

            return Ok(averageMark / countRated);
        }

        // GET: api/AspNetUsers
        public IQueryable<AspNetUser> GetAspNetUsers()
        {
            return db.AspNetUsers;
        }

        [HttpGet]
        [Route("api/AspNetUsers/Login/{email}/{password}")]
        public AspNetUserViewModel Login(string email, string password)
        {
            AspNetUser user = db.AspNetUsers.Where(x => x.Email == email).Include(u => u.AspNetRoles).FirstOrDefault();

            if(user == null || user.AspNetRoles.FirstOrDefault().Name == "Super-Administrator" 
                || user.AspNetRoles.FirstOrDefault().Name == "Administrator")
                return new AspNetUserViewModel() { Error = true };

            return Util.LoginHelper.VerifyHashedPassword(user.PasswordHash, password)
                ? new AspNetUserViewModel()
                {
                    Id = user.Id,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    SecurityStamp = user.SecurityStamp,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IdentificationNumber = user.IdentificationNumber,
                    PictureUrl = user.PictureUrl,
                    Address = user.Address,
                    RegistrationDate = user.RegistrationDate,
                    Township = db.Townships.Find(user.TownshipId).Name,
                    Role = user.AspNetRoles.FirstOrDefault().Name,
                    Error = false
                }
                : new AspNetUserViewModel() { Error = true };

            //moze se ovo rijesiti ako saljemo negoLoginModel u njega strpamo password koji sadrzi taj znak i onda ce proci :)
            //puca ako ima na kraju passworda neki retardirani znak poput .
        }

        // GET: api/AspNetUsers/5
        [ResponseType(typeof(BuyerViewModel))]
        public IHttpActionResult GetAspNetUser(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers
                .Where(u => u.Id == id)
                .Include(u => u.Township)
                .Include(u => u.AspNetRoles).FirstOrDefault();

            if (aspNetUser == null || aspNetUser.Id == "")
                return Ok(new BuyerViewModel() { Error = true });

            return Ok(new BuyerViewModel() {

                Id = aspNetUser.Id,
                FullName = String.Format("{0} {1}", aspNetUser.FirstName, aspNetUser.LastName),
                Address = aspNetUser.Address,
                Email = aspNetUser.Email,
                Error = false,
                PhoneNumber = aspNetUser.PhoneNumber,
                RegistrationDate = aspNetUser.RegistrationDate.ToShortDateString(),
                Township = aspNetUser.Township.Name,
                Role = aspNetUser.AspNetRoles.FirstOrDefault().Name,
                NumberOfSuccededRequests = db.BuyerRequests.Where(r => r.BuyerId == id && r.Accepted == true && r.Status == false).Count(),
                NumberOfFailedRequests = db.BuyerRequests.Where(r => r.BuyerId == id && r.Accepted == false && r.Status == false).Count(),
                NumberOfBoughtProducts = GetNumberOfBoughtProducts(id)

            });
        }

        private int GetNumberOfBoughtProducts(string BuyerId)
        {
            var number = 0;
            var requests = db.BuyerRequests
                .Where(r => r.BuyerId == BuyerId && r.Status == false && r.Accepted == true)
                .Include(r => r.ReservedProducts)
                .ToList();

            foreach (var item in requests)
            {
                number += item.ReservedProducts.Count();
            }


            return number;
        }

        // PUT: api/AspNetUsers/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAspNetUser(string id, AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aspNetUser.Id)
            {
                return BadRequest();
            }

            db.Entry(aspNetUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(id))
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

        // POST: api/AspNetUsers
        [ResponseType(typeof(AspNetUser))]
        public IHttpActionResult PostAspNetUser(AspNetUser aspNetUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AspNetUsers.Add(aspNetUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserExists(aspNetUser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aspNetUser.Id }, aspNetUser);
        }

        // DELETE: api/AspNetUsers/5
        [ResponseType(typeof(AspNetUser))]
        public IHttpActionResult DeleteAspNetUser(string id)
        {
            AspNetUser aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspNetUser);
            db.SaveChanges();

            return Ok(aspNetUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AspNetUserExists(string id)
        {
            return db.AspNetUsers.Count(e => e.Id == id) > 0;
        }
    }
}