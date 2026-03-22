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

    public override void _Ready()
    {
        dieMenu = GetNode<Control>("DieMenu");
        dieMenu.Visible = false; // hidden at start
    }

    /// <summary>
    /// Shows the death menu
    /// </summary>
    public void ShowMenu()
    {
        dieMenu.Visible = true;
    }

    /// <summary>
    /// Restarts the game by resetting game state and reloading the current scene
    /// </summary>
    public void OnRestartPressed()
    {
        // Unpause the game so everything can run normally again
        GetTree().Paused = false;

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
        GetTree().Quit();
    }
}