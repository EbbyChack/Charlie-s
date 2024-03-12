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
    public class UtenteController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        

        

        // GET: Utente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Utente/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,TipoUtente")] Utente utente)
        {
            if (ModelState.IsValid)
            {
                try
                {                  
                    db.Utentes.Add(utente);
                    db.SaveChanges();
                    return RedirectToAction("Create");
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }

            return View(utente);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Utente utente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var utenteLoggato = db.Utentes.Where(u => u.Username == utente.Username && u.Password == utente.Password).FirstOrDefault();
                    if (utenteLoggato != null)
                    {
                        FormsAuthentication.SetAuthCookie(utente.Username, false);
                        Session["Role"] = utenteLoggato.TipoUtente;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.Error = "Username o password errati";
                    }
                }
                catch (Exception ex)
                {
                    Response.Write(ex.Message);
                }
            }
            return View(utente);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Remove("Role");
            //ti riporta alla pagina dove ti trovavi
            return Redirect(Request.UrlReferrer.ToString());
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
