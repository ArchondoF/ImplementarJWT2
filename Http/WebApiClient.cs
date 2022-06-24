using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Http
{
    public class WebApiClient : IWebApiClient
    {
        private readonly HttpClient _client;

        public WebApiClient(HttpClient client)
        {
            this._client = client;
        }

        public HttpResponseMessage Get(Uri adress, Dictionary<string, string> parameters)
        {
            var queryParameters = new List<string>();

            foreach (var parameter in parameters)
            {
                queryParameters.Add($"{parameter.Key}={parameter.Value}");
            }

            var query = $"?{string.Join("&", queryParameters)}";

            var adressWithQuery = new Uri(adress, query);

            return this._client.GetAsync(adressWithQuery).Result;
        }

        public HttpResponseMessage Post<T>(Uri adress, T transferObject)
        {
            this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, adress);

            request.Content = new StringContent(JsonConvert.SerializeObject(transferObject), Encoding.UTF8, "application/json");

            return this._client.SendAsync(request).Result;
        }

        public HttpResponseMessage Delete<T>(Uri adress, T transferObject)
        {
            this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Delete, adress);

            request.Content = new StringContent(JsonConvert.SerializeObject(transferObject), Encoding.UTF8, "application/json");

            return this._client.SendAsync(request).Result;
        }

        public HttpResponseMessage Put<T>(Uri adress, T transferObject)
        {
            this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Put, adress);

            request.Content = new StringContent(JsonConvert.SerializeObject(transferObject), Encoding.UTF8, "application/json");

            return this._client.SendAsync(request).Result;
        }

        public HttpResponseMessage Patch<T>(Uri adress, T transferObject)
        {
            this._client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), adress);

            request.Content = new StringContent(JsonConvert.SerializeObject(transferObject), Encoding.UTF8, "application/json");

            return this._client.SendAsync(request).Result;
        }
    }
}
