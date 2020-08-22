namespace ProductApi
{
    public interface IApiTestTunnelBuilder
    {
        string RootUrl { get; }
        string OpenApiDocumentEndpoint { get; }
         IApiTestTunnelBuilder WithVersion(string version);
         IApiTestTunnelBuilder WithRootUrl(string rootUrl);
         IApiTestTunnelBuilder WithOpenApiEndpoint(string openApiEndpoint);
    }
}