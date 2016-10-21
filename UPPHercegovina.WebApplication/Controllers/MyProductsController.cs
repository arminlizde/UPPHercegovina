using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class MyProductsController : Controller
    {
        // GET: MyProducts
        public ActionResult Index()
        {
            return View();
        }
    }
}