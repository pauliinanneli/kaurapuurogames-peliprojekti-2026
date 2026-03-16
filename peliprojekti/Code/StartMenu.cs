using Godot;
using System;

public partial class StartMenu : Control
{
	[Export] public string GameScenePath = "res://Scenes/Level1.tscn";
    [Export] public string LevelsScenePath = "res://Scenes/LevelChooser.tscn";

	public void OnStartButtonPressed()
    {
        GetTree().ChangeSceneToFile(GameScenePath);
    }

    public void OnLevelsButtonPressed()
    {
        GetTree().ChangeSceneToFile(LevelsScenePath);
    }
}
