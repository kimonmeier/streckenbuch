using System.Text.Json.Serialization;

namespace Streckenbuch.Miku.Models.Fahrten;

public class Abfahrtszeiten
{
    [JsonPropertyName("verspaetung")]
    public int Verspaetung { get; set; }

    [JsonPropertyName("verspaetungPrefix")]
    public string VerspaetungPrefix { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
}

public class Ankunftszeiten
{
    [JsonPropertyName("verspaetung")]
    public int Verspaetung { get; set; }

    [JsonPropertyName("verspaetungPrefix")]
    public string VerspaetungPrefix { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }
}

public class MehrsprachigerText
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("de")]
    public string De { get; set; }

    [JsonPropertyName("fr")]
    public string Fr { get; set; }

    [JsonPropertyName("it")]
    public string It { get; set; }
}

public class BetriebspunktAusfall
{
    [JsonPropertyName("bpUic")]
    public int BpUic { get; set; }

    [JsonPropertyName("bezOff")]
    public string BezOff { get; set; }

    [JsonPropertyName("abk")]
    public string Abk { get; set; }

    [JsonPropertyName("qosBitfeld")]
    public int QosBitfeld { get; set; }
}

public class DetailAusfall
{
    [JsonPropertyName("richtung")]
    public string Richtung { get; set; }

    [JsonPropertyName("bp")]
    public BetriebspunktAusfall Betriebspunkt { get; set; }
}

public class Ersatz
{
    [JsonPropertyName("detail")]
    public DetailAusfall Detail { get; set; }

    [JsonPropertyName("fahrt")]
    public FahrtAusfall Fahrt { get; set; }
}

public class FahrtAusfall
{
    [JsonPropertyName("fahrtId")]
    public FahrtId FahrtId { get; set; }
}

public class Fahrt
{
    [JsonPropertyName("fahrtId")]
    public FahrtId FahrtId { get; set; }

    [JsonPropertyName("ersatz")]
    public Ersatz? Ersatz { get; set; }

    [JsonPropertyName("zuglaeufe")]
    public List<Zuglauf> Zuglaeufe { get; } = new List<Zuglauf>();
}

public class FahrtId
{
    [JsonPropertyName("fahrtBezeichner")]
    public string FahrtBezeichner { get; set; }

    [JsonPropertyName("betriebstag")]
    public string Betriebstag { get; set; }
}

public class Haltestellen
{
    [JsonPropertyName("bpUic")]
    public int BetriebspunktId { get; set; }

    [JsonPropertyName("abk")]
    public string Abkuerzung { get; set; }

    [JsonPropertyName("bezeichnung")]
    public string Bezeichnung { get; set; }

    [JsonPropertyName("flags")]
    public List<Flags> Flags { get; } = new List<Flags>();

    [JsonPropertyName("abfahrtszeiten")]
    public Abfahrtszeiten Abfahrtszeiten { get; set; }

    [JsonPropertyName("ankunftszeiten")]
    public Ankunftszeiten Ankunftszeiten { get; set; }

    [JsonPropertyName("verspaetungsgrund")]
    public MehrsprachigerText? Verspaetungsgrund { get; set; }

    [JsonPropertyName("ausfallgrund")]
    public MehrsprachigerText? Ausfallgrund { get; set; }

    [JsonPropertyName("ersatz")]
    public bool? Ersatz { get; set; }
}

public class FahrtenRoot
{
    [JsonPropertyName("serverZeit")]
    public DateTime ServerZeit { get; set; }

    [JsonPropertyName("fahrten")]
    public List<Fahrt> Fahrten { get; } = new List<Fahrt>();
}

public class Zuglauf
{
    [JsonPropertyName("fahrtId")]
    public FahrtId FahrtId { get; set; }

    [JsonPropertyName("haltestellen")]
    public List<Haltestellen> Haltestellen { get; } = new List<Haltestellen>();
}