using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Data.Entities;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Server.Data.Repositories;
using Streckenbuch.Server.Helper;
using Streckenbuch.Shared.Data;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Server.Services;

public class RecordingService : Streckenbuch.Shared.Services.RecordingService.RecordingServiceBase
{
    private readonly DbTransactionFactory _dbTransactionFactory;
    private readonly WorkDriverRepository _workDriverRepository;
    private readonly WorkShiftRepository _workShiftRepository;
    private readonly WorkTripRepository _workTripRepository;
    private readonly TripRecordingRepository _tripRecordingRepository;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IMapper _mapper;

    public RecordingService(DbTransactionFactory dbTransactionFactory, WorkDriverRepository workDriverRepository, WorkShiftRepository workShiftRepository, WorkTripRepository workTripRepository, TripRecordingRepository tripRecordingRepository, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        _dbTransactionFactory = dbTransactionFactory;
        _workDriverRepository = workDriverRepository;
        _workShiftRepository = workShiftRepository;
        _workTripRepository = workTripRepository;
        _tripRecordingRepository = tripRecordingRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    public override async Task<StartRecordingSessionResponse> StartRecordingSession(StartRecordingSessionRequest request, ServerCallContext context)
    {
        using var transaction = _dbTransactionFactory.CreateTransaction();

        var workDriver = await _workDriverRepository.FindByDriverNumberAsync(request.TrainDriverNumber);

        if (workDriver is null)
        {
            workDriver = await _workDriverRepository.AddAsync(new WorkDriver()
            {
                TrainDriverNumber = request.TrainDriverNumber
            });
        }
        
        var workShift = await _workShiftRepository.FindByDateAsync(DateOnly.FromDateTime(DateTime.Now));

        if (workShift is null)
        {
            workShift = await _workShiftRepository.AddAsync(new WorkShift()
            {
                WorkDriver = workDriver, Datum = DateOnly.FromDateTime(DateTime.Now)
            });
        }

        var workTrip = await _workTripRepository.AddAsync(new WorkTrip()
        {
            WorkShift = workShift, TripNumber = request.TrainNumber
        });
        
        
        await transaction.Commit(context.CancellationToken);

        return new StartRecordingSessionResponse()
        {
            WorkTrip = workTrip.Id
        };
    }

    public override async Task<Empty> SendRecordedLocations(SendRecordedLocationsRequest request, ServerCallContext context)
    {
        using var transaction = _dbTransactionFactory.CreateTransaction();

        foreach (RecordPosition requestPosition in request.Positions)
        {
            await _tripRecordingRepository.AddAsync(new TripRecording()
            {
                WorkTripId = request.WorkTripId, Location = requestPosition.Location, Time = TimeOnly.FromDateTime(new DateTime(requestPosition.DateTime))
            });
        }
        
        await transaction.Commit(context.CancellationToken);

        return new Empty();
    }

    public override async Task<GetHistoryHeadsResponse> GetHistoryHeads(GetHistoryHeadsRequest request, ServerCallContext context)
    {
        ApplicationUser applicationUser = await context.GetAuthenticatedUser(_userManager);

        WorkDriver? driver = await _workDriverRepository.FindByApplicationUserAsync(applicationUser.Id);

        if (driver is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "The user is not a driver. Please contact the administrator."));
        }

        List<WorkShift> workShifts = await _workShiftRepository.FindByDriverWithTrips(driver.Id);
        GetHistoryHeadsResponse response = new GetHistoryHeadsResponse();
        response.Days.Add(_mapper.Map<List<HistoryDays>>(workShifts));
        
        return response;
    }

    public override async Task<GetHistoryDataResponse> GetHistoryData(GetHistoryDataRequest request, ServerCallContext context)
    {
        ApplicationUser applicationUser = await context.GetAuthenticatedUser(_userManager);

        WorkDriver? driver = await _workDriverRepository.FindByApplicationUserAsync(applicationUser.Id);

        if (driver is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "The user is not a driver. Please contact the administrator."));
        }

        WorkTrip? workTrip = await _workTripRepository.FindByEntityAsync(request.TripId);
        if (workTrip is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Trip not found"));
        }

        var list = await _tripRecordingRepository.FindByTrip(workTrip.Id);
        
        GetHistoryDataResponse response = new GetHistoryDataResponse();
        response.PositionData.Add(_mapper.Map<List<HistoryPositionData>>(list));

        return response;
    }
}