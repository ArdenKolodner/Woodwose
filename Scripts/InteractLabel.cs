using Godot;
using System;

public class InteractLabel : Label
{
    public override void _Ready()
    {
        ((OverallMethods)GetTree().Root.GetNode("Game")).Connect("MenuOpening", this, "HideOnMenu");
        ((OverallMethods)GetTree().Root.GetNode("Game")).Connect("MenuClosing", this, "ShowOnMenu");
    }
    public void HideOnMenu() {Hide();} public void ShowOnMenu() {Show();}

    public void ChangeInteractText() {
        ChangeInteractText("");
    }

    public void ChangeInteractText(string interactText) {
        Text = interactText;
    }

    public void ClearInteractText() {
        ChangeInteractText("");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
