using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Miku;

public abstract class BaseAPI
{
    private BaseAPI? Parent { get; init; }

    protected abstract string CurrentPath { get; }

    protected string Path => Parent is null ? this.CurrentPath : Flurl.Url.Combine(this.Parent?.Path, this.CurrentPath);

    protected BaseAPI(BaseAPI? parent)
    {
        Parent = parent;
    }
}
