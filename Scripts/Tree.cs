using Godot;
using System;
using System.Collections.Generic;

public class Tree : Area2D
{
    [Export]
    List<Texture> hitTextures;
    [Export]
    Texture fallenTexture;
    int hitsTaken;
    WeaponController weaponController;
    Player_KinematicBody2D player;
    Sprite sprite;
    float hitDelay;
    public override void _Ready()
    {
        hitsTaken = 0;
        player = (Player_KinematicBody2D)GetNode("/root/Game/Player");
        weaponController = (WeaponController)GetNode("/root/Game/Player/Weapon");
        sprite = (Sprite)GetNode("./Sprite");
        hitDelay = 0;
    }

    public override void _Process(float delta) {
        if (hitDelay > 0) hitDelay -= delta;
    }

    public void _AreaEntered(Area2D area) {
        if (hitsTaken == hitTextures.Count || hitDelay > 0) return;
        if (weaponController.AreaIsActiveWeapon(area) && player.weapon == Pickup.Ax && weaponController.IsWeaponAnimationPlaying()) {
            hitDelay = .6f; // length of ax swing, to make sure it's only hit once per swing
            ++hitsTaken;
            if (hitsTaken < hitTextures.Count) sprite.Texture = hitTextures[hitsTaken];
            else {
                sprite.Texture = fallenTexture;
                Node2D wood = (Node2D)GD.Load<PackedScene>("res://Scenes/Pickup/Wood.tscn").Instance();
                GetNode("/root/Game/Pickups").CallDeferred("add_child", wood);
                wood.Position = Position + new Vector2(40, 30);
                wood.RotationDegrees = 90;
            }
        }
    }
}
