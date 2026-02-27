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
    private int _health = 0;

    public int Health
    {
        get {return _health;}
        set
        {
            //to do: mieti onko järkevä maksimiarvo
            _health = Mathf.Clamp(value, 0, 10);
            //to do: päivitä pisteet käyttöliittymälle
        }
    }
    #endregion

    public bool AddHealth(int amount)
    {
        if (amount < 0)
        {
            return false;
        }

        Health += amount;
        return true;
    }

    public bool SubstractHealth(int amount)
    {
        if (amount < 0)
        {
            return false;
        }

        Health -= amount;
        return true;
    }
}
