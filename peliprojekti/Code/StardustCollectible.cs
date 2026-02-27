using Godot;
using System;

public partial class StardustCollectible : Collectible
{
	[Export] private float _glowAmount = 0.2f;

    protected override void Collect(PlayerCharacter playerCharacter)
    {
		// to do: lisää glow ("pisteet") järjestelmään joka pitää siitä kirjaa
		GD.Print($"Glow amount: {_glowAmount}");

    }
}
