using Godot;
using System;

public class ArrowCount : Label
{
    Player_KinematicBody2D player;
    public override void _Ready()
    {
        player = (Player_KinematicBody2D)GetNode("/root/Game/Player");
        Hide();
        ((Sprite)GetNode("../ArrowCountSprite")).Hide();
    }

    public override void _Process(float delta) {
        Text = player.inventory[Pickup.Arrow].ToString();
    }

    public void ShowArrowCount() {
        Show();
        ((Sprite)GetNode("../ArrowCountSprite")).Show();
    }
}
