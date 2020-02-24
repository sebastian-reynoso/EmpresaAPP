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
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        public ActionResult Index()
        {
            IEnumerable<mvcUsuario> usuarioList;
            HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Usuarios").Result;
            usuarioList = response.Content.ReadAsAsync<IEnumerable<mvcUsuario>>(new List<MediaTypeFormatter>{new XmlMediaTypeFormatter(),new JsonMediaTypeFormatter()}).Result;
            return View(usuarioList);
        }

        //[HttpPut]
        public ActionResult AddOrEdit(int id = 0)
        {
            if (id == 0)
            {
                return View(new mvcUsuario());
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Usuarios/" + id.ToString()).Result;
                return View(response.Content.ReadAsAsync<mvcUsuario>().Result);

            }
            
        }

        [HttpPost]
        public ActionResult AddOrEdit(mvcUsuario newUsuario)
        {
            if (newUsuario.UsuarioId == 0)
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Usuarios", newUsuario).Result;
                TempData["SuccessMessage"] = "Guardado Satisfactoriamente";
                return RedirectToAction("Index");
            }
            else
            {
                HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Usuarios/" + newUsuario.UsuarioId, newUsuario).Result;
                TempData["SuccessMessage"] = "Actualizado Satisfactoriamente";
                return RedirectToAction("Index");
            }            
        }

        public ActionResult Delete(int id)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Usuarios/"+id.ToString()).Result;
            TempData["SuccessMessage"] = "Eliminado Satisfactoriamente";
            return RedirectToAction("Index");
        }
    }
}