using Microsoft.JSInterop;
using Streckenbuch.Components.Models.Fahren;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Components.Services;

public interface IFahrenPositionService
{
    event EventHandler? DataChanged;

    void AddSpecialStop(Guid betriebspunktId);
    List<IBaseEntry> Initialize(List<IBaseEntry> fahrplanEntries, Action<Action> beforeUpdateAction);
    void SetStops(List<Guid> betriebspunktId);
    void SkipPosition();
    Task UpdatePosition(GeolocationPosition newPosition);
}

