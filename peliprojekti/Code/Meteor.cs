using Godot;
using System;

public partial class Meteor : Area2D
{
	[Export]
	public float Speed = 300f;

	public override void _Process(double delta)
	{
		Position += new Vector2(-Speed * (float)delta, 0);
		if (Position.X < -100)
		{
			QueueFree();
		}
	}

	/* private void OnBodyEntered(Node body)
	{
		if (body.Name == "Player")

	}

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	} */
}