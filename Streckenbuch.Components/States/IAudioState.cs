using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Components.States;

public interface IAudioState
{
    Task SayText(string text);
}
