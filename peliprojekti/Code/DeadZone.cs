using Godot;
using System;

public partial class DeadZone : Area2D
{

    public override void _EnterTree()
    {
        AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is StardustCollectible stardust)
        {
            stardust.Miss();
        }
        else if (area is Meteor meteor)
        {
            meteor.QueueFree();
        }
    }
}
