using Godot;
using System;

public partial class Credits : Control
{
    [Export] public string BackScenePath = "res://Scenes/StartMenu.tscn";

     public void OnBackButtonPressed()
    {
        Input.VibrateHandheld(50); // small haptic feedbackk
        GetTree().ChangeSceneToFile(BackScenePath);
    }
}
