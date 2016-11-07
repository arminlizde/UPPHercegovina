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
    public class PostsController : ApiController
    {
        private Entities db = new Entities();

        // GET: api/Posts
        public IQueryable<PostViewModel> GetPosts()
        {
            var posts = db.Posts.OrderByDescending(p => p.PostDate)
                .ThenBy(p => p.Recommended)
                .ThenBy(p => p.CategoryId).Where(p => p.Status == true).ToList();

            List<PostViewModel> list = new List<PostViewModel>();

            foreach (var item in posts)
            {
                var author = db.AspNetUsers.Find(item.Author);

                var post = new PostViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    Text = item.Text,
                    AuthorName = String.Format("{0} {1}", author.FirstName, author.LastName),
                    PostDate = item.PostDate,
                    PictureUrl = item.PictureUrl,
                    Recommended = item.Recommended,
                    Picture = item.Picture
                };
                list.Add(post);
            }
            return list != null && list.Count() > 0 ? list.AsQueryable()
                : new List<PostViewModel>().AsQueryable();
        }

        // GET: api/Posts/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult GetPost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/Posts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPost(int id, Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.Id)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Posts
        [ResponseType(typeof(Post))]
        public IHttpActionResult PostPost(Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Posts.Add(post);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [ResponseType(typeof(Post))]
        public IHttpActionResult DeletePost(int id)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            db.Posts.Remove(post);
            db.SaveChanges();

            return Ok(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PostExists(int id)
        {
            return db.Posts.Count(e => e.Id == id) > 0;
        }
    }
}