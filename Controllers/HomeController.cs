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
            //Verifica se l'utente è autenticato
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
                //Recupero gli ordini dell'utente loggato
                var OrdiniUtente = db.Ordines.Where(o => o.Utente.Username == User.Identity.Name);
                return View(OrdiniUtente.ToList());

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult DettagliOrdine(int? id)
        {
            if (User.Identity.IsAuthenticated && id != null)
            {
                //Recupero i dettagli dell'ordine dell'utente loggato
                var DettagliOrdine = db.DettaglioOrdines.Where(o => o.IdOrdine == id).ToList();

                //Recupero gli id degli articoli presenti nei dettagli dell'ordine
                var IdArticoloLista= DettagliOrdine.Select(o => o.IdArticolo).ToList();

                //Recupero gli articoli presenti nei dettagli dell'ordine
                var Articoli = db.Articoloes.Where(a => IdArticoloLista.Contains(a.IdArticolo)).ToList();

                //Recupero l'ordine con l'id specificato
                var Ordine = db.Ordines.FirstOrDefault(o => o.IdOrdine == id);

                //Creo una tupla con i dettagli dell'ordine e gli articoli
                var model = new Tuple<List<DettaglioOrdine>, List<Articolo>, Ordine>(DettagliOrdine, Articoli, Ordine);

                return View(model);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

       
    }
}