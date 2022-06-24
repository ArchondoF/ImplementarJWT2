using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Http
{
    public class HttpClientAccessor
    {
        private static Func<HttpClient> ValueFactory = () =>
        {
            return new HttpClient();
        };

        private static readonly Lazy<HttpClient> client = new Lazy<HttpClient>(ValueFactory);

        private HttpClientAccessor()
        {

        }

        public static HttpClient HttpClient
        {
            get { return client.Value; }
        }

    }
}
