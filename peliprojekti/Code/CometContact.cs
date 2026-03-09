using Godot;
using System;

/// <summary>
/// Konkreettinen meteoriitti, joka vähentää pelaajan healthia törmätessä.
/// </summary>
public partial class CometContact : Crashable
{
    [Export] private float _damageAmount = 0.2f; // kuinka paljon healthia menetetään osuessa, säädettävä arvo

    protected override void Crash(PlayerCharacter playerCharacter)
    {
        GD.Print($"Hit by comet! Health decreased by {_damageAmount}");
        GameManager.Instance.SubstractHealth(_damageAmount);
    }
}