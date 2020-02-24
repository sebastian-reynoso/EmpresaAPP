using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using EmpresaAPI.Models;

namespace EmpresaAPI.Controllers
{
    [Authorize]
    public class PaquetesController : ApiController
    {   
        private DBModel db = new DBModel();

        // GET: api/Paquetes
        public IQueryable<Paquete> GetPaquetes()
        {
            return db.Paquetes;
        }

        // GET: api/Paquetes/5
        [ResponseType(typeof(Paquete))]
        public IHttpActionResult GetPaquete(int id)
        {
            Paquete paquete = db.Paquetes.Find(id);
            if (paquete == null)
            {
                return NotFound();
            }

            return Ok(paquete);
        }

        // PUT: api/Paquetes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPaquete(int id, Paquete paquete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != paquete.PaqueteId)
            {
                return BadRequest();
            }

            db.Entry(paquete).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaqueteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Paquetes
        [ResponseType(typeof(Paquete))]
        public IHttpActionResult PostPaquete(Paquete paquete)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Paquetes.Add(paquete);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = paquete.PaqueteId }, paquete);
        }

        // DELETE: api/Paquetes/5
        [ResponseType(typeof(Paquete))]
        public IHttpActionResult DeletePaquete(int id)
        {
            Paquete paquete = db.Paquetes.Find(id);
            if (paquete == null)
            {
                return NotFound();
            }

            db.Paquetes.Remove(paquete);
            db.SaveChanges();

            return Ok(paquete);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PaqueteExists(int id)
        {
            return db.Paquetes.Count(e => e.PaqueteId == id) > 0;
        }
    }
}