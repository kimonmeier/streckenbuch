using Streckenbuch.Shared.Models;

namespace Streckenbuch.Client.Models.Fahren.Signal;

public interface ISignalEntry : IBaseEntry
{
    public SignalTyp SignalTyp { get; }
}