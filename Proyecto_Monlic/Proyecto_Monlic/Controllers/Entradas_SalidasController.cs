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
    public class Entradas_SalidasController : Controller
    {
        private BDMonlic1Entities2 db = new BDMonlic1Entities2();

        // GET: Entradas_Salidas
        public ActionResult Index()
        {
            var entradas_Salidas = db.Entradas_Salidas.Include(e => e.Materiales).Include(e => e.Movimientos);
            return View(entradas_Salidas.ToList());
        }

        // GET: Entradas_Salidas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entradas_Salidas entradas_Salidas = db.Entradas_Salidas.Find(id);
            if (entradas_Salidas == null)
            {
                return HttpNotFound();
            }
            return View(entradas_Salidas);
        }

        // GET: Entradas_Salidas/Create
        public ActionResult Create()
        {
            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material");
            ViewBag.IdMovimiento = new SelectList(db.Movimientos, "IdMovimiento", "Estado");
            return View();
        }

        // POST: Entradas_Salidas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdE_S,Precio,Cantidad,IdMaterial,IdMovimiento,Estado")] Entradas_Salidas entradas_Salidas)
        {
            if (ModelState.IsValid)
            {
                db.Entradas_Salidas.Add(entradas_Salidas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", entradas_Salidas.IdMaterial);
            ViewBag.IdMovimiento = new SelectList(db.Movimientos, "IdMovimiento", "Estado", entradas_Salidas.IdMovimiento);
            return View(entradas_Salidas);
        }

        // GET: Entradas_Salidas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entradas_Salidas entradas_Salidas = db.Entradas_Salidas.Find(id);
            if (entradas_Salidas == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", entradas_Salidas.IdMaterial);
            ViewBag.IdMovimiento = new SelectList(db.Movimientos, "IdMovimiento", "Estado", entradas_Salidas.IdMovimiento);
            return View(entradas_Salidas);
        }

        // POST: Entradas_Salidas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdE_S,Precio,Cantidad,IdMaterial,IdMovimiento,Estado")] Entradas_Salidas entradas_Salidas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(entradas_Salidas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", entradas_Salidas.IdMaterial);
            ViewBag.IdMovimiento = new SelectList(db.Movimientos, "IdMovimiento", "Estado", entradas_Salidas.IdMovimiento);
            return View(entradas_Salidas);
        }

        // GET: Entradas_Salidas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entradas_Salidas entradas_Salidas = db.Entradas_Salidas.Find(id);
            if (entradas_Salidas == null)
            {
                return HttpNotFound();
            }
            return View(entradas_Salidas);
        }

        // POST: Entradas_Salidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Entradas_Salidas entradas_Salidas = db.Entradas_Salidas.Find(id);
            db.Entradas_Salidas.Remove(entradas_Salidas);
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
