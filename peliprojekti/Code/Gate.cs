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
        // if playercharacter touches gate area, tell gamemanager to save choice and to give health boost
        if (body is PlayerCharacter)
        {
            GD.Print($"Player chose {Role} path");

            GameManager.Instance.SaveChoice(Role);
            GameManager.Instance.AddHealth(0.2f);

            QueueFree();
        }
    }
}
