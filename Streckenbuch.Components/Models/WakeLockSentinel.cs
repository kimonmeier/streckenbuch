using Microsoft.JSInterop;

namespace Streckenbuch.Components.Models;

public class WakeLockSentinel
{
    public int Id { get; }

    public IJSObjectReference JsObjectReference { get; }
    
    public WakeLockSentinel(int id, IJSObjectReference jsObjectReference)
    {
        Id = id;
        JsObjectReference = jsObjectReference;
    }
}