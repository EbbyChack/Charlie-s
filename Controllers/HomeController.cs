using EsercizioSettimana11Marzo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EsercizioSettimana11Marzo.Controllers
{
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        public ActionResult Index()
        {
            return View();
        }

       public ActionResult Prodotti()
        {
            if(User.Identity.IsAuthenticated)
            {

                return View(db.Articoloes.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            

        }

        public ActionResult OrdiniUtente()
        {
            if (User.Identity.IsAuthenticated)
            {
                var OrdiniUtente = db.Ordines.Where(o => o.Utente.Username == User.Identity.Name);
                return View(OrdiniUtente.ToList());

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        

       
    }
}