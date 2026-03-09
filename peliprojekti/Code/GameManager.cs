using Godot;
using System;
using System.Collections;

/// <summary>
/// GameManager huolehtii pelisessioon liittyvästä datasta.
/// </summary>
public partial class GameManager : Node
{
    #region Singleton


    public static GameManager Instance
    {
        get;
        private set;
    }

    public GameManager()
    {
        if (Instance == null)
        {
            Instance = this;
            _health = 0.6f;
        }
        else if (Instance != this)
        {
            QueueFree();
            return;
        }
    }
    #endregion

    #region Game Data
    private float _health;
    [Export] private float _drainHealth = 0.1f; // loses 10% glow per second, evaluate if thats a smart value or not

    public float Health
    {
        get {return _health;}
        set
        {
            //to do: mieti onko järkevä maksimiarvo
            _health = Mathf.Clamp(value, 0, 1);
            // GD.Print($"Health atm: {_health}");

            if (_health <= 0)
            {
                GetTree().ReloadCurrentScene(); // reloads level when not enough health. need to also add some kind of message for losing
            }
        }
    }
    #endregion

    /// <summary>
    /// runs every frame and handles the health (light) draining
    /// </summary>
    public override void _Process(double delta)
    {
        Health -= _drainHealth * (float)delta;
    }

    public bool AddHealth(float amount)
    {
        if (amount < 0)
        {
            return false;
        }

        Health += amount;
        return true;
    }

    public bool SubstractHealth(float amount)
    {
        if (amount < 0)
        {
            return false;
        }

        Health -= amount;
        return true;
    }
}
