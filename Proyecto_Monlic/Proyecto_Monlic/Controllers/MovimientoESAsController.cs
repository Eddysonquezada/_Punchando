using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Proyecto_Monlic.Models;

namespace Proyecto_Monlic.Controllers
{
    public class MovimientoESAsController : Controller
    {
        private BDMonlic1Entities2 db = new BDMonlic1Entities2();

        // GET: MovimientoESAs
        public ActionResult Index()
        {
            var movimientoESA = db.MovimientoESA.Include(m => m.Contactos)
              .Include(m => m.MovimientoESADet).Include(m => m.TiposMovimientos);
            return View(movimientoESA.ToList().OrderBy(p => p.Fecha));
            // return View();
        }

        // GET: MovimientoESAs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Session["Carrito"]= new 
            var mov = new MovimientoView
            {
                Fecha = DateTime.Now,
                IdMovimientoESA = 0,
                Cantidad = 1
            };

            var mo = db.MovimientoESA
                .Where(p => p.IdMovimientoESA == id)
                .Include(i => i.MovimientoESADet)
                .FirstOrDefault();

            if (mo != null)
            {
                mov.IdMovimientoESA = mo.IdMovimientoESA;
                mov.SubTotalGeneral = mo.SubTotalGeneral;
                mov.CargosGenerales = mo.CargosGenerales;
                mov.DescuentosGenerales = mo.DescuentosGenerales;
                mov.TotalGeneral = mo.TotalGeneral;
                mov.Estado = mo.Estado;
                mov.IdContacto = mo.IdContacto;
                mov.IdTipoM = mo.IdTipoM;

                ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", mo.IdContacto);
                ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", mo.IdTipoM);

                foreach (var item in mo.MovimientoESADet)
                {
                    mov.MovimientoESADet.Add(item);
                }
            }

            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material");
            
            return View(mov);
        }

        // GET: MovimientoESAs/Create
        public ActionResult Create(int Id = 0)
        {
            //Session["Carrito"]= new 
            var mov = new MovimientoView
            {
                Fecha = DateTime.Now,
                IdMovimientoESA = 0,
                Cantidad = 1
            };
            //var detalle = new MovimientoView
            //{
            //    Cantidad = 1, IdMovimientoESADet = 0, IdMovimientoESA = 0
            //};
            //mov.MovimientoESADet.Add(detalle);

            if (Id != 0)
            {
                var mo = db.MovimientoESA
                    .Where(p => p.IdMovimientoESA == Id)
                    .Include(i => i.MovimientoESADet)
                    .FirstOrDefault();

                if (mo != null)
                {
                    mov.IdMovimientoESA = mo.IdMovimientoESA;
                    mov.SubTotalGeneral = mo.SubTotalGeneral;
                    mov.CargosGenerales = mo.CargosGenerales;
                    mov.DescuentosGenerales = mo.DescuentosGenerales;
                    mov.TotalGeneral = mo.TotalGeneral;
                    mov.Estado = mo.Estado;
                    mov.IdContacto = mo.IdContacto;
                    mov.IdTipoM = mo.IdTipoM;

                    ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", mo.IdContacto);
                    ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", mo.IdTipoM);

                    foreach (var item in mo.MovimientoESADet)
                    {
                        mov.MovimientoESADet.Add(item);
                    }
                }

            }
            else
            {
                ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto");
                ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento");
            }

            ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material");


            return View(mov);
        }

