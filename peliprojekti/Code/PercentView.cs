using Godot;
using System;

public partial class PercentView : Control
{
    [Export] public Label Edistäjä;
    [Export] public Label Etenijä;
    [Export] public Label Etsijä;
    [Export] public ProgressBar EdistäjäBar;
    [Export] public ProgressBar EtenijäBar;
    [Export] public ProgressBar EtsijäBar;

    [Export] public string NextScenePath = "res://Scenes/StartMenu.tscn";
    private bool _waitingForTap = false;

    public override void _Ready()
    {
        CalculateResults();
        _waitingForTap = true;
    }

    /// <summary>
    /// updates text labels to match the current animated value of progress bars
    /// </summary>
    public override void _PhysicsProcess(double delta)
    {
        if (EdistäjäBar != null && Edistäjä != null)
        {
            Edistäjä.Text = $"{(int)EdistäjäBar.Value}%";
        }

        if (EtenijäBar != null && Etenijä != null)
        {
            Etenijä.Text = $"{(int)EtenijäBar.Value}%";
        }

        if (EtsijäBar != null && Etsijä != null)
        {
            Etsijä.Text = $"{(int)EtsijäBar.Value}%";
        }
    }

    public override void _Input(InputEvent @event)
    {
        // check if waiting for tap and if event is tap
        if (_waitingForTap && @event is InputEventMouseButton tap)
        {
            // trigger when finger is pressed down
            if (tap.Pressed && tap.ButtonIndex == MouseButton.Left)
            {
                GameManager.Instance.PlayClick();
                _waitingForTap = false;
                GetTree().ChangeSceneToFile(NextScenePath);
            }
        }
    }

    /// <summary>
    /// calculates % based on player choices
    /// </summary>
    private void CalculateResults()
    {
        var choices = GameManager.Instance.GetChoices();
        float total = choices.Count;
        float edistäjäCount = 0;
        float etenijäCount = 0;
        float etsijäCount = 0;

        foreach (string choice in choices)
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

        if (total > 0)
        {
            // round numbers to int so they dont show decimals
            int edistäjäPercent = Mathf.RoundToInt((edistäjäCount / total) * 100);
            int etenijäPercent = Mathf.RoundToInt((etenijäCount / total) * 100);
            int etsijäPercent = Mathf.RoundToInt((etsijäCount / total) * 100);


            // call animation method for bars to animate them from bottom to top
            AnimateBar(EdistäjäBar, edistäjäPercent);
            AnimateBar(EtsijäBar, etsijäPercent);
            AnimateBar(EtenijäBar, etenijäPercent);
        }
    }

    /// <summary>
    /// a method for animating the progress bars from bottom to top with the values of players answers
    /// </summary>
    /// <param name="progressbar">the progress bar</param>
    /// <param name="targetPercent">the percent value the bar will be animated to from 0</param>
    private void AnimateBar(ProgressBar progressbar, int targetPercent)
    {
        if (progressbar == null)
        {
            return;
        }

        // start progressbar from zero
        progressbar.Value = 0;

        var tween = GetTree().CreateTween();

        // change value of progressbar over 2,5 seconds and set type of transition and easing for it
        tween.TweenProperty(progressbar, "value", (float)targetPercent, 2.5f).SetTrans(Tween.TransitionType.Quart).SetEase(Tween.EaseType.Out);
    }
}
