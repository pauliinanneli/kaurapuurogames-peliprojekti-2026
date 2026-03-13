using Godot;
using System;

public partial class ConnectStar : Area2D
{
    [Export] public int _starIndex;
    [Export] public PointLight2D _light;
    public bool IsLit = false;
    private static int _currentIndex;

    public override void _Ready()
    {
        if (_starIndex == 0)
        {
            _currentIndex = 0; //reset counter when level starts
        }
        MouseEntered += OnTouch;
    }

    private void OnTouch()
    {
        if (!Input.IsMouseButtonPressed(MouseButton.Left))
        {
            return;
        }
        if (_starIndex == _currentIndex && !IsLit)
        {
            LightUp();
            _currentIndex++;
        }
    }

    public void LightUp()
    {
        IsLit = true;
        _light.Enabled = true;
    }
}
