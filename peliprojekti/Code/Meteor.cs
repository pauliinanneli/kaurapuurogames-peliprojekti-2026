using Godot;
using System;

public partial class Meteor : Area2D
{
	[Export] public float _floatAmplitude = 150f;
	[Export] public float _floatSpeed = 2f;
	[Export] public float _smoothing = 5f;
	[Export] private float _damageAmount = 0.1f;

	private float _time = 0f;
	private float _startY;

    public override void _Ready()
    {
        _startY = Position.Y; // position where it starts moving
		_time = (float)GD.RandRange(0, 6); // randomize start time so meteors arent all in sync
    }

    public override void _Process(double delta)
    {
        _time += (float)delta * _floatSpeed;
		float targetY = _startY + (Mathf.Sin(_time) * _floatAmplitude); //calculate where meteor "wants" to be
		float finalY = Mathf.Lerp(Position.Y, targetY, _smoothing * (float)delta); // use lerp to smooth movement between current and target y
		Position = new Vector2(Position.X, finalY);
    }

    public override void _EnterTree()
    {
        BodyEntered += OnBodyEntered;
    }

	private void OnBodyEntered(Node2D body)
    {
        if (body is PlayerCharacter playerCharacter)
        {
			// flash red
			playerCharacter.OnMeteorHit();
            //lose health
			GameManager.Instance.SubstractHealth(_damageAmount);
			Input.VibrateHandheld(500);
			// TO DO test vibration!!!
			QueueFree();

        }
    }
}