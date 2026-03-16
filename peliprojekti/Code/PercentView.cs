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

    public override void _Ready()
    {
        CalculateResults();
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

            Edistäjä.Text = $"{edistäjäPercent}%";
            Etenijä.Text = $"{etenijäPercent}%";
            Etsijä.Text = $"{etsijäPercent}%";
        }
    }
}
