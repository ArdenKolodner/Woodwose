using Godot;
using System;

public class MenuItem : Area2D
{
    #pragma warning disable 0649
    Player_KinematicBody2D player;

    [Export]
    Menu menu;

    [Export]
    string interactText;
    public string GetInteractText() {return interactText;}

    bool menuActive;
    Node2D inventoryContainer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = (Player_KinematicBody2D)GetNode("../Player");
        //player.Connect("PlayerInteract", this, "_EventPlayerInteracted");
        inventoryContainer = (Node2D)GetNode("../Inventory/Container");
    }

    public override void _Process(float delta)
    {
        if (menuActive && Input.IsKeyPressed((int)KeyList.Escape)) {
            ((CraftingMenu)player.GetNode("CraftingMenu")).HideMenu();
        }
    }

    public void Interacted() {
        if (((CutscenePlayer)GetNode("/root/Game/Camera2D/CutscenePlayer")).CutscenePlaying) return;
        if (menu == Menu.CraftingMenu) {
            if (inventoryContainer.Visible) return;
            menuActive = true;
            ((CraftingMenu)player.GetNode("CraftingMenu")).ShowMenu();
            ((Hints)GetNode("../Camera2D/Hints")).LearnHint(Hint.Craft);
        }
    }
}
