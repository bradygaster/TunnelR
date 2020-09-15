using Microsoft.Extensions.Hosting;

namespace TunnelR
{
    public class TunnelBuilder : ITunnelBuilder
    {
        string _rootUrl;
        string _openApiEndpoint;
        string _version;

        public TunnelBuilder(IHost host)
        {
            Host = host;
        }

        public string RootUrl => _rootUrl;
        public string OpenApiDocumentEndpoint => _openApiEndpoint;
        public string Version => _version;
        public IHost Host { get; set; }

        public ITunnelBuilder WithVersion(string version)
        {
            _version = version;
            return this;
        }

        public ITunnelBuilder WithRootUrl(string rootUrl)
        {
            _rootUrl = rootUrl;
            return this;
        }

        public ITunnelBuilder WithOpenApiEndpoint(string openApiEndpoint)
        {
            _openApiEndpoint = openApiEndpoint;
            return this;
        }
    }
}