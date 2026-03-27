using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

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

    /// <summary>
    /// Called when node enters the scene tree.
    /// Used here to reset death state when scene is reloaded.
    /// </summary>
    public override void _Ready()
    {
        /// <summary>
        /// Reset death flag so player can die again after restarting the game.
        /// Without this, _isDead would remain true after first death,
        /// preventing Die() from triggering again.
        /// </summary>
        _isDead = false;
    }

        /// <summary>
        /// Resets game state when restarting the game
        /// </summary>
    public void ResetGame()
    {
        _isDead = false;
        _health = 0.6f;     // same value in constructor

        _personalityChoices.Clear(); // clear choices from before restarting
    }

#region health
    private float _health;

    /// <summary>
    /// Tracks if player is already dead to prevent multiple Die() calls
    /// </summary>
    private bool _isDead = false;

    private List<string> _personalityChoices = new List<string>(); // list that will store which Muusa role answers correspond with
    [Export] private float _drainHealth = 0.12f; // loses 10% glow per second, evaluate if thats a smart value or not

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

            /// <summary>
            /// Trigger death only once when health reaches zero
            /// </summary>
            if (_health <= 0 && !_isDead)
            {
                Die(); // triggers death state and shows die menu
            }
        }
    }

    /// <summary>
    /// Handles player death: pauses game and shows DieMenu UI
    /// </summary>
    public void Die()
    {
        _isDead = true;

        GetTree().Paused = true;

        // try to find DieUI safely from current scene
        var dieUI = GetTree().CurrentScene.GetNodeOrNull<DieUI>("DieUI");

        if (dieUI != null)
        {
            dieUI.ShowMenu();
        }
        else
        {
            GD.Print("DieUI not found in scene!");
        }
    }

    /// <summary>
    /// runs every frame and handles the health (light) draining
    /// </summary>
    public override void _Process(double delta)
    {
        if (GetTree().CurrentScene.Name == "StartMenu" || GetTree().CurrentScene.Name == "ConnectingStars"
            || GetTree().CurrentScene.Name == "PercentView" || GetTree().CurrentScene.Name == "LevelChooser")
        {
            return;
        }
        else
        {
            Health -= _drainHealth * (float)delta;
        }
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
    #endregion

#region personalityscore

    public List<string> GetChoices()
    {
        return _personalityChoices;
    }
        public void SaveChoice(string Role)
    {
        _personalityChoices.Add(Role);
        GD.Print("Current answers: " + string.Join(", ", _personalityChoices));


        if (HUD.Instance != null)
        {
            HUD.Instance.RefreshDots();
        }
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
    #endregion

#region gamespeed

/// <summary>
/// changes speed of the game. used during questions and answers
/// </summary>
/// <param name="targetSpeed">the speed we want to reach</param>
/// <param name="duration">the time that it will take to do the transition to targetSpeed</param>
public void ChangeGameSpeed(float targetSpeed, float duration)
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(Engine.GetSingleton("Engine"), "time_scale", targetSpeed, duration).SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
    }
#endregion
}