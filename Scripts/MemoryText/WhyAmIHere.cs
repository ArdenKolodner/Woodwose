using Godot;
using System;

public class WhyAmIHere : MemoryText
{
    public override void Shown() {
        if (CutsceneData.cutscenePlayed[Cutscene.Sword]) {
            Text = "To think, and to make things\nright again.";
        } else if (CutsceneData.cutscenePlayed[Cutscene.FindRing]) {
            Text = "A promise broken.";
        } else if (CutsceneData.cutscenePlayed[Cutscene.Armor]) {
            Text = "Because of what Kay said?";
        } else if (CutsceneData.cutscenePlayed[Cutscene.FirstKill]) {
            Text = "And why am I not with my wife?";
        } else {
            Text = "I can't remember.";
        }
    }
}
