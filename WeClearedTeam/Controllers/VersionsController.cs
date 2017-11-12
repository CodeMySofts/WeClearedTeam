using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeClearedTeam.Models;

namespace WeClearedTeam.Controllers
{
    public class VersionsController : System.Web.Http.ApiController
    {
        private WeClearedDBContext db = new WeClearedDBContext();

        /// <summary>
        /// Retourne tout les addons proposés par le client ainsi que les descriptions et versions.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Addon> Get()
        {
            var addonsList = db.Addons;
            return addonsList;
        }
    }
}