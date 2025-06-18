using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Data.Entities;
using Streckenbuch.Server.Data.Entities.Signale;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Helper;
using Streckenbuch.Shared.Data;
using Streckenbuch.Shared.Models;
using Streckenbuch.Shared.Services;
using System.Data;

namespace Streckenbuch.Server.Services;

public class SignaleService : Streckenbuch.Shared.Services.SignaleService.SignaleServiceBase
{
    private readonly SignalRepository _signalRepository;
    private readonly SignalStreckenZuordnungRepository _signalStreckenZuordnungRepository;
    private readonly SignalStreckenZuordnungSortingStreckeRepository _signalStreckenZuordnungSortingStreckeRepository;
    private readonly SignalStreckenZuordnungSortingBetriebspunktRepository _signalStreckenZuordnungSortingBetriebspunktRepository;
    private readonly SignalStreckenZuordnungSortingSignalRepository _signalStreckenZuordnungSortingSignalRepository;
    private readonly DbTransactionFactory _dbTransactionFactory;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public SignaleService(DbTransactionFactory dbTransactionFactory, UserManager<ApplicationUser> userManager, IMapper mapper, SignalRepository signalRepository,
        SignalStreckenZuordnungRepository signalStreckenZuordnungRepository, SignalStreckenZuordnungSortingStreckeRepository signalStreckenZuordnungSortingStreckeRepository,
        SignalStreckenZuordnungSortingBetriebspunktRepository signalStreckenZuordnungSortingBetriebspunktRepository,
        SignalStreckenZuordnungSortingSignalRepository signalStreckenZuordnungSortingSignalRepository)
    {
        _dbTransactionFactory = dbTransactionFactory;
        _userManager = userManager;
        _mapper = mapper;
        _signalRepository = signalRepository;
        _signalStreckenZuordnungRepository = signalStreckenZuordnungRepository;
        _signalStreckenZuordnungSortingStreckeRepository = signalStreckenZuordnungSortingStreckeRepository;
        _signalStreckenZuordnungSortingBetriebspunktRepository = signalStreckenZuordnungSortingBetriebspunktRepository;
        _signalStreckenZuordnungSortingSignalRepository = signalStreckenZuordnungSortingSignalRepository;
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
                Location = request.Location,
                Name = request.Name,
                Seite = (SignalSeite)request.SignalSeite,
                Typ = (SignalTyp)request.SignalTyp,
            });
            await dbTransaction.Commit(context.CancellationToken);
        }


        return new Empty();
    }

    public override async Task<Empty> DeleteSignal(DeleteSignalRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _signalRepository.RemoveAsync(request.SignalId);
            await dbTransaction.Commit(context.CancellationToken);
        }

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
                NonStandard = request.HasIsSpecialCase && request.IsSpecialCase,
                NonStandardKommentar = request.HasSpecialCase ? request.SpecialCase : null,
            });
            await dbTransaction.Commit(context.CancellationToken);
        }

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
        }

        return new Empty();
    }

    public override async Task<ListZuordnungBySignalAnswer> ListZuordnungBySignal(ListZuordnungBySignalRequest request, ServerCallContext context)
    {
        var answer = new ListZuordnungBySignalAnswer();
        var list = await _signalStreckenZuordnungRepository.ListBySignalId(request.SignalId);
        answer.Zuordnungen.Add(_mapper.Map<List<SignalZuordnung>>(list));

        return answer;
    }

    public override async Task<ListSignalSortingResponse> ListSignalSorting(ListSignalSortingRequest request, ServerCallContext context)
    {
        ListSignalSortingResponse answer = new ListSignalSortingResponse();
        List<SignalStreckenZuordnungSortingStrecke> sortingStrecken =
            await _signalStreckenZuordnungSortingStreckeRepository.GetByStreckeKonfigurationIdWithChild(request.StreckeKonfigurationId);
        answer.Strecken.Add(_mapper.Map<List<SignalSortingStrecke>>(sortingStrecken));

        return answer;
    }

    public override async Task<Empty> ChangeSignalSorting(ChangeSignalSortingRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            var existingStrecke = await _signalStreckenZuordnungSortingStreckeRepository.GetWithChild(request.Strecke.SortingId);

            if (existingStrecke is null)
            {
                throw new Exception();
            }

            // Delete old Entries
            // NOT OPTIMAL, BUT MOST TIME EFFICIENT. THIS CAUSES A LOT OF WRITE OPERATIONS
            foreach (var betriebspunkt in existingStrecke.Betriebspunkte)
            {
                foreach (var signal in betriebspunkt.Signale)
                {
                    await _signalStreckenZuordnungSortingSignalRepository.RemoveAsync(signal.Id);
                }

                await _signalStreckenZuordnungSortingBetriebspunktRepository.RemoveAsync(betriebspunkt.Id);
            }

            // Add new Entries
            // NOT OPTIMAL, BUT MOST TIME EFFICIENT. THIS CAUSES A LOT OF WRITE OPERATIONS
            foreach (var betriebspunkt in request.Strecke.Betriebspunkte)
            {
                var newBetriebspunkt = new SignalStreckenZuordnungSortingBetriebspunkt
                {
                    BetriebspunktId = betriebspunkt.BetriebspunktId, SignalStreckenZuordnungSortingStreckeId = existingStrecke.Id,
                };
                await _signalStreckenZuordnungSortingBetriebspunktRepository.AddAsync(newBetriebspunkt);

                foreach (var signal in betriebspunkt.Signale)
                {
                    await _signalStreckenZuordnungSortingSignalRepository.AddAsync(new SignalStreckenZuordnungSortingSignal
                    {
                        SignalId = signal.SignalId, SignalStreckenZuordnungSortingBetriebspunkt = newBetriebspunkt, SortingOrder = (short)signal.SortingNumber
                    });
                }
            }

            await dbTransaction.Commit(context.CancellationToken);
        }

        return new Empty();
    }

    public override async Task<Empty> CreateSignalSorting(CreateSignalSortingRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (Transaction dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            var list = await _signalStreckenZuordnungSortingStreckeRepository.ListByStreckeKonfigurationId(request.StreckenKonfigurationId);

            Guid? toCopy = null;
            if (request.BisDatum is null)
            {
                var entry = list.SingleOrDefault(x => x.GueltigVon <= request.VonDatum && x.GueltigBis is null);

                if (entry is not null)
                {
                    if (request.CopyPreviousSorting)
                    {
                        toCopy = entry.Id;
                    }

                    entry.GueltigBis = ((DateOnly)request.VonDatum).AddDays(-1);

                    await _signalStreckenZuordnungSortingStreckeRepository.UpdateAsync(entry);
                }
            }
            else
            {
                var firstEntry = list.SingleOrDefault(x => x.GueltigVon <= request.BisDatum && x.GueltigBis >= request.BisDatum);
                if (firstEntry is not null)
                {
                    firstEntry.GueltigBis = ((DateOnly)request.VonDatum).AddDays(-1);
                    await _signalStreckenZuordnungSortingStreckeRepository.UpdateAsync(firstEntry);
                }

                var lastEntry = list.FirstOrDefault(x => x.GueltigVon >= request.VonDatum);
                if (lastEntry is not null)
                {
                    lastEntry.GueltigVon = ((DateOnly)request.BisDatum).AddDays(1);
                    await _signalStreckenZuordnungSortingStreckeRepository.UpdateAsync(lastEntry);
                }
            }

            var sortingStrecke = new SignalStreckenZuordnungSortingStrecke
            {
                GueltigVon = (DateOnly?)request.VonDatum ?? DateOnly.MinValue, GueltigBis = ((DateOnly?)request.BisDatum), StreckenKonfigurationId = request.StreckenKonfigurationId
            };
            await _signalStreckenZuordnungSortingStreckeRepository.AddAsync(sortingStrecke);

            if (toCopy is not null)
            {
                var toCopyStrecke = await _signalStreckenZuordnungSortingStreckeRepository.GetWithChild(toCopy.Value);

                foreach (var betriebspunkt in toCopyStrecke!.Betriebspunkte)
                {
                    var copiedBetriebspunkt = await _signalStreckenZuordnungSortingBetriebspunktRepository.AddAsync(new SignalStreckenZuordnungSortingBetriebspunkt()
                    {
                        BetriebspunktId = betriebspunkt.BetriebspunktId, SignalStreckenZuordnungSortingStrecke = sortingStrecke,
                    });

                    foreach (var sortingSignal in betriebspunkt.Signale)
                    {
                        await _signalStreckenZuordnungSortingSignalRepository.AddAsync(new SignalStreckenZuordnungSortingSignal()
                        {
                            SignalId = sortingSignal.SignalId, SortingOrder = sortingSignal.SortingOrder, SignalStreckenZuordnungSortingBetriebspunkt = copiedBetriebspunkt,
                        });
                    }
                }
            }

            await dbTransaction.Commit(context.CancellationToken);
        }


        return new Empty();
    }

    public override async Task<Empty> DeleteSignalSorting(DeleteSignalSortingRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _signalStreckenZuordnungSortingStreckeRepository.RemoveAsync(request.Id);
            await dbTransaction.Commit(context.CancellationToken);
        }


        return new Empty();
    }

    public override async Task<ListSignaleAnswer> ListSignaleByBetriebspunktAndStreckenkonfiguration(ListSignaleByBetriebspunktAndStreckenkonfigurationRequest request,
        ServerCallContext context)
    {
        ListSignaleAnswer answer = new ListSignaleAnswer();
        var list = await _signalStreckenZuordnungRepository.ListByBetriebspunktAndStrecke(request.BetriebspunktId, request.StreckenKonfigurationId);
        answer.Signale.Add(_mapper.Map<List<SignalProto>>(list.Select(x => x.Signal)));

        return answer;
    }

    public override async Task<Empty> EditSignal(EditSignalRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            var signal = await _signalRepository.FindByEntityAsync(request.SignalId);

            if (signal is null)
            {
                throw new DataException("Signal not Found");
            }

            signal.Location = request.Location;
            signal.Name = request.Name;
            signal.Seite = (SignalSeite)request.SignalSeite;
            signal.Typ = (SignalTyp)request.SignalTyp;

            await _signalRepository.UpdateAsync(signal);
            await dbTransaction.Commit(context.CancellationToken);
        }


        return new Empty();
    }
}