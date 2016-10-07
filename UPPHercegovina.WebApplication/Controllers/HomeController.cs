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

        public ActionResult Index()
        {

            //Field f1 = new Field()
            //{
            //    Id = 1,
            //    PlaceName = "Field 1",
            //    Details = "Details 1",
            //    GeoLong = "36.401081",
            //    GeoLat = "10.16596"
            //};
            //Field f2 = new Field()
            //{
            //    Id = 2,
            //    PlaceName = "Field 2",
            //    Details = "Details 2",
            //    GeoLong = "36.4",
            //    GeoLat = "10.616667"
            //};
            //Field f3 = new Field()
            //{
            //    Id = 3,
            //    PlaceName = "Field 3",
            //    Details = "Details 3",
            //    GeoLong = "35.8329809",
            //    GeoLat = "10.63875"
            //};
            //Field f4 = new Field()
            //{
            //    Id = 4,
            //    PlaceName = "Field 4",
            //    Details = "Details 4",
            //    GeoLong = "34.745159",
            //    GeoLat = "10.7613"
            //};

            //List<Field> list = new List<Field>();
            //list.Add(f1);
            //list.Add(f2);
            //list.Add(f3);
            //list.Add(f4);
            //ViewBag.Data = list;

            //return View();
             return RedirectToAction("About");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Udruženje poljoprivrednih proizvođača Hercegovina.";

            return View(context.Posts.ToList());
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}