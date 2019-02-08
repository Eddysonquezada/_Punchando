using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_Monlic.Models;

namespace Proyecto_Monlic.Controllers
{
    public class MovimientosController : Controller
    {
        private BDMonlic1Entities db = new BDMonlic1Entities();

        // GET: Movimientos
        public ActionResult Index()
        {
            var movimientos = db.Movimientos.Include(m => m.Contactos).Include(m => m.TiposMovimientos);
            return View(movimientos.ToList());
        }

        // GET: Movimientos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimientos movimientos = db.Movimientos.Find(id);
            if (movimientos == null)
            {
                return HttpNotFound();
            }
            return View(movimientos);
        }

        // GET: Movimientos/Create
        public ActionResult Create()
        {
            ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto");
            ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento");
            return View();
        }

        // POST: Movimientos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdMovimiento,Fecha,IdTipoM,IdContacto,Estado")] Movimientos movimientos)
        {
            if (ModelState.IsValid)
            {
                db.Movimientos.Add(movimientos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", movimientos.IdContacto);
            ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", movimientos.IdTipoM);
            return View(movimientos);
        }

        // GET: Movimientos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimientos movimientos = db.Movimientos.Find(id);
            if (movimientos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", movimientos.IdContacto);
            ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", movimientos.IdTipoM);
            return View(movimientos);
        }

        // POST: Movimientos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMovimiento,Fecha,IdTipoM,IdContacto,Estado")] Movimientos movimientos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movimientos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", movimientos.IdContacto);
            ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", movimientos.IdTipoM);
            return View(movimientos);
        }

        // GET: Movimientos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movimientos movimientos = db.Movimientos.Find(id);
            if (movimientos == null)
            {
                return HttpNotFound();
            }
            return View(movimientos);
        }

        // POST: Movimientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movimientos movimientos = db.Movimientos.Find(id);
            db.Movimientos.Remove(movimientos);
            db.SaveChanges();
            return RedirectToAction("Index");
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
