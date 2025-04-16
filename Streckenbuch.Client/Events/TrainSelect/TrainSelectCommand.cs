using MediatR;

namespace Streckenbuch.Client.Events.TrainSelect;

public class TrainSelectCommand : IRequest<Unit>
{
    public int TrainNumber { get; set; }
}