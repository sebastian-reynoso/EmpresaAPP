using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using EmpresaWEB.Models;

namespace EmpresaWEB.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Send(mvcLogin login)
        {
            HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Login/authentication", login).Result;
            return View(response);
        }
    }
}   