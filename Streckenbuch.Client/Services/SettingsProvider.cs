using Blazored.LocalStorage;
using Streckenbuch.Components.Models;
using Streckenbuch.Components.Services;

namespace Streckenbuch.Client.Services;

public class SettingsProvider : ISettingsProvider
{
    public event EventHandler? SettingsChanged;

    public bool IsVoiceActivated { get; set; }

    public bool IsDarkMode { get; set; }

    public RecordingOption IsRecordingActive { get; set; }

    public int TrainDriverNumber { get; set; }

    private bool _isLoaded = false;

    private readonly IServiceProvider _serviceProvider;

    public SettingsProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }


    public async Task SaveAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var localStorageService = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

        await localStorageService.SetItemAsync(Konst.DarkMode, IsDarkMode);
        await localStorageService.SetItemAsync(Konst.VoiceActivated, IsVoiceActivated);
        await localStorageService.SetItemAsync(Konst.RecordingActiveV2, IsRecordingActive);
        await localStorageService.SetItemAsync(Konst.TrainDriverNumber, TrainDriverNumber);

        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    public async Task LoadAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var localStorageService = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();

        this.IsVoiceActivated = await localStorageService.GetItemAsync<bool?>(Konst.VoiceActivated) ?? false;
        this.IsDarkMode = await localStorageService.GetItemAsync<bool?>(Konst.DarkMode) ?? true;
        this.TrainDriverNumber = await localStorageService.GetItemAsync<int?>(Konst.TrainDriverNumber) ?? 0;
        this.IsRecordingActive = await localStorageService.GetItemAsync<RecordingOption?>(Konst.RecordingActiveV2) ?? RecordingOption.None;

        await MigrateOldSettingsAsync(localStorageService);

        _isLoaded = true;

        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }

    private async Task MigrateOldSettingsAsync(ILocalStorageService storageService)
    {
        await MigrateRecordingSettingsAsync(storageService);
    }

    private async Task MigrateRecordingSettingsAsync(ILocalStorageService storageService)
    {
        RecordingOption? recordingOption = await storageService.GetItemAsync<RecordingOption?>(Konst.RecordingActiveV2);

        if (recordingOption != null)
        {
            return;
        }

        bool? oldRecordingActive = await storageService.GetItemAsync<bool?>(Konst.RecordingActive);

        if (oldRecordingActive is null)
        {
            return;
        }

        IsRecordingActive = oldRecordingActive.Value ? RecordingOption.Manual : RecordingOption.None;
        await storageService.SetItemAsync(Konst.RecordingActiveV2, oldRecordingActive.Value ? RecordingOption.Manual : RecordingOption.None);
    }

    public async Task LoadIfNotLoadedAsync()
    {
        if (_isLoaded)
        {
            return;
        }

        await LoadAsync();
    }

    private static class Konst
    {
        public const string VoiceActivated = "voiceActivated";
        public const string DarkMode = "darkMode";
        public const string RecordingActive = "recordingActive";
        public const string RecordingActiveV2 = "recordingActiveV2";
        public const string TrainDriverNumber = "trainDriverNumber";
    }
}