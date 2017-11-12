using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeClearedTeam.Controllers
{
    public class HangfireController : Controller
    {
        // GET: Hangfire
        public ActionResult Index()
        {
            return View();
        }
    }
}