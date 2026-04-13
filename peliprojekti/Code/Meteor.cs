using Godot;
using System;

public partial class Meteor : Area2D
{
	[Export] public float _floatAmplitude = 150f;
	[Export] public float _floatSpeed = 2f;
	[Export] public float _smoothing = 5f;
	[Export] private float _damageAmount = 0.1f;
    [Export] private Sprite2D _sprite = null;
    [Export] private GpuParticles2D _particleEffect = null;
    [Export] private AudioStreamPlayer2D _audioClip = null;

	private float _time = 0f;
	private float _startY;
    private bool _hasExploded = false;

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
        // if meteor has already exploded, no need to execute
        if (_hasExploded)
        {
            return;
        }


        if (body is PlayerCharacter playerCharacter)
        {
            _hasExploded = true; // to only hit the meteor once
			// flash red
			playerCharacter.OnMeteorHit();
            //lose health
			GameManager.Instance.SubstractHealth(_damageAmount);
			Input.VibrateHandheld(500);
			// TO DO test vibration!!!
			Explode();

        }
    }

    private void Explode()
    {
        if (_sprite != null)
        {
            _sprite.Hide();
        }

        if (_audioClip != null)
        {
            _audioClip.Play();
        }

        if (_particleEffect != null)
        {
            _particleEffect.Emitting = true;
        }

        GetTree().CreateTimer(1.0f).Timeout += QueueFree; // timer to wait until particle effect has played before destroying
    }
}