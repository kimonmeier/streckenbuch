using Streckenbuch.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Components.States;

public interface IDataState
{
    Task<BetriebspunktProto> FetchBetriebspunkt(Guid id);
    Task<List<BetriebspunktProto>> FetchBetriebspunkte();
}
