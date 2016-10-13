using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.Abstractions;
using UPPHercegovina.WebApplication.Models;
using UPPHercegovina.WebApplication.Repositories;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class MarksController : Controller
    {
        private IMarkRepository markRepository;

        //from s in markRepository.GetMarks()
          //     select s;

        public MarksController()
        {
            this.markRepository = new MarkRepository(new ApplicationDbContext());
        }

        public MarksController(IMarkRepository markRepository)
        {
            this.markRepository = markRepository;
        }

        public ActionResult Index()
        {
            return View(markRepository.GetMarks());
        }

        protected override void Dispose(bool disposing)
        {
            markRepository.Dispose();
            base.Dispose(disposing);
        }
    }
}
