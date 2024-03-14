using EsercizioSettimana11Marzo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EsercizioSettimana11Marzo.Controllers
{
    public class OperazioniController : Controller
    {
        private ModelDbContext db = new ModelDbContext();
        // GET: Operazioni
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult OrdiniEvasi()
        {
            var TotaleOrdiniEvasi = db.Ordines.Where(x => x.IsEvaso == true).Count();
            return Json(TotaleOrdiniEvasi, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TotaleIncasso(DateTime date)
        {
            var orders = db.Ordines.Where(x => x.DataOrdine == date);
            var TotaleIncasso = orders.Any() ? orders.Sum(x => x.TotaleOrdine) : 0;
            return Json(TotaleIncasso, JsonRequestBehavior.AllowGet);
        }
    }
}