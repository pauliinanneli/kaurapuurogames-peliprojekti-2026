using Godot;
using System;

public partial class StartMenu : Control
{
	[Export] public string GameScenePath = "res://Scenes/Level1.tscn";
    [Export] public string LevelsScenePath = "res://Scenes/LevelChooser.tscn";

    [Export] public string TutorialScenePath = "res://Scenes/Tutorial.tscn";
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

    public void OnEnPressed()
    {
        GameManager.Instance.SetLocale("en");
    }

    public void OnFiPressed()
    {
        GameManager.Instance.SetLocale("fi");
    }
}
