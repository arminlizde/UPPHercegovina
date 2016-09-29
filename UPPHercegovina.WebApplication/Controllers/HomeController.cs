using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Apa"] = "Ucim da radim sa viewdata";
            ViewBag.test = "Ucim da radim sa viewvag";

            TempData["TempModel"] = "Ucim da radim sa TempModelom";
            Session["SessionModel"] = "ucim koristenje sesije";
             return RedirectToAction("About");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}