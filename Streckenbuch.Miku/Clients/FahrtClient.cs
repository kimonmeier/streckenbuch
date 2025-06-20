using Flurl;
using Flurl.Http;
using Streckenbuch.Miku.Models.Fahrten;

namespace Streckenbuch.Miku.Clients;

public class FahrtClient : BaseAPI
{
    protected override string CurrentPath => "fahrt";
    
    public FahrtClient(BaseAPI? parent) : base(parent)
    {
    }

    public async Task<Zuglauf> ListByTrainNumber(int trainNumber)
    {
        FahrtenRoot fahrtenRoot = await this.Path.AppendPathSegment(trainNumber).GetJsonAsync<FahrtenRoot>();

        return fahrtenRoot.Fahrten.First().Zuglaeufe.First();
    }
}