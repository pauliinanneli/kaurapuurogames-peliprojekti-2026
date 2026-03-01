using Godot;
using System;

public partial class StardustCollectible : Collectible
{
	[Export] private float _glowAmount = 0.2f;

    protected override void Collect(PlayerCharacter playerCharacter)
    {
		// to do: lisää glow ("pisteet") järjestelmään joka pitää siitä kirjaa
		GD.Print($"Collected glow amount: {_glowAmount}");
    GameManager.Instance.AddHealth(_glowAmount);
    }

    public void Miss()
    {
        GD.Print($"Missed stardust, health lost {_glowAmount}");
        // TO DO: evaluate if it's a smart amount of health to lose when missing
        GameManager.Instance.SubstractHealth(_glowAmount);
        QueueFree();
    }
}
