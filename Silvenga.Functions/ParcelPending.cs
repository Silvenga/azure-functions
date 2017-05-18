using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using AngleSharp.Parser.Html;

using JetBrains.Annotations;

using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Silvenga.Functions
{
    public static class ParcelPending
    {
        [FunctionName("ParseParcelPending"), UsedImplicitly]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestMessage req,
                                                          TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            var text = (string) data.email;

            text = Regex.Replace(text, "<br>", "\n");

            var parser = new HtmlParser();
            var document = parser.Parse(text).Body.TextContent;

            var parsedResults = new
            {
                RecipientName = ParseFirstGroup(document, "Dear (.+?):"),
                RecipientLocation = ParseFirstGroup(document, @"Oh happy day, you had a parcel delivered to (.+?)\."),
                PickupDate = ParseFirstGroup(document, @"pick it up by midnight on ([\d-]{10})\."),
                AccessCode = ParseFirstGroup(document, @"Your access code is ([\d]{6})"),
                LockerLocation = ParseFirstGroup(document, @"Your locker is located: (.+) -"),
                LockerNumber = ParseFirstGroup(document, @"Your Locker number is: (\d+)"),
                PackageBarcode = ParseFirstGroup(document, @"Your tracking #\/barcode is: (.+)"),
                PackageCarrier = ParseFirstGroup(document, @"Your package was delivered by (.+)"),
            };

            return req.CreateResponse(HttpStatusCode.OK, parsedResults);
        }

        private static string ParseFirstGroup(string input, string regex)
        {
            if (!Regex.IsMatch(input, regex))
            {
                return null;
            }

            var value = Regex.Match(input, regex).Groups[1].Value;
            return value.Trim();
        }
    }
}