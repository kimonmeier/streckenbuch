using MediatR;

namespace Streckenbuch.Components.Events.TrainSelect;

public class TrainSelectCommand : IRequest<Unit>
{
    public int TrainNumber { get; set; }
}