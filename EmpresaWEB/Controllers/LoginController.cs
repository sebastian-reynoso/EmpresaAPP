using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using EmpresaWEB.Models;
using Newtonsoft.Json;

namespace EmpresaWEB.Controllers
{
    public class LoginController : Controller
    {
        public bool message = false;
        // GET: Login
        public ActionResult Index()
        {
            
            TempData["ErrorMessage"] = "Correo o Contraseña Incorrecto";          

            message = false;
            return View(new mvcLogin());
        }

        [HttpPost]
        public async Task Send(string Correo, string Contraseña)
        {
            mvcLogin login = new mvcLogin();
            login.Correo = Correo;
            login.Contraseña = Contraseña;

            var tokenBased = string.Empty;
            var client = new HttpClient();
            
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await client.PostAsJsonAsync("/api/Login/authenticate", login);
            if (response.IsSuccessStatusCode)
            {
                var resultMessage = response.Content.ReadAsStringAsync().Result;
                tokenBased = JsonConvert.DeserializeObject<string>(resultMessage);
                Session["TokenNumber"] = tokenBased;
                client.Dispose();
                message = false;
                ViewBag.message = "Bearer " + tokenBased;
                HttpContext.Response.StatusCode = 200;
            }
            else
            {
                HttpContext.Response.StatusCode = 401;
            }
            client.Dispose();          
            
        }

        public ActionResult Send()
        {            
            return View();
        }


    }
}   