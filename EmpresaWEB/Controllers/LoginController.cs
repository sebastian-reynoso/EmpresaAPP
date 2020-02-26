using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
            if (message == true)
            {
                TempData["ErrorMessage"] = "Correo o Contraseña Incorrecto";
                message = false;
            }
            return View(new mvcLogin());
        }

        [HttpPost]
        public async Task<ActionResult> Send(string Correo, string Contraseña)
        {
            mvcLogin login = new mvcLogin();
            login.Correo = Correo;
            login.Contraseña = Contraseña;

            var tokenBased = string.Empty;
            var client = new HttpClient();

            /*client.DefaultRequestHeaders.Clear();
            //client.BaseAddress = ;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var rq = new HttpRequestMessage
            {
                RequestUri = new Uri(ConfigurationManager.AppSettings["WebAPIURL"] + "/api/Login/authenticate"),
                Method = HttpMethod.Post,
                Content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json")
            };
            using(var response = await client.SendAsync(rq))
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    tokenBased = JsonConvert.DeserializeObject<string>(jsonString);
                    Session["TokenNumber"] = tokenBased;
                }
            }
            client.Dispose();*/

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
                return View();
            }
            client.Dispose();
            message = true;
            return RedirectToAction("Index");

            


            
        }
    }
}   