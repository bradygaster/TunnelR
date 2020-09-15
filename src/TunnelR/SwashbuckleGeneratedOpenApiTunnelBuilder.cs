using Microsoft.Extensions.Hosting;

namespace TunnelR
{
    public class SwashbuckleGeneratedOpenApiTunnelBuilder :
        ITunnelBuilderThatHasAnOpenApiDocumentEndpoint
    {
        private ITunnelBuilder _tunnelBuilder;
        private string _version;
        private string _openApiEndpoint;

        public SwashbuckleGeneratedOpenApiTunnelBuilder(ITunnelBuilder tunnelBuilder)
        {
            _tunnelBuilder = tunnelBuilder;
        }

        public string Version { get; set; }

        public string OpenApiDocumentEndpoint { get; set; }

        public string RootUrl => _tunnelBuilder.RootUrl;

        public IHost Host => _tunnelBuilder.Host;

        public ITunnelBuilder WithRootUrl(string rootUrl)
        {
            return _tunnelBuilder.WithRootUrl(rootUrl);
        }
    }
}
