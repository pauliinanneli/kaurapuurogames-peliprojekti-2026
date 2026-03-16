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

        if (_starIndex == 0)
        {
            _currentIndex = 0; //reset counter when level starts

            _line = GetParent().GetNode<Line2D>("Line2D"); // gets line from parent node
            if (_line != null)
            {
                _line.ClearPoints(); // deletes old points from line2d
                _line.AddPoint(Position); // add first point to the middle of first star
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

        try
        {
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
        catch (Exception e)
        {
            GD.Print("caught a crash: " + e.Message);
        }
    }

    private void UpdateStretchyLine()
    {
        if (_line == null)
        {
            return;
        }

        Vector2 mousePosition = _line.ToLocal(GetGlobalMousePosition());

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

        Vector2 starPosition = _line.ToLocal(GlobalPosition);
        // snap finger point and snap to stars center
        if (_line.GetPointCount() > _currentIndex)
        {
            _line.SetPointPosition(_line.GetPointCount() - 1, starPosition);
        }

        LightUp();
        _currentIndex++;
    }

    private void FinalizeConstellation()
    {
        _currentIndex = 999;
        // TO DO add logic for when constellation is finished
    }

    public void LightUp()
    {
        IsLit = true;
        _light.Enabled = true;
    }
}
