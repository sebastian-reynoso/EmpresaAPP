using EmpresaAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;

namespace EmpresaAPI.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/login")]
    public class LoginController : ApiController
    {
        private DBModel db = new DBModel();

        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {
            return Ok(true);
        }

        [HttpGet]
        [Route("echouser")]
        public IHttpActionResult EchoUser()
        {
            var identity = Thread.CurrentPrincipal.Identity;
            return Ok($" IPrincipal-user: {identity.Name} - IsAuthenticated: {identity.IsAuthenticated}");
        }

        [HttpPost]
        [Route("authenticate")]
        public IHttpActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            //TODO: Validate credentials Correctly, this code is only for demo !!
            Usuario usuario = db.Usuarios.SingleOrDefault(d => d.Correo == login.Correo);
            bool isCredentialValid = false;
            if (usuario != null)
                isCredentialValid = (login.Contraseña == usuario.Contraseña);
            
            if (isCredentialValid)
            {
                var token = TokenGenerator.GenerateTokenJwt(login.Correo);                
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}
