using Godot;
using System;

public partial class CenterConnectStars : Control
{
    public override void _Ready()
    {
        // snap to middle of phone
        GlobalPosition = GetViewportRect().Size / 2;

        // make sure pivot doesnt shift the constellation
        PivotOffset = Vector2.Zero;
    }
}
