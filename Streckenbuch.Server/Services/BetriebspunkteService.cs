using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Data.Entities;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Helper;
using Streckenbuch.Shared.Data;
using Streckenbuch.Shared.Models;
using Streckenbuch.Shared.Services;
using Streckenbuch.Shared.Types;

namespace Streckenbuch.Server.Services;

public class BetriebspunkteService : Streckenbuch.Shared.Services.BetriebspunkteService.BetriebspunkteServiceBase
{
    private readonly IMapper _mapper;
    private readonly DbTransactionFactory _dbTransactionFactory;
    private readonly BetriebspunkteRepository _betriebspunkteRepository;
    private readonly BetriebspunktStreckenZuordnungRepository _streckenZuordnungRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public BetriebspunkteService(IMapper mapper, DbTransactionFactory dbTransactionFactory, BetriebspunkteRepository betriebspunkteRepository, UserManager<ApplicationUser> userManager, BetriebspunktStreckenZuordnungRepository streckenZuordnungRepository)
    {
        _mapper = mapper;
        _dbTransactionFactory = dbTransactionFactory;
        _betriebspunkteRepository = betriebspunkteRepository;
        _userManager = userManager;
        _streckenZuordnungRepository = streckenZuordnungRepository;
    }

    public override async Task<ListBetriebspunkteAnswer> ListAllBetriebspunkte(Empty request, ServerCallContext context)
    {
        var answer = new ListBetriebspunkteAnswer();
        var list = await _betriebspunkteRepository.ListAllAsync();
        answer.Betriebspunkte.Add(_mapper.Map<List<BetriebspunktProto>>(list).OrderBy(x => x.Name));
        return answer;
    }

    public override async Task<Empty> CreateBetriebspunkt(CreateBetriebspunktReqeust request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _betriebspunkteRepository.AddAsync(new Data.Entities.Betriebspunkte.Betriebspunkt()
            {
                Name = request.Name,
                Kommentar = request.Kommentar,
                Location = request.Location,
                Typ = (BetriebspunktTyp)request.Typ,
            });
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<ListBetriebspunkteAnswer> ListBetriebspunkteByStreckenKonfiguration(ListBetriebspunkteByStreckenKonfigurationRequest request, ServerCallContext context)
    {
        var answer = new ListBetriebspunkteAnswer();
        var list = await _streckenZuordnungRepository.ListByStreckenKonfigurationId(request.StreckenKonfigurationId);
        answer.Betriebspunkte.Add(_mapper.Map<List<BetriebspunktProto>>(list.OrderBy(x => x.SortNummer).Select(x => x.Betriebspunkt)));
        
        return answer;
    }

    public override async Task<BetriebspunktProto> GetBetriebspunktById(GuidProto request, ServerCallContext context)
    {
        var betriebspunkt = await _betriebspunkteRepository.FindByEntityAsync(request);

        if (betriebspunkt is null)
        {
            throw new Exception();
        }

        return _mapper.Map<BetriebspunktProto>(betriebspunkt);
    }
}
