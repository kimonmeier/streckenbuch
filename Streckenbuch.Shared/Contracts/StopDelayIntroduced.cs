using System.Text.Json.Serialization;

namespace Streckenbuch.Shared.Contracts;

public class StopDelayIntroduced : IRequest<Unit>
{
    [JsonPropertyName("betriebspunktId")]
    public required Guid BetriebspunktId { get; set; }
    
    [JsonPropertyName("minutesDelayed")]
    public required int MinutesDelayed { get; set; }
    
    [JsonPropertyName("reason")]
    public required string Reason { get; set; }
}