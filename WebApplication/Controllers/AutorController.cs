using Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class AutorController : Controller
    {
        // GET: Autores
        public ActionResult Consulta()
        {
            List<AutorModel> modelo = new List<AutorModel>();

            //Creamos una instancia del apiclient recibiendo una instancia unica para todo el sistema del HttpClient a taves del HttpClientAccessor
            IWebApiClient apiClient = new WebApiClient(HttpClientAccessor.HttpClient);

            //Creamos parametros para la query de la url
            var parameters = new Dictionary<string, string>();

            //Obtenemos url base desde el web.config de la aplicacion web
            var baseUrl = ConfigurationManager.AppSettings["WEB_API_ENDPOINT"];

            //Formamos url para el metodo de cosnsulta de autores
            Uri consultaAutorUri = new Uri($"{baseUrl}Autor/GetAutores");

            //Ejecutamos llamda GET
            var response = apiClient.Get(consultaAutorUri, parameters);

            if (response.IsSuccessStatusCode)
            {
                var autores = JsonConvert.DeserializeObject<List<AutorModel>>(response.Content.ReadAsStringAsync().Result);

                modelo = autores;
            }

            return View(modelo);
        }
        //GET: Autor
        public ActionResult GetAutor(long id)
        {
            IWebApiClient apiClient = new WebApiClient(HttpClientAccessor.HttpClient);

            List<AutorModel> colAutores = new List<AutorModel>();

            var parameters = new Dictionary<string, string>()
            {
                ["Id"] = id.ToString()
            };

            var baseUrl = ConfigurationManager.AppSettings["WEB_API_ENDPOINT"];

            Uri consultaAutorUri = new Uri(baseUrl + "Autor/GetAutorById");

            var response = apiClient.Get(consultaAutorUri, parameters);

            if (response.IsSuccessStatusCode)
            {
                var Autor = JsonConvert.DeserializeObject<AutorModel>(response.Content.ReadAsStringAsync().Result);
                colAutores.Add(Autor);

            }
            else
            {

            }


            return View(colAutores);
        }
        public ActionResult EditarAutor(long id)
        {
            IWebApiClient apiClient = new WebApiClient(HttpClientAccessor.HttpClient);

            List<AutorModel> colAutores = new List<AutorModel>();

            var parameters = new Dictionary<string, string>()
            {
                ["Id"] = id.ToString()
            };

            var baseUrl = ConfigurationManager.AppSettings["WEB_API_ENDPOINT"];

            Uri consultaAutorUri = new Uri(baseUrl + "Autor/GetAutorById");

            var response = apiClient.Get(consultaAutorUri, parameters);

            if (response.IsSuccessStatusCode)
            {
                var Autor = JsonConvert.DeserializeObject<AutorModel>(response.Content.ReadAsStringAsync().Result);
                colAutores.Add(Autor);

            }
            else
            {

            }


            return View(colAutores);
        }
        public ActionResult Creacion()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Creacion(AutorModel autorModel)
        {
            if (ModelState.IsValid)
            {
                //Instanciar apiclient para realizar llamdas HTTP a apis RESTful
                IWebApiClient apiClient = new WebApiClient(HttpClientAccessor.HttpClient);

                //Obtener url base desde web.config para la web api
                var webApiUrl = ConfigurationManager.AppSettings["WEB_API_ENDPOINT"];

                //Crear url para metodo de creacion de autores
                Uri crearAutorUri = new Uri(webApiUrl + "Autor/AddAutor");

                //Ejecutar llamda http post y obtener respuesta
                var response = apiClient.Post(crearAutorUri, autorModel);

                if (!response.IsSuccessStatusCode)
                {
                    ModelState.AddModelError("Error de API", "Ocurrio un error al intentar crear el autor en el servidor remoto");
                }
                else
                {
                    return RedirectToAction("Consulta");
                }
            }

            return View(autorModel);
        }

    }
}