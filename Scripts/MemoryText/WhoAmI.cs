using Godot;
using System;

public class WhoAmI : MemoryText
{
    public override void Shown() {
        if (CutsceneData.cutscenePlayed[Cutscene.Armor] || CutsceneData.cutscenePlayed[Cutscene.Sword]) {
            Text = "I am Yvain, knight of Camelot.";
        } else if (CutsceneData.cutscenePlayed[Cutscene.Armor] && CutsceneData.cutscenePlayed[Cutscene.FindCup]) {
            Text = "I am Yvain... a prince?";
        } else if (CutsceneData.cutscenePlayed[Cutscene.Armor] && !CutsceneData.cutscenePlayed[Cutscene.FindCup]) {
            Text = "I am Yvain... a lord of some sort?";
        } else if (CutsceneData.cutscenePlayed[Cutscene.FindCup]) {
            Text = "A prince, perhaps?";
        } else if (CutsceneData.cutscenePlayed[Cutscene.Knife] || CutsceneData.cutscenePlayed[Cutscene.CookedMeat] || CutsceneData.cutscenePlayed[Cutscene.Bow]) {
            Text = "Some lord, perhaps?";
        } else if (CutsceneData.cutscenePlayed[Cutscene.Rope] || CutsceneData.cutscenePlayed[Cutscene.FirstKill] || CutsceneData.cutscenePlayed[Cutscene.Iron]) {
            Text = "A knight of some sort?";
        } else if (CutsceneData.cutscenePlayed[Cutscene.Rock]) {
            Text = "Some sort of warrior?";
        } else {
            Text = "I can't remember.";
        }
    }
}