        [HttpPost]
        public async Task<JsonResult> Guardar(MovimientoView objMovimiento)
        {
            bool status = false;
            int idi = 0;
            if (!ModelState.IsValid) return new JsonResult { Data = new { status, idi } };

            var cantidad = objMovimiento.Cantidad;
            decimal subtotal = 0;
            decimal cargo = 0;
            decimal descuento = 0;

            var mat = db.Materiales.Find(objMovimiento.IdMaterial);
            if (mat != null)
            {
                mat.Existencia -= cantidad;
                subtotal = (decimal)mat.Precio * cantidad;

                cargo = 0; // subtotal * (mat.cargo/100)
                descuento = 0; // subtotal * (mat.descuento/100)

                var objDetalle = new MovimientoESADet
                {
                    IdMovimientoESA = 0,
                    IdMaterial = objMovimiento.IdMaterial,
                    Cantidad = cantidad,
                    SubTotal = subtotal,
                    Cargos = cargo,
                    Descuentos = descuento,
                    Total = subtotal + cargo - descuento
                };

                if (objMovimiento.IdMovimientoESA != 0)
                {
                    var mo = db.MovimientoESA.FirstOrDefault(p =>
                    p.IdMovimientoESA == objMovimiento.IdMovimientoESA);
                    if (mo != null)
                    {
                        mo.SubTotalGeneral += subtotal;
                        mo.CargosGenerales += cargo;
                        mo.DescuentosGenerales += descuento;
                        mo.TotalGeneral = mo.SubTotalGeneral + mo.CargosGenerales - mo.DescuentosGenerales;
                        mo.Estado = objMovimiento.Estado;
                        mo.IdContacto = objMovimiento.IdContacto;
                        mo.IdTipoM = objMovimiento.IdTipoM;

                        mo.Fecha = objMovimiento.Fecha;
                        db.Entry(mo).State = EntityState.Modified;
                    }

                    objMovimiento.IdMovimientoESA = mo.IdMovimientoESA;
                }
                else
                {
                    var mo = new MovimientoESA
                    {
                        SubTotalGeneral = mat.Precio * cantidad,
                        CargosGenerales = cargo,
                        DescuentosGenerales = descuento,
                        TotalGeneral = (mat.Precio * cantidad) + cargo - descuento,
                        Estado = objMovimiento.Estado,
                        IdContacto = objMovimiento.IdContacto,
                        IdTipoM = objMovimiento.IdTipoM,
                        Fecha = objMovimiento.Fecha
                    };

                    db.MovimientoESA.Add(mo);
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                    objMovimiento.IdMovimientoESA = mo.IdMovimientoESA;
                }

                objDetalle.IdMovimientoESA = objMovimiento.IdMovimientoESA;

                db.MovimientoESADet.Add(objDetalle);

            }

            db.Entry(mat).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            idi = objMovimiento.IdMovimientoESA;

            if (idi != 0)
            {
                status = true;
            }

            return new JsonResult { Data = new { status, idi } };
        }

        [HttpPost]
        public async Task<JsonResult> Remover(int id)
        {
            bool status = false;
            int idi = 0;
            if (id == 0) return new JsonResult { Data = new { status, idi } };

            var mod = db.MovimientoESADet.Include(x => x.MovimientoESA)
                .FirstOrDefault(p => p.IdMovimientoESADet == id);


            var mo = db.MovimientoESA.Find(mod.IdMovimientoESA);

            mo.SubTotalGeneral -= mod.SubTotal;
            mo.DescuentosGenerales -= mod.Descuentos;
            mo.CargosGenerales -= mod.Cargos;
            mo.TotalGeneral = mo.SubTotalGeneral + mo.CargosGenerales - mo.DescuentosGenerales;

            db.Entry(mo).State = EntityState.Modified;

            var mat = db.Materiales.Find(mod.IdMaterial);

            mat.Existencia += mod.Cantidad;

            db.Entry(mat).State = EntityState.Modified;
            db.MovimientoESADet.Remove(mod);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            idi = mo.IdMovimientoESA;

            if (idi != 0)
            {
                status = true;
            }

            return new JsonResult { Data = new { status, idi } };
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(MovimientoESADet movimientoESA)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.StockManage2( idTipoM, Cantidad, idMaterial, Fecha, Precio, IdContacto);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", movimientoESA.IdContacto);
        //    ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", movimientoESA.IdMaterial);
        //    ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", movimientoESA.IdTipoM);
        //    return View(movimientoESA);
        //}

        // GET: MovimientoESAs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction($"Create/{id}");
            //MovimientoESA movimientoESA = db.MovimientoESA.Find(id);
            //if (movimientoESA == null)
            //{
            //    return HttpNotFound();
            //}
            //ViewBag.IdContacto = new SelectList(db.Contactos, "IdContacto", "Contacto", movimientoESA.IdContacto);
            ////  ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", movimientoESA.IdMaterial);
            //ViewBag.IdTipoM = new SelectList(db.TiposMovimientos, "IdTipoM", "Movimiento", movimientoESA.IdTipoM);
            //return View(movimientoESA);
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
            //  ViewBag.IdMaterial = new SelectList(db.Materiales, "IdMaterial", "Material", movimientoESA.IdMaterial);
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
