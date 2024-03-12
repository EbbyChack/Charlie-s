using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using EsercizioSettimana11Marzo.Models;

namespace EsercizioSettimana11Marzo.Controllers
{
    
    public class ArticoloController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Articolo
        
        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "ADMIN"))
            {
                return View(db.Articoloes.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        // GET: Articolo/Details/5
        public ActionResult Details(int? id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "ADMIN"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Articolo articolo = db.Articoloes.Find(id);
                if (articolo == null)
                {
                    return HttpNotFound();
                }
                return View(articolo);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // GET: Articolo/Create
        public ActionResult Create()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "ADMIN"))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        // POST: Articolo/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdArticolo,Nome,Immagine,Prezzo,TempiDiConsegna,Ingredienti")] Articolo articolo)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "ADMIN"))
            {
                if (ModelState.IsValid)
                {
                    db.Articoloes.Add(articolo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(articolo);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        // GET: Articolo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "ADMIN"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Articolo articolo = db.Articoloes.Find(id);
                if (articolo == null)
                {
                    return HttpNotFound();
                }
                return View(articolo);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        // POST: Articolo/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdArticolo,Nome,Immagine,Prezzo,TempiDiConsegna,Ingredienti")] Articolo articolo)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "ADMIN"))
            {
                if (ModelState.IsValid)
                {
                    db.Entry(articolo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(articolo);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        // GET: Articolo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "ADMIN"))
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Articolo articolo = db.Articoloes.Find(id);
                if (articolo == null)
                {
                    return HttpNotFound();
                }
                return View(articolo);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        // POST: Articolo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "ADMIN"))
            {
                Articolo articolo = db.Articoloes.Find(id);
                db.Articoloes.Remove(articolo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
