using Godot;
using System;

public class BeginningCutscene : Node2D
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
                    scrollingText.BeginTextBuffer("Like waking from a long sleep, the world begins to coalesce around you.");
                    ((Label)GetNode("./TextBox/HintLabel")).Show();
                }
                break;
            case CutsceneStage.PreText1:
                if (scrollingText.PlayerExited) {
                    ((Label)GetNode("./TextBox/HintLabel")).Hide();
                    scrollingText.BeginTextBuffer("Blurs form into shapes, leaves and stones and sunbeams.");
                    fader.Play("FadeIn");
                    stage = CutsceneStage.FadeInGameAndText;
                }
                break;
            case CutsceneStage.FadeInGameAndText:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("It all feels wrong somehow -- and yet when you try to remember what is \"right,\" you feel a strange emptiness.");
                    stage = CutsceneStage.DisplayText1;
                }
                break;
            case CutsceneStage.DisplayText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Memories of yesterday, like half-remembered dreams, begin to fade from your mind,");
                    stage = CutsceneStage.DisplayText2;
                }
                break;
            case CutsceneStage.DisplayText2:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("and the void in which your consciousness seems suspended widens.");
                    stage = CutsceneStage.DisplayText2b;
                }
                break;
            case CutsceneStage.DisplayText2b:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Maybe if you could just get your memories back, you could find your way out, but until then, you're stuck in here.");
                    stage = CutsceneStage.DisplayText3;
                }
                break;
            case CutsceneStage.DisplayText3:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("You don’t know who you are or where you are.");
                    stage = CutsceneStage.DisplayText4;
                }
                break;
            case CutsceneStage.DisplayText4:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("But you are determined to survive.");
                    stage = CutsceneStage.DisplayText5;
                }
                break;
            case CutsceneStage.DisplayText5:
                if (scrollingText.PlayerExited) {
                    scrollingText.Hide();
                    TBfader.Play("FadeIn");
                    stage = CutsceneStage.Exit;
                }
                break;
            case CutsceneStage.Exit:
                if (((ColorRect)textBox.GetNode("ColorRect")).Color.a == 0.0) {
                    CutsceneData.cutscenePlayed[CUTSCENE] = true;
                    try {
                        ((CutscenePlayer)GetParent()).EndCutscene(this);
                    } catch (InvalidCastException) {GD.Print("Stuck in cutscene"); stage = CutsceneStage.None;} // An InvalidCastException  means there is no parent, so it's not being run in the main scene but rather by itself, which is fine
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
        DisplayText2b,
        DisplayText3,
        DisplayText4,
        DisplayText5,
        Exit
    }

    private void SetColor(Color color) {
        scrollingText.Modulate = color;
    }
}