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
    public class ReservasController : ApiController
    {
        
        private DBModel db = new DBModel();

        // GET: api/Reservas
        public IQueryable<Reserva> GetReservas()
        {
            return db.Reservas;
        }

        // GET: api/Reservas/5
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult GetReserva(int id)
        {
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return NotFound();
            }

            return Ok(reserva);
        }

        // PUT: api/Reservas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutReserva(int id, Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != reserva.ReservaId)
            {
                return BadRequest();
            }

            db.Entry(reserva).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReservaExists(id))
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

        // POST: api/Reservas
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult PostReserva(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Reservas.Add(reserva);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = reserva.ReservaId }, reserva);
        }

        // DELETE: api/Reservas/5
        [ResponseType(typeof(Reserva))]
        public IHttpActionResult DeleteReserva(int id)
        {
            Reserva reserva = db.Reservas.Find(id);
            if (reserva == null)
            {
                return NotFound();
            }

            db.Reservas.Remove(reserva);
            db.SaveChanges();

            return Ok(reserva);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ReservaExists(int id)
        {
            return db.Reservas.Count(e => e.ReservaId == id) > 0;
        }
    }
}