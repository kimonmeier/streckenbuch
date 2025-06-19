using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Data.Entities;
using Streckenbuch.Server.Data.Entities.Linien;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Helper;
using Streckenbuch.Shared.Data;
using Streckenbuch.Shared.Models;
using Streckenbuch.Shared.Services;
using System.Diagnostics;

namespace Streckenbuch.Server.Services;

public class LinienService : Streckenbuch.Shared.Services.LinienService.LinienServiceBase
{
    private readonly IMapper _mapper;
    private readonly DbTransactionFactory _dbTransactionFactory;
    private readonly LinienRepository _linienRepository;
    private readonly LinienKonfigurationRepository _linienKonfigurationRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly LinieTrainRepository _linieTrainRepository;

    public LinienService(IMapper mapper, LinienRepository linienRepository, LinienKonfigurationRepository linienKonfigurationRepository, DbTransactionFactory dbTransactionFactory,
        UserManager<ApplicationUser> userManager, LinieTrainRepository linieTrainRepository)
    {
        _mapper = mapper;
        _linienRepository = linienRepository;
        _linienKonfigurationRepository = linienKonfigurationRepository;
        _dbTransactionFactory = dbTransactionFactory;
        _userManager = userManager;
        _linieTrainRepository = linieTrainRepository;
    }

    public override async Task<Empty> EditStreckenZuordnung(EditStreckenZuordnungRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            var konfiguration = await _linienKonfigurationRepository.ListByLinie(request.LinieId);

            foreach (var conf in konfiguration)
            {
                await _linienKonfigurationRepository.RemoveAsync(conf);
            }

            foreach (var item in request.Strecken)
            {
                await _linienKonfigurationRepository.AddAsync(new Data.Entities.Linien.LinienStreckenKonfigurationen()
                {
                    LinieId = request.LinieId,
                    VonBetriebspunktId = item.VonBetriebspunktId,
                    BisBetriebspunktId = item.BisBetriebspunktId,
                    StreckenKonfigurationId = item.StreckenKonfigurationId,
                    Order = item.SortingNumber,
                });
            }

            await dbTransaction.Commit(context.CancellationToken);
        }

        ;

        return new Empty();
    }

    public override async Task<Empty> CreateLinie(CreateLinieRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            await _linienRepository.AddAsync(new Data.Entities.Linien.Linie()
            {
                Nummer = request.Nummer, Typ = (LinienTyp)request.Typ, VonBetriebspunktId = request.VonBetriebspunktId, BisBetriebspunktId = request.BisBetriebspunktId,
            });
            await dbTransaction.Commit(context.CancellationToken);
        }

        ;

        return new Empty();
    }

    public override async Task<GetAllLinienResponse> GetAllLinien(Empty request, ServerCallContext context)
    {
        GetAllLinienResponse response = new GetAllLinienResponse();
        var list = await _linienRepository.ListAllAsync();
        response.Linien.Add(_mapper.Map<List<LinienProto>>(list));

        return response;
    }

    public override async Task<GetTrainNumberLinkResponse> GetTrainNumberLinked(GetTrainNumberLinkRequest request, ServerCallContext context)
    {
        var linieTrain = await _linieTrainRepository.FindByTrainNumberAsync(request.TrainNumber);

        if (linieTrain is null)
        {
            return new GetTrainNumberLinkResponse()
            {
                LinieId = Guid.Empty, TrainNumber = -1, LinieTrainId = Guid.Empty
            };
        }

        return new GetTrainNumberLinkResponse()
        {
            LinieTrainId = linieTrain.Id, LinieId = linieTrain.LinieId, TrainNumber = request.TrainNumber
        };
    }

    public override async Task<GetTrainNumberLinkResponse> ChangeTrainNumberLink(ChangeTrainNumberLinkRequest request, ServerCallContext context)
    {
        await context.GetAuthenticatedUser(_userManager);

        using (var dbTransaction = _dbTransactionFactory.CreateTransaction())
        {
            var linieTrain = await _linieTrainRepository.FindByTrainNumberAsync(request.TrainNumber);

            if (linieTrain is null)
            {
                linieTrain = new LinieTrain()
                {
                    TrainNumber = request.TrainNumber,
                };

                await _linieTrainRepository.AddAsync(linieTrain);
            }

            linieTrain.LinieId = request.LinieId;

            await _linieTrainRepository.UpdateAsync(linieTrain);
            await dbTransaction.Commit(context.CancellationToken);

            return new GetTrainNumberLinkResponse()
            {
                LinieId = request.LinieId, TrainNumber = request.TrainNumber, LinieTrainId = linieTrain.Id
            };
        }
    }
}