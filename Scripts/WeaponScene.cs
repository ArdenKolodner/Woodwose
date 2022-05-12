using Godot;
using System;

public class WeaponScene : Area2D
{
    [Export]
    public bool chopTrees;
    [Export]
    public bool swingBackAndForth;
    [Export]
    public bool isBow;
    public AnimationPlayer anim {
        get;
        private set;
    }
    public override void _Ready()
    {
        anim = (AnimationPlayer)GetNode("./AnimationPlayer");
    }
}
