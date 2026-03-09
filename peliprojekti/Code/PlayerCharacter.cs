using Godot;
using System;

public partial class PlayerCharacter : CharacterBody2D
{

	[Export] private float _speed = 300.0f;

	[Export] private float _friction = 0.2f;
	private PointLight2D _starLight;
	private Sprite2D _glowSprite;
	private Vector2 _inputDirection = Vector2.Zero;

	private bool _isTouching = false;

	private Vector2 _lastTouchPos;

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventScreenTouch touch)
        {
            // update _isTouching to betrue when you touch the screen and false when not touching
			_isTouching = touch.Pressed;

			// stop movement direction when touch stops
			if (!touch.Pressed)
            {
                _inputDirection = Vector2.Zero;
            }
        }
		else if (@event is InputEventScreenDrag drag && _isTouching)
        {
			// drag.relative is the distance moved since last frame
			//length > 0 so it wont work when not dragged
			if (drag.Relative.Length() > 0)
            {
				// normalized makes character move at constant speed
                _inputDirection = drag.Relative.Normalized();
            }
        }
    }
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
    {
        _starLight = GetNode<PointLight2D>("PointLight2D");
		_glowSprite = GetNode<Sprite2D>("Glow");
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_inputDirection = Input.GetVector(InputConfig.InputLeft, InputConfig.InputRight, InputConfig.InputUp, InputConfig.InputDown);

		UpdateGlowVisuals();
	}


    public override void _PhysicsProcess(double delta)
    {
		// multiply direction with speed
		// if input direction is 0, we stop
		// PROBABLY NEEDS TO BE CHANGED LATER FOR AUTO SCROLLING
        Vector2 targetVelocity = _inputDirection * _speed;

		// add to character
		Velocity = Velocity.Lerp(targetVelocity, _friction);

		MoveAndSlide();
		// to make it move on limited area of the screen:
		ClampArea();

		//to make it rotate to the direction the player is dragging:
		if (_inputDirection.Length() > 0)
        {
            float targetAngle = _inputDirection.Angle();

			var playerStar = GetNode<Sprite2D>("Star");
			float smoothRotation = (float) Mathf.LerpAngle(playerStar.Rotation, targetAngle, 0.04f);
			playerStar.Rotation = smoothRotation;
			_glowSprite.Rotation = smoothRotation;
        }

    }

	/// <summary>
    /// Method for limiting area where character can be moved on screen
    /// </summary>
	private void ClampArea()
    {
		// get current screen size
		Vector2 screenSize = GetViewportRect().Size;

		// padding so doesnt touch the edge
		float padding = 50.0f;

		// where to crop the area on the right, area can use 40% of screen
		float max = screenSize.X * 0.35f;

		Vector2 position = Position;

		// clamp between left side and "wall" on the right
		position.X = Mathf.Clamp(position.X, padding, max);

		// full size vertically
		position.Y = Mathf.Clamp(position.Y, padding, screenSize.Y - padding);

		Position = position;
    }

	private void UpdateGlowVisuals()
    {
		float currentGlow = GameManager.Instance.Health; // get current health from gamemanager
		Color targetColor = GameManager.Instance.PersonalityColor(); // color that corresponds to role with most answers
		Color lerpedColor = _glowSprite.SelfModulate.Lerp(targetColor, 0.05f);

        if (_starLight != null)
        {
			_starLight.Color = lerpedColor;
            _starLight.Energy = currentGlow * 1.05f; // update light brightness, evaluate if it is a good value for this
			_starLight.TextureScale = Mathf.Lerp(0.1f, 0.3f, currentGlow); // update glow scale so that it shrinks when you lose energy etc
        }

		if (_glowSprite != null)
        {
            _glowSprite.SelfModulate = new Color(lerpedColor.R, lerpedColor.G, lerpedColor.B, currentGlow); // changing alpha to fade it out

			float glowScale = Mathf.Lerp(0.1f, 0.3f, currentGlow);
			_glowSprite.Scale = new Vector2(glowScale, glowScale);

        }
    }

}
