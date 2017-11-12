using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WeClearedTeam.Models;
using WeClearedTeam.ViewModels;

namespace WeClearedTeam.Controllers
{
    public class RaidersController : Controller
    {
        private WeClearedDBContext db = new WeClearedDBContext();

        // GET: Raiders
        public ActionResult Index()
        {
            // Sortir les éléments à afficher
            var raiders = db.Raiders.OrderBy(x => x.Nom).ToList();
            foreach (var raider in raiders)
            {
                raider.Classe = (Classes)raider.ClassId;
                raider.Specialisation = (Specializations)raider.SpecializationId;
            }

            return View(raiders);
        }
        
        // GET: Raiders/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Raiders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Raiders/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var classe = Request.Form["Classe"];
                var specialisation = Request.Form["Specialisation"];
                var nom = Request.Form["Nom"];
                if (classe == "0" || specialisation == "0" || nom == "")
                {
                    ViewBag.Commentaire = nom == "" ? "Vous devez tout d'abord entrer un nom pour le raider." : classe == "0" ? "Vous devez choisir une classe pour le raider" : specialisation == "0" ? "Vous devez sélectionner une spécialisation pour le raider." : "";
                    ViewBag.NomChoisi = nom;
                    return View("Create");
                }

                // Variables du formulaire
                var classId = (int)(Classes)Enum.Parse(typeof(Classes), classe);
                var specializationId = (int) (Specializations) Enum.Parse(typeof(Specializations), specialisation);

                // Ajout du nouvel élément dans la base de données
                var newRaider = new Raider {Id = db.Raiders.Count(), Nom = nom, ClassId = classId, SpecializationId = specializationId, Joined = DateTime.Now};
                db.Raiders.Add(newRaider);

                // Enregistrer les changements dans la base de données
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Raiders/Edit/5
        public ActionResult Edit(int id)
        {
            // Sortir l'élément à afficher
            var raider = db.Raiders.FirstOrDefault(x => x.Id == id);

            // Afficher l'élément
            if (raider != null)
            {
                // Il faut s'assurer d'envoyer les enums en texte pour le selecteur
                raider.Classe = (Classes)raider.ClassId;
                raider.Specialisation = (Specializations)raider.SpecializationId;
                return View(raider);
            }
            return RedirectToAction("Index");
        }

        // POST: Raiders/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // Trouver l'élément à retirer de la base de données
                var toEdit = db.Raiders.FirstOrDefault(x => x.Id == id);

                // Si on à trouvé l'élément, on le modifie
                if (toEdit != null)
                {
                    // Variables du formulaire
                    var classId = (int)(Classes)Enum.Parse(typeof(Classes), Request.Form["Classe"]);
                    var specializationId = (int)(Specializations)Enum.Parse(typeof(Specializations), Request.Form["Specialisation"]);
                    var nom = Request.Form["Nom"];

                    // Modifier l'élément
                    toEdit.Nom = nom;
                    toEdit.ClassId = classId;
                    toEdit.SpecializationId = specializationId;

                    // Enregistrer les changements.
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Raiders/Delete/5
        public ActionResult Delete(int id)
        {
            // Trouver l'élément à retirer de la base de données
            var toDelete = db.Raiders.FirstOrDefault(x => x.Id == id);

            // Si on à trouvé l'élément, on le retire
            if (toDelete != null)
            {
                db.Raiders.Remove(toDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
