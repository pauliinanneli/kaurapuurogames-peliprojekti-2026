using Godot;
using System;

public partial class StardustCollectible : Collectible
{
	[Export] private float _glowAmount = 0.2f;
  [Export] private float _lostGlow = 0.05f; // evaluate if sensible

    protected override void Collect(PlayerCharacter playerCharacter)
    {
		GD.Print($"Collected glow amount: {_glowAmount}");
    Input.VibrateHandheld(100); // vibration when collecting
    GameManager.Instance.AddHealth(_glowAmount);
    }

    public void Miss()
    {
        GD.Print($"Missed stardust, health lost {_lostGlow}");
        // TO DO: evaluate if it's a smart amount of health to lose when missing
        GameManager.Instance.SubstractHealth(_lostGlow);
        QueueFree();
    }
}
