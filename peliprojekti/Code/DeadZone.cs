using Godot;
using System;

public partial class DeadZone : Area2D
{
    // evaluate later if it's a sane amount of health to lose?
    [Export] private float _healthLost = 0.1f;

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
        else if (area is Collectible otherCollectible)
        {
            otherCollectible.QueueFree();
        }
    }
}
