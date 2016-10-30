using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UPPHercegovina.WebApplication.CustomFilters;
using UPPHercegovina.WebApplication.Models;

namespace UPPHercegovina.WebApplication.Controllers
{
    public class OverviewMembersController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        private int _pageSize = 5;


        public ActionResult Index(int? page)
        {
            var users = context.Users.Where(u => u.Status == true).Include(u => u.Township).ToList();
            var us = users.Where(u => u.GetMarkCount > 0).OrderByDescending(u => u.GetAverageMark).ToList();

            int pageNumber = (page ?? 1);

            return View(us.ToPagedList(pageNumber, _pageSize));
        }


    }
}