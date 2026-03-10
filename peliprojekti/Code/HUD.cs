using Godot;
using System;

public partial class HUD : CanvasLayer
{
    public static HUD Instance
    {
        get;
        private set;
    }
	[Export] public PackedScene _dot;

	[Export] private HBoxContainer _container;

    public override void _Ready()
    {
        Instance = this;
    }

	public void RefreshDots()
    {
        GD.Print("HUD: REFRESH METHOD TRIGGERED!");
        if (_container == null)
        {
            return;
        }

        foreach (Node child in _container.GetChildren())
        {
            child.QueueFree();
        }

		var choices = GameManager.Instance.GetChoices();

        GD.Print($"Creating {choices.Count} dots");

		foreach (string roleName in choices)
        {
            ColorRect dot = _dot.Instantiate<ColorRect>();

			if (roleName == "Edistäjä") {
				dot.Color = GameManager.Instance._edistäjäColor;
			}
			else if (roleName == "Etenijä")
			{
                dot.Color = GameManager.Instance._etenijäColor;
            }
			else if (roleName == "Etsijä")
            {
                dot.Color = GameManager.Instance._etsijäColor;
            }

			_container.AddChild(dot);
        }
    }
}
