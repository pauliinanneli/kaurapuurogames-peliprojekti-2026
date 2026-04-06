using Godot;
using System;

public partial class StartMenu : Control
{
	[Export] public string GameScenePath = "res://Scenes/Level1.tscn";
    [Export] public string LevelsScenePath = "res://Scenes/LevelChooser.tscn";

    [Export] public string TutorialScenePath = "res://Scenes/Tutorial.tscn";
    [Export] public string CreditsScenePath = "res://Scenes/Credits.tscn";
    [Export] public string SettingsScenePath = "res://Scenes/Settings.tscn";
    private Button fiButton;
    private Button enButton;


    public void OnStartButtonPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk
        GetTree().ChangeSceneToFile(GameScenePath);
    }
    public void OnLevelsButtonPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk
        GetTree().ChangeSceneToFile(LevelsScenePath);
    }

    public void OnTutorialButtonPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk
        GetTree().ChangeSceneToFile(TutorialScenePath);
    }

    public void OnCreditsButtonPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk
        GetTree().ChangeSceneToFile(CreditsScenePath);
    }

    public void OnSettingsButtonPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk
        GetTree().ChangeSceneToFile(SettingsScenePath);
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
}
