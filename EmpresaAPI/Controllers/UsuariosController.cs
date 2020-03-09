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
    public class UsuariosController : ApiController
    {
        private DBModel db = new DBModel();

        
        // GET: api/Usuarios
        public IQueryable<Usuario> GetUsuarios()
        {
            return db.Usuarios;
        }

        // GET: api/Usuarios/5
       
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult GetUsuario(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        // PUT: api/Usuarios/5
        
        [ResponseType(typeof(void))]
        public IHttpActionResult PutUsuario(int id, Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != usuario.UsuarioId)
            {
                return BadRequest();
            }

            var nuevo = db.Usuarios.FirstOrDefault(u => u.Correo == usuario.Correo.ToLower());
            var anterior = db.Usuarios.FirstOrDefault(u => u.UsuarioId == id);

            if (anterior.Correo != usuario.Correo)
            {                
                if (nuevo != null)
                {
                    return BadRequest();
                }
            }

            anterior.Nombre = usuario.Nombre;
            anterior.Apellido = usuario.Apellido;
            anterior.Correo = usuario.Correo;
            anterior.Contraseña = usuario.Contraseña;
            anterior.Rol = usuario.Rol;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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

        // POST: api/Usuarios
        
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult PostUsuario(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var us = db.Usuarios.FirstOrDefault(u => u.Correo == usuario.Correo.ToLower());
            if (us == null)
            {
                db.Usuarios.Add(usuario);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = usuario.UsuarioId }, usuario);
            }
            else
            {
                return BadRequest("El email se encuentra en uso");
            }
            
        }

        // DELETE: api/Usuarios/5
       
        [ResponseType(typeof(Usuario))]
        public IHttpActionResult DeleteUsuario(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }

            db.Usuarios.Remove(usuario);
            db.SaveChanges();

            return Ok(usuario);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UsuarioExists(int id)
        {
            return db.Usuarios.Count(e => e.UsuarioId == id) > 0;
        }
    }
}