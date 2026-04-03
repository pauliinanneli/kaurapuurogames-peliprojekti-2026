using Godot;
using System;

public partial class ConnectingStars : Node2D
{
	[Export] public Label _instructionLabel;
	[Export] public string NextScenePath = "res://Scenes/PercentView.tscn";
	private bool _waitingForTap = false;

    public override void _Ready()
    {
		// set first text of label
        if (_instructionLabel != null)
        {
            _instructionLabel.Text = "Yhdistä tähdet";
        }
    }

	public override void _Input(InputEvent @event)
    {
		//check if waiting for tap and if event is a tap
        if (_waitingForTap && @event is InputEventMouseButton tap)
        {
			// trigger when finger is pressed down
            if (tap.Pressed && tap.ButtonIndex == MouseButton.Left)
            {
                _waitingForTap = false;
				GetTree().ChangeSceneToFile(NextScenePath);
            }
        }
    }
	public void OnConstellationFinished()
    {
		//change text on instructionlabel when constellation done
        if (_instructionLabel != null)
        {
                // TO DO: find out how this works out with the translation?
            _instructionLabel.Text = "Napauta näyttöä nähdäksesi tuloksesi.";
        }
		_waitingForTap = true;
    }
}
