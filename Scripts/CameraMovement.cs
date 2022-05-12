using Godot;
using System;

public class CameraMovement : Camera2D
{
    [Export]
    public float minX;
    [Export]
    public float maxX;
        [Export]
    public float minY;
        [Export]
    public float maxY;
    Player_KinematicBody2D player;
    public override void _Ready()
    {
        player = (Player_KinematicBody2D)GetNode("../Player");
    }

    public override void _Process(float delta)
    {
        Position = new Vector2(YvainMath.clamp(player.Position.x, minX, maxX), YvainMath.clamp(player.Position.y, minY, maxY));
        GetParent().MoveChild(this, GetParent().GetChildren().Count);
    }
}
