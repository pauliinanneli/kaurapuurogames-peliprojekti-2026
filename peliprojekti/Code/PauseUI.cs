using Godot;
using System;

/// <summary>
/// Hoitaa pelin pause-toiminnon ja pause-menun näyttämisen.
/// </summary>
public partial class PauseUI : CanvasLayer
{
    /// <summary>
    /// Viittaus pause menu -nodeen.
    /// </summary>
    private Control pauseMenu;

    /// <summary>
    /// gets called when node is ready
    /// Gets pause menu and hides it
    /// </summary>
    public override void _Ready()
    {
        pauseMenu = GetNode<Control>("PauseMenu"); // gets PauseMenu node
        pauseMenu.Visible = false; // menu not visible 
    }

    /// <summary>
    /// Stops the game and shows PauseMenu
    /// </summary>
    public void OnPausePressed()
    {
        GetTree().Paused = true; // pause game
        pauseMenu.Visible = true; // shows pause-menu
    }

    /// <summary>
    /// Continues the game and hides the menu
    /// </summary>
    public void OnContinuePressed()
    {
        GetTree().Paused = false; // jatkaa peliä
        pauseMenu.Visible = false; // piilottaa pause-menun
    }

    /// <summary>
    /// Restarts the game from pause menu
    /// </summary>
    public void OnRestartPressed()
    {
        GetTree().Paused = false; // unpause first
        GameManager.Instance.ResetGame(); // reset singleton state
        GetTree().ReloadCurrentScene(); // reload the current scene
    }

    /// <summary>
    /// Quits the game from pause menu
    /// </summary>
    public void OnQuitPressed()
    {
        GetTree().Quit();
    }
}