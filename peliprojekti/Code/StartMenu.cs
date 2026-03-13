using Godot;
using System;

public partial class StartMenu : Control
{
	[Export] public string GameScenePath = "res://Scenes/Level1.tscn";

	public void OnStartButtonPressed()
    {
        Error result = GetTree().ChangeSceneToFile(GameScenePath);

		if (result != Error.Ok)
        {
            GD.PrintErr("Failed to load scene");
        }
    }
}
