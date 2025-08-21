namespace Streckenbuch.Components.Models;

public class StreckenModel
{
    public Guid Id { get; set; }

    public int Nummer { get; set; }

    public bool ShowDetails { get; set; }

    public List<StreckenKonfigurationenModel> Konfigurationen { get; set; } = new List<StreckenKonfigurationenModel>();
}
