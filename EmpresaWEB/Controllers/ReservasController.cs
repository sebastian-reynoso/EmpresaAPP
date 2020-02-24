using EmpresaWEB.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;

namespace EmpresaWEB.Controllers
{
    public class ReservasController : Controller
    {
        // GET: Reservas
        public ActionResult Index()
        {
            IEnumerable<mvcReserva> reservaList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Reservas").Result;
            reservaList = response.Content.ReadAsAsync<IEnumerable<mvcReserva>>(new List<MediaTypeFormatter> { new XmlMediaTypeFormatter(), new JsonMediaTypeFormatter() }).Result;
            return View(reservaList);
        }

        public ActionResult AddOrEdit(int id = 0)
        {


            if (id == 0)
            {
                mvcReserva mvcReserva = new mvcReserva();
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Paquetes/").Result;
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                mvcReserva.PaqueteCollection = JsonConvert.DeserializeObject<List<mvcPaquete>>(jsonString.Result);

                HttpResponseMessage response2 = GlobalVariables.WebApiClient.GetAsync("api/Usuarios/").Result;
                var jsonString2 = response2.Content.ReadAsStringAsync();
                jsonString2.Wait();
                mvcReserva.UsuarioCollection = JsonConvert.DeserializeObject<List<mvcUsuario>>(jsonString2.Result);
                return View(mvcReserva);
            }
            else
            {
                mvcReserva mvcReserva = new mvcReserva();
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Reservas/" + id.ToString()).Result;
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();
                mvcReserva = JsonConvert.DeserializeObject<mvcReserva>(jsonString.Result);

                HttpResponseMessage response2 = GlobalVariables.WebApiClient.GetAsync("api/Paquetes/").Result;
                var jsonString2 = response2.Content.ReadAsStringAsync();
                jsonString2.Wait();
                mvcReserva.PaqueteCollection = JsonConvert.DeserializeObject<List<mvcPaquete>>(jsonString2.Result);

                HttpResponseMessage response3 = GlobalVariables.WebApiClient.GetAsync("api/Usuarios/").Result;
                var jsonString3 = response3.Content.ReadAsStringAsync();
                jsonString3.Wait();
                mvcReserva.UsuarioCollection = JsonConvert.DeserializeObject<List<mvcUsuario>>(jsonString3.Result);

                return View(mvcReserva);
            }

        }

        [HttpPost]
        public ActionResult AddOrEdit(mvcReserva newReserva)
        {
            if (newReserva.ReservaId == 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Reservas", newReserva).Result;
                TempData["SuccessMessage"] = "Guardado Satisfactoriamente";
                return RedirectToAction("Index");
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Reservas/" + newReserva.ReservaId, newReserva).Result;
                TempData["SuccessMessage"] = "Actualizado Satisfactoriamente";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Reservas/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Eliminado Satisfactoriamente";
            return RedirectToAction("Index");
        }
    }
}