using Godot;
using System;

/// <summary>
/// Handles the death menu UI and its functionality
/// </summary>
public partial class DieUI : CanvasLayer
{
    /// <summary>
    /// Reference to the actual menu UI
    /// </summary>
    private Control dieMenu;
    private Button pauseButton;
    [Export] public string MenuScenePath = "res://Scenes/StartMenu.tscn";

    public override void _Ready()
    {
        dieMenu = GetNode<Control>("DieMenu");
        dieMenu.Visible = false; // hidden at start

        // get PauseButton from PauseUI for hiding || -button from backround when DieMenu is visible
        pauseButton = GetTree().CurrentScene.GetNode<Button>("PauseUI/PauseButton"); 
    }

    /// <summary>
    /// Shows the death menu
    /// </summary>
    public void ShowMenu()
    {
        dieMenu.Visible = true;         // shows DieMenu
        pauseButton.Visible = false;    // hides || -button from backround when DieMenu visible
    }

    /// <summary>
    /// Restarts the game by resetting game state and reloading the current scene
    /// </summary>
    public void OnRestartPressed()
    {
        // Unpause the game so everything can run normally again
        GetTree().Paused = false;       // game unpaused
        pauseButton.Visible = true;     // shows || -button when restarting the game

        // Reset GameManager state (health & etc.)
        // This is required because GameManager is a singleton and does not reset automatically
        GameManager.Instance.ResetGame();

        // Reload the current scene to start the game from the beginning
        GetTree().ReloadCurrentScene();
    }

    /// <summary>
    /// Quits the game completely
    /// </summary>
    public void OnQuitPressed()
    {
        GetTree().Paused = false; // fixed bug: unpause so startmenu works
        GameManager.Instance.ResetGame(); // reset singleton state
        GetTree().ChangeSceneToFile(MenuScenePath); // opens StartMenu
    }
}