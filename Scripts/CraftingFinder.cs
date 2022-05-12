using Godot;
using System;

public class CraftingFinder : Node2D
{
    [Export]
    float showDistance;
    Node2D player;
    Node2D craftingSpot;
    public override void _Ready()
    {
        player = (Node2D)GetNode("/root/Game/Player");
        craftingSpot = (Node2D)GetNode("/root/Game/CraftingStump");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        Vector2 toSpot = craftingSpot.Position - player.Position;
        if (toSpot.Length() > showDistance) {
            Rotation = toSpot.Angle();
            Show();
        } else {
            Hide();
        }
    }
}
