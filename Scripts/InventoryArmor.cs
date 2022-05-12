using Godot;
using System;

public class InventoryArmor : InventoryObject
{
    Sprite sprite;
    public override void _Ready()
    {
        base._Ready();
        sprite = (Sprite)GetNode("./Sprite");
    }

    public override void Shown()
    {
        sprite.Texture = ResourceLoader.Load(ArmorData.armorTextures[player.armor]) as Texture;
        if (player.armor == Pickup.None) {
            count.Text = "No armor equipped";
        } else if (player.armor == Pickup.Gambeson) {
            count.Text = "Armor: gambeson";
        } else if (player.armor == Pickup.Armor) {
            count.Text = "Armor: plate mail";
        }
    }
}
