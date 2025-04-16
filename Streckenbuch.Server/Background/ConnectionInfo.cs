using Grpc.Core;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Background;

public record ConnectionInfo
{
    public IServerStreamWriter<StartStreamResponse>? Stream { get; set; }
    
    public bool IsConnected { get; set; }
}
