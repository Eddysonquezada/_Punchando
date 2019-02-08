using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Proyecto_Monlic.Models;

namespace Proyecto_Monlic.Controllers
{
    public class UnidadMedidasController : Controller

    {
        private BDMonlic1Entities db = new BDMonlic1Entities();
        int r;

        // GET: UnidadMedidas
        public ActionResult Index(string Estado)
        {
            
            return View(db.UnidadMedidas.ToList());
        }

        // GET: UnidadMedidas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadMedidas unidadMedidas = db.UnidadMedidas.Find(id);
            if (unidadMedidas == null)
            {
                return HttpNotFound();
            }
            return View(unidadMedidas);
        }

        // GET: UnidadMedidas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UnidadMedidas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Unidad)
        {
            

            if (ModelState.IsValid)
            {
                r = db.validarrepetidos(Unidad);
                 db.SaveChanges();
                
                if (r == 1) {


                    ViewBag.alerta = "swal('Se Registro Correctamente')";

                   
                }
                else
                {
                    ViewBag.alerta = "swal('Esta Unidad ya está Registrada')";
                    return View("Create");
                }

            }
            return RedirectToAction("Index");

        }



        // GET: UnidadMedidas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UnidadMedidas unidadMedidas = db.UnidadMedidas.Find(id);
            if (unidadMedidas == null)
            {
                return HttpNotFound();
            }
            return View(unidadMedidas);
        }

        // POST: UnidadMedidas/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdUnidad,Unidad,Estado")] UnidadMedidas unidadMedidas)
        {
            if (ModelState.IsValid)
            {
                db.Entry(unidadMedidas).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(unidadMedidas);
        }

        // GET: Ciudades/Estado
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
            
           
            db.estado_Medida(id);
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
