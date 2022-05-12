using Godot;
using System;

public class OverallMethods : Node2D
{
    [Export]
    float enemyFrequency;
    [Export]
    float passiveFrequency;
    string enemyPath;
    string passivePath;
    [Signal]
    public delegate void MenuOpening();
    [Signal]
    public delegate void MenuClosing();
    public override void _Ready() {
        foreach (Hint hint in Enum.GetValues(typeof(Hint))) {
            HintData.learned[hint] = false;
        }

        foreach (Cutscene cutscene in Enum.GetValues(typeof(Cutscene))) {
            CutsceneData.cutscenePlayed[cutscene] = false;
        }


        ((CutscenePlayer)GetNode("./Camera2D/CutscenePlayer")).PlaySceneCutscene(Cutscene.Beginning);
    }

    public override void _Process(float delta) {
        if (Input.IsActionJustPressed("Quit")) {
            GetTree().Quit();
        }
    }

    public void SendSignalMenuClosing() {EmitSignal(nameof(MenuClosing));}
    public void SendSignalMenuOpening() {EmitSignal(nameof(MenuOpening));}
}
