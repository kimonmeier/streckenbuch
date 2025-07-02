using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Identity;
using Streckenbuch.Server.Data.Entities;
using Streckenbuch.Server.Data.Entities.Shift;
using Streckenbuch.Server.Data.Repositories;
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
    private readonly ILogger<RecordingService> _logger;

    public RecordingService(DbTransactionFactory dbTransactionFactory, WorkDriverRepository workDriverRepository, WorkShiftRepository workShiftRepository, WorkTripRepository workTripRepository, TripRecordingRepository tripRecordingRepository, ILogger<RecordingService> logger)
    {
        _dbTransactionFactory = dbTransactionFactory;
        _workDriverRepository = workDriverRepository;
        _workShiftRepository = workShiftRepository;
        _workTripRepository = workTripRepository;
        _tripRecordingRepository = tripRecordingRepository;
        _logger = logger;
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
}