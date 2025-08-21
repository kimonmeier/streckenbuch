
using Grpc.Core;
using Grpc.Net.Testing.Moq.Extensions;
using Moq;
using Streckenbuch.Client.Services;
using Streckenbuch.Client.Test.Data;
using Streckenbuch.Shared.Services;

namespace Streckenbuch.Client.Test.Services;

public class RecordingServicesTest
{

    [Fact]
    public async Task ShouldNotStartWorkTrip()
    {
        Mock<RecordingService.RecordingServiceClient> apiClient = this.BuildMockApiClient();
        Mock<SettingsProvider> settingsProvider = new Mock<SettingsProvider>(new Mock<IServiceProvider>().Object);

        RecordingServices service = new RecordingServices(apiClient.Object, settingsProvider.Object);

        await Assert.ThrowsAsync<Exception>(() => service.StartWorkTrip(1, 0));
        await Assert.ThrowsAsync<Exception>(() => service.StartWorkTrip(0, 1));
        await Assert.ThrowsAsync<Exception>(() => service.StartWorkTrip(0, 0));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(100, 112)]
    [InlineData(100, 23242)]
    public async Task ShouldStartWorkTrip(int trainDriverNUmber, int trainNumber)
    {
        Mock<RecordingService.RecordingServiceClient> apiClient = this.BuildMockApiClient();
        Mock<SettingsProvider> settingsProvider = new Mock<SettingsProvider>(new Mock<IServiceProvider>().Object);

        RecordingServices service = new RecordingServices(apiClient.Object, settingsProvider.Object);

        await service.StartWorkTrip(trainDriverNUmber, trainNumber);

        apiClient.Verify(s => s.StartRecordingSessionAsync(It.IsAny<StartRecordingSessionRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()));
    }

    [Theory]
    [InlineData(Streckenbuch.Components.Models.RecordingOption.None)]
    public async Task ShouldStartWorkTripAndNotSendRecordings(Streckenbuch.Components.Models.RecordingOption option)
    {
        Mock<RecordingService.RecordingServiceClient> apiClient = this.BuildMockApiClient();
        Mock<SettingsProvider> settingsProvider = new Mock<SettingsProvider>(new Mock<IServiceProvider>().Object);

        settingsProvider.Object.IsRecordingActive = option;

        RecordingServices service = new RecordingServices(apiClient.Object, settingsProvider.Object);

        service.StartBackgroundTask(CancellationToken.None);

        await service.StartWorkTrip(1, 1);

        apiClient.Verify(s => s.StartRecordingSessionAsync(It.IsAny<StartRecordingSessionRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()), Times.Once);

        foreach (var position in TestData.TestPositions.Take(5))
        {
            service.AddRecording(position);
        }

        await Task.Delay(10_000);

        apiClient.Verify(s => s.SendRecordedLocationsAsync(It.IsAny<SendRecordedLocationsRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Theory]
    [InlineData(Streckenbuch.Components.Models.RecordingOption.Manual)]
    [InlineData(Streckenbuch.Components.Models.RecordingOption.Auto)]
    public async Task ShouldStartWorkTripAndSendRecordings(Streckenbuch.Components.Models.RecordingOption option)
    {
        Mock<RecordingService.RecordingServiceClient> apiClient = this.BuildMockApiClient();

        Mock<SettingsProvider> settingsProvider = new Mock<SettingsProvider>(new Mock<IServiceProvider>().Object);

        settingsProvider.Object.IsRecordingActive = option;

        RecordingServices service = new RecordingServices(apiClient.Object, settingsProvider.Object);

        service.StartBackgroundTask(CancellationToken.None);

        await service.StartWorkTrip(1, 1);

        apiClient.Verify(s => s.StartRecordingSessionAsync(It.IsAny<StartRecordingSessionRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()), Times.Once);

        foreach (var position in TestData.TestPositions.Take(5))
        {
            service.AddRecording(position);
        }

        await Task.Delay(10_000);

        apiClient.Verify(s => s.SendRecordedLocationsAsync(It.IsAny<SendRecordedLocationsRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()), Times.Once);
    }


    private Mock<RecordingService.RecordingServiceClient> BuildMockApiClient()
    {
        Mock<RecordingService.RecordingServiceClient> apiClient = new Mock<RecordingService.RecordingServiceClient>();
        apiClient.Setup(s => s.StartRecordingSessionAsync(It.IsAny<StartRecordingSessionRequest>(), It.IsAny<Metadata>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>())).ReturnsAsync<RecordingService.RecordingServiceClient, StartRecordingSessionRequest, StartRecordingSessionResponse>(r => new StartRecordingSessionResponse()
        {
            WorkTrip = Guid.NewGuid()
        });

        return apiClient;
    }
}