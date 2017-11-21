using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeClearedTeam.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Dirige vers la vue principale de l'application
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Dirige vers la vue de contact
        /// </summary>
        /// <returns></returns>
        public ActionResult Contact()
        {
            return View();
        }

        /// <summary>
        /// Dirige vers la vue de Présentation
        /// </summary>
        /// <returns></returns>
        public ActionResult Historique()
        {
            return View();
        }

        /// <summary>
        /// Dirige vers la vue de reglements
        /// </summary>
        /// <returns></returns>
        public ActionResult Reglements()
        {
            return View();
        }

        /// <summary>
        /// Dirige vers la vue des explications client
        /// </summary>
        /// <returns></returns>
        public ActionResult ExplicationsClient()
        {
            return View();
        }
    }
}