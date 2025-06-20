using System.Text.Json.Serialization;

namespace Streckenbuch.Shared.Contracts;

public class StopsLoaded : IRequest<Unit>
{
    [JsonPropertyName("betriebspunkteId")]
    [JsonIgnore(Condition = JsonIgnoreCondition.Never)]
    public required List<Guid> BetriebspunkteId { get; set; } = new List<Guid>();
}