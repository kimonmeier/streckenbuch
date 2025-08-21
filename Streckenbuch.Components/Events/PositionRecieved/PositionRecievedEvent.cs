using MediatR;
using Microsoft.JSInterop;

namespace Streckenbuch.Components.Events.PositionRecieved;

public class PositionRecievedEvent : IRequest
{
    public required GeolocationPosition Position { get; set; }
}