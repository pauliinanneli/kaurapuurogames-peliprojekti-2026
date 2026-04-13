using Godot;
using System;

public partial class Tutorial : Node2D
{
    [Export] public string StartMenuPath = "res://Scenes/StartMenu.tscn";
    [Export] public string LevelChooserPath = "res://Scenes/LevelChooser.tscn";

    public void OnBackButtonPressed()
    {
        GameManager.Instance.PlayClick();

        if (GameManager.Instance._cameFromStartMenu)
        {
            GameManager.Instance.PlayClick();
            GetTree().ChangeSceneToFile(StartMenuPath);
        }
        else
        {
            GameManager.Instance.PlayClick();
            GetTree().ChangeSceneToFile(LevelChooserPath);
        }
    }
}
