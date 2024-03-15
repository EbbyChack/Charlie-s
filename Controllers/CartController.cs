using EsercizioSettimana11Marzo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EsercizioSettimana11Marzo.Controllers
{
    public class CartController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Cart
        public ActionResult Index()
        {
            // Recupero il carrello dalla sessione
            var cart = Session["Cart"] as List<Tuple<Articolo, int>> ?? new List<Tuple<Articolo, int>>();
            return View(cart);
        }

        public ActionResult AggiungiCarello(int id, int quantita)
        {
            // Recupero il prodotto dal database
            List<Articolo> prodotti = db.Articoloes.ToList();
            Articolo articolo = prodotti.FirstOrDefault(x => x.IdArticolo == id);

            if (articolo != null)
            {
                // Recupero il carrello dalla sessione e aggiungo il prodotto con la quantità
                var cart = Session["Cart"] as List<Tuple<Articolo, int>> ?? new List<Tuple<Articolo, int>>();
                // Verifico se l'articolo è già presente nel carrello
                var articoloGiaPresente = cart.FirstOrDefault(x => x.Item1.IdArticolo == articolo.IdArticolo);
                // Se l'articolo è già presente nel carrello, aggiorno la quantità
                if (articoloGiaPresente != null)
                {
                    
                    int index = cart.IndexOf(articoloGiaPresente);
                    cart[index] = new Tuple<Articolo, int>(articolo, articoloGiaPresente.Item2 + quantita);
                }
                else
                {

                    cart.Add(new Tuple<Articolo, int>(articolo, quantita));
                }
                Session["Cart"] = cart;

            }

            return RedirectToAction("Index");
        }


        public ActionResult RimuoviCarello(int id)
        {
            // Recupero il carrello dalla sessione
            var cart = Session["Cart"] as List<Tuple<Articolo, int>> ?? new List<Tuple<Articolo, int>>();
            // Rimuovo l'articolo dal carrello
            var articolo = cart.FirstOrDefault(x => x.Item1.IdArticolo == id);
            if (articolo != null)
            {
                cart.Remove(articolo);
                Session["Cart"] = cart;
            }

            return RedirectToAction("Index");
        }

        
        public ActionResult CheckoutForm()
        {
            // Verifico se il carrello è vuoto
            // Se il carrello è vuoto, reindirizzo alla pagina principale
            if (Session["Cart"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

        public ActionResult Checkout(string indirizzo, string note)
        {
            // Recupero il carrello dalla sessione
            var cart = Session["Cart"] as List<Tuple<Articolo, int>>;
            // Verifico se il carrello è vuoto
            if (cart != null && cart.Any())
            {
                // Creo un nuovo ordine
                var ordine = new Ordine
                {
                    TotaleOrdine = cart.Sum(x => x.Item1.Prezzo * x.Item2),
                    DataOrdine = DateTime.Now.Date,
                    IsEvaso = false,
                    IdUtente = (int)(db.Utentes.FirstOrDefault(u => u.Username == User.Identity.Name)?.IdUtente),
                    Indirizzo = indirizzo,
                    Note = note

                };
                // Salvo l'ordine nel database
                db.Ordines.Add(ordine);
                db.SaveChanges();
                // Creo i dettagli dell'ordine
                var dettagliOrdine = cart.Select(x => new DettaglioOrdine
                {
                    IdArticolo = x.Item1.IdArticolo,
                    IdOrdine = ordine.IdOrdine,
                    Quantita = x.Item2
                }).ToList();
                // Salvo i dettagli dell'ordine nel database
                foreach (var dettaglio in dettagliOrdine)
                {
                    db.DettaglioOrdines.Add(dettaglio);
                }

                db.SaveChanges();

                // Svuoto il carrello
                Session.Clear();
                // Segnalo che l'ordine è stato salvato
                Session["OrderSaved"] = true;

                // Reindirizzo alla pagina di conclusione ordine
                return RedirectToAction("ConclusioneOrdine");
            }

            return RedirectToAction("Index");
        }

        public ActionResult ConclusioneOrdine()
        {
            // Verifico se l'ordine è stato salvato
            if (Session["OrderSaved"] != null && (bool)Session["OrderSaved"])
            {
                // Svuoto la variabile di sessione
                Session.Clear();
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

    }
}