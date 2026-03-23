using Godot;
using System;

public partial class LevelChooser : Control
{
    public override void _Ready()
    {
        //wait one frame for godot to calculate map size before scrolling to bottom
        CallDeferred(MethodName.ScrollToBottom);
    }

    private void ScrollToBottom()
    {
        var scrollContainer = GetNode<ScrollContainer>("ScrollContainer");

        // get scroll bar and set our position to its max value
        var vBar = scrollContainer.GetVScrollBar();
        scrollContainer.ScrollVertical = (int)vBar.MaxValue;
    }
}