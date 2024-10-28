using Streckenbuch.Shared.Services;

namespace Streckenbuch.Client.Models.Fahren;

public class FahrenStreckenEntryModel
{
    public StreckenProto? SelectedStrecke { get; set; }

    public StreckenKonfigurationProto? SelectedKonfiguration { get; set; }

    public BetriebspunktProto? VonBetriebspunkt { get; set; }

    public BetriebspunktProto? BisBetriebspunkt { get; set; }

    public string? ErrorMessage { get; set; }
}
