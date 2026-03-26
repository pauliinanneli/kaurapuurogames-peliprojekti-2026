using Godot;
using System;
using System.Threading.Tasks;

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
        if (body is PlayerCharacter player)
        {
            Input.VibrateHandheld(200); // vibration when going in and changing scene
            // TO DO dip to black before going straight into next scene??

            player.BlackHoleSuck(NextScenePath, GlobalPosition); // trigger animation and give path to change scene to
        }
    }
}
