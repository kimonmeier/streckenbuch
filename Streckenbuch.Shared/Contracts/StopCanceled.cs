using System.Text.Json.Serialization;

namespace Streckenbuch.Shared.Contracts;

public class StopCanceled : IRequest<Unit>
{
    [JsonPropertyName("betriebspunktId")]
    public required Guid BetriebspunktId { get; set; }
    
    [JsonPropertyName("reason")]
    public required string Reason { get; set; }
}