using Godot;
using System;

public partial class Spawner : Node2D
{
	[Export]
	public PackedScene MeteorScene;

	private RandomNumberGenerator rng = new RandomNumberGenerator();

	public override void _Ready()
	{
		rng.Randomize();
		GetNode<Timer>("Timer").Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout()
	{
		var meteor = MeteorScene.Instantiate<Area2D>();

		float screenHeight = GetViewportRect().Size.Y;
		float randomY = rng.RandfRange(50, screenHeight - 50);

		meteor.Position = new Vector2(
			GetViewportRect().Size.X + 50,
			randomY
		);

		AddChild(meteor);
	}
}
