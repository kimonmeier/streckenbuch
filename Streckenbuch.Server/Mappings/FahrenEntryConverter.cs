using AutoMapper;
using Streckenbuch.Server.Models;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Mappings;

public class FahrenEntryConverter : ITypeConverter<FahrenTransferEntry, FahrenEntry>
{
    public FahrenEntry Convert(FahrenTransferEntry source, FahrenEntry destination, ResolutionContext context)
    {
        if (source.Betriebspunkt is not null)
        {
            return context.Mapper.Map<FahrenEntry>(source.Betriebspunkt);
        }

        if (source.SignalZuordnung is not null)
        {
            return context.Mapper.Map<FahrenEntry>(source.SignalZuordnung);
        }

        throw new NotImplementedException();
    }
}
