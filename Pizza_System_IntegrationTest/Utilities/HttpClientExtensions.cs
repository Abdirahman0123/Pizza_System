using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Pizza_System_IntegrationTest.Utilities
{
    public static class HttpClientExtensions
    {
        public static HttpClient WithJwtBearerToken(this HttpClient client, Action<TestJwtToken> configure)
        {
            var token = new TestJwtToken();
            configure(token);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Build());
            return client;
        }
    }
}
