using Godot;
using GDArray = Godot.Collections.Array;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player_KinematicBody2D : KinematicBody2D
{
    #pragma warning disable 0649
    [Export]
    public float speed;

    public bool canMove;
    [Export]
    float maxStamina;
    [Export]
    float staminaSpeedFactor;
    [Export]
    float staminaPenalty;
    [Export]
    int maxHealth;
    [Export]
    float invincibilityTime;
    float invincibility;
    int health;

    public float stamina;
    public Dictionary<Pickup,int> inventory;
    Sprite sprite;
    ColorRect staminaBar;
    TextureRect damageBar;
    float staminaBarMaxWidth;
    float damageBarMaxWidth;
    Area2D interactArea;
    CollisionShape2D hitbox;

    WeaponController weaponController;
    public Pickup weapon;
    public Pickup armor;
    List<Pickup> collectedWeapons;

    private InteractLabel interactLabel;

    private float timeMoved;
    private float timeSprinted;
    [Export]
    int cookedMeatHealthRestore;
    [Export]
    int uncookedMeatHealthRestore;
    Hints hints;
    Node2D inventoryContainer;
    [Export]
    float damageRecoverySpeed;
    [Signal]
    public delegate void SwitchedWeaponsInInventory();

    /*[Signal] // When the player presses E, this signal is sent to every interactable object. area is the object the player interacts with, an Area2D.
    public delegate void PlayerInteract(Area2D area);*/


    public override void _Ready()
    {
        health = maxHealth;
        stamina = maxStamina;
        canMove = true;
        inventory = new Dictionary<Pickup, int>();
        collectedWeapons = new List<Pickup>();
        collectedWeapons.Add(Pickup.None);
        weapon = Pickup.None;
        armor = Pickup.None;

        timeMoved = 0;
        timeSprinted = 0;

        foreach (Pickup item in Enum.GetValues(typeof(Pickup))) {
            inventory[item] = 0;
        }
        
        interactLabel = (InteractLabel)GetNode("./InteractLabel");
        sprite = (Sprite)GetNode("./Sprite");
        staminaBar = (ColorRect)GetNode("./StaminaBar");
        damageBar = (TextureRect)GetNode("./DamageBar");
        weaponController = (WeaponController)GetNode("./Weapon");
        interactArea = (Area2D)GetNode("./InteractArea");
        hitbox = (CollisionShape2D)GetNode("./CollisionShape2D");
        hints = (Hints)GetNode("../Camera2D/Hints");
        inventoryContainer = (Node2D)GetNode("/root/Game/Inventory/Container");
        
        staminaBarMaxWidth = staminaBar.RectSize.x;
        damageBarMaxWidth = damageBar.RectSize.x;

        invincibility = 0;
        
        ((OverallMethods)GetTree().Root.GetNode("Game")).Connect("MenuOpening", this, "HideOnMenu");
        ((OverallMethods)GetTree().Root.GetNode("Game")).Connect("MenuClosing", this, "ShowOnMenu");
    }
    public void HideOnMenu() {staminaBar.Hide();damageBar.Hide();} public void ShowOnMenu() {staminaBar.Show();damageBar.Show();}
    
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (sprite.SelfModulate.g < 1) sprite.SelfModulate = new Color(sprite.SelfModulate.r, Math.Min(sprite.SelfModulate.g + damageRecoverySpeed * delta, 1f), Math.Min(sprite.SelfModulate.b + damageRecoverySpeed * delta, 1f), 1);

        GDArray overlappingBodies = interactArea.GetOverlappingAreas();
        int oBodiesCount = overlappingBodies.Count;
        if (oBodiesCount > 0) {
            interactLabel.ChangeInteractText("E: Interact");
            foreach (Area2D area in overlappingBodies) {
                try {
                    interactLabel.ChangeInteractText(((MenuItem)area).GetInteractText());
                } catch (InvalidCastException) {
                    try {
                    interactLabel.ChangeInteractText(((Pickup_DisappearOnE)area).GetInteractText());
                    } catch (InvalidCastException) {}
                }
                if (typeof(Campfire).IsInstanceOfType(area)) {
                    if (!((Campfire)area).isBuilt) {
                        Campfire cf = (Campfire)area;
                        interactLabel.ChangeInteractText("Build campfire:\n" + cf.flintCount.ToString() + " flint, " + cf.sticksCount.ToString() + " sticks, "+ cf.woodCount.ToString() + " wood");
                    } else {
                        if (inventory[Pickup.Meat] > 0) interactLabel.ChangeInteractText("Cook Meat");
                        else interactLabel.ChangeInteractText("No raw meat to cook");
                    }
                }
                if (typeof(Tree).IsInstanceOfType(area)) {
                    if (weapon == Pickup.Ax) interactLabel.ChangeInteractText("Attack: cut tree");
                    else interactLabel.ChangeInteractText("Tree: requires axe\nto cut");
                }
            }
        } else {
            interactLabel.ChangeInteractText();
        }

        // INTERACT
        if (oBodiesCount > 0 && Input.IsActionJustPressed("Interact")) {
			foreach (Area2D area in overlappingBodies) {
                try {
                    Pickup_DisappearOnE pickup = (Pickup_DisappearOnE)area;
                    if (pickup.GetPickup() != Pickup.Wheat || (pickup.GetPickup() == Pickup.Wheat && weapon == Pickup.Knife)) pickup.Interacted();
                } catch (InvalidCastException) {}

                try {
                    ((MenuItem)area).Interacted();
                } catch (InvalidCastException) {}

                try {
                    ((Campfire)area).Interacted();
                } catch (InvalidCastException) {}

                try {
                    ((CutsceneObject)area).Interacted();
                } catch (InvalidCastException) {}
            }
            //EmitSignal("PlayerInteract", currentInteractObject);
        }

        // MOVE
        if (canMove) {
            if (CutsceneData.cutscenePlayed[Cutscene.Sword] && CutsceneData.cutscenePlayed[Cutscene.Armor] && !CutsceneData.cutscenePlayed[Cutscene.End]) {
                ((CutscenePlayer)GetNode("/root/Game/Camera2D/CutscenePlayer")).PlaySceneCutscene(Cutscene.End);
            }
            staminaBar.Show(); damageBar.Show();
            
            Vector2 velocity = new Vector2(0,0);
            if (Input.IsActionPressed("Up")) {
                velocity.y = -speed;
            } else if (Input.IsActionPressed("Down")) {
                velocity.y = speed;
            }
            if (Input.IsActionPressed("Left")) {
                velocity.x = -speed;
            } else if (Input.IsActionPressed("Right")) {
                velocity.x = speed;
            }

            if (velocity.y != 0 && velocity.x != 0) { // correct for diagonal movement so it isn't faster
                velocity.y *= (float)Math.Sqrt(2)/2f;
                velocity.x *= (float)Math.Sqrt(2)/2f;
            }

            if (Input.IsKeyPressed((int)KeyList.Shift) && (velocity.x != 0 || velocity.y != 0)) {
                if (stamina > 0) {
                    velocity *= staminaSpeedFactor;
                    stamina -= delta;
                } else {
                    stamina = -staminaPenalty;
                }

                if (!HintData.learned[Hint.Sprint]) {
                    timeSprinted += delta;
                    if (timeMoved > 2) hints.LearnHint(Hint.Sprint);
                }
            } else {
                stamina += delta;
            }

            MoveAndSlide(velocity);

            if (velocity.x != 0 || velocity.y != 0) {
                weaponController.RotateWhenFree(velocity.Angle());
                sprite.Rotation = velocity.Angle() - (float)Math.PI/2;
                hitbox.Rotation = velocity.Angle() - (float)Math.PI/2;
                interactArea.Rotation = velocity.Angle() - (float)Math.PI/2;

                if (!HintData.learned[Hint.Move]) {
                    timeMoved += delta;
                    if (timeMoved > 2) {
                        hints.LearnHint(Hint.Move);
                        hints.QueueHint(Hint.Sprint);
                    }
                }
            }

            if (Input.IsActionJustPressed("NextWeapon")) {
                NextWeapon();
                hints.LearnHint(Hint.SwitchWeapons);
            } else if (Input.IsActionJustPressed("PreviousWeapon")) {
                PreviousWeapon();
                hints.LearnHint(Hint.SwitchWeapons);
            } else if (Input.IsActionPressed("Attack") && !weaponController.IsWeaponAnimationPlaying()) {
                weaponController.PlayAttackAnimation();

                if (!HintData.learned[Hint.Attack]) {
                    hints.LearnHint(Hint.Attack);
                }
            }

            if (Input.IsActionJustPressed("Eat") && health < maxHealth) {
                hints.LearnHint(Hint.Eat);
                if (inventory[Pickup.CookedMeat] > 0) {
                    --inventory[Pickup.CookedMeat];
                    Heal(cookedMeatHealthRestore);
                } else if (inventory[Pickup.Meat] > 0) {
                    --inventory[Pickup.Meat];
                    Heal(uncookedMeatHealthRestore);
                }
            } else if (health < maxHealth && (inventory[Pickup.CookedMeat] > 0 || inventory[Pickup.Meat] > 0)) {
                hints.QueueHint(Hint.Eat);
            }

            //if (stamina > maxStamina * (((float)health)/((float)maxHealth))) stamina = maxStamina * (((float)health)/((float)maxHealth));
            if (stamina > maxStamina * FractionHealth()) stamina = maxStamina * FractionHealth();
            staminaBar.RectSize = new Vector2(staminaBarMaxWidth * (stamina/maxStamina), staminaBar.RectSize.y);
            damageBar.RectSize = new Vector2(damageBarMaxWidth * (1-FractionHealth()), damageBar.RectSize.y);
            
            if (invincibility > 0 && canMove) {
                invincibility -= delta;
            }
        } else if (inventoryContainer.Visible) { // Can switch weapons while viewing the inventory, but nothing else
            if (Input.IsActionJustPressed("NextWeapon")) {
                NextWeapon();
                hints.LearnHint(Hint.SwitchWeapons);
                EmitSignal(nameof(SwitchedWeaponsInInventory));
            } else if (Input.IsActionJustPressed("PreviousWeapon")) {
                PreviousWeapon();
                hints.LearnHint(Hint.SwitchWeapons);
                EmitSignal(nameof(SwitchedWeaponsInInventory));
            }
        }
    }

    public void TakeDamage(int amount) {
        amount = (int)(amount * GetArmorDefense());
        if (invincibility <= 0) {
            health -= amount;
            invincibility = invincibilityTime;
            if (health <= 0) {
                Die();
            }

            sprite.SelfModulate = new Color(1, 0, 0, 1);
        }
    }

    private void Heal(int amount) {
        health = Math.Min(health + amount, maxHealth);
    }

    public void Die() {
        ((BGAudioStreamPlayer)GetNode("../BGAudioStreamPlayer")).QueueFree(); // youre dead so no more sound. ever.
        ((CutscenePlayer)GetNode("../Camera2D/CutscenePlayer")).PlaySceneCutscene(Cutscene.Death);
    }

    public void EnableMove() {canMove = true;}
    public void DisableMove() {canMove = false;}

    public float FractionHealth() {
        return (float)health / (float)maxHealth;
    }

    public int GetWeaponDamage() {
        return WeaponData.weaponDamages[weapon];
    }

    public float GetArmorDefense() {
        return ArmorData.defenseValues[armor];
    }

    public void SwitchWeapon(Pickup newWeapon) {
        weapon = newWeapon;
        weaponController.SwitchWeapon(newWeapon);
    }

    public void GainWeapon(Pickup newWeapon) {
        if (!collectedWeapons.Contains(newWeapon)) {
            collectedWeapons.Add(newWeapon);
        }
        SwitchWeapon(newWeapon);
    }

    private void NextWeapon() {
        int currentWeaponIndex = collectedWeapons.IndexOf(weapon);
        int nextIndex = (currentWeaponIndex + 1) % collectedWeapons.Count;
        SwitchWeapon(collectedWeapons.ElementAt(nextIndex));
    }

    private void PreviousWeapon() {
        int currentWeaponIndex = collectedWeapons.IndexOf(weapon);
        int prevIndex = (((currentWeaponIndex - 1) % collectedWeapons.Count) + collectedWeapons.Count) % collectedWeapons.Count;
        SwitchWeapon(collectedWeapons.ElementAt(prevIndex));
    }
    
    public void SwitchArmor(Pickup newArmor) {
        armor = newArmor;
        
        sprite.Texture = ResourceLoader.Load(ArmorData.armorTextures[armor]) as Texture;
    }

    public bool HasWeapon(Pickup w) {
        return collectedWeapons.Contains(w);
    }

    public bool HasArmor(Pickup a) {
        if (!ArmorData.defenseValues.ContainsKey(a)) return false;
        return ArmorData.defenseValues[armor] <= ArmorData.defenseValues[a]; // Player is considered to have an armor if they have a better armor; gambeson is required to craft armor anyway
    }
}