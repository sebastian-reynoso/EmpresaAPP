using EmpresaWEB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Threading.Tasks;

namespace EmpresaWEB.Controllers
{
    public class UsuariosController : Controller
    {
         // GET: Usuarios
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());           
            IEnumerable<mvcUsuario> usuarioList;
            var response = await client.GetAsync("api/Usuarios");      
            if (response.IsSuccessStatusCode)
            {                
                usuarioList = response.Content.ReadAsAsync<IEnumerable<mvcUsuario>>(new List<MediaTypeFormatter> { new XmlMediaTypeFormatter(), new JsonMediaTypeFormatter() }).Result;
                return View(usuarioList);                
            }
            return View("Index", "Login");

        }

        //[HttpPut]
        public ActionResult AddOrEdit(int id = 0)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

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
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

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
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());

            HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Usuarios/"+id.ToString()).Result;            
            TempData["SuccessMessage"] = "Eliminado Satisfactoriamente";
            return RedirectToAction("Index");
        }
    }
}