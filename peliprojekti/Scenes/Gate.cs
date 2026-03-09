using Godot;
using System;

public partial class Gate : Area2D
{
    [Export] public string Role = "Empty (replace this)";

    public override void _Ready()
    {
        BodyEntered += OnLaneEntered;
    }

    private void OnLaneEntered(Node2D body)
    {
        if (body is PlayerCharacter)
        {
            GD.Print($"Player chose {Role} path");
        }
    }
}
