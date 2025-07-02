using MediatR;
using Microsoft.JSInterop;

namespace Streckenbuch.Client.Events.PositionRecieved;

public class PositionRecievedEvent : IRequest
{
    public required GeolocationPosition Position { get; set; }
}