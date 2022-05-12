using Godot;
using System;

public class Campfire : Area2D
{
    [Export]
    public int woodCount;
    [Export]
    public int sticksCount;
    [Export]
    public int flintCount;
    CampfireSprite sprite;
    Player_KinematicBody2D player;
    public bool isBuilt;
    public override void _Ready()
    {
        isBuilt = false;
        sprite = (CampfireSprite)GetNode("./Sprite");
        player = (Player_KinematicBody2D)GetNode("../Player");
    }

    public void Interacted() {
        if (isBuilt) {
            player.inventory[Pickup.CookedMeat] += player.inventory[Pickup.Meat];
            player.inventory[Pickup.Meat] = 0;
        } else {
            if (player.inventory[Pickup.Flint] >= flintCount && player.inventory[Pickup.Wood] >= woodCount && player.inventory[Pickup.Stick] >= sticksCount) {
                player.inventory[Pickup.Flint] -= flintCount;
                player.inventory[Pickup.Wood] -= woodCount;
                player.inventory[Pickup.Stick] -= sticksCount;

                isBuilt = true;

                sprite.Texture = sprite.litTexture;

                ((CutscenePlayer)GetNode("../Camera2D/CutscenePlayer")).PlaySceneCutscene(Cutscene.Flint);
            }
        }
    }
}
