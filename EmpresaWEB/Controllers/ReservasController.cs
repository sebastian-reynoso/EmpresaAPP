using EmpresaWEB.AuthData;
using EmpresaWEB.Models;
using Newtonsoft.Json;
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
    public class ReservasController : Controller
    {
        // GET: Reservas
        [Auth]
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());
            if (client.DefaultRequestHeaders.Authorization == null)
            {
                return RedirectToAction("Index", "Login");
            }
            IEnumerable<mvcReserva> reservaList;
            var response = await client.GetAsync("api/Reservas");
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {
                reservaList = response.Content.ReadAsAsync<IEnumerable<mvcReserva>>(new List<MediaTypeFormatter> { new XmlMediaTypeFormatter(), new JsonMediaTypeFormatter() }).Result;
                return View(reservaList);
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
            if (client.DefaultRequestHeaders.Authorization == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (id == 0)
            {
                mvcReserva mvcReserva = new mvcReserva();
                //HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Paquetes/").Result;
                var response = await client.GetAsync("api/Paquetes/");
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                mvcReserva.PaqueteCollection = JsonConvert.DeserializeObject<List<mvcPaquete>>(jsonString.Result);

                //HttpResponseMessage response2 = GlobalVariables.WebApiClient.GetAsync("api/Usuarios/").Result;
                var response2 = await client.GetAsync("api/Usuarios/");
                var jsonString2 = response2.Content.ReadAsStringAsync();
                jsonString2.Wait();
                mvcReserva.UsuarioCollection = JsonConvert.DeserializeObject<List<mvcUsuario>>(jsonString2.Result);
                client.Dispose();
                return View(mvcReserva);

                
            }
            else
            {
                mvcReserva mvcReserva = new mvcReserva();
                //HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Reservas/" + id.ToString()).Result;
                var response = client.GetAsync("api/Reservas/" + id.ToString()).Result;
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                mvcReserva = JsonConvert.DeserializeObject<mvcReserva>(jsonString.Result);

                //HttpResponseMessage response2 = GlobalVariables.WebApiClient.GetAsync("api/Paquetes/").Result;
                var response2 = client.GetAsync("api/Paquetes/").Result;
                var jsonString2 = response2.Content.ReadAsStringAsync();
                jsonString2.Wait();
                mvcReserva.PaqueteCollection = JsonConvert.DeserializeObject<List<mvcPaquete>>(jsonString2.Result);

                //HttpResponseMessage response3 = GlobalVariables.WebApiClient.GetAsync("api/Usuarios/").Result;
                var response3 = client.GetAsync("api/Usuarios/").Result;
                var jsonString3 = response3.Content.ReadAsStringAsync();
                jsonString3.Wait();
                mvcReserva.UsuarioCollection = JsonConvert.DeserializeObject<List<mvcUsuario>>(jsonString3.Result);
                client.Dispose();
                return View(mvcReserva);
            }

        }

        [Auth]
        [HttpPost]
        public async Task AddOrEdit(mvcReserva newReserva)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

            if (newReserva.ReservaId == 0)
            {
                //HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Reservas", newReserva).Result;
                var response = await client.PostAsJsonAsync("api/Reservas", newReserva);
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
                //HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Reservas/" + newReserva.ReservaId, newReserva).Result;
                var response = await client.PutAsJsonAsync("api/Reservas/" + newReserva.ReservaId, newReserva);
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

            //HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Reservas/" + id.ToString()).Result;
            var response = await client.DeleteAsync("api/Reservas/" + id.ToString());
            client.Dispose();
            Thread.Sleep(1000);
            return RedirectToAction("Index");
        }
    }
}