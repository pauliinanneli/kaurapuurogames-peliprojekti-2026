using Godot;
using System;

public partial class StartMenu : Control
{
	[Export] public string GameScenePath = "res://Scenes/Level1.tscn";
    [Export] public string LevelsScenePath = "res://Scenes/LevelChooser.tscn";

    [Export] public string TutorialScenePath = "res://Scenes/Tutorial.tscn";
    [Export] public string CreditsScenePath = "res://Scenes/Credits.tscn";


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
}
