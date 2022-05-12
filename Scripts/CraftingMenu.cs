using Godot;
using System;

public class CraftingMenu : Node2D
{
    #pragma warning disable 0649
    Player_KinematicBody2D player;
    InteractLabel interactLabel;
    public bool craftingMenuOpen {
        get;
        private set;
    }
    public override void _Ready()
    {
        craftingMenuOpen = false;
        player = (Player_KinematicBody2D)GetNode("../");
        interactLabel = (InteractLabel)player.GetNode("InteractLabel");
    }

    public void ShowMenu() {
        craftingMenuOpen = true;
        player.DisableMove();
        //interactLabel.Hide();
        ((OverallMethods)GetTree().Root.GetNode("Game")).SendSignalMenuOpening();
        Show();
    }

    public void HideMenu() {
        craftingMenuOpen = false;
        player.EnableMove();
        //interactLabel.Show();
        ((OverallMethods)GetTree().Root.GetNode("Game")).SendSignalMenuClosing();
        Hide();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
