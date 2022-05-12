using Godot;
using System;

public class Arrow : Area2D
{
    [Export]
    float speed;
    [Export]
    float lifetime;
    float age;
    public override void _Ready()
    {
        age = 0;
    }

    public override void _Process(float delta)
    {
        age += delta;
        Position += (new Vector2(1, 0)).Rotated(Rotation) * speed;
        if (age > lifetime) QueueFree();
    }

    public void _BodyEntered(KinematicBody2D body) {
        if (typeof(Enemy).IsInstanceOfType(body)) {
            ((Enemy)body).HitByArrow();
        } else if (typeof(Chicken).IsInstanceOfType(body)) {
            ((Chicken)body).HitByArrow();
        }
    }
}
