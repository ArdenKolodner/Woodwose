using Godot;
using System;

public class Chicken : KinematicBody2D
{
    #pragma warning disable 0649
    [Export]
    int maxHealth;
    int health;
    [Export]
    float speed;
    [Export]
    float idleSpeed;
    [Export]
    float viewThreshold;
    [Export]
    float scaredThreshold;
    [Export]
    float timePerMovement;
    [Export]
    float variance;
    Vector2 movementDirection;
    float movementTimeElapsed;
    Player_KinematicBody2D player;
    [Export]
    float invincibilityTime;
    float invincibility;
    bool scared;
    ColorRect healthBar;
    float healthBarFullLength;
    [Export]
    int meatNum;
    [Export]
    int feathersNum;
    [Export]
    int extraArrowMeat;
    CutscenePlayer cutscenePlayer;
    Sprite sprite;

    public override void _Ready()
    {
        scared = false;
        health = maxHealth;
        invincibility = 0;
        GD.Randomize();
        player = (Player_KinematicBody2D)GetNode("/root/Game/Player");
        sprite = (Sprite)GetNode("./Sprite");
        
        ((WeaponController)player.GetNode("./Weapon")).Connect("EnemyHit", this, "_EventWeaponConnected");
        
        movementTimeElapsed = -999;
        healthBar = (ColorRect)GetNode("./Sprite/HealthBar");
        cutscenePlayer = (CutscenePlayer)GetNode("/root/Game/Camera2D/CutscenePlayer");

        healthBarFullLength = healthBar.RectSize.x;
    }

    public override void _Process(float delta)
    {
        if (invincibility > 0) invincibility -= delta;
        if (player.canMove) {
            float distToPlayer = (player.Position - Position).Length();
            if (scared || distToPlayer < viewThreshold) {
                if (distToPlayer < viewThreshold) scared = true;
                else if (distToPlayer > scaredThreshold) scared = false;
                Vector2 vectorTo = (player.Position - Position).Normalized();
                MoveAndCollide(speed * vectorTo * delta); // Runs AWAY from player because speed set to negative in editor

                sprite.Rotation = (-vectorTo).Angle();
            } else {
                if (movementTimeElapsed == -999 || movementTimeElapsed >= timePerMovement) {
                    float dir = (float)(GD.Randf() * 2 * Math.PI);
                    movementDirection = new Vector2((float)Math.Cos(dir), (float)Math.Sin(dir)).Normalized(); // Should already be of magnitude 1 but just in case I call Normalized to guarantee
                    movementTimeElapsed = (float)(GD.Randf() * variance);
                }
                MoveAndSlide(movementDirection * idleSpeed);
                movementTimeElapsed += delta;

                sprite.Rotation = movementDirection.Angle();
            }
        }
    }

    public void _EventWeaponConnected(KinematicBody2D body, bool hitByArrow) {
        if (body == this) {
            if (invincibility > 0) return;
            health -= player.GetWeaponDamage();
            healthBar.RectSize = new Vector2(healthBarFullLength * ((float)health/maxHealth), healthBar.RectSize.y);
            invincibility = invincibilityTime;
            if (health <= 0) {
                PackedScene meatData = GD.Load<PackedScene>("res://Scenes/Pickup/Meat.tscn");
                PackedScene feathersData = GD.Load<PackedScene>("res://Scenes/Pickup/Feathers.tscn");

                for (int i = 0; i < (hitByArrow ? meatNum + extraArrowMeat : meatNum); i++) {
                    Node2D newMeat = (Node2D)meatData.Instance();
                    GetNode("/root/Game/Pickups").CallDeferred("add_child", newMeat);
                    newMeat.Position = new Vector2(Position.x + GD.Randi() % 30 - 15, Position.y + GD.Randi() % 30 - 15);
                }

                for (int i = 0; i < feathersNum; i++) {
                    Node2D newFeathers = (Node2D)feathersData.Instance();
                    GetNode("/root/Game/Pickups").CallDeferred("add_child", newFeathers);
                    newFeathers.Position = new Vector2(Position.x + GD.Randi() % 30 - 15, Position.y + GD.Randi() % 30 - 15);
                }

                if (!CutsceneData.cutscenePlayed[Cutscene.FirstKill]) cutscenePlayer.PlaySceneCutscene(Cutscene.FirstKill, true);

                QueueFree();
            }
        }
    }

    public void _EventWeaponConnected(KinematicBody2D body) {
        _EventWeaponConnected(body, false);
    }

    public void HitByArrow() {
        _EventWeaponConnected(this, true);
    }
}
