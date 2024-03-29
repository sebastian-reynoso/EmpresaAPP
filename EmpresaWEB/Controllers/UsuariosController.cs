﻿using EmpresaWEB.Models;
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
using System.Threading;
using EmpresaWEB.AuthData;

namespace EmpresaWEB.Controllers
{
    public class UsuariosController : Controller
    {
        // GET: Usuarios
        [Auth]
        public async Task<ActionResult> Index()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());            
            IEnumerable<mvcUsuario> usuarioList;
            var response = await client.GetAsync("api/Usuarios");
            client.Dispose();
            if (response.IsSuccessStatusCode)
            {                
                usuarioList = response.Content.ReadAsAsync<IEnumerable<mvcUsuario>>(new List<MediaTypeFormatter> { new XmlMediaTypeFormatter(), new JsonMediaTypeFormatter() }).Result;
                return View(usuarioList);                
            }
            return View("Index", "Login");

        }

        //[HttpPut]
        [Auth]
        public async Task<ActionResult> AddOrEdit(int id = 0)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());
            

            if (id == 0)
            {
                client.Dispose();
                return View(new mvcUsuario());
            }
            else
            {
                //HttpResponseMessage response = GlobalVariables.WebApiClient.GetAsync("api/Usuarios/" + id.ToString()).Result;   
                var response = await client.GetAsync("api/Usuarios/" + id.ToString());
                client.Dispose();
                return View(response.Content.ReadAsAsync<mvcUsuario>().Result);

            }
            
        }

        [Auth]
        [HttpPost]
        public async Task AddOrEdit(mvcUsuario newUsuario)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Clear();
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["WebAPIURL"]);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Session["TokenNumber"].ToString());
            

            if (newUsuario.UsuarioId == 0)
            {
                //HttpResponseMessage response = GlobalVariables.WebApiClient.PostAsJsonAsync("api/Usuarios", newUsuario).Result;                
                var response = await client.PostAsJsonAsync("api/Usuarios", newUsuario);
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
                //HttpResponseMessage response = GlobalVariables.WebApiClient.PutAsJsonAsync("api/Usuarios/" + newUsuario.UsuarioId, newUsuario).Result;                
                var response = await client.PutAsJsonAsync("api/Usuarios/"+newUsuario.UsuarioId,newUsuario);
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


            //HttpResponseMessage response = GlobalVariables.WebApiClient.DeleteAsync("api/Usuarios/"+id.ToString()).Result;            
            var response = await client.DeleteAsync("api/Usuarios/" + id.ToString());
            client.Dispose();
            Thread.Sleep(1000);
            return RedirectToAction("Index");
            
        }
    }
}