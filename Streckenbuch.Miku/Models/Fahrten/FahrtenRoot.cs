using System.Text.Json.Serialization;

namespace Streckenbuch.Miku.Models.Fahrten;

public class Abfahrtszeiten : IEquatable<Abfahrtszeiten>
{
    [JsonPropertyName("verspaetung")]
    public int Verspaetung { get; set; }

    [JsonPropertyName("verspaetungPrefix")]
    public string VerspaetungPrefix { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }

    public bool Equals(Abfahrtszeiten? other)
    {
        if (other is null) return false;
        return Verspaetung == other.Verspaetung && 
               VerspaetungPrefix == other.VerspaetungPrefix &&
               Status == other.Status;
    }
}

public class Ankunftszeiten : IEquatable<Ankunftszeiten>
{
    [JsonPropertyName("verspaetung")]
    public int Verspaetung { get; set; }

    [JsonPropertyName("verspaetungPrefix")]
    public string VerspaetungPrefix { get; set; }
    
    [JsonPropertyName("status")]
    public string Status { get; set; }

    public bool Equals(Ankunftszeiten? other)
    {
        if (other is null) return false;
        return Verspaetung == other.Verspaetung &&
               VerspaetungPrefix == other.VerspaetungPrefix &&
               Status == other.Status;
    }
}

public class MehrsprachigerText : IEquatable<MehrsprachigerText>
{
    [JsonPropertyName("id")]
    public string Id { get; set; }

    [JsonPropertyName("de")]
    public string De { get; set; }

    [JsonPropertyName("fr")]
    public string Fr { get; set; }

    [JsonPropertyName("it")]
    public string It { get; set; }

    public bool Equals(MehrsprachigerText? other)
    {
        if (other is null) return false;
        return Id == other.Id &&
               De == other.De &&
               Fr == other.Fr &&
               It == other.It;
    }
}

public class BetriebspunktAusfall : IEquatable<BetriebspunktAusfall>
{
    [JsonPropertyName("bpUic")]
    public int BpUic { get; set; }

    [JsonPropertyName("bezOff")]
    public string BezOff { get; set; }

    [JsonPropertyName("abk")]
    public string Abk { get; set; }

    [JsonPropertyName("qosBitfeld")]
    public int QosBitfeld { get; set; }

    public bool Equals(BetriebspunktAusfall? other)
    {
        if (other is null) return false;
        return BpUic == other.BpUic &&
               BezOff == other.BezOff &&
               Abk == other.Abk &&
               QosBitfeld == other.QosBitfeld;
    }
}

public class DetailAusfall : IEquatable<DetailAusfall>
{
    [JsonPropertyName("richtung")]
    public string Richtung { get; set; }

    [JsonPropertyName("bp")]
    public BetriebspunktAusfall Betriebspunkt { get; set; }

    public bool Equals(DetailAusfall? other)
    {
        if (other is null) return false;
        return Richtung == other.Richtung &&
               Betriebspunkt.Equals(other.Betriebspunkt);
    }
}

public class Ersatz : IEquatable<Ersatz>
{
    [JsonPropertyName("detail")]
    public DetailAusfall Detail { get; set; }

    [JsonPropertyName("fahrt")]
    public FahrtAusfall Fahrt { get; set; }

    public bool Equals(Ersatz? other)
    {
        if (other is null) return false;
        return Detail.Equals(other.Detail) &&
               Fahrt.Equals(other.Fahrt);
    }
}

public class FahrtAusfall : IEquatable<FahrtAusfall>
{
    [JsonPropertyName("fahrtId")]
    public FahrtId FahrtId { get; set; }

    public bool Equals(FahrtAusfall? other)
    {
        if (other is null) return false;
        return FahrtId.Equals(other.FahrtId);
    }
}

public class Fahrt : IEquatable<Fahrt>
{
    [JsonPropertyName("fahrtId")]
    public FahrtId FahrtId { get; set; }

    [JsonPropertyName("ersatz")]
    public Ersatz? Ersatz { get; set; }

    [JsonPropertyName("zuglaeufe")]
    public List<Zuglauf> Zuglaeufe { get; } = new List<Zuglauf>();

    public bool Equals(Fahrt? other)
    {
        if (other is null) return false;
        return FahrtId.Equals(other.FahrtId) &&
               ((Ersatz == null && other.Ersatz == null) || (Ersatz != null && Ersatz.Equals(other.Ersatz))) &&
               Zuglaeufe.SequenceEqual(other.Zuglaeufe);
    }
}

public class FahrtId : IEquatable<FahrtId>
{
    [JsonPropertyName("fahrtBezeichner")]
    public string FahrtBezeichner { get; set; }

    [JsonPropertyName("betriebstag")]
    public string Betriebstag { get; set; }

    public bool Equals(FahrtId? other)
    {
        if (other is null) return false;
        return FahrtBezeichner == other.FahrtBezeichner &&
               Betriebstag == other.Betriebstag;
    }
}

public class Haltestellen : IEquatable<Haltestellen>
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

    public bool Equals(Haltestellen? other)
    {
        if (other is null) return false;
        return BetriebspunktId == other.BetriebspunktId &&
               Abkuerzung == other.Abkuerzung &&
               Bezeichnung == other.Bezeichnung &&
               Flags.SequenceEqual(other.Flags) &&
               Abfahrtszeiten.Equals(other.Abfahrtszeiten) &&
               Ankunftszeiten.Equals(other.Ankunftszeiten) &&
               ((Verspaetungsgrund == null && other.Verspaetungsgrund == null) || (Verspaetungsgrund != null && Verspaetungsgrund.Equals(other.Verspaetungsgrund))) &&
               ((Ausfallgrund == null && other.Ausfallgrund == null) || (Ausfallgrund != null && Ausfallgrund.Equals(other.Ausfallgrund))) &&
               Ersatz == other.Ersatz;
    }
}

public class FahrtenRoot : IEquatable<FahrtenRoot>
{
    [JsonPropertyName("serverZeit")]
    public DateTime ServerZeit { get; set; }

    [JsonPropertyName("fahrten")]
    public List<Fahrt> Fahrten { get; } = new List<Fahrt>();

    public bool Equals(FahrtenRoot? other)
    {
        if (other is null) return false;
        return ServerZeit == other.ServerZeit &&
               Fahrten.SequenceEqual(other.Fahrten);
    }
}

public class Zuglauf : IEquatable<Zuglauf>
{
    [JsonPropertyName("fahrtId")]
    public FahrtId FahrtId { get; set; }

    [JsonPropertyName("haltestellen")]
    public List<Haltestellen> Haltestellen { get; } = new List<Haltestellen>();

    public bool Equals(Zuglauf? other)
    {
        if (other is null) return false;
        return FahrtId.Equals(other.FahrtId) &&
               Haltestellen.SequenceEqual(other.Haltestellen);
    }
}