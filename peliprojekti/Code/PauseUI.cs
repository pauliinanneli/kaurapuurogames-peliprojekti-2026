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
    /// Kutsutaan kun node on valmis. 
    /// Haetaan pause menu ja piilotetaan se aluksi.
    /// </summary>
    public override void _Ready()
    {
        pauseMenu = GetNode<Control>("PauseMenu"); // haetaan PauseMenu node
        pauseMenu.Visible = false; // menu ei näy pelin alussa
    }

    /// <summary>
    /// Pysäyttää pelin ja näyttää pause-menun.
    /// </summary>
    public void OnPausePressed()
    {
        GetTree().Paused = true; // pausettaa pelin
        pauseMenu.Visible = true; // näyttää pause-menun
    }

    /// <summary>
    /// Jatkaa peliä ja piilottaa pause-menun.
    /// </summary>
    public void OnContinuePressed()
    {
        GetTree().Paused = false; // jatkaa peliä
        pauseMenu.Visible = false; // piilottaa pause-menun
    }
}