using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Components.Services;
public interface IClipdboardService
{
    ValueTask<string> ReadTextAsync();

    ValueTask WriteTextAsync(string text);
}
