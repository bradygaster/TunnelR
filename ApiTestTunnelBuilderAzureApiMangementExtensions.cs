using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using Azure.Identity;
using Microsoft.Azure.Management.ApiManagement;
using Microsoft.Azure.Management.ApiManagement.Models;
using Microsoft.Rest;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.Reflection;

namespace ProductApi
{
    public static class ApiTestTunnelBuilderAzureApiMangementExtensions
    {
        public static ApiManagementClient ApiManagementClient { get; private set; }

        public static void UseAzureApiMangement(this IApiTestTunnelBuilder builder,
            AzureApiManagementCreateApiOptions options)
        {
            var publicSwaggerUrl = $"{builder.RootUrl}/{builder.OpenApiDocumentEndpoint}";
            var subscriptionId = Environment.GetEnvironmentVariable("AZURE_SUBSCRIPTION_ID");
            var clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID");
            var clientSecret = Environment.GetEnvironmentVariable("AZURE_CLIENT_SECRET");
            var tenantId = Environment.GetEnvironmentVariable("AZURE_TENANT_ID");

            var credentials = SdkContext.AzureCredentialsFactory
                .FromServicePrincipal(clientId,
                    clientSecret,
                    tenantId,
                    AzureEnvironment.AzureGlobalCloud)
                .WithDefaultSubscription(subscriptionId);

            ApiManagementClient = new ApiManagementClient(credentials);
            ApiManagementClient.SubscriptionId = subscriptionId;

            ApiCreateOrUpdateParameter parms = new ApiCreateOrUpdateParameter
            {
                Path = $"{options.ApiId}/{builder.Version}",
                Format = ContentFormat.SwaggerLinkJson,
                Value = publicSwaggerUrl,
                ServiceUrl = builder.RootUrl
            };

            ApiManagementClient.Api.CreateOrUpdate(
                options.ResourceGroupName,
                options.ApiManagementServiceName,
                $"{options.ApiId}-{builder.Version}",
                parms
            );

            Console.WriteLine(publicSwaggerUrl);
        }
    }

    public class AzureApiManagementCreateApiOptions
    {
        public string ApiManagementServiceName { get; set; }
        public string ResourceGroupName { get; set; }
        internal string ApiId { get; } = Assembly.GetExecutingAssembly().GetName().Name;
    }
}