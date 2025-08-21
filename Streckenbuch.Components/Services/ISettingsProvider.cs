using Streckenbuch.Components.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Streckenbuch.Components.Services;

public interface ISettingsProvider
{
    event EventHandler? SettingsChanged;
    
    bool IsDarkMode { get; set; }
    RecordingOption IsRecordingActive { get; set; }
    bool IsVoiceActivated { get; set; }
    int TrainDriverNumber { get; set; }

    Task LoadAsync();
    Task LoadIfNotLoadedAsync();
    Task SaveAsync();
}
