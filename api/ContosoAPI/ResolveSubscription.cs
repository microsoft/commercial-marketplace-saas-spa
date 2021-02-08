using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Marketplace.SaaS;
using System.Reflection;
using System.Threading.Tasks;

namespace Contoso.Api
{
    public static class ResolveSubscription
    {
        private static IConfiguration Configuration { set; get; }

        static ResolveSubscription()
        {

            var builder = new ConfigurationBuilder();
            builder
                   .AddUserSecrets(Assembly.GetExecutingAssembly(), false);
            Configuration = builder.Build();

        }

        [FunctionName("ResolveSubscription")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            // decodeURIComponent does not seem to be decoding spaces (%2b) with +. Replace here 
            // in case the page detects no token parameter, and asks the user for one.
            var marketplaceToken = req.Query["token"].ToString();

            var tenantId = Configuration["TenantId"];
            var appId = Configuration["AppId"];
            var clientSecret = Configuration["ClientSecret"];

            var creds = new ClientSecretCredential(Configuration["TenantId"], Configuration["AppId"], Configuration["ClientSecret"]);
            var marketplaceClient = new MarketplaceSaaSClient(creds);

            var subscriptionDetailsResponse = await marketplaceClient.Fulfillment.ResolveAsync(marketplaceToken).ConfigureAwait(false);

            string responseMessage = subscriptionDetailsResponse.Value == null
               ? "No subscription found."
               : System.Text.Json.JsonSerializer.Serialize(subscriptionDetailsResponse.Value);

            return new OkObjectResult(responseMessage);
        }
    }
}
