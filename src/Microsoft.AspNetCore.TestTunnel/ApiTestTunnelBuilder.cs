namespace ProductApi
{
    public class ApiTestTunnelBuilder : IApiTestTunnelBuilder
    {
        string _rootUrl;
        string _openApiEndpoint;
        string _version;

        public string RootUrl => _rootUrl;
        public string OpenApiDocumentEndpoint => _openApiEndpoint;
        public string Version => _version;

        public IApiTestTunnelBuilder WithVersion(string version)
        {
            _version = version;
            return this;
        }

        public IApiTestTunnelBuilder WithRootUrl(string rootUrl)
        {
            _rootUrl = rootUrl;
            return this;
        }

        public IApiTestTunnelBuilder WithOpenApiEndpoint(string openApiEndpoint)
        {
            _openApiEndpoint = openApiEndpoint;
            return this;
        }
    }
}