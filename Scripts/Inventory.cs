using Godot;
using System;

public class Inventory : CanvasLayer
{
    #pragma warning disable 0649
    bool pressedLastFrame = false;
    Node2D inventoryContainer;
    Player_KinematicBody2D player;
    public override void _Ready()
    {
        inventoryContainer = (Node2D)GetNode("./Container");
        player = (Player_KinematicBody2D)GetNode("../Player");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.Q)) {
            if (!pressedLastFrame) {
                if (inventoryContainer.Visible) {
                    inventoryContainer.Hide();
                    //((OverallMethods)GetTree().GetRoot().GetNode("Game")).SendSignalMenuClosingSpecific(Menu.Inventory);
                    player.EnableMove();
                } else if (player.canMove) {
                    ((Hints)GetNode("../Camera2D/Hints")).LearnHint(Hint.Inventory);
                    ((Hints)GetNode("../Camera2D/Hints")).QueueHint(Hint.Craft);

                    foreach (Node inventoryItem in inventoryContainer.GetChildren()) {
                        if (typeof(InventoryObject).IsInstanceOfType(inventoryItem)) {
                            ((InventoryObject)inventoryItem).Shown();
                        }
                    }
                    inventoryContainer.Show();
                    //((OverallMethods)GetTree().GetRoot().GetNode("Game")).SendSignalMenuOpeningSpecific(Menu.Inventory);
                    player.DisableMove();
                }
            }
            pressedLastFrame = true;
        } else {
            pressedLastFrame = false;
        }
    }
}
