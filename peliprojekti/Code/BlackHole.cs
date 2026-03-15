using Godot;
using System;

public partial class BlackHole : Area2D
{
    [Export] public string NextScenePath = "res://Scenes/ConnectingStars.tscn";

    public override void _Ready()
    {
        BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D body)
    {
        // check if the thing that hits is player
        if (body is CharacterBody2D)
        {
            GD.Print("kusipaska");

            //change scene
            GetTree().ChangeSceneToFile(NextScenePath);
        }
    }
}
