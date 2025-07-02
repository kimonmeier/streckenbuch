using Blazored.LocalStorage;

namespace Streckenbuch.Client.Services;

public class SettingsProvider
{
    public EventHandler? SettingsChanged;

    public bool IsVoiceActivated { get; set; }
    
    public bool IsDarkMode { get; set; }
    
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
        
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public async Task LoadAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var localStorageService = scope.ServiceProvider.GetRequiredService<ILocalStorageService>();
        
        this.IsVoiceActivated = await localStorageService.GetItemAsync<bool>(Konst.VoiceActivated);
        this.IsDarkMode = await localStorageService.GetItemAsync<bool>(Konst.DarkMode);
        
        SettingsChanged?.Invoke(this, EventArgs.Empty);
    }
    
    private static class Konst
    {
        public const string VoiceActivated = "voiceActivated";
        public const string DarkMode = "darkMode";  
    }
}