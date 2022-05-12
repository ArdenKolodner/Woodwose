using Godot;
using System;

public class InventoryObject : Node2D
{
    #pragma warning disable 0649
    [Export]
    protected Pickup item;
    protected Label count;
    protected Player_KinematicBody2D player;
    public override void _Ready()
    {
        count = (Label)GetNode("./Count");
        player = (Player_KinematicBody2D)GetNode("../../../Player");
    }

    public virtual void Shown() {
        count.Text = item.ToString() + ": " + player.inventory[item].ToString();
    }
}
