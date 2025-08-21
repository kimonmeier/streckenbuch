using Streckenbuch.Components.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Components.Services;

public interface IScreenWakeLockService
{
    Task<bool> IsSupportedAsync();
    Task ReleaseWakeLockAsync(WakeLockSentinel sentinel);
    Task<WakeLockSentinel> RequestWakeLockAsync();
}