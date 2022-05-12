using Godot;
using System;

public class CraftButton : Button
{
    #pragma warning disable 0649
    [Export]
    Pickup ingredient1;
    [Export]
    int count1;
    [Export]
    Pickup ingredient2;
    [Export]
    int count2;
    [Export]
    Pickup ingredient3;
    [Export]
    int count3;
    [Export]
    Pickup result;
    [Export]
    int countResult;
    [Export]
    Pickup requiredArmor;
    [Export]
    Cutscene cutsceneOrNone;
    Player_KinematicBody2D player;
    CutscenePlayer cutscenePlayer;
    Label resourceHint;
    public override void _Ready()
    {
        resourceHint = (Label)GetNode("./Label");
        player = (Player_KinematicBody2D)GetNode("../../..");
        cutscenePlayer = (CutscenePlayer)GetTree().Root.GetNode("Game").GetNode("Camera2D/CutscenePlayer");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Visible) {
            if (player.HasWeapon(result) || player.HasArmor(result)) { // Weapons are only supposed to be crafted once; if result is not a weapon, this method will just be guaranteed to return false since it won't be in the player's weapons
                Modulate = new Color(0,1,0,1);
            }
            else if (player.inventory[ingredient1] >= count1 && (
                ingredient2 == Pickup.None || player.inventory[ingredient2] >= count2) && (
                ingredient3 == Pickup.None || player.inventory[ingredient3] >= count3)
                && (requiredArmor == Pickup.None || player.HasArmor(requiredArmor))) {
                Modulate = new Color(1,1,1,1);
            } else {
                Modulate = new Color(1,0,0,1);
            }

            if (IsHovered()) resourceHint.Show();
            else resourceHint.Hide();
        }
    }

    public override void _Pressed() {
        if (player.HasWeapon(result) || player.HasArmor(result)) return; // If this button is to craft a weapon and the player already has it (button is green), don't let them craft it again
        if (requiredArmor != Pickup.None && !player.HasArmor(requiredArmor)) return;

        if (player.inventory[ingredient1] >= count1 && (
                ingredient2 == Pickup.None || player.inventory[ingredient2] >= count2) && (
                ingredient3 == Pickup.None || player.inventory[ingredient3] >= count3)) {
            player.inventory[ingredient1] -= count1;
            if (ingredient2 != Pickup.None) player.inventory[ingredient2] -= count2;
            if (ingredient3 != Pickup.None) player.inventory[ingredient3] -= count3;
            player.inventory[result] += countResult;

            if (cutsceneOrNone == Cutscene.None) ((AudioStreamPlayer2D)GetNode("/root/Game/CraftSoundEffect")).Play();

            if (result == Pickup.Arrow || result == Pickup.Bow) ((ArrowCount)GetNode("/root/Game/Camera2D/ArrowCountLabel")).ShowArrowCount();

            if (WeaponData.weaponDamages.ContainsKey(result)) {
                player.GainWeapon(result);
                ((Hints)GetNode("/root/Game/Camera2D/Hints")).QueueHint(Hint.SwitchWeapons);
            }

            if (ArmorData.defenseValues.ContainsKey(result)) {
                if (ArmorData.defenseValues[result] < player.GetArmorDefense()) player.SwitchArmor(result);
            }

            if (cutsceneOrNone != Cutscene.None && !CutsceneData.cutscenePlayed[cutsceneOrNone]) {
                cutscenePlayer.PlaySceneCutscene(cutsceneOrNone, false);
            }
        }
    }
}
