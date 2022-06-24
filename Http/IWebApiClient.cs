using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Http
{
    public interface IWebApiClient
    {
        HttpResponseMessage Get(Uri adress, Dictionary<string, string> parameters);
        HttpResponseMessage Post<T>(Uri adress, T transferObject);
        HttpResponseMessage Delete<T>(Uri adress, T transferObject);
        HttpResponseMessage Put<T>(Uri adress, T transferObject);
        HttpResponseMessage Patch<T>(Uri adress, T transferObject);
    }
}
