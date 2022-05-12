using Godot;
using System;

public class CutscenePlayer : Node
{
    Player_KinematicBody2D player;
    ScrollingText scrollingText;

    //int testflag = 0; // don't remember why this is here but comment instead of delete in case it breaks something
    public bool CutscenePlaying = false;
    public bool reenableMoveAfterwards;
    BGAudioStreamPlayer bgAudio;
    Hints hintHandler;

    static bool shownInventoryHint = false;
    public override void _Ready()
    {
        player = (Player_KinematicBody2D)GetNode("../../Player");
        scrollingText = (ScrollingText)GetNode("./ScrollingText");
        bgAudio = (BGAudioStreamPlayer)GetNode("../../BGAudioStreamPlayer");
        hintHandler = (Hints)GetNode("../Hints");
    }

 // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (scrollingText.PlayerExited) {
            scrollingText.HideText();
        }
    }

    public void PlaySceneCutscene(Cutscene cutscene, bool reenableMoveAfterwards) {
        if (cutscene == Cutscene.Beginning || cutscene == Cutscene.End || cutscene == Cutscene.Death || cutscene == Cutscene.None) {
            bgAudio.Notify_FadeOutForCutscene(false);
        } else {
            bgAudio.Notify_FadeOutForCutscene(true);
            if (!shownInventoryHint) {
                ((Hints)GetNode("../Hints")).QueueHint(Hint.Memory);
                shownInventoryHint = true;
            }
        }

        this.reenableMoveAfterwards = reenableMoveAfterwards;
        player.DisableMove();
        CutscenePlaying = true;
        CutsceneData.cutscenePlayed[cutscene] = true;
        PackedScene cutsceneScene = GD.Load<PackedScene>("res://Scenes/Cutscene/" + CutsceneData.cutsceneScenes[cutscene] + ".tscn");
        AddChild(cutsceneScene.Instance());
    }
    public void PlaySceneCutscene(Cutscene cutscene) {
        PlaySceneCutscene(cutscene, true);
    }
    
    public void EndCutscene(Node2D cutsceneNode) {
        bgAudio.Notify_CutsceneOver();

        CutscenePlaying = false;
        RemoveChild(cutsceneNode);
        if (reenableMoveAfterwards) player.EnableMove();

        hintHandler.ShowHint(Hint.Move); // This will only happen after the beginning cutscene, after which the player will have learned how to move and the hint won't be shown
    }
}