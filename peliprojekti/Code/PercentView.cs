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

    public override void _Input(InputEvent @event)
    {
        // check if waiting for tap and if event is tap
        if (_waitingForTap && @event is InputEventMouseButton tap)
        {
            // trigger when finger is pressed down
            if (tap.Pressed && tap.ButtonIndex == MouseButton.Left)
            {
                _waitingForTap = false;
                Input.VibrateHandheld(50); // small haptic feedback
                GetTree().ChangeSceneToFile(NextScenePath);
            }
        }
    }

    private void CalculateResults()
    {
        var choices = GameManager.Instance.GetChoices();
        float total = choices.Count;
        float edistäjäCount = 0;
        float etenijäCount = 0;
        float etsijäCount = 0;


        if (total == 0)
        {
            return;
        }

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

            // put those numbers into progresss bars
            if (EdistäjäBar != null)
            {
                EdistäjäBar.Value = edistäjäPercent;
            }
            if (EtenijäBar != null)
            {
                EtenijäBar.Value = etenijäPercent;
            }
            if (EtsijäBar != null)
            {
                EtsijäBar.Value = etsijäPercent;
            }

            // add % to these labels
            Edistäjä.Text = $"{edistäjäPercent}%";
            Etenijä.Text = $"{etenijäPercent}%";
            Etsijä.Text = $"{etsijäPercent}%";
        }
    }
}
