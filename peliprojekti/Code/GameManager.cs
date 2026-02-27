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
        }
        else if (Instance != this)
        {
            QueueFree();
            return;
        }
    }
    #endregion

    #region Game Data
    private float _health = 0;

    public float Health
    {
        get {return _health;}
        set
        {
            //to do: mieti onko järkevä maksimiarvo
            _health = Mathf.Clamp(value, 0, 1);
            GD.Print($"Health atm: {_health}");
            //to do: päivitä pisteet käyttöliittymälle
        }
    }
    #endregion

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
