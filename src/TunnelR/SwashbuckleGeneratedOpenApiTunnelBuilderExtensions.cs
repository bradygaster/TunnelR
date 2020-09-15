using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using TunnelR;

namespace Microsoft.AspNetCore.Hosting
{
    public static class SwashbuckleGeneratedOpenApiTunnelBuilderExtensions
    {
        public static ITunnelBuilderThatHasAnOpenApiDocumentEndpoint UseSwashbuckleOpenApiEndpoint(
            this ITunnelBuilder tunnelBuilder,
            Action<ITunnelBuilderThatHasAnOpenApiDocumentEndpoint> buildTunnel = null)
        {
            var swaggerOptions = tunnelBuilder.Host.Services.GetService<IOptions<SwaggerOptions>>();
            var docTemplate = swaggerOptions.Value.RouteTemplate;

            var swaggerGenOptions = tunnelBuilder.Host.Services.GetService<IOptions<SwaggerGeneratorOptions>>();
            var openApiDoc = swaggerGenOptions.Value.SwaggerDocs.First();
            var version = openApiDoc.Value.Version;
            var swaggerEndpoint = docTemplate.Replace("{documentName}", openApiDoc.Key);

            var result = new SwashbuckleGeneratedOpenApiTunnelBuilder(tunnelBuilder)
            {
                OpenApiDocumentEndpoint = swaggerEndpoint,
                Version = version
            };

            buildTunnel?.Invoke(result);

            return result;
        }
    }
}
