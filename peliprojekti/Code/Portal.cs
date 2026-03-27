using Godot;
using System;

public partial class Portal : Area2D
{
[Export] public string NextScenePath = "res://Scenes/Level1.tscn";

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        // check if the thing that hits is player
        if (body is PlayerCharacter player)
        {
            Input.VibrateHandheld(200); // vibration when going in and changing scene

            GetTree().ChangeSceneToFile(NextScenePath);
        }
    }
}
