using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Components.States;

public interface IContinuousConnectionState
{
    int? GetRegisteredTrainNumber();
    Task RegisterTrain(int trainNumber);
    void StartBackgroundTask(CancellationToken cancellationToken);
    Task UnregisterTrain();
}
