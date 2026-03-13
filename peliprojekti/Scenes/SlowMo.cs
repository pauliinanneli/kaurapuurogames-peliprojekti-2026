using Godot;
using System;

public partial class SlowMo : Area2D
{
    private bool _triggered = false;

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (!_triggered && body is PlayerCharacter)
        {
            _triggered = true;
            // slow to 50% speed over 0,5 seconds
            GameManager.Instance.ChangeGameSpeed(0.5f, 0.5f);
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is PlayerCharacter)
        {
            // speed back to normal over 0,5 seconds
            GameManager.Instance.ChangeGameSpeed(1.0f, 0.5f);
        }
    }
}
