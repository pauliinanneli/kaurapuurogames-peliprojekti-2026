using Godot;
using System;

public partial class Settings : Control
{
    public const string MasterBus = "Master";
    public const string MusicBus = "Music";

    public const string EffectsBus = "Effects";

    [Export] private Slider _masterVolume = null;
    [Export] private Slider _musicVolume = null;
    [Export] private Slider _effectsVolume = null;

    private Button fiButton;
    private Button enButton;
    private AudioEffectLowPassFilter _muffle;

    private int _masterBusIndex = -1;
    private int _musicBusIndex = -1;
    private int _effectsBusIndex = -1;
    private bool _unmuffleOnClosing = true;

    public override void _Ready()
    {
        _masterBusIndex = AudioServer.GetBusIndex(MasterBus);
        _musicBusIndex = AudioServer.GetBusIndex(MusicBus);
        _effectsBusIndex = AudioServer.GetBusIndex(EffectsBus);

        _muffle = (AudioEffectLowPassFilter)AudioServer.GetBusEffect(_musicBusIndex, 0);
    }

    public void OpenSettings(bool doUnmuffleOnClosing = true)
    {
        _unmuffleOnClosing = doUnmuffleOnClosing;
        Show();
        SetMuffle(true); // make music sound muffled when opening settings
    }

    public void OnEnPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk
        GameManager.Instance.SetLocale("en");  // switches the lang to eng
    }

    public void OnFiPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk
        GameManager.Instance.SetLocale("fi");  // switches the lang to fi
    }

     public void OnBackButtonPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk

        if (_unmuffleOnClosing) // ONLY unmuffle when going back from settings if we are going back to startmenu instead of pauseui
        {
            SetMuffle(false); // stop muffling music when going back from settings
        }
        Hide();
    }

    public void OnMasterVolumeChanged(double value)
    {
        SetVolumeToBus(_masterBusIndex, (float)value);
    }

    public void OnMusicVolumeChanged(double value)
    {
        SetVolumeToBus(_musicBusIndex, (float)value);
    }

    public void OnEffectsVolumeChanged(double value)
    {
        SetVolumeToBus(_effectsBusIndex, (float)value);
    }

    private void SetVolumeToBus(int busIndex, float linearVolume)
    {
        float dbVolume = Mathf.LinearToDb(linearVolume);

        AudioServer.SetBusVolumeDb(busIndex, dbVolume);
    }

    public void SetMuffle(bool isMuffled)
    {
        if (_muffle == null)
        {
            return;
        }

        if (isMuffled)
        {
            _muffle.CutoffHz = 500f; // cut off high frequencies to make sound muffled
        }
        else
        {
            _muffle.CutoffHz = 20000f; // all frequencies heard
        }
    }
}
