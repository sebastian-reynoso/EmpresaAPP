using EmpresaWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;

namespace EmpresaWEB.Controllers
{
    public class PaquetesController : Controller
    {
        // GET: Paquetes
        public ActionResult Index()
        {
            IEnumerable<mvcPaquete> paqueteList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Paquetes").Result;
            paqueteList = response.Content.ReadAsAsync<IEnumerable<mvcPaquete>>(new List<MediaTypeFormatter> { new XmlMediaTypeFormatter(), new JsonMediaTypeFormatter() }).Result;
            return View(paqueteList);
        }

        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new mvcPaquete());
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Paquetes/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<mvcPaquete>().Result);

            }

        }

        [HttpPost]
        public ActionResult AddOrEdit(mvcPaquete newPaquete)
        {
            if (newPaquete.PaqueteId == 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Paquetes", newPaquete).Result;
                TempData["SuccessMessage"] = "Guardado Satisfactoriamente";
                return RedirectToAction("Index");
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Paquetes/" + newPaquete.PaqueteId, newPaquete).Result;
                TempData["SuccessMessage"] = "Actualizado Satisfactoriamente";
                return RedirectToAction("Index");
            }
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Paquetes/" + id.ToString()).Result;
            TempData["SuccessMessage"] = "Eliminado Satisfactoriamente";
            return RedirectToAction("Index");
        }
    }
}