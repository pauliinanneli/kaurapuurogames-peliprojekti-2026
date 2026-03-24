using Godot;
using System;

public partial class Glow : Sprite2D
{
	[Export] public Node2D _targetStar; //put playercharacter node here in godot
	[Export] public float _followSpeed = 7.0f; // TEST IF NICE SPEED

    public override void _Ready()
    {
        if (_targetStar != null)
        {
            GlobalPosition = _targetStar.GlobalPosition; // when level starts jump straight to where star is to avoid stupid ahh jump from 0,0 to there
        }
    }
    public override void _Process(double delta)
    {
        if (_targetStar == null)
        {
            return;
        }
		GlobalPosition = GlobalPosition.Lerp(_targetStar.GlobalPosition, (float)delta * _followSpeed); // using lerp to make glow follow stars position with slight lag
    }

}
