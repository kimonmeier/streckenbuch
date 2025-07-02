using Howler.Blazor.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Streckenbuch.Client.Services;
using System.Text.Json;

namespace Streckenbuch.Client.States;

public class AudioState
{
    private readonly IServiceProvider _serviceProvider;
    private readonly SettingsProvider _settingsProvider;

    public AudioState(IServiceProvider serviceProvider, SettingsProvider settingsProvider)
    {
        _serviceProvider = serviceProvider;
        _settingsProvider = settingsProvider;
    }
    
    public async Task SayText(string text)
    {
        if (!_settingsProvider.IsVoiceActivated)
        {
            return;
        }
        
        await using var scope = _serviceProvider.CreateAsyncScope();
        var howl = scope.ServiceProvider.GetRequiredService<IHowl>();
        var speechSynthesis = scope.ServiceProvider.GetRequiredService<ISpeechSynthesisService>();
        var navigationManager = scope.ServiceProvider.GetRequiredService<NavigationManager>();

        var voices = await speechSynthesis.GetVoicesAsync();;
        
        await howl.Play(new HowlOptions() {
            Sources = [new Uri(new Uri(navigationManager.BaseUri), "audio/beep.mp3").ToString()],
            Volume = 1.0,
        });
        
        await Task.Delay(500);
        
        speechSynthesis.Speak(new SpeechSynthesisUtterance()
        {
            Text = text,
            Lang = "de-CH",
            Volume = 1.0,
            Rate = 0.9,
            Voice = voices.SingleOrDefault(x => x.VoiceURI == "Microsoft Katja Online (Natural) - German (Germany)") ?? voices.SingleOrDefault(x => x.VoiceURI == "Microsoft Jan Online (Natural) - German (Switzerland)") ?? voices.SingleOrDefault(x => x.VoiceURI == "Microsoft Karsten - German (Switzerland)")
        });

        while (speechSynthesis.Speaking)
        {
            await Task.Delay(100);
        }
    }
}