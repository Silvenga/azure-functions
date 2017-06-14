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
            var text = await req.Content.ReadAsStringAsync();
            var parsedResults = Parse(text);

            return req.CreateResponse(HttpStatusCode.OK, parsedResults);
        }

        public class ParsedResult
        {
            public string RecipientName { get; set; }
            public string RecipientLocation { get; set; }
            public string PickupDate { get; set; }
            public string AccessCode { get; set; }
            public string LockerLocation { get; set; }
            public string LockerNumber { get; set; }
            public string PackageBarcode { get; set; }
            public string PackageCarrier { get; set; }
        }

        public static ParsedResult Parse(string text)
        {
            text = Regex.Replace(text, "<br>", "\n");

            var parser = new HtmlParser();
            var document = parser.Parse(text).Body.TextContent;

            var parsedResults = new ParsedResult
            {
                RecipientName = ParseFirstGroup(document, "Dear (.+?):"),
                RecipientLocation = ParseFirstGroup(document, @"Oh happy day, you had a parcel delivered to ([\w\W]+?)\."),
                PickupDate = ParseFirstGroup(document, @"([\d-]{10})\."),
                AccessCode = ParseFirstGroup(document, @"Your access code is ([\d]{6})"),
                LockerLocation = ParseFirstGroup(document, @"Your locker is located: (.+) -"),
                LockerNumber = ParseFirstGroup(document, @"Your Locker number is: (\d+)"),
                PackageBarcode = ParseFirstGroup(document, @"Your tracking #\/barcode is: (.+)"),
                PackageCarrier = ParseFirstGroup(document, @"Your package was delivered by (.+)")
            };

            if (parsedResults.AccessCode != null)
            {
                var code = parsedResults.AccessCode;
                parsedResults.AccessCode = $"{code.Substring(0, 3)} - {code.Substring(3, 3)}";
            }

            return parsedResults;
        }

        private static string ParseFirstGroup(string input, string regex)
        {
            if (!Regex.IsMatch(input, regex))
            {
                return null;
            }

            var value = Regex.Match(input, regex).Groups[1].Value;
            return Regex.Replace(value.Trim(), "\r?\n", "");
        }
    }
}