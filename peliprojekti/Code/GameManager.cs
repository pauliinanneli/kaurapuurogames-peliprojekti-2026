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

    private float _health;
    private bool _isDead = false;

    private List<string> _personalityChoices = new List<string>(); // list that will store which Muusa role answers correspond with
    [Export] private float _drainHealth = 0.08f; // loses 8% glow per second, evaluate if thats a smart value or not

    [Export] public Color _defaultColor = Color.FromHtml("#FFD580");
    [Export] public Color _etsijäColor = Color.FromHtml("#00E5FF");
    [Export] public Color _etenijäColor = Color.FromHtml("#FF7EB9");
    [Export] public Color _edistäjäColor = Color.FromHtml("#D4FF91");
    [Export] private AudioStreamPlayer _clickPlayer;
    [Export] private AudioStreamPlayer _gateSoundPlayer;
    [Export] private AudioStreamPlayer _musicPlayer;
    public bool _cameFromStartMenu = true;

    #region Singleton

    /// <summary>
    /// getter and setter for singleton instance
    /// </summary>
    public static GameManager Instance
    {
        get;
        private set;
    }

    /// <summary>
    /// constructor that initializes singleton and sets initial health.
    /// if instance already exists, this is destroyed with queuefree
    /// </summary>
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

    /// <summary>
    /// players current health/glow level. values are between 0 and 1
    /// triggers die() when reaches 0
    /// </summary>
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
        bool isMainMenu = GetTree().CurrentScene.Name == "StartMenu" || GetTree().CurrentScene.Name == "LevelChooser" || GetTree().CurrentScene.Name == "Credits";

        if (isMainMenu)
        {
            if (_musicPlayer.Playing)
            {
                _musicPlayer.Stop(); // dont play level music when in main menu scenes
            }
            return;
        }

        else
        {
            if (!_musicPlayer.Playing)
            {
                _musicPlayer.Play(); // if in level or settings etc, let the music play
            }

            bool isSafeScene = GetTree().CurrentScene.Name == "StartMenu" || GetTree().CurrentScene.Name == "ConnectingStars"
                                || GetTree().CurrentScene.Name == "Tutorial" || GetTree().CurrentScene.Name == "PercentView";

            if (!GetTree().Paused && !isSafeScene)
            {
                Health -= _drainHealth * (float)delta;
            }
        }
    }

/// <summary>
/// used to manage language settings ( fi / en )
/// </summary>
/// <param name="locale"></param>
    public void SetLocale(string locale)
    {
        TranslationServer.SetLocale(locale);
    }


    /// <summary>
    /// increases players health by a specific amount
    /// </summary>
    /// <param name="amount">the amount of health to add</param>
    /// <returns>true if worked</returns>
    public bool AddHealth(float amount)
    {
        if (amount < 0)
        {
            return false;
        }

        Health += amount;
        return true;
    }

    /// <summary>
    /// decreases health by a specific amount
    /// </summary>
    /// <param name="amount">the amount of health to subtract</param>
    /// <returns>true if worked</returns>
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

    /// <summary>
    /// gets list of all personality choices saved during game session
    /// </summary>
    /// <returns>list of strings that has roles chosen during game</returns>
    public List<string> GetChoices()
    {
        return _personalityChoices;
    }


    /// <summary>
    /// saves chosen personality choice and refreshes hud visuals with "personality score"
    /// </summary>
    /// <param name="Role">name of the personality role</param>
    public void SaveChoice(string Role)
    {
        _personalityChoices.Add(Role);
        GD.Print("Current answers: " + string.Join(", ", _personalityChoices));


        if (HUD.Instance != null)
        {
            HUD.Instance.RefreshDots();
        }
    }

    /// <summary>
    /// calculates which personality has been chosen most times and returns its color
    /// goes to defaultcolor if no choices made
    /// </summary>
    /// <returns>the color corresponding to the personality that has been chosen most times</returns>
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

#region audio

    public void PlayClick()
    {
        if (_clickPlayer != null)
        {
            _clickPlayer.Play();
        }
    }

    public void GateSound()
    {
        if (_gateSoundPlayer != null)
        {
            _gateSoundPlayer.Play();
        }
    }

#endregion
}