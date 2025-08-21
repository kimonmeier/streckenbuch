namespace Streckenbuch.Components.Models;

public class StreckenKonfigurationenModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string VonBetriebspunkt { get; set; } = null!;

    public string BisBetriebspunkt { get; set; } = null!;
}
