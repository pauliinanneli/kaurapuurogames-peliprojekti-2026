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

	[Export] private HBoxContainer _etsijäContainer;
    [Export] private HBoxContainer _edistäjäContainer;
    [Export] private HBoxContainer _etenijäContainer;

    public override void _Ready()
    {
        Instance = this;
    }

	public void RefreshDots()
    {
        if (_edistäjäContainer != null)
        {
            foreach (Node child in _edistäjäContainer.GetChildren())
            {
                child.QueueFree();
            }
        }
        if (_etenijäContainer != null)
        {
            foreach (Node child in _etenijäContainer.GetChildren())
            {
                child.QueueFree();
            }
        }
        if (_etsijäContainer != null)
        {
            foreach (Node child in _etsijäContainer.GetChildren())
            {
                child.QueueFree();
            }
        }


		var choices = GameManager.Instance.GetChoices();


		foreach (string roleName in choices)
        {
            ColorRect dot = _dot.Instantiate<ColorRect>();

			if (roleName == "Edistäjä") {
				dot.Color = GameManager.Instance._edistäjäColor;
                _edistäjäContainer.AddChild(dot);
			}
			else if (roleName == "Etenijä")
			{
                dot.Color = GameManager.Instance._etenijäColor;
                _etenijäContainer.AddChild(dot);
            }
			else if (roleName == "Etsijä")
            {
                dot.Color = GameManager.Instance._etsijäColor;
                _etsijäContainer.AddChild(dot);
            }

        }
    }
}
