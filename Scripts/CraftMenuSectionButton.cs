using Godot;
using System;

public class CraftMenuSectionButton : Button
{
    bool menuVisible = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public override void _Pressed() {
        if (menuVisible) {
            menuVisible = false;
            foreach (Node child in GetChildren()) {
                ((CanvasItem)child).Hide();
            }
        } else {
            menuVisible = true;
            foreach (Node child in GetChildren()) {
                ((CanvasItem)child).Show();
            }
        }
    }
}
