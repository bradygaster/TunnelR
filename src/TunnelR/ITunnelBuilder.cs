using Microsoft.Extensions.Hosting;

namespace TunnelR
{
    public interface ITunnelBuilder
    {
        string RootUrl { get; }
        ITunnelBuilder WithRootUrl(string rootUrl);
        IHost Host { get; }
    }
}