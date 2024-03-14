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
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
           
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
                        string Maiuscolo = char.ToUpper(utente.Username[0]) + utente.Username.Substring(1);
                        FormsAuthentication.SetAuthCookie(Maiuscolo, false);
                        
                        if (!Roles.RoleExists(utenteLoggato.TipoUtente))
                        {
                            Roles.CreateRole(utenteLoggato.TipoUtente);
                        }
                        Roles.AddUserToRole(utente.Username, utenteLoggato.TipoUtente);
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
            Roles.RemoveUserFromRole(User.Identity.Name, Roles.GetRolesForUser(User.Identity.Name).First());
            Session.Clear();
            //ti riporta alla pagina dove ti trovavi
            return RedirectToAction("Index", "Home");
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
