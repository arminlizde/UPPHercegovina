using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Helpers;
using UPPHercegovina.WebApplication.Models;
using UPPHercegovina.WebApplication.Extensions;
using UPPHercegovina.WebApplication.CustomFilters;
using PagedList;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class PostsController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        [AuthLog(Roles = "Super-Administrator, Administrator")]
        public ActionResult Index(int? page)
        {
            var postlist = context.Posts.OrderBy(p => p.PostDate).ThenBy(p => p.Author).ToList();
            var postViewModelList = Mapper.MapTo<List<PostListViewModel>, List<Post>>(postlist);

            int pageNumber = (page ?? 1);

            return postViewModelList != null ? View(postViewModelList.ToPagedList(pageNumber, _pageSize)) : View(new List<PostListViewModel>());
        }

        public ActionResult Details_old(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.TextWithHtml = post.Text;

            post.RelatedPost = post.GetRelatedPosts();
            post.LastPost = post.GetLastPosts();
            post.RecommendedPost = post.GetRecommendedPosts();

            SetGoogleMap();

            return View(post);
        }

        private void SetGoogleMap()
        {
            var user = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault();
            //This works only if we allow only registered users to visit 

            if(user != null)
                ViewBag.Location = context.PlaceOfResidences.Find(user.TownshipId);


            ViewBag.Markers = context.Warehouses1.ToList();
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            ViewBag.TextWithHtml = post.Text;

            post.RelatedPost = post.GetRelatedPosts();
            post.LastPost = post.GetLastPosts();
            post.RecommendedPost = post.GetRecommendedPosts();

            SetGoogleMap();

            List<Warehouse1> warehouses = context.Warehouses1.ToList();
            List<GeographicPosition> locations = new List<GeographicPosition>();

            foreach (var warehouse in warehouses)
            {
                locations.Add(warehouse.GeographicPosition);
            }

            ViewBag.lokacije = locations;

            return View(post);
        }

        [AuthLog(Roles = "Super-Administrator, Administrator")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(context.PostCategories
                 .OrderBy(c => c.Title), "Id", "Title");

            return View();
        }

        [AuthLog(Roles = "Super-Administrator, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Title,Text,PictureUrl,Recommended, CategoryId")] PostCreateViewModel post,
            FormCollection form, HttpPostedFileBase file, string text)
        {            
            if (ModelState.IsValid)
            {
                if (SavePictureToServer(file, post))
                {
                    var author = context.Users.Where(u => u.Email == User.Identity.Name).FirstOrDefault().Id;
                    context.Posts.Add(PostCreateViewModel.CreatePost(post, author));
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.message = "Please choose only Image file";
                    return RedirectToAction("Index");
                }
            }

            return View(post);
        }

        private bool SavePictureToServer(HttpPostedFileBase file, PostCreateViewModel post)
        {
            post.PictureUrl = string.Format("/Images/PostImg/Post-{0}-{1}-{2}-{3}.jpg", post.Title, post.CategoryId,
                    DateTime.Now.ToString("dd-M-yyyy"), Extensions.Extensions.GetRndNumber());

            PictureHelper pictureHelper = new PictureHelper(file, post.PictureUrl);

            return pictureHelper.Save();
        }
        private bool SavePictureToServer(HttpPostedFileBase file, Post post)
        {
            post.PictureUrl = string.Format("/Images/PostImg/Post-{0}-{1}-{2}-{3}.jpg", post.Title, post.CategoryId,
                    DateTime.Now.ToString("dd-M-yyyy"), Extensions.Extensions.GetRndNumber());

            PictureHelper pictureHelper = new PictureHelper(file, post.PictureUrl);

            return pictureHelper.Save();
        }

        [AuthLog(Roles = "Super-Administrator, Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            ViewBag.AuthorName = context.Users.Find(post.Author).GetDisplayName();

            var postTypesList = context.PostCategories.
                    OrderByDescending(p => p.Id == post.CategoryId).ThenBy(p => p.Title).ToList();

            ViewBag.ProductTypeId = new SelectList(postTypesList, "Id", "Title");

            if (String.IsNullOrWhiteSpace(post.PictureUrl))
            {
                post.PictureUrl = "/Images/Hercegovina_grb.png";
            }
            return View(post);
        }

        [AuthLog(Roles = "Super-Administrator, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Text,Author,PostDate,PictureUrl,Recommended,Status")] Post post,
            FormCollection form, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    if (!SavePictureToServer(file, post))
                    {
                        ViewBag.message = "Please choose only Image file";
                        return RedirectToAction("Index");
                    }
                }

                context.Entry(post).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        [AuthLog(Roles = "Super-Administrator, Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var post = context.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var post = context.Posts.Find(id);
            context.Posts.Remove(post);
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
