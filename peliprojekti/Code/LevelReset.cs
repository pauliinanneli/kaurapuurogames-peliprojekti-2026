using Godot;
using System;

public partial class LevelReset : Node2D
{

    public override void _Ready()
    {
        GameManager.Instance.ResetGame();
    }
}
