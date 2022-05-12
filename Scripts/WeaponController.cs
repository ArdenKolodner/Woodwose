using Godot;
using System;

public class WeaponController : Node2D
{
    #pragma warning disable 0649
    WeaponScene weaponArea;
    Player_KinematicBody2D player;
    float targetRotation;
    CraftingMenu craftingMenu;
    PackedScene arrowScene;
    bool lastSwingDirection = false;
    [Signal]
    public delegate void EnemyHit(KinematicBody2D body);
    public override void _Ready()
    {
        player = (Player_KinematicBody2D)GetNode("/root/Game/Player");
        craftingMenu = (CraftingMenu)GetNode("/root/Game/Player/CraftingMenu");
        arrowScene = GD.Load<PackedScene>("res://Scenes/Weapon/Arrow.tscn");

        Node fist = GD.Load<PackedScene>("res://Scenes/Weapon/Fist.tscn").Instance();
        AddChild(fist);
        weaponArea = (WeaponScene)fist;
        targetRotation = Rotation;

        weaponArea.Connect("body_entered", this, "_WeaponHit");
        ((OverallMethods)GetTree().Root.GetNode("Game")).Connect("MenuOpening", this, "HideOnMenu");
        ((OverallMethods)GetTree().Root.GetNode("Game")).Connect("MenuClosing", this, "ShowOnMenu");
    }

    public void HideOnMenu() {weaponArea.Hide();} public void ShowOnMenu() {weaponArea.Show();}

    public void RotateWhenFree(float amount) {
        targetRotation = amount;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (weaponArea.anim.IsPlaying()) {
            weaponArea.Monitoring = true;
        } else {
            weaponArea.Monitoring = false;
            if (targetRotation != Rotation) {
                Rotation = targetRotation;
            }
        }
    }

    public void SwitchWeapon(Pickup newWeapon) {
        weaponArea.Disconnect("body_entered", this, "_WeaponHit");
        weaponArea.QueueFree();

        Node newWeaponNode;
        if (newWeapon == Pickup.None) {
            newWeaponNode = GD.Load<PackedScene>("res://Scenes/Weapon/Fist.tscn").Instance();
        } else {
            newWeaponNode = GD.Load<PackedScene>("res://Scenes/Weapon/" + newWeapon.ToString() + ".tscn").Instance();
        }
        AddChild(newWeaponNode);
        if (craftingMenu.craftingMenuOpen) ((Node2D)newWeaponNode).Hide();
        weaponArea = (WeaponScene)newWeaponNode;
        weaponArea.Connect("body_entered", this, "_WeaponHit");

        lastSwingDirection = false;
        //weaponArea.Rotation = (float)Math.PI/2;
    }

    public bool IsWeaponAnimationPlaying() {
        return weaponArea.anim.IsPlaying();
    }

    public void PlayAttackAnimation() {
        if (weaponArea.swingBackAndForth) {
            if (lastSwingDirection) weaponArea.anim.Play("AttackBackwards");
            else weaponArea.anim.Play("Attack");
            lastSwingDirection = !lastSwingDirection;
        } else if (weaponArea.isBow) {
            if (player.inventory[Pickup.Arrow] == 0) return;
            --player.inventory[Pickup.Arrow];
            Arrow arrow = (Arrow)arrowScene.Instance();
            GetNode("/root/Game").AddChild(arrow);
            arrow.Position = weaponArea.GlobalPosition;
            arrow.Rotation = Rotation;
            weaponArea.anim.Play("Attack");
        }
        else weaponArea.anim.Play("Attack");
    }

    public void _WeaponHit(KinematicBody2D body) {
        EmitSignal(nameof(EnemyHit), body);
    }

    public bool AreaIsActiveWeapon(Area2D area) {
        return area == weaponArea;
    }
}
