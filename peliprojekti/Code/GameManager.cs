using Godot;
using System;
using System.Collections;

public partial class GameManager : Node
{
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
        }
        else if (Instance != this)
        {
            QueueFree();
            return;
        }
    }
}
