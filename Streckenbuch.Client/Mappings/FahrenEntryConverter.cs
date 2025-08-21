using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Streckenbuch.Components.Models.Fahren;
using Streckenbuch.Components.Models.Fahren.Betriebspunkt;
using Streckenbuch.Components.Models.Fahren.Signal;
using Streckenbuch.Shared.Models;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Client.Mappings;

public class FahrenEntryConverter : ITypeConverter<FahrenEntry, IBaseEntry>, ITypeConverter<FahrenEntry, ISignalEntry>, ITypeConverter<FahrenEntry, IBetriebspunktEntry>
{
    public IBaseEntry Convert(FahrenEntry source, IBaseEntry destination, ResolutionContext context)
    {
        var entryType = (EntryType)source.EntryTyp;
        switch (entryType)
        {
            case EntryType.Betriebspunkt:
                return context.Mapper.Map<IBetriebspunktEntry>(source);
            case EntryType.Signal:
                return context.Mapper.Map<ISignalEntry>(source);
        }

        throw new NotImplementedException();
    }

    public ISignalEntry Convert(FahrenEntry source, ISignalEntry destination, ResolutionContext context)
    {
        var signalTyp = (SignalTyp)source.SignalTyp;
        switch (signalTyp)
        {
            case SignalTyp.Hauptsignal:
                return context.Mapper.Map<HauptSignalEntry>(source);
            case SignalTyp.Streckengeschwindigkeit:
                return context.Mapper.Map<ChivronSignalEntry>(source);
            case SignalTyp.Vorsignal:
                return context.Mapper.Map<VorsignalEntry>(source);
            case SignalTyp.Kombiniert:
                return context.Mapper.Map<KombiniertSignalEntry>(source);
            case SignalTyp.Wiederholung:
                return context.Mapper.Map<WiederholungsSignalEntry>(source);
            case SignalTyp.Fahrstellungsmelder:
                return context.Mapper.Map<FahrtstellungsmelderEntry>(source);
        }

        throw new NotImplementedException();
    }

    public IBetriebspunktEntry Convert(FahrenEntry source, IBetriebspunktEntry destination, ResolutionContext context)
    {
        var betriebspunktTyp = (BetriebspunktTyp)source.BetriebspunktTyp;
        switch (betriebspunktTyp)
        {
            case BetriebspunktTyp.Dienstbahnhof:
                return context.Mapper.Map<DienstbahnhofEntry>(source);
            case BetriebspunktTyp.Bahnhof:
                return context.Mapper.Map<BahnhofEntry>(source);
            case BetriebspunktTyp.Haltestelle:
                return context.Mapper.Map<HaltestelleEntry>(source);
        }

        throw new NotImplementedException();
    }
}
