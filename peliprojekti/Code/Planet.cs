using Godot;
using System;

public partial class Planet : Area2D
{
    [Export] public string LevelPath = "res://Scenes/Level1.tscn";
    private Vector2 _touchStartPos;

    public override void _Ready()
    {
        InputEvent += OnInputEvent;
    }

    private void OnInputEvent(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            GameManager.Instance.PlayClick();
            Input.VibrateHandheld(50); // small haptic feedback
            GetTree().ChangeSceneToFile(LevelPath);
        }
        else if (@event is InputEventScreenTouch touchEvent && touchEvent.Pressed)
        {
            GameManager.Instance.PlayClick();
            Input.VibrateHandheld(50); // small haptic feedback
            GetTree().ChangeSceneToFile(LevelPath);
        }
    }
}
