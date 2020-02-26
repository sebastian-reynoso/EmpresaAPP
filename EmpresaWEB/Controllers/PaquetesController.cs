using EmpresaWEB.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EmpresaWEB.Controllers
{
    public class PaquetesController : Controller
    {
        // GET: Paquetes
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

        [HttpPost]
        public async Task<ActionResult> AddOrEdit(mvcPaquete newPaquete)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

            if (newPaquete.PaqueteId == 0)
            {
                var response = await client.PostAsJsonAsync("api/Paquetes", newPaquete);
                client.Dispose();
                TempData["SuccessMessage"] = "Guardado Satisfactoriamente";
                return RedirectToAction("Index");
            }
            else
            {
                var response = await client.PutAsJsonAsync("api/Paquetes/" + newPaquete.PaqueteId, newPaquete);
                client.Dispose();
                TempData["SuccessMessage"] = "Actualizado Satisfactoriamente";
                return RedirectToAction("Index");
            }
        }

        public async Task<ActionResult> Delete(int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

            var response = await client.DeleteAsync("api/Paquetes/" + id.ToString());
            TempData["SuccessMessage"] = "Eliminado Satisfactoriamente";
            return RedirectToAction("Index");
        }
    }
}