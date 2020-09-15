using System;
using Microsoft.Azure.Management.ApiManagement;
using Microsoft.Azure.Management.ApiManagement.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using System.Reflection;
using TunnelR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Hosting
{
    public static class TunnelBuilderApiManagementExtensions
    {
        public static ApiManagementClient ApiManagementClient { get; private set; }

        public static void UseAzureApiMangement(this ITunnelBuilderThatHasAnOpenApiDocumentEndpoint builder,
            AzureApiManagementCreateApiOptions options = null)
        {
            options ??= new AzureApiManagementCreateApiOptions();
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
                Path = $"{options.ApiId}/{CleanVersion(builder.Version)}",
                Format = ContentFormat.OpenapijsonLink,
                Value = publicSwaggerUrl,
                ServiceUrl = builder.RootUrl
            };

            // wait for the host to start to try to hit it
            builder.Host.Services.GetService<IHostApplicationLifetime>().ApplicationStarted.Register(() =>
            {
                ApiManagementClient.Api.CreateOrUpdate(
                    options.ResourceGroupName,
                    options.ApiManagementServiceName,
                    $"{options.ApiId}-{CleanVersion(builder.Version)}",
                    parms
                );
            });
        }

        static string CleanVersion(string version)
        {
            return version.Replace('.', '-');
        }
    }

    public class AzureApiManagementCreateApiOptions
    {
        public AzureApiManagementCreateApiOptions()
        {
            ApiManagementServiceName = Environment.GetEnvironmentVariable("AZURE_API_MANAGEMENT_SERVICE");
            ResourceGroupName = Environment.GetEnvironmentVariable("AZURE_RESOURCE_GROUP");
        }

        public string ApiManagementServiceName { get; set; }
        public string ResourceGroupName { get; set; }
        internal string ApiId { get; } = Assembly.GetEntryAssembly().GetName().Name;
    }
}