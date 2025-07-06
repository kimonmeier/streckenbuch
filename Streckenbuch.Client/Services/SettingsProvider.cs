using Blazored.LocalStorage;

namespace Streckenbuch.Client.Services;

public class SettingsProvider
{
    public EventHandler? SettingsChanged;

    public bool IsVoiceActivated { get; set; }
    
    public bool IsDarkMode { get; set; }
    
    public bool IsRecordingActive { get; set; }
    
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
        await localStorageService.SetItemAsync(Konst.RecordingActive, IsRecordingActive);
        await localStorageService.SetItemAsync(Konst.TrainDriverNumber, TrainDriverNumber);
        
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public async Task LoadAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var localStorageService = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
        
        this.IsVoiceActivated = await localStorageService.GetItemAsync<bool?>(Konst.VoiceActivated) ?? false;
        this.IsDarkMode = await localStorageService.GetItemAsync<bool?>(Konst.DarkMode) ?? true;
        this.IsRecordingActive = await localStorageService.GetItemAsync<bool?>(Konst.RecordingActive) ?? false;
        this.TrainDriverNumber = await localStorageService.GetItemAsync<int?>(Konst.TrainDriverNumber) ?? 0;

        _isLoaded = true;
        
        SettingsChanged?.Invoke(this, EventArgs.Empty);
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
        public const string TrainDriverNumber = "trainDriverNumber"; 
    }
}