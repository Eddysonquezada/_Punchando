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
    public class ContactosController : Controller
    {
        private BDMonlic1Entities2 db = new BDMonlic1Entities2();
        int r;
        // GET: Contactos
        public ActionResult Index()
        {
            var contactos = db.Contactos.Include(c => c.Ciudades).Include(c => c.TiposDocumentos);
            return View(contactos.ToList());
        }

        // GET: Contactos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contactos contactos = db.Contactos.Find(id);
            if (contactos == null)
            {
                return HttpNotFound();
            }
            return View(contactos);
        }

        // GET: Contactos/Create
        public ActionResult Create()
        {
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "IdCiudad", "Ciudad");
            ViewBag.IdTipoD = new SelectList(db.TiposDocumentos, "IdTipoD", "Tipo");
            return View();
        }

        // POST: Contactos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string Contacto, int Documento, int Telefono, int IdtipoD, int Idciudad, Contactos contactos)
        {
            if (ModelState.IsValid)
            {
                r = db.validar_contactos(Contacto, Documento,Telefono, IdtipoD, Idciudad);
                db.SaveChanges();

                if (r == 1)
                {


                    ViewBag.alerta = "alert('Se Registro Correctamente')";


                }
                else
                {
                    ViewBag.alerta = "alert('Esta persona ya está Registrada')";
                    ViewBag.IdCiudad = new SelectList(db.Ciudades, "IdCiudad", "Ciudad", contactos.IdCiudad);
                    ViewBag.IdTipoD = new SelectList(db.TiposDocumentos, "IdTipoD", "Tipo", contactos.IdTipoD);
                    return View("Create");
                }

            }
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "IdCiudad", "Ciudad", contactos.IdCiudad);
            ViewBag.IdTipoD = new SelectList(db.TiposDocumentos, "IdTipoD", "Tipo", contactos.IdTipoD);
            return RedirectToAction("Index");
        }
       
// GET: Contactos/Edit/5
public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contactos contactos = db.Contactos.Find(id);
            if (contactos == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "IdCiudad", "Ciudad", contactos.IdCiudad);
            ViewBag.IdTipoD = new SelectList(db.TiposDocumentos, "IdTipoD", "Tipo", contactos.IdTipoD);
            return View(contactos);
        }

        // POST: Contactos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdContacto,Contacto,Documento,Telefono,IdTipoD,IdCiudad,Estado")] Contactos contactos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contactos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCiudad = new SelectList(db.Ciudades, "IdCiudad", "Ciudad", contactos.IdCiudad);
            ViewBag.IdTipoD = new SelectList(db.TiposDocumentos, "IdTipoD", "Tipo", contactos.IdTipoD);
            return View(contactos);
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

            db.estado_contacto(id);
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