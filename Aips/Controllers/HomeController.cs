using BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aips.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Queries queries = new Queries();
            ViewBag.AdminMessages = queries.GetAdminMessages();
            ViewBag.Events = queries.GetUniversityEvents();

            return View();
        }
    }
}