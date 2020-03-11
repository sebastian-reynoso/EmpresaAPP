using EmpresaWEB.AuthData;
using EmpresaWEB.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmpresaWEB.Controllers
{
    public class PaquetesController : Controller
    {
        // GET: Paquetes
        [Auth]
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());
            IEnumerable<mvcPaquete> paqueteList;
            var response = await client.GetAsync("api/Paquetes");
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                paqueteList = response.Content.ReadAsAsync<IEnumerable<mvcPaquete>>(new List<MediaTypeFormatter> { new XmlMediaTypeFormatter(), new JsonMediaTypeFormatter() }).Result;
                return View(paqueteList);
            }
            return View("Index", "Login");
        }

        [Auth]
        public async Task<ActionResult> AddOrEdit(int id = 0)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

            if (id == 0)
            {
                client.Dispose();
                return View(new mvcPaquete());
            }
            else
            {
                var response = await client.GetAsync("api/Paquetes/" + id.ToString());
                client.Dispose();
                return View(response.Content.ReadAsAsync<mvcPaquete>().Result);

            }

        }

        [Auth]
        [HttpPost]
        public async Task AddOrEdit(mvcPaquete newPaquete)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

            if (newPaquete.PaqueteId == 0)
            {
                var response = await client.PostAsJsonAsync("api/Paquetes", newPaquete);
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Response.StatusCode = 200;
                }
                else
                {
                    HttpContext.Response.StatusCode = 402;
                }
                client.Dispose();
            }
            else
            {
                var response = await client.PutAsJsonAsync("api/Paquetes/" + newPaquete.PaqueteId, newPaquete);
                if (response.IsSuccessStatusCode)
                {
                    HttpContext.Response.StatusCode = 201;
                }
                else
                {
                    HttpContext.Response.StatusCode = 400;
                }
                client.Dispose();
            }
        }

        [Auth]
        public async Task<ActionResult> Delete(int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

            var response = await client.DeleteAsync("api/Paquetes/" + id.ToString());
            client.Dispose();
            Thread.Sleep(1000);
            return RedirectToAction("Index");
        }
    }
}