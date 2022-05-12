using Godot;
using System;

public class Pickup_DisappearOnE : Area2D
{
    #pragma warning disable 0649
    Player_KinematicBody2D player;

    [Export]
    Pickup pickup;
    [Export]
    Cutscene cutsceneOnFirstPickup;

    static bool inventoryHintQueued = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = (Player_KinematicBody2D)GetNode("/root/Game/Player");
        //player.Connect("PlayerInteract", this, "_EventPlayerInteracted");
    }

    public void Interacted() {
        player.inventory[pickup]++;

        if (cutsceneOnFirstPickup != Cutscene.None && !CutsceneData.cutscenePlayed[cutsceneOnFirstPickup]) {
            ((CutscenePlayer)GetNode("/root/Game/Camera2D/CutscenePlayer")).PlaySceneCutscene(cutsceneOnFirstPickup, true);
        }

        if (!inventoryHintQueued) {
            ((Hints)GetNode("/root/Game/Camera2D/Hints")).QueueHint(Hint.Inventory);
            inventoryHintQueued = true;
        }


        QueueFree();
    }

    public Pickup GetPickup() {return pickup;}

    public string GetInteractText() {
        if (pickup == Pickup.Wheat && player.weapon != Pickup.Knife) return "Wheat: Requires\nknife to cut";
        if (pickup == Pickup.SharpRock) return "E: Collect Sharp Rock";
        return "E: Collect " + pickup.ToString();
    }
}
