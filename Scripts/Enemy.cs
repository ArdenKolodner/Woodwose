using Godot;
using System;

public class Enemy : KinematicBody2D
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
    int damage;
    [Export]
    float viewThreshold;
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
    [Export]
    bool isHostile;
    ColorRect healthBar;
    float healthBarFullLength;
    [Export]
    int meatNum;
    [Export]
    int extraArrowMeat;
    static bool attackHintQueued;
    CutscenePlayer cutscenePlayer;
    BGAudioStreamPlayer bgAudio;

    public override void _Ready()
    {
        attackHintQueued = false;
        health = maxHealth;
        invincibility = 0;
        GD.Randomize();
        player = (Player_KinematicBody2D)GetNode("/root/Game/Player");
        
        ((WeaponController)player.GetNode("./Weapon")).Connect("EnemyHit", this, "_EventWeaponConnected");
        
        movementTimeElapsed = -999;
        healthBar = (ColorRect)GetNode("./Sprite/HealthBar");
        cutscenePlayer = (CutscenePlayer)GetNode("/root/Game/Camera2D/CutscenePlayer");
        bgAudio = (BGAudioStreamPlayer)GetNode("/root/Game/BGAudioStreamPlayer");

        healthBarFullLength = healthBar.RectSize.x;
        //((OverallMethods)GetTree().GetRoot().GetNode("Game")).Connect("MenuOpening", this, "HideOnMenu");
        //((OverallMethods)GetTree().GetRoot().GetNode("Game")).Connect("MenuClosing", this, "ShowOnMenu");
    }

    public override void _Process(float delta)
    {
        if (invincibility > 0) invincibility -= delta;
        if (player.canMove) {
            float distToPlayer = (player.Position - Position).Length();
            if (isHostile && distToPlayer < viewThreshold) {
                bgAudio.Notify_InCombat();

                if (!attackHintQueued) {
                    ((Hints)GetNode("/root/Game/Camera2D/Hints")).QueueHint(Hint.Attack);
                    attackHintQueued = true;
                }

                Vector2 vectorTo = (player.Position - Position).Normalized();
                var collision = MoveAndCollide(speed * vectorTo * delta);
                if (collision != null && collision.Collider == player) {
                    player.TakeDamage(damage);
                }

                Rotation = vectorTo.Angle();
            } else {
                if (movementTimeElapsed == -999 || movementTimeElapsed >= timePerMovement) {
                    float dir = (float)(GD.Randf() * 2 * Math.PI);
                    movementDirection = new Vector2((float)Math.Cos(dir), (float)Math.Sin(dir)).Normalized(); // Should already be of magnitude 1 but just in case I call Normalized to guarantee
                    movementTimeElapsed = (float)(GD.Randf() * variance);
                }
                MoveAndSlide(movementDirection * idleSpeed);
                movementTimeElapsed += delta;

                Rotation = movementDirection.Angle();
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

                for (int i = 0; i < (hitByArrow ? meatNum + extraArrowMeat : meatNum); i++) {
                    Node2D newMeat = (Node2D)meatData.Instance();
                    GetNode("/root/Game/Pickups").CallDeferred("add_child", newMeat);
                    newMeat.Position = new Vector2(Position.x + GD.Randi() % 30 - 15, Position.y + GD.Randi() % 30 - 15);
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
