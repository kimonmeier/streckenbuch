using System.Text.Json.Serialization;

namespace Streckenbuch.Shared.Contracts;

public class StopRemoved : IRequest<Unit>
{
    
    [JsonPropertyName("betriebspunktId")]
    public required Guid BetriebspunktId { get; set; }
}