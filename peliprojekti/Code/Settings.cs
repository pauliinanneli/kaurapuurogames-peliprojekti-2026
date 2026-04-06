using Godot;
using System;

public partial class Settings : Control
{

    private Button fiButton;
    private Button enButton;
    [Export] public string BackScenePath = "res://Scenes/StartMenu.tscn";

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
        GetTree().ChangeSceneToFile(BackScenePath);
    }
}
