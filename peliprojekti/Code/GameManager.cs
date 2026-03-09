using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

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


    private float _health;
    private List<string> _personalityChoices = new List<string>(); // list that will store which Muusa role answers correspond with
    [Export] private float _drainHealth = 0.1f; // loses 10% glow per second, evaluate if thats a smart value or not

    [Export] public Color _defaultColor = Color.FromHtml("#FFD580");
    [Export] public Color _etsijäColor = Color.FromHtml("#00E5FF");
    [Export] public Color _etenijäColor = Color.FromHtml("#FF7EB9");
    [Export] public Color _edistäjäColor = Color.FromHtml("#D4FF91");

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

    public void SaveChoice(string Role)
    {
        _personalityChoices.Add(Role);
        GD.Print("Current answers: " + string.Join(", ", _personalityChoices));
    }

    public Color PersonalityColor()
    {
        //count how many time the word is in the list
        int edistäjäCount = 0;
        int etenijäCount = 0;
        int etsijäCount = 0;

        foreach (string choice in _personalityChoices)
        {
            if (choice == "Edistäjä")
            {
                edistäjäCount++;
            }
            else if (choice == "Etenijä")
            {
                etenijäCount++;
            }
            else if (choice == "Etsijä")
            {
                etsijäCount++;
            }
        }

        if (edistäjäCount == 0 && etenijäCount == 0 && etsijäCount == 0)
        {
            return _defaultColor;
        }

        if (edistäjäCount >= etenijäCount && edistäjäCount >= etsijäCount)
        {
            return _edistäjäColor;
        }
        else if (etenijäCount >= edistäjäCount && etenijäCount >= etsijäCount)
        {
            return _etenijäColor;
        }
        else if (etsijäCount >= edistäjäCount && etsijäCount >= etenijäCount)
        {
            return _etsijäColor;
        }
        else
        {
            return _defaultColor;
        }
    }
}
