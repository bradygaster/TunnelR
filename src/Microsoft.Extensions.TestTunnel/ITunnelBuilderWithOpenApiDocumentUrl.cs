using System;
using System.Collections.Generic;
using System.Text;

namespace TunnelR
{
    public interface ITunnelBuilderThatHasAnOpenApiDocumentEndpoint : ITunnelBuilder
    {
        string Version { get; set; }
        string OpenApiDocumentEndpoint { get; set;  }
    }
}
