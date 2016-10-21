using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;

        public ActionResult Index()
        {
             return RedirectToAction("News");
        }

        public ActionResult News(int? page)
        {
            ViewBag.Message = "Udruženje poljoprivrednih proizvođača Hercegovina.";

            int pageNumber = (page ?? 1);

            return View(context.Posts.OrderByDescending(p => p.PostDate)
                .ThenBy(p => p.Recommended)
                .ThenBy(p => p.CategoryId).Where(p => p.Status == true).ToPagedList(pageNumber, _pageSize));
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}