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
    public class MaterialesController : Controller
    {
        private BDMonlic1Entities2 db = new BDMonlic1Entities2();
        int r;
        // GET: Materiales
        public ActionResult Index()
        {
            var materiales = db.Materiales.Include(m => m.UnidadMedidas);
            return View(materiales.ToList());
        }

        // GET: Materiales/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materiales materiales = db.Materiales.Find(id);
            if (materiales == null)
            {
                return HttpNotFound();
            }
            return View(materiales);
        }

        // GET: Materiales/Create
        public ActionResult Create()
        {
            ViewBag.IdUnidad = new SelectList(db.UnidadMedidas, "IdUnidad", "Unidad");
            return View();
        }

        // POST: Materiales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Material, string Descripcion, int Existencia, int Precio, int IdUnidad, Materiales materiales)
        {


            if (ModelState.IsValid)
            {
                r = db.validarrepetidos_material(Material, Descripcion, Existencia, Precio, IdUnidad );
                db.SaveChanges();

                if (r == 1)
                {


                    ViewBag.alerta = "alert('Se Registro Correctamente')";


                }
                else
                {
                    ViewBag.alerta = "alert('Este material ya está Registrado')";
                    ViewBag.IdUnidad = new SelectList(db.UnidadMedidas, "IdUnidad", "Unidad", materiales.IdUnidad);
                    return View("Create");
                }

            }
            ViewBag.IdUnidad = new SelectList(db.UnidadMedidas, "IdUnidad", "Unidad", materiales.IdUnidad);
            //return View(materiales);
            return RedirectToAction("Index");

                    }

        // GET: Materiales/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Materiales materiales = db.Materiales.Find(id);
            if (materiales == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUnidad = new SelectList(db.UnidadMedidas, "IdUnidad", "Unidad", materiales.IdUnidad);
            return View(materiales);
        }

        // POST: Materiales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdMaterial,Material,Descripcion,Precio,IdUnidad,Estado")] Materiales materiales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(materiales).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUnidad = new SelectList(db.UnidadMedidas, "IdUnidad", "Unidad", materiales.IdUnidad);
            return View(materiales);
        }

        public ActionResult Estado(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ciudades ciudades = db.Ciudades.Find(id);
            if (ciudades == null)
            {
                return HttpNotFound();
            }
            return View(ciudades);
        }

        // POST: Ciudades/Estado
        [HttpPost, ActionName("Estado")]
        [ValidateAntiForgeryToken]
        public ActionResult EstadoConfirmed(int id)
        {
            
            db.estado_material(id);
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