using Streckenbuch.Shared.Models;

namespace Streckenbuch.Components.Models.Fahren.Signal;

public interface ISignalEntry : IBaseEntry
{
    public SignalTyp SignalTyp { get; }
}