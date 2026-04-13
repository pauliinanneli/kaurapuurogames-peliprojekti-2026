using Godot;
using System;

public partial class StardustCollectible : Collectible
{
	[Export] private float _glowAmount = 0.2f;
    [Export] private float _lostGlow = 0.05f; // evaluate if sensible
    [Export] private AudioStreamPlayer2D _audioClip = null;
    [Export] private Sprite2D _sprite = null;

    protected override void Collect(PlayerCharacter playerCharacter)
    {
		GD.Print($"Collected glow amount: {_glowAmount}");
        Input.VibrateHandheld(100); // vibration when collecting
        GameManager.Instance.AddHealth(_glowAmount);
    }

    public void Miss()
    {
        GameManager.Instance.SubstractHealth(_lostGlow);
        QueueFree();
    }

    protected override void Clear()
    {
        if (_sprite != null)
        {
            _sprite.Hide();
        }

        if (_audioClip != null)
        {
            _audioClip.Finished += OnEffectFinished;
            _audioClip.Play();
        }
    }

    private void OnEffectFinished()
    {
        if (_audioClip != null)
        {
            _audioClip.Finished -= OnEffectFinished;
        }

        QueueFree();
    }
}
