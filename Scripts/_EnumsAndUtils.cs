using System.Collections.Generic;
/*public enum InteractMode {
    None,
    PICKUP,
    MENU
}*/
public enum Menu {
    None,
    CraftingMenu,
    Inventory
}
public enum Pickup {
    None,
    TestPickup, // scene created
    Rock, // scene created
    SharpRock, // scene created
    Flint, // scene created
    Iron, // scene created
    Stick, // scene created
    Wood, // scene created
    Meat, // scene created
    CookedMeat, // no scene created
    Wheat, // scene created
    Feathers,
    // Crafted only:
    Fabric, // no pickup
    Gambeson, // no pickup
    Armor, // no pickup
    Club, // scene created (no sprite)
    Knife, // scene created (no sprite)
    Ax, // scene created (no sprite)
    Bow, // scene created (no sprite)
    Sword, // scene created (no sprite)
    Arrow, // scene created (no sprite)
    Rope
}

public enum Cutscene {
    None,
    TestCutscene,
    Beginning,
    End,
    Death,
    Knife,
    Axe,
    FindCup,
    FindRing,
    Rock,
    Rope,
    FirstKill,
    Armor,
    Flint,
    CookedMeat,
    Iron,
    Sword,
    Bow
}

public enum Hint {
    None,
    Move,
    Sprint,
    Attack,
    Inventory,
    Craft,
    Memory,
    Eat,
    SwitchWeapons
}

public class HintData {
    public static Dictionary<Hint, bool> learned = new Dictionary<Hint, bool>(); // Initialized in main game code, on _Ready, so every entry is initially false
}

public class CutsceneData {
    public readonly static Dictionary<Cutscene, string> cutsceneScenes = new Dictionary<Cutscene, string>() {
        {Cutscene.None,""},
        {Cutscene.TestCutscene,"TestCutscene"},
        {Cutscene.Beginning,"BeginningCutscene"},
        {Cutscene.Death,"DeathCutscene"},
        {Cutscene.Knife,"KnifeCutscene"},
        {Cutscene.Axe,"AxeCutscene"},
        {Cutscene.FindCup,"CupCutscene"},
        {Cutscene.FindRing,"RingCutscene"},
        {Cutscene.Rock,"RockCutscene"},
        {Cutscene.Rope,"RopeCutscene"},
        {Cutscene.FirstKill,"FirstKillCutscene"},
        {Cutscene.Armor,"ArmorCutscene"},
        {Cutscene.Flint,"FlintCutscene"},
        {Cutscene.CookedMeat,"CookedMeatCutscene"},
        {Cutscene.Iron,"IronCutscene"},
        {Cutscene.Sword,"SwordCutscene"},
        {Cutscene.Bow,"BowCutscene"},
        {Cutscene.End,"EndingCutscene"}
    };
    public static Dictionary<Cutscene, bool> cutscenePlayed = new Dictionary<Cutscene, bool>(); // Initialized in main game code, on _Ready, so every entry is initially false
}

public class WeaponData {
    public readonly static Dictionary<Pickup, int> weaponDamages = new Dictionary<Pickup, int>() {
        {Pickup.None, 2},
        {Pickup.Club, 6}, // but slow!
        {Pickup.Knife, 6},
        {Pickup.Ax, 10}, // but slow!
        {Pickup.Bow, 12},
        {Pickup.Sword, 15}
    };
}

public class ArmorData {
    public readonly static Dictionary<Pickup, float> defenseValues = new Dictionary<Pickup, float>() {
        {Pickup.None,1},
        {Pickup.Gambeson,0.8f},
        {Pickup.Armor,0.4f}
    };

    public readonly static Dictionary<Pickup, string> armorTextures = new Dictionary<Pickup, string>() {
        {Pickup.None,"res://Textures/Player naked small.png"},
        {Pickup.Gambeson,"res://Textures/Player gambeson small.png"},
        {Pickup.Armor,"res://Textures/Player armor small.png"}
    };
}

// For some reason the standard Math.Clamp() can't be found so I just wrote my own bc it's simple
 public class YvainMath {
    public static float clamp(float n, float min, float max) {
        if (n < min) return min;
        if (n > max) return max;
        return n;
    }
 }