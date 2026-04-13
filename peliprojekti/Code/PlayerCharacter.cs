using Godot;
using System;
using System.Threading.Tasks;

public partial class PlayerCharacter : CharacterBody2D
{

	[Export] private float _speed = 300.0f;
	[Export] private float _friction = 0.2f;
	[Export] private float _maxStretch = 0.3f; // max % star can stretch to
	[Export] private float _stretchSmoothing = 0.1f;
	[Export] private Color _hitColor = Colors.Red;


	private PointLight2D _starLight;
	private Sprite2D _glowSprite;
	private Sprite2D _starSprite;
	private Vector2 _inputDirection = Vector2.Zero;
	private bool _isTouching = false;
	private bool _isHit = false;
	private bool _blackHole = false;
	private string _targetScene;


	private Vector2 _starInitialScale; // save scale of star from editor


/// <summary>
/// handles touch and drag inputs on mobile device
/// </summary>
/// <param name="event"></param>
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
		_starSprite = GetNode<Sprite2D>("Star");
		_starInitialScale = GetNode<Sprite2D>("Star").Scale; // get scale of star from editor
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_blackHole) // so that visuals wont update with this if sucked into black hole
        {
            return;
        }
		_inputDirection = Input.GetVector(InputConfig.InputLeft, InputConfig.InputRight, InputConfig.InputUp, InputConfig.InputDown);

		UpdateGlowVisuals();
	}


    public override void _PhysicsProcess(double delta)
    {
		// dont execute rest of this if currently being sucked into the black hole
		if (_blackHole)
        {
			MoveAndSlide(); // so gravity will still move us
            return;
        }

		// multiply direction with speed
		// if input direction is 0, we stop
        Vector2 targetVelocity = _inputDirection * _speed;

		// add to character
		Velocity = Velocity.Lerp(targetVelocity, _friction);

		MoveAndSlide();

		// to make it move on limited area of the screen:
		ClampArea();
		SquashAndStretch(); // to distort star based on velocity
		HandleRotation(); // to rotate to direction of dragging

    }

	/// <summary>
    /// Method for limiting area where character can be moved on screen
    /// </summary>
	private void ClampArea()
    {
		// get current screen size
		Vector2 globalPos = GlobalPosition;
		Rect2 worldRect = GetViewport().GetCanvasTransform().AffineInverse() * GetViewportRect();

		// padding so doesnt touch the edge
		float padding = 50.0f;

		// define playable zone (now 35% of what screen is being used)
		float minX = worldRect.Position.X + padding;
		float maxX = worldRect.Position.X + (worldRect.Size.X * 0.35f);
		float minY = worldRect.Position.Y + padding;
		float maxY = worldRect.End.Y - padding;


		// clamp between left side and "wall" on the right
		globalPos.X = Mathf.Clamp(globalPos.X, minX, maxX);
		globalPos.Y = Mathf.Clamp(globalPos.Y, minY, maxY);

		GlobalPosition = globalPos;
    }

	/// <summary>
    /// updates glow sprite and pointlight2d based on health and speed
    /// </summary>
	private void UpdateGlowVisuals()
    {
		float currentGlow = GameManager.Instance.Health; // get current health from gamemanager
		Color targetColor = GameManager.Instance.PersonalityColor(); // color that corresponds to role with most answers
		Color lerpedColor = _glowSprite.SelfModulate;

		if (!_isHit && _glowSprite != null) // only update if not hit/flashing red
        {
			lerpedColor = _glowSprite.SelfModulate.Lerp(targetColor, 0.05f);
			_glowSprite.SelfModulate = new Color(lerpedColor.R, lerpedColor.G, lerpedColor.B, currentGlow); // changing alpha to fade it out
        }

        if (_starLight != null) // update pointlight called "starlight"
        {
			_starLight.Color = lerpedColor;
            _starLight.Energy = currentGlow * 1.05f; // update light brightness, evaluate if it is a good value for this
			_starLight.TextureScale = Mathf.Lerp(0.1f, 0.3f, currentGlow); // update glow scale so that it shrinks when you lose energy etc
        }

		// velocity stretch (same as star has)
		float speedRatio = Velocity.Length() / _speed;
		float stretch = speedRatio * _maxStretch;

		if (_glowSprite != null) // update the glow sprite that is used
        {
			float initialGlowScale = Mathf.Lerp(0.1f, 0.3f, currentGlow);

			Vector2 targetGlowScale = new Vector2(
												initialGlowScale + (initialGlowScale * stretch),
												initialGlowScale - (initialGlowScale * stretch * 0.5f)
												);


			_glowSprite.Scale = _glowSprite.Scale.Lerp(targetGlowScale, _stretchSmoothing);

        }
    }

	/// <summary>
	/// method that flashes color of player character in red if character hit by a meteor
	/// </summary>
	public void OnMeteorHit()
    {
        if (_isHit)
        {
            return; // dont execute again if already hit
        }

		_isHit = true;

		var flashing = GetTree().CreateTween();

		// flashing star3 times in red by looping this 3 times
		for (int i = 0; i < 3; i++)
        {
			//flashing to red
			flashing.TweenProperty(_starSprite, "modulate", _hitColor, 0.05); // snap star to red color

			// going back to normal
			flashing.TweenInterval(0.05f); // creates a tiny pause

			flashing.TweenProperty(_starSprite, "modulate", Colors.White, 0.1f); // snap to personality color
        }
		flashing.Finished += ResetFlash;
    }

	/// <summary>
	/// method for just switching back to not flashing from being hit
	/// </summary>
	private void ResetFlash()
    {
		_isHit = false;
    }

	/// <summary>
	/// distorts star based on velocity
	/// </summary>
	private void SquashAndStretch()
    {
		//get star sprite
		var playerStar = GetNode<Sprite2D>("Star");

		// squash and stretch:
		// calculate stretching based on velocity, dividing by speed so 0.0 = stopped and 1.0 = max speed
		float speedRatio = Velocity.Length() / _speed;
		float stretch = speedRatio * _maxStretch; // tells how much should scale grow to stretch length of star

		//apply stretch to initial scale
		Vector2 targetScale = new Vector2(
										_starInitialScale.X + (_starInitialScale.X * stretch),
										_starInitialScale.Y - (_starInitialScale.Y * stretch * 0.5f));

		playerStar.Scale = playerStar.Scale.Lerp(targetScale, _stretchSmoothing);
    }

	/// <summary>
    /// rotates star and its glow with direction of movement
    /// </summary>
	private void HandleRotation()
    {
		var playerStar = GetNode<Sprite2D>("Star");
		//to make it rotate to the direction the player is dragging:
		if (_inputDirection.Length() > 0)
        {
            float targetAngle = _inputDirection.Angle();

			float smoothRotation = (float) Mathf.LerpAngle(playerStar.Rotation, targetAngle, 0.04f);
			playerStar.Rotation = smoothRotation;
			_glowSprite.Rotation = smoothRotation;
        }
    }

	/// <summary>
    /// executes what happens when player hits black hole at the end of level
    /// </summary>
	public void BlackHoleSuck(string scenePath, Vector2 holePosition)
    {
        if (_blackHole)
        {
            return;
        }

		_blackHole = true;
		_targetScene = scenePath;

		LookAt(holePosition); // make star "look at" center of black hole

		// create sucking tween
		var suckTween = GetTree().CreateTween();

		suckTween.SetParallel(true); // make animations run at same time

		// visuals
		suckTween.TweenProperty(_starLight, "energy", 0.0f, 0.2f); // kill pointlight because it cant be stretched
		suckTween.TweenProperty(_glowSprite, "modulate:a", 0.0f, 0.1f); // kill glow sprite because it looks dumb

		suckTween.TweenProperty(this, "global_position", holePosition, 1.5f); // move player body to center of black hole
		suckTween.TweenProperty(_starSprite, "scale", new Vector2(7.0f, 0.01f), 1.5f); // stretch player star to be long and thin
		suckTween.TweenProperty(this, "modulate", new Color(0, 0, 0, 0), 1.5f); // make star color modulate to black and transparency to 0

		suckTween.SetParallel(false); // stop running animations when they end

		suckTween.Finished += FinishBlackHole;
    }

	private void FinishBlackHole()
    {
        GetTree().ChangeSceneToFile(_targetScene);
    }
}
