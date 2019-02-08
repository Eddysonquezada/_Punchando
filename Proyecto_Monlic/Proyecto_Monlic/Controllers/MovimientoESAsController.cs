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
    public class MovimientoESAsController : Controller
    {
        private BDMonlic1Entities db = new BDMonlic1Entities();

        // GET: MovimientoESAs
        public ActionResult Index()
        {
            var movimientoESA = db.MovimientoESA.Include(m => m.Contactos).Include(m => m.Materiales).Include(m => m.TiposMovimientos);
            return View(movimientoESA.ToList());
        }

        // GET: MovimientoESAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoESA movimientoESA = db.MovimientoESA.Find(id);
            if (movimientoESA == null)
            {
                return HttpNotFound();
            }
            return View(movimientoESA);
        }

        // GET: MovimientoESAs/Create
        public ActionResult Create()
        {
            ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto");
            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material");
            ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento");
            return View();
        }

        // POST: MovimientoESAs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( int idTipoM, int Cantidad, int idMaterial, DateTime Fecha, int Precio, int IdContacto,MovimientoESA movimientoESA)
        {
            if (ModelState.IsValid)
            {
                db.StockManage2( idTipoM, Cantidad, idMaterial, Fecha, Precio, IdContacto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", movimientoESA.IdContacto);
            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", movimientoESA.IdMaterial);
            ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", movimientoESA.IdTipoM);
            return View(movimientoESA);
        }

        // GET: MovimientoESAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoESA movimientoESA = db.MovimientoESA.Find(id);
            if (movimientoESA == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", movimientoESA.IdContacto);
            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", movimientoESA.IdMaterial);
            ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", movimientoESA.IdTipoM);
            return View(movimientoESA);
        }

        // POST: MovimientoESAs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMovimientoESA,Fecha,Precio,Cantidad,IdMaterial,IdTipoM,IdContacto,Estado")] MovimientoESA movimientoESA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movimientoESA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", movimientoESA.IdContacto);
            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", movimientoESA.IdMaterial);
            ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", movimientoESA.IdTipoM);
            return View(movimientoESA);
        }

        // GET: MovimientoESAs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovimientoESA movimientoESA = db.MovimientoESA.Find(id);
            if (movimientoESA == null)
            {
                return HttpNotFound();
            }
            return View(movimientoESA);
        }

        // POST: MovimientoESAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovimientoESA movimientoESA = db.MovimientoESA.Find(id);
            db.MovimientoESA.Remove(movimientoESA);
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
