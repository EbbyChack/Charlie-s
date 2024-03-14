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
            var cart = Session["Cart"] as List<Tuple<Articolo, int>> ?? new List<Tuple<Articolo, int>>();
            return View(cart);
        }

        public ActionResult AggiungiCarello(int id, int quantita)
        {
            
            List<Articolo> prodotti = db.Articoloes.ToList();
            Articolo articolo = prodotti.FirstOrDefault(x => x.IdArticolo == id);

            if (articolo != null)
            {

                var cart = Session["Cart"] as List<Tuple<Articolo, int>> ?? new List<Tuple<Articolo, int>>();
                var articoloGiaPresente = cart.FirstOrDefault(x => x.Item1.IdArticolo == articolo.IdArticolo);
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
            var cart = Session["Cart"] as List<Tuple<Articolo, int>> ?? new List<Tuple<Articolo, int>>();
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
            if (Session["Cart"] != null)
            {
                return View();
            }else
            {
                return RedirectToAction("Index");
            }
           
        }

        public ActionResult Checkout(string indirizzo, string note)
        {
            
            var cart = Session["Cart"] as List<Tuple<Articolo, int>>;
            if (cart != null && cart.Any())
            {
                var ordine = new Ordine
                {
                    TotaleOrdine = cart.Sum(x => x.Item1.Prezzo * x.Item2),
                    DataOrdine = DateTime.Now.Date,
                    IsEvaso = false,
                    IdUtente = (int)(db.Utentes.FirstOrDefault(u => u.Username == User.Identity.Name)?.IdUtente),
                    Indirizzo= indirizzo,
                    Note = note

                };
                db.Ordines.Add(ordine);
                db.SaveChanges();

                var dettagliOrdine = cart.Select(x => new DettaglioOrdine
                {
                    IdArticolo = x.Item1.IdArticolo,
                    IdOrdine = ordine.IdOrdine,
                    Quantita = x.Item2
                }).ToList();

                foreach (var dettaglio in dettagliOrdine)
                {
                    db.DettaglioOrdines.Add(dettaglio);
                }

                db.SaveChanges();

                Session.Clear();
                Session["OrderSaved"] = true;


                return RedirectToAction("ConclusioneOrdine");
            }

            return RedirectToAction("Index");
        }

        public ActionResult ConclusioneOrdine()
        {
            if (Session["OrderSaved"] != null && (bool)Session["OrderSaved"])
            {
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