using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace ProductApi
{
    public static class HostApplicationLifetimeTestTunnelExtensions
    {
        public static void UseApiTestTunnel(this IHostApplicationLifetime hostApplicationLifetime,
            IApplicationBuilder app,
            Action<IApiTestTunnelBuilder> buildTunnel = null)
        {
            var swaggerOptions = app.ApplicationServices.GetService<IOptions<SwaggerOptions>>();
            var docTemplate = swaggerOptions.Value.RouteTemplate;
            
            var swaggerGenOptions = app.ApplicationServices.GetService<IOptions<SwaggerGeneratorOptions>>();
            var openApiDoc = swaggerGenOptions.Value.SwaggerDocs.First();
            var version = openApiDoc.Value.Version;
            var swaggerEndpoint = docTemplate.Replace("{documentName}", openApiDoc.Key);

            var ASPNETCORE_URLS = System.Environment.GetEnvironmentVariables()["ASPNETCORE_URLS"];
            
            if(ASPNETCORE_URLS == null)
            {
                throw new ApplicationException(Strings.NoAspNetCoreUrlSetting);
            }

            var rootUrl = 
                ASPNETCORE_URLS.ToString().Split(';').FirstOrDefault(x => x.StartsWith("http:")) 
                ?? ASPNETCORE_URLS.ToString().Split(';').First();

            var builder = new ApiTestTunnelBuilder()
                        .WithRootUrl(rootUrl)
                        .WithOpenApiEndpoint(swaggerEndpoint)
                        .WithVersion(version);

            hostApplicationLifetime.ApplicationStarted.Register(() => 
            {
                buildTunnel?.Invoke(builder);
            });
        }
    }
}