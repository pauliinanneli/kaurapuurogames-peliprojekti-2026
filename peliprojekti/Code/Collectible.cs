using Godot;
using System;

public partial class Collectible : Area2D
{

	private bool _isCollected = false;

	public bool IsCollected
    {
        get {return _isCollected; }
    }

    public override void _EnterTree()
    {
        // aloita bodyentered-signaalin kuuntelu
        BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree()
    {
        // lopeta onbodyentered-signaalin kuuntelu
        BodyEntered -= OnBodyEntered;
    }

/// <summary>
/// suoritetaan kun bodyentered signaali laukeaa. signaalilla tiedotetaan collectible-oliota törmäyksestä toisen kappaleen kanssa
/// </summary>
/// <param name="body">viittaus törmäävään kappaleeseen</param>
/// <exception cref="NotImplementedException"></exception>
    private void OnBodyEntered(Node2D body)
    {
        if (body is PlayerCharacter playerCharacter)
        {
            //törmäys tapahtui pelaajan kanssa, reagoi
            _isCollected = true;
        }
    }

    protected virtual void Collect(PlayerCharacter playerCharacter)
    {

    }
}
