using Godot;
using System;

public class InventoryWeapon : InventoryObject
{
    [Export]
    string weaponName;
    public override void _Ready()
    {
        base._Ready();
        player.Connect("SwitchedWeaponsInInventory", this, "SwitchWeaponsInInventory");
    }

    public override void Shown()
    {
        if (player.HasWeapon(item)) {
            count.Text = weaponName + ": " + ((player.weapon == item) ? "Equipped" : "Available");
        } else {
            count.Text = weaponName + ": " + "Not Crafted";
        }
    }

    public void SwitchWeaponsInInventory() {
        Shown();
    }
}
