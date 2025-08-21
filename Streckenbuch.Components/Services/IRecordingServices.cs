using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Components.Services;

public interface IRecordingServices
{
    void AddRecording(GeolocationPosition coordinate);
    void StartBackgroundTask(CancellationToken cancellationToken);
    Task StartWorkTrip(int trainNumber, int trainDriverNumber);
}
