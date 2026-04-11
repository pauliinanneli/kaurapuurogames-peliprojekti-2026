using Godot;
using System;

/// <summary>
/// Handles pause function and determines if the pause menu is showed or not.
/// </summary>
public partial class PauseUI : CanvasLayer
{
    /// <summary>
    /// Reference to the pause menu node.
    /// </summary>
    private Control pauseMenu;
    private Button pauseButton;
    [Export] public string MenuScenePath = "res://Scenes/StartMenu.tscn";
    private Settings _settingsMenu;



    /// <summary>
    /// gets called when node is ready
    /// Gets pause menu and hides it
    /// </summary>
    public override void _Ready()
    {
        pauseMenu = GetNode<Control>("PauseMenu"); // gets PauseMenu node
        pauseMenu.Visible = false; // menu not visible
        pauseButton = GetNode<Button>("PauseButton"); // gets PauseButton
         _settingsMenu = GetNode<Settings>("Settings");
    }

    /// <summary>
    /// Stops the game and shows PauseMenu
    /// </summary>
    public void OnPausePressed()
    {
        GameManager.Instance.PlayClick();
        GetTree().Paused = true; // pause game
        pauseMenu.Visible = true; // shows pause-menu
        pauseButton.Visible = false; // hides the || -button when paused

        _settingsMenu.SetMuffle(true); // make music muffled when opening PauseUI
    }


    /// <summary>
    /// Continues the game and hides the menu
    /// </summary>
    public void OnContinuePressed()
    {
        GameManager.Instance.PlayClick();
        GetTree().Paused = false; // the game continues
        pauseMenu.Visible = false; // hides the pause menu
        pauseButton.Visible = true; // shows the || -button when resuming the game

        _settingsMenu.SetMuffle(false); // stop muffling music when continue is pressed
    }

    /// <summary>
    /// Restarts the game from pause menu
    /// </summary>
    public void OnRestartPressed()
    {
        GameManager.Instance.PlayClick();
        _settingsMenu.SetMuffle(false); // stop muffling music when restart pressed
        GetTree().Paused = false; // unpause first
        GameManager.Instance.ResetGame(); // reset singleton state
        GetTree().ReloadCurrentScene(); // reload the current scene
    }

    /// <summary>
    /// Quits the game from pause menu and opens start menu
    /// </summary>
    public void OnQuitPressed()
    {
        GameManager.Instance.PlayClick();
        _settingsMenu.SetMuffle(false); // stop muffling music when quit pressed
        GetTree().Paused = false; // fixed bug: unpause so startmenu works
        GetTree().ChangeSceneToFile(MenuScenePath);
    }

    public void OnSettingsButtonPressed()
    {
        GameManager.Instance.PlayClick();
        _settingsMenu.OpenSettings(false);
    }
}