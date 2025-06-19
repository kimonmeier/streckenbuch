using Streckenbuch.Miku.Clients;

namespace Streckenbuch.Miku;

public class MikuApi : BaseAPI
{
    private readonly string baseUrl;

    protected override string CurrentPath => Flurl.Url.Combine(this.baseUrl, "ws");

    public FahrtClient Fahrt { get; init; }

    public MikuApi(string baseUrl) : base(null)
    {
        this.baseUrl = baseUrl;

        this.Fahrt = new FahrtClient(this);
    }
}