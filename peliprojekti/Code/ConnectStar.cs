using Godot;
using System;

public partial class ConnectStar : Area2D
{
    [Export] public int _starIndex;
    [Export] public PointLight2D _light;
    [Export] public int _starsNeededToFinish = 7;
    [Export] public int _loopBackIndex = 3;

    public bool IsLit = false;

    private static int _currentIndex = 0;
    private static Line2D _line; // line that will connect all the stars to each other
    private bool _isDragging = false;

    public override void _Ready()
    {
        _line = GetParent().GetTree().CurrentScene.FindChild("Line2D") as Line2D; // gets line from parent node

        if (_starIndex == 0)
        {
            _currentIndex = 0; //reset counter when level starts


            if (_line != null)
            {
                _line.ClearPoints(); // deletes old points from line2d
                _line.AddPoint(GlobalPosition); // add first point to the middle of first star
            }
        }
        MouseEntered += OnTouch;
    }

    public override void _Process(double delta)
    {
        if (_line == null)
        {
            return;
        }


        if (IsLit && _starIndex == _currentIndex - 1) // only last lit star can start stretching line
        {
            // if finger is down, move the line
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                UpdateStretchyLine();
            }
            else
            {
                RemoveStretchyLine();
            }

        }
    }

    private void UpdateStretchyLine()
    {
        if (_line == null)
        {
            return;
        }

        Vector2 mousePosition = GetGlobalMousePosition();

        if (_line.GetPointCount() <= _currentIndex)
        {
            _line.AddPoint(mousePosition);
        }
        else
        {
            _line.SetPointPosition(_line.GetPointCount() - 1, mousePosition);
        }
    }

    private void RemoveStretchyLine()
    {
        // if player stops dragging all of a sudden, delete the stretchy line
        if (_line.GetPointCount() > _currentIndex)
        {
            _line.RemovePoint(_line.GetPointCount() - 1);
        }
    }

    private void OnTouch()
    {
        // dont do anything if player isnt touching screen
        if (!Input.IsMouseButtonPressed(MouseButton.Left))
        {
            return;
        }

        // go through stars in order
        if (_starIndex == _currentIndex && !IsLit)
        {
            RegisterHit();
        }
        // closing looping constellation
        else if (_starIndex == _loopBackIndex && _currentIndex == _starsNeededToFinish)
        {
            FinalizeConstellation();
        }
    }

    private void RegisterHit()
    {
        if (_line == null)
        {
            return;
        }


        // snap finger point and snap to stars center
        if (_line.GetPointCount() > _currentIndex)
        {
            _line.SetPointPosition(_line.GetPointCount() - 1, GlobalPosition);
        }

        LightUp();
        _currentIndex++;
    }

    private void FinalizeConstellation()
    {
        //snap back to loopback star
        if (_line != null && _line.GetPointCount() > _currentIndex)
        {
            _line.SetPointPosition(_line.GetPointCount() - 1, GlobalPosition);
            Input.VibrateHandheld(150); // vibration
        }

        // look for script in scene root
        if (GetTree().CurrentScene is ConnectingStars levelRoot)
        {
            levelRoot.OnConstellationFinished();
            levelRoot.PlayStarSound();
        }

        _currentIndex = 999;
    }

    public void LightUp()
    {
        if (GetTree().CurrentScene is ConnectingStars levelRoot)
        {
            levelRoot.PlayStarSound();
        }

        IsLit = true;
        Input.VibrateHandheld(100); // vibration when lighting up
        _light.Enabled = true;
    }
}
