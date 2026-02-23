using Godot;
using System;

public partial class LevelMover : Node2D
{

	// the normal speed of game without slow motion etc
	[Export] private float _normalSpeed = 200.0f

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
