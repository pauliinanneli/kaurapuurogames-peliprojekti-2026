using Godot;
using System;

public partial class LevelChooser : Node2D
{
    [Export] public string Level1Path = "res://Scenes//Level1.tscn";
    private bool _pressed = false;

    public override void _Input(InputEvent @event)
    {
        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            _pressed = true;
        }
    }
}
