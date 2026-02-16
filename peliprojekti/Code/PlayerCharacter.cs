using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{

	[Export] public float _speed = 300.0f;
	private Vector2 _inputDirection = Vector2.Zero;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_inputDirection = Input.GetVector(InputConfig.InputUp, InputConfig.InputDown, InputConfig.InputLeft, InputConfig.InputRight);
	}

}
