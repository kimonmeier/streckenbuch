using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Data.Entities;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Helper;
using Streckenbuch.Shared.Data;
using Streckenbuch.Shared.Services;
using Streckenbuch.Shared.Types;

namespace Streckenbuch.Server.Services;

public class StreckenService : Streckenbuch.Shared.Services.StreckenService.StreckenServiceBase
{
    private readonly StreckenRepository _streckenRepository;
    private readonly StreckenKonfigurationRepository _streckenKonfigurationRepository;
    private readonly BetriebspunktStreckenZuordnungRepository _betriebspunktStreckenZuordnungRepository;
    private readonly DbTransactionFactory _dbTransactionFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public StreckenService(DbTransactionFactory dbTransactionFactory, IMapper mapper, StreckenRepository streckenRepository, UserManager<ApplicationUser> userManager, StreckenKonfigurationRepository streckenKonfigurationRepository, BetriebspunktStreckenZuordnungRepository betriebspunktStreckenZuordnungRepository)
    {
        _dbTransactionFactory = dbTransactionFactory;
        _mapper = mapper;
        _streckenRepository = streckenRepository;
        _userManager = userManager;
        _streckenKonfigurationRepository = streckenKonfigurationRepository;
        _betriebspunktStreckenZuordnungRepository = betriebspunktStreckenZuordnungRepository;
    }

    public override async Task<ListStreckenAnswer> ListAllStrecken(Empty request, ServerCallContext context)
    {
        var answer = new ListStreckenAnswer();
        var list = await _streckenRepository.ListAllAsync();
        answer.Strecken.Add(_mapper.Map<List<StreckenProto>>(list).OrderBy(x => x.Nummer));

        return answer;
    }

    public override async Task<Empty> CreateStrecke(CreateStreckeRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _streckenRepository.AddAsync(new Data.Entities.Strecken.Strecke()
            {
                StreckenNummer = request.Nummer,
            });
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<Empty> CreateKonfiguration(CreateKonfigurationRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _streckenKonfigurationRepository.AddAsync(new Data.Entities.Strecken.StreckenKonfiguration()
            {
                Name = request.HasName ? request.Name : null,
                StreckeId = request.StreckeId,
                VonBetriebspunktId = request.VonBetriebspunktId,
                BisBetriebspunktId = request.BisBetriebspunktId,
            });
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<Empty> CreateStreckenZuordnung(CreateStreckenZuordnungRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _betriebspunktStreckenZuordnungRepository.AddZuordnungAsync(new Data.Entities.Betriebspunkte.BetriebspunktStreckenZuordnung()
            {
                BetriebspunktId = request.Betriebspunkt,
                StreckenKonfigurationId = request.StreckenKonfiguration,
                SortNummer = request.SortNummer + 1,
            });
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<ListZuordnungByStreckeAnswer> ListStreckenZuordnungByStrecke(ListZuordnungByStreckeRequest request, ServerCallContext context)
    {
        var answer = new ListZuordnungByStreckeAnswer();
        var list = await _betriebspunktStreckenZuordnungRepository.ListByStreckenKonfigurationId(request.StreckenKonfigurationId);
        answer.Zuordnungen.Add(_mapper.Map<List<StreckeZuordnung>>(list));

        return answer;
    }

    public override async Task<Empty> DeleteStreckenZuordnung(DeleteStreckenZuordnungRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            Guid zuordnungId = await _betriebspunktStreckenZuordnungRepository.FindZuordnungByKonfigurationAndBetriebspunkt(request.StreckenKonfigurationId, request.BetriebspunktId);
            await _betriebspunktStreckenZuordnungRepository.RemoveAsync(zuordnungId);
            await _betriebspunktStreckenZuordnungRepository.RecalculateSortNumbersAsync(request.StreckenKonfigurationId);
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<Empty> ChangeStreckenZuordnung(ChangeStreckenZuordnungRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            Guid zuordnungId = await _betriebspunktStreckenZuordnungRepository.FindZuordnungByKonfigurationAndBetriebspunkt(request.StreckenKonfigurationId, request.BetriebspunktId);
            await _betriebspunktStreckenZuordnungRepository.ChangeZuordnungAsync(zuordnungId, request.OldSortNummer + 1, request.NewSortNummer + 1);
            await _betriebspunktStreckenZuordnungRepository.RecalculateSortNumbersAsync(request.StreckenKonfigurationId);
            await dbTransaction.Commit(context.CancellationToken);
        };

        return new Empty();
    }

    public override async Task<ListZuordnungByBetriebspunktAnswer> ListStreckenZuordnungByBetriebspunkt(ListZuordnungByBetriebspunktRequest request, ServerCallContext context)
    {
        var answer = new ListZuordnungByBetriebspunktAnswer();
        var list = await _betriebspunktStreckenZuordnungRepository.ListByBetriebspunktAsync(request.BetriebspunktId);
        answer.Zuordnungen.Add(_mapper.Map<List<StreckeZuordnungSignal>>(list));

        return answer;
    }

    public override async Task<StreckenProto> GetStreckeById(GuidProto request, ServerCallContext context)
    {
        return _mapper.Map<StreckenProto>(await _streckenRepository.FindByEntityAsync(request));
    }

    public override async Task<StreckenKonfigurationProto> GetStreckenKonfigurationById(GuidProto request, ServerCallContext context)
    {
        return _mapper.Map<StreckenKonfigurationProto>(await _streckenKonfigurationRepository.FindByIdIncludeBetriebspunkte(request));
    }

    public override async Task<GuidProto> GetStreckeIdByKonfigurationId(GuidProto request, ServerCallContext context)
    {
        var result = await _streckenKonfigurationRepository.FindByEntityAsync(request);

        if (result is null)
        {
            throw new Exception("Not found");
        }

        return result.StreckeId;
    }

    public override async Task<Empty> DeleteStrecke(DeleteStreckeRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);
        
        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _streckenRepository.RemoveAsync(request.StreckeId);
            await dbTransaction.Commit(context.CancellationToken);
        }

        return new Empty();
    }

    public override async Task<Empty> DeleteKonfiguration(DeleteKonfigurationRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);
        
        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _streckenKonfigurationRepository.RemoveAsync(request.KonfigurationId);
            await dbTransaction.Commit(context.CancellationToken);
        }

        return new Empty();
    }
}
