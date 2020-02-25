using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using EmpresaWEB.Models;
using Newtonsoft.Json;

namespace EmpresaWEB.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View(new mvcLogin());
        }

        [HttpPost]
        public ActionResult Send(string Correo, string Contraseña)
        {
            mvcLogin login = new mvcLogin();
            login.Correo = Correo;
            login.Contraseña = Contraseña;
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Login/authenticate", login).Result;
            var jsonString = response.Content.ReadAsStringAsync();
            jsonString.Wait();
            ViewBag.message = "Bearer " + jsonString.Result;
            return View();
        }
    }
}   