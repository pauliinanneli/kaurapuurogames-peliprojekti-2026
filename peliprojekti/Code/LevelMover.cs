using Godot;
using System;

public partial class LevelMover : Node2D
{

	// the normal speed of game without slow motion etc
	[Export] private float _normalSpeed = 250.0f;
	private float _currentSpeed;

	public override void _Ready()
    {
        _currentSpeed = _normalSpeed;
    }

	public override void _PhysicsProcess(double delta)
    {
		// move the node and its children forward every frame
        Position += new Vector2(_currentSpeed * (float)delta, 0);
    }
}
