using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;

namespace ProductApi
{
    public static class ApiTestTunnelBuilderNGrokExtensions
    {
        public static IApiTestTunnelBuilder UseNGrok(this IApiTestTunnelBuilder builder,
            string tunnelApiUrl = "http://localhost:4040/api/tunnels")
        {
            var nswagTunnelRootUrl = string.Empty;
            
            using(var nswagApiClient = new HttpClient())
            {
                var json = nswagApiClient.GetStringAsync(tunnelApiUrl).Result;
                var ngrokTunnelResponse = JsonSerializer.Deserialize<NGrokTunnelApiResponse>(json);
                var tunnel = ngrokTunnelResponse.Tunnels.FirstOrDefault(x => x.Protocol == "https") 
                    ?? ngrokTunnelResponse.Tunnels.First();

                builder.WithRootUrl(tunnel.PublicUrl);
            }

            return builder;
        }
    }

    public class NGrokTunnelApiResponse
    {
        [JsonPropertyName("tunnels")]
        public NGrokTunnel[] Tunnels { get; set; }
    }

    public class NGrokTunnel
    {
        [JsonPropertyName("proto")]
        public string Protocol { get; set; }

        [JsonPropertyName("public_url")]
        public string PublicUrl { get; set; }
    }
}