using MediatR;
using Microsoft.JSInterop;
using Moq;
using Newtonsoft.Json;
using Streckenbuch.Client.Components.Fahren;
using Streckenbuch.Client.Events.ApproachingStop;
using Streckenbuch.Client.Events.PositionRecieved;
using Streckenbuch.Client.Models.Fahren;
using Streckenbuch.Client.Models.Fahren.Betriebspunkt;
using Streckenbuch.Client.Models.Fahren.Signal;
using Streckenbuch.Client.Services;
using Streckenbuch.Client.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Client.Test.Services;

public class FahrenPositionServiceTest
{
    public interface TestInterface
    {
        void DoSomething();
    }

    private GeolocationPosition[] Positions => TestData.TestPositions;


    [Theory]
    [InlineData([5])]
    [InlineData([10])]
    [InlineData([3])]
    public async Task ShouldRecordPositions(int takePositions)
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TestData.TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        var positions = Positions.Take(takePositions);

        foreach (var position in positions)
        {
            await positionService.UpdatePosition(position);
        }

        sender.Verify(s => s.Send(It.IsAny<PositionRecievedEvent>(), CancellationToken.None), Times.Exactly(takePositions));
    }

    [Theory]
    [InlineData([5, 3])]
    [InlineData([10, 2])]
    [InlineData([3, 0])]
    public async Task ShouldRecordPositionsButNotWithBadAccuracy(int takePositions, int failPositions)
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TestData.TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        var positions = Positions.Take(takePositions).ToList();

        for (int i = 0; i < failPositions; i++)
        {
            positions[i].Coords.Accuracy = 200;
        }

        foreach (var position in positions)
        {
            await positionService.UpdatePosition(position);
        }

        sender.Verify(s => s.Send(It.IsAny<PositionRecievedEvent>(), CancellationToken.None), Times.Exactly(takePositions - failPositions));
    }

    [Fact]
    public void SouldNotMove()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TestData.TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        var firstPosition = Positions.First();

        positionService.UpdatePosition(firstPosition);
        positionService.UpdatePosition(firstPosition);
        positionService.UpdatePosition(firstPosition);
        positionService.UpdatePosition(firstPosition);

        mock.Verify(s => s.DoSomething(), Times.Never());
    }
    
    [Fact]
    public async Task ShouldMoveOnePositionWithMultipleUpdatesInOneCycle()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TestData.TimeLineEntries, (action) =>
        {
            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(2.1));
                mock.Object.DoSomething();
                action();
            }).ConfigureAwait(false);
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        IEnumerable<GeolocationPosition> positions = Positions.Take(10);

        foreach(GeolocationPosition pos in positions)
        {
            await Task.Delay(TimeSpan.FromSeconds(0.5));
            await positionService.UpdatePosition(pos);
        }
        
        await Task.Delay(TimeSpan.FromSeconds(5));

        mock.Verify(s => s.DoSomething(), Times.Exactly(2));
    }
    
    [Fact]
    public async Task ShouldMoveOnlyOnePositionWhenSpammedWithUpdates()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TestData.TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        IEnumerable<GeolocationPosition> positions = Positions.Take(10);

        foreach(GeolocationPosition pos in positions)
        {
            _ = positionService.UpdatePosition(pos);
        }
        
        await Task.Delay(TimeSpan.FromSeconds(5));

        mock.Verify(s => s.DoSomething(), Times.Exactly(2));
    }

    [Fact]
    public async Task ShouldMoveOnePosition()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TestData.TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        IEnumerable<GeolocationPosition> positions = Positions.Take(10);

        foreach(GeolocationPosition pos in positions)
        {
            await positionService.UpdatePosition(pos);
        }

        mock.Verify(s => s.DoSomething(), Times.Exactly(2));
    }
    
    [Fact]
    public async Task ShouldMoveFourPositionAndAnnounce()
    {
        Mock<TestInterface> mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TestData.TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });
        positionService.SetStops(new List<Guid>()
        {
            Guid.AllBitsSet
        });

        mock.Verify(s => s.DoSomething(), Times.Never());
        sender.Verify(s => s.Send(It.IsAny<ApproachingStopEvent>(), CancellationToken.None), Times.Never());

        IEnumerable<GeolocationPosition> positions = Positions.Take(80);

        foreach(GeolocationPosition pos in positions)
        {
            await positionService.UpdatePosition(pos);
        }

        mock.Verify(s => s.DoSomething(), Times.Exactly(4));
        sender.Verify(s => s.Send(It.IsAny<ApproachingStopEvent>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public void ShouldNotMoveAwayFromLastTimelineEntry()
    {
        var mock = new Mock<TestInterface>();
        Mock<ISender> sender = new Mock<ISender>();

        FahrenPositionService positionService = new FahrenPositionService(sender.Object);
        positionService.Initialize(TestData.TimeLineEntries, (action) =>
        {
            mock.Object.DoSomething();
            action();
        });

        mock.Verify(s => s.DoSomething(), Times.Never());

        // Get last two timeline entries
        var lastEntry = TestData.TimeLineEntries[^1]; // Rupperswil
        var secondLastEntry = TestData.TimeLineEntries[^2]; // Abschnitt_Ausfahrt Signal

        // Calculate the direction vector from second-last to last
        var directionLatitude = lastEntry.Location.Y - secondLastEntry.Location.Y;
        var directionLongitude = lastEntry.Location.X - secondLastEntry.Location.X;
        
        List<GeolocationPosition> positions = new List<GeolocationPosition>();

        // 1. First position exactly at the last entry
        positions.Add(new GeolocationPosition
        {
            Coords = new GeolocationCoordinates
            {
                Latitude = lastEntry.Location.X,
                Longitude = lastEntry.Location.Y,
                Accuracy = 5.0
            },
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        });
        
        // 2. Next positions slowly moving *away* from lastEntry
        for (int i = 1; i <= 100; i++)
        {            
            Console.WriteLine("{{coords:{{accuracy:1,latitude:{0},longitude:{1}}}}},", lastEntry.Location.X + i * directionLatitude, lastEntry.Location.Y + i * directionLongitude);

            positions.Add(new GeolocationPosition
            {
                Coords = new GeolocationCoordinates
                {
                    Latitude = lastEntry.Location.X + i * directionLatitude, // Every step a bit farther
                    Longitude = lastEntry.Location.Y + i * directionLongitude,
                    Accuracy = 5.0
                },
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            });
        }

        foreach (var pos in positions)
        {
            positionService.UpdatePosition(pos);
        }

        // The service should NOT trigger DoSomething, because movement is away from the timeline
        mock.Verify(s => s.DoSomething(), Times.Never());
    }
}
