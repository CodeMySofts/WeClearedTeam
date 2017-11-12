using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeClearedTeam.Models;

namespace WeClearedTeam.Controllers
{
    public class AddonsController : Controller
    {
        private WeClearedDBContext db = new WeClearedDBContext();

        // GET: Addons
        public ActionResult Index()
        {
            // Sortir les éléments à afficher
            var addons = db.Addons.OrderBy(x => x.Nom).ToList();
            foreach (var addon in addons)
            {
                // check and add the addons versions
            }

            return View(addons);
        }

        // GET: Addons/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Addons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Addons/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var nom = Request.Form["Nom"];
                var description = Request.Form["Description"];
                var webPath = Request.Form["WebPath"];
                var versionPath = Request.Form["VersionPath"];
                if (nom == "" || description == "" || webPath == "" || versionPath == "")
                {
                    ViewBag.Commentaire = nom == "" ? "Vous devez tout d'abord entrer un nom pour le addon." : description == "" ? "Ajoutez une description pour le addon." : webPath == "" ? "Ajoutez un Web Path pour l'addon." : versionPath == "" ? "Ajouter le nom du dossier contenant le .toc qui contient la version." : "";
                    ViewBag.Nom = nom;
                    ViewBag.Description = description;
                    ViewBag.WebPath = webPath;
                    ViewBag.VersionPath = versionPath;
                    return View("Create");
                }

                // Ajout du nouvel élément dans la base de données
                var newAddon = new Addon { Nom = nom, Description = description, WebPath = webPath, VersionPath = versionPath };
                db.Addons.Add(newAddon);

                // Enregistrer les changements dans la base de données
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Addons/Edit/5
        public ActionResult Edit(int id)
        {
            // Sortir l'élément à afficher
            var addon = db.Addons.FirstOrDefault(x => x.Id == id);

            // Afficher l'élément
            if (addon != null)
            {
                return View(addon);
            }

            // si on ne trouve pas l'élément, retour à l'index
            return RedirectToAction("Index");
        }

        // POST: Addons/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                var toEdit = db.Addons.FirstOrDefault(x => x.Id == id);
                if (toEdit != null)
                {
                    var nom = Request.Form["Nom"];
                    var description = Request.Form["Description"];
                    var webPath = Request.Form["WebPath"];
                    var versionPath = Request.Form["VersionPath"];
                    if (nom == "" || description == "" || versionPath == "")
                    {
                        ViewBag.Commentaire = nom == "" ? "Vous devez tout d'abord entrer un nom pour le addon." : description == "" ? "Ajoutez une description pour le addon." : versionPath == "" ? "Ajouter le nom du dossier contenant le .toc qui contient la version." : "";
                        ViewBag.Nom = nom;
                        ViewBag.Description = description;
                        ViewBag.WebPath = webPath;
                        ViewBag.VersionPath = versionPath;
                        return View("Create");
                    }

                    // Ajout du nouvel élément dans la base de données
                    toEdit.Nom = nom;
                    toEdit.Description = description;
                    toEdit.WebPath = webPath;
                    toEdit.VersionPath = versionPath;

                    // Enregistrer les changements dans la base de données
                    db.SaveChanges();
                }


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Addons/Delete/5
        public ActionResult Delete(int id)
        {
            // Trouver l'élément à retirer de la base de données
            var toDelete = db.Addons.FirstOrDefault(x => x.Id == id);

            // Si on à trouvé l'élément, on le retire
            if (toDelete != null)
            {
                db.Addons.Remove(toDelete);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
