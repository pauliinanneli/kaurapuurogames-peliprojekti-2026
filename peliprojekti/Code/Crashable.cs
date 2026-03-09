using Godot;
using System;

/// <summary>
/// Perusluokka pelin törmäyskohteille, jotka voivat vähentää käyttäjän elämäpisteitä.
/// </summary>
public partial class Crashable : Area2D
{
    private bool _isCrashed = false;

    public bool IsCrashed
    {
        get { return _isCrashed; }
    }

    public override void _EnterTree()
    {
        // aloita BodyEntered-signaalin kuuntelu
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree()
    {
        // lopeta BodyEntered-signaalin kuuntelu
        BodyEntered -= OnBodyEntered;
    }

    /// <summary>
    /// Suoritetaan kun BodyEntered-signaali laukeaa.
    /// </summary>
    /// <param name="body">Törmäävä Node2D</param>
    private void OnBodyEntered(Node2D body)
    {
        if (body is PlayerCharacter playerCharacter)
        {
            _isCrashed = true;
            Crash(playerCharacter);

            QueueFree();
        }
    }

    /// <summary>
    /// Määritellään mitä tapahtuu, kun pelaaja osuu tähän objektiin.
    /// </summary>
    /// <param name="playerCharacter"></param>
    protected virtual void Crash(PlayerCharacter playerCharacter)
    {
        // Perusluokka ei tee mitään
    }
}