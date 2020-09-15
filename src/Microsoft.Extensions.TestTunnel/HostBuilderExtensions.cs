using System;
using Microsoft.Extensions.Hosting;
using System.Linq;
using TunnelR;

namespace Microsoft.AspNetCore.Hosting
{
    public static class HostBuilderExtensions
    {
         public static IHost ConfigureTestTunnel(this IHost host,
            Action<ITunnelBuilder> builderAction = null)
        {
            var ASPNETCORE_URLS = System.Environment.GetEnvironmentVariables()["ASPNETCORE_URLS"];

            if (ASPNETCORE_URLS == null)
            {
                throw new ApplicationException(Strings.NoAspNetCoreUrlSetting);
            }

            var rootUrl =
                ASPNETCORE_URLS.ToString().Split(';').FirstOrDefault(x => x.StartsWith("http:"))
                ?? ASPNETCORE_URLS.ToString().Split(';').First();

            var tunnelBuilder = new TunnelBuilder(host).WithRootUrl(rootUrl);

            builderAction?.Invoke(tunnelBuilder);

            return host;
        }
    }
}