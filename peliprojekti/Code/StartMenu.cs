using Godot;
using System;

public partial class StartMenu : Control
{
	[Export] public string GameScenePath = "res://Scenes/Level1.tscn";
    [Export] public string LevelsScenePath = "res://Scenes/LevelChooser.tscn";

    [Export] public string TutorialScenePath = "res://Scenes/Tutorial.tscn";
    [Export] public string CreditsScenePath = "res://Scenes/Credits.tscn";
    [Export] private AudioStreamPlayer _click;

    private Settings _settingsMenu;

    private Button fiButton;
    private Button enButton;

    public override void _Ready()
    {
        _settingsMenu = GetNode<Settings>("Settings");
    }


    public void OnStartButtonPressed()
    {
        GameManager.Instance.PlayClick();
        GetTree().ChangeSceneToFile(GameScenePath);
    }
    public void OnLevelsButtonPressed()
    {
        GameManager.Instance.PlayClick();
        GetTree().ChangeSceneToFile(LevelsScenePath);
    }

    public void OnTutorialButtonPressed()
    {
        GameManager.Instance._cameFromStartMenu = true;
        GameManager.Instance.PlayClick();
        GetTree().ChangeSceneToFile(TutorialScenePath);
    }

    public void OnCreditsButtonPressed()
    {
        GameManager.Instance.PlayClick();
        GetTree().ChangeSceneToFile(CreditsScenePath);
    }

    public void OnSettingsButtonPressed()
    {
        GameManager.Instance.PlayClick();
        _settingsMenu.OpenSettings(true);
    }

    public void OnEnPressed()
    {
        GameManager.Instance.PlayClick();
        GameManager.Instance.SetLocale("en");  // switches the lang to eng
    }

    public void OnFiPressed()
    {
        GameManager.Instance.PlayClick();
        GameManager.Instance.SetLocale("fi");  // switches the lang to fi
    }
 }
