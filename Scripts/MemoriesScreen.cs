using Godot;
using System;

public class MemoriesScreen : CanvasLayer
{
    #pragma warning disable 0649
    bool pressedLastFrame = false;
    Node2D container;
    Player_KinematicBody2D player;
    public override void _Ready()
    {
        container = (Node2D)GetNode("./Container");
        player = (Player_KinematicBody2D)GetNode("../Player");
    }

    public override void _Process(float delta)
    {
        if (Input.IsKeyPressed((int)KeyList.M)) {
            if (!pressedLastFrame) {
                if (container.Visible) {
                    container.Hide();
                    player.EnableMove();
                } else if (player.canMove) {
                    ((Hints)GetNode("../Camera2D/Hints")).LearnHint(Hint.Memory);

                    foreach (Node textItem in container.GetChildren()) {
                        if (typeof(MemoryText).IsInstanceOfType(textItem)) {
                            ((MemoryText)textItem).Shown();
                        }
                    }
                    container.Show();
                    player.DisableMove();
                }
            }
            pressedLastFrame = true;
        } else {
            pressedLastFrame = false;
        }
    }
}
