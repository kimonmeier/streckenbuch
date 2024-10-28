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

namespace Streckenbuch.Server.Services;

public class SignaleService : Streckenbuch.Shared.Services.SignaleService.SignaleServiceBase
{
    private readonly SignalRepository _signalRepository;
    private readonly SignalStreckenZuordnungRepository _signalStreckenZuordnungRepository;
    private readonly DbTransactionFactory _dbTransactionFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public SignaleService(DbTransactionFactory dbTransactionFactory, UserManager<ApplicationUser> userManager, IMapper mapper, SignalRepository signalRepository, SignalStreckenZuordnungRepository signalStreckenZuordnungRepository)
    {
        _dbTransactionFactory = dbTransactionFactory;
        _userManager = userManager;
        _mapper = mapper;
        _signalRepository = signalRepository;
        _signalStreckenZuordnungRepository = signalStreckenZuordnungRepository;
    }

    public override async Task<ListSignaleAnswer> ListAllSignale(Empty request, ServerCallContext context)
    {
        var answer = new ListSignaleAnswer();
        var list = await _signalRepository.ListAllAsync();
        answer.Signale.Add(_mapper.Map<List<SignalProto>>(list));
        return answer;
    }

    public override async Task<ListSignaleAnswer> ListSignaleByBetriebspunkt(ListByBetriebspunktRequest request, ServerCallContext context)
    {
        var answer = new ListSignaleAnswer();
        var list = await _signalRepository.ListByBetriebspunktAsync(request.BetriebspunktId);
        answer.Signale.Add(_mapper.Map<List<SignalProto>>(list));
        return answer;
    }

    public override async Task<Empty> CreateSignal(CreateSignalRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _signalRepository.AddAsync(new Data.Entities.Signale.Signal()
            {
                BetriebspunktId = request.BetriebspunktId,
                Location = new NetTopologySuite.Geometries.Coordinate(request.Location.Longitude, request.Location.Latitude),
                Name = request.Name,
                Seite = (SignalSeite)request.SignalSeite,
                Typ = (SignalTyp)request.SignalTyp,
            });
            await dbTransaction.Commit(context.CancellationToken);
        };


        return new Empty();
    }

    public override async Task<Empty> DeleteSignal(DeleteSignalRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _signalRepository.RemoveAsync(request.SignalId);
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<Empty> CreateSignalZuordnung(CreateSignalZuordnungRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _signalStreckenZuordnungRepository.AddAsync(new Data.Entities.Signale.SignalStreckenZuordnung()
            {
                SignalId = request.SignalId,
                StreckeId = request.StreckeBetriebspunktZuordnungId,
                GueltigAb = ((DateOnly?)request.AbDatum!).Value,
                GueltigBis = request.BisDatum,
                NonStandard = request.HasIsSpecialCase && request.IsSpecialCase,
                NonStandardKommentar = request.HasSpecialCase ? request.SpecialCase : null,
            });
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<Empty> DeleteSignalZuordnung(DeleteSignalZuordnungRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            var zuordnung = await _signalStreckenZuordnungRepository.FindZuordnungBySignalAndStrecke(request.SignalId, request.StreckeBetriebspunktZuordnungId);
            await _signalStreckenZuordnungRepository.RemoveAsync(zuordnung);
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<ListZuordnungBySignalAnswer> ListZuordnungBySignal(ListZuordnungBySignalRequest request, ServerCallContext context)
    {
        var answer = new ListZuordnungBySignalAnswer();
        var list = await _signalStreckenZuordnungRepository.ListBySignalId(request.SignalId);
        answer.Zuordnungen.Add(_mapper.Map<List<SignalZuordnung>>(list));

        return answer;
    }
}
