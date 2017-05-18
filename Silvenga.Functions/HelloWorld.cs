using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

using JetBrains.Annotations;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Silvenga.Functions
{
    public static class HelloWorld
    {
        [FunctionName("HelloWorld"), UsedImplicitly]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
                                                          TraceWriter log)
        {
            return req.CreateResponse(HttpStatusCode.OK, $"Hello World! {DateTimeOffset.Now}");
        }
    }
}