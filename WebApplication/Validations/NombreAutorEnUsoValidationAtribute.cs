using Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApplication.Validations
{
    public class NombreAutorEnUsoValidationAtribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                IWebApiClient apiClient = new WebApiClient(HttpClientAccessor.HttpClient);

                var parameters = new Dictionary<string, string>()
                {
                    ["name"] = value.ToString()
                };

                var webApiUrl = ConfigurationManager.AppSettings["WEB_API_ENDPOINT"];

                Uri consultaExisteAutor = new Uri(webApiUrl + "Autor/ExistAutorByName");

                var response = apiClient.Get(consultaExisteAutor, parameters);

                if (response.IsSuccessStatusCode)
                {
                    var existeAutor = JsonConvert.DeserializeObject<bool>(response.Content.ReadAsStringAsync().Result);

                    if (existeAutor)
                        return new ValidationResult("El nombre ya esta en uso");
                }
                else
                {
                    return new ValidationResult("No es posible validar el nombre del autor");
                }
            }

            return ValidationResult.Success;
        }
    }
}