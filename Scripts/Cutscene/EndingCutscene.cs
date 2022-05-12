using Godot;
using System;

public class EndingCutscene : Node2D
{
    #pragma warning disable 0649
    [Export]
    Cutscene CUTSCENE;
    ColorRect fadeToBlack;
    AnimationPlayer fader;
    AnimationPlayer TBfader;
    Node2D textBox;
    Camera2D sceneCamera;
    ScrollingText scrollingText;
    CutsceneStage stage;
    Voices voices;
    public override void _Ready()
    {
        scrollingText = (ScrollingText)GetNode("./ScrollingText");
        fadeToBlack = (ColorRect)GetNode("./FadeToBlack");
        fader = (AnimationPlayer)GetNode("./FadeToBlack/Fader");
        textBox = (Node2D)GetNode("./TextBox");
        TBfader = (AnimationPlayer)textBox.GetNode("./TBFader");
        sceneCamera = (Camera2D)GetNode("./Camera2D");
        voices = (Voices)GetNode("/root/Voices");
        ((Label)GetNode("./FadeToBlack/Label")).Modulate = new Color(1,1,1,0);
        if (!typeof(CutscenePlayer).IsInstanceOfType(GetParent())) {
            // If this scene is NOT inside a larger one, turn on the camera. Otherwise do not.
            sceneCamera.Current = true;
        }

        stage = CutsceneStage.FadeInTextBox;
        TBfader.PlayBackwards("FadeIn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (stage) {
            case CutsceneStage.FadeInTextBox:
                if (((ColorRect)textBox.GetNode("ColorRect")).Color.a == 1.0) {
                    stage = CutsceneStage.PreText1;
                    scrollingText.BeginTextBuffer("Yvain, son of Urien, knight of the Round Table.");
                }
                break;
            case CutsceneStage.PreText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("The knowledge comes flooding back as you don your armor and grasp your sword.");
                    stage = CutsceneStage.FadeInGameAndText;
                }
                break;
            case CutsceneStage.FadeInGameAndText:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("At long last, you remember who you are, and how you found yourself in these woods.");
                    stage = CutsceneStage.DisplayText1;
                }
                break;
            case CutsceneStage.DisplayText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Although you know not how long you’ve spent here, you feel a sense of certainty that it is not too late:");
                    stage = CutsceneStage.DisplayText2;
                }
                break;
            case CutsceneStage.DisplayText2:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("not too late to find your way out, not too late to regain your lost station,");
                    stage = CutsceneStage.DisplayText3;
                }
                break;
            case CutsceneStage.DisplayText3:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("and maybe, in time, to reclaim your lost beloved.");
                    stage = CutsceneStage.DisplayText4;
                }
                break;
            case CutsceneStage.DisplayText4:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Filled with confidence, you turn westwards, towards the sun beginning to set, and begin to walk.");
                    stage = CutsceneStage.DisplayText5;
                }
                break;
            case CutsceneStage.DisplayText5:
                if (scrollingText.PlayerExited) {
                    scrollingText.Hide();
                    TBfader.Play("FadeInSlow");
                    fader.PlayBackwards("FadeInSlow");
                    fadeToBlack.Show();
                    stage = CutsceneStage.Exit;
                }
                break;
            case CutsceneStage.Exit:
                if (((ColorRect)textBox.GetNode("ColorRect")).Color.a == 0.0) {
                    CutsceneData.cutscenePlayed[CUTSCENE] = true;
                    //Don't exit because game over
                }
                break;
        }
    }

    private enum CutsceneStage {
        None,
        FadeInTextBox,
        PreText1,
        FadeInGameAndText,
        DisplayText1,
        DisplayText2,
        DisplayText3,
        DisplayText4,
        DisplayText5,
        Exit
    }

    private void SetColor(Color color) {
        scrollingText.Modulate = color;
    }
}