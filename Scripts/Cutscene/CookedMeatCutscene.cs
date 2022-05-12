using Godot;
using System;

public class CookedMeatCutscene : Node2D
{
    #pragma warning disable 0649
    [Export]
    Cutscene CUTSCENE;
    ColorRect fadeToBlack;
    AnimationPlayer fader;
    Sprite cutsceneBG;
    Camera2D sceneCamera;
    ScrollingText scrollingText;
    CutsceneStage stage;
    Voices voices;
    public override void _Ready()
    {
        scrollingText = (ScrollingText)GetNode("./ScrollingText");
        cutsceneBG = (Sprite)GetNode("./CutsceneBG");
        fadeToBlack = (ColorRect)GetNode("./FadeToBlack");
        fader = (AnimationPlayer)GetNode("./FadeToBlack/Fader");
        sceneCamera = (Camera2D)GetNode("./Camera2D");
        voices = (Voices)GetNode("/root/Voices");
        if (!typeof(CutscenePlayer).IsInstanceOfType(GetParent())) {
            // If this scene is NOT inside a larger one, turn on the camera. Otherwise do not.
            sceneCamera.Current = true;
        }

        cutsceneBG.Hide();
        fadeToBlack.Show();
        stage = CutsceneStage.FadeOutGame;
        fader.PlayBackwards("FadeIn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (stage) {
            case CutsceneStage.FadeOutGame:
                if (fadeToBlack.Color.a == 1.0) {
                    cutsceneBG.Show();
                    stage = CutsceneStage.FadeIn;
                    fader.Play("FadeIn");
                }
                break;
            case CutsceneStage.FadeIn:
                if (fadeToBlack.Color.a == 0.0) {
                    scrollingText.SetAudio(voices.luneteVoice);
                    SetColor(new Color(1,.7f,.7f));
                    scrollingText.BeginTextBuffer("It’s all I could sneak out, but it’s better than nothing. My lady Laudine employs the finest chef in the land.");
                    stage = CutsceneStage.DisplayText1;
                }
                break;
            case CutsceneStage.DisplayText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Indeed! This steak is exquisite. The fare of kings -- hardly fit for the likes of me.");
                    stage = CutsceneStage.DisplayText2;
                }
                break;
            case CutsceneStage.DisplayText2:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.luneteVoice);
                    SetColor(new Color(1,.7f,.7f));
                    scrollingText.BeginTextBuffer("Oh, your modesty does you credit, my lord. Yet you are clearly a gentleman and a man of fine character.");
                    stage = CutsceneStage.DisplayText3;
                }
                break;
            case CutsceneStage.DisplayText3:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer(" Is that so? Peer out that window, Lunete, and tell me what you see.");
                    stage = CutsceneStage.DisplayText4;
                }
                break;
            case CutsceneStage.DisplayText4:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.luneteVoice);
                    SetColor(new Color(1,.7f,.7f));
                    scrollingText.BeginTextBuffer("I see my lady at her husband’s funeral, grieving and in need of comfort.");
                    stage = CutsceneStage.DisplayText5;
                }
                break;
            case CutsceneStage.DisplayText5:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("I see that too, and it is my fault. I cannot bear to see the woman I love cry so.");
                    stage = CutsceneStage.DisplayText6;
                }
                break;
            case CutsceneStage.DisplayText6:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.luneteVoice);
                    SetColor(new Color(1,.7f,.7f));
                    scrollingText.BeginTextBuffer("Why don’t you marry her?");
                    stage = CutsceneStage.DisplayText7;
                }
                break;
            case CutsceneStage.DisplayText7:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("I- What?");
                    stage = CutsceneStage.DisplayText8;
                }
                break;
            case CutsceneStage.DisplayText8:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.luneteVoice);
                    SetColor(new Color(1,.7f,.7f));
                    scrollingText.BeginTextBuffer("Anyone can see you are a man of great honor and virtue.");
                    stage = CutsceneStage.DisplayText9;
                }
                break;
            case CutsceneStage.DisplayText9:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Did you forget that I was the one who killed her husband? And I’ve never even talked to her once?");
                    stage = CutsceneStage.DisplayText10;
                }
                break;
            case CutsceneStage.DisplayText10:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.luneteVoice);
                    SetColor(new Color(1,.7f,.7f));
                    scrollingText.BeginTextBuffer("She just needs someone to show her what a good person you are.");
                    stage = CutsceneStage.DisplayText11;
                }
                break;
            case CutsceneStage.DisplayText11:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("I can do that, I can arrange it -- I already have ideas.");
                    stage = CutsceneStage.DisplayText12;
                }
                break;
            case CutsceneStage.DisplayText12:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("You are a foolish girl. I expect they would put me to death.");
                    stage = CutsceneStage.DisplayText13;
                }
                break;
            case CutsceneStage.DisplayText13:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("But if she were to love me back... hmm, I’ll consider it.");
                    stage = CutsceneStage.DisplayText14;
                }
                break;
            case CutsceneStage.DisplayText14:
                if (scrollingText.PlayerExited) {
                    scrollingText.HideText();
                    fader.PlayBackwards("FadeIn");
                    stage = CutsceneStage.FadeOut;
                }
                break;
            case CutsceneStage.FadeOut:
                if (fadeToBlack.Color.a == 1.0) {
                    cutsceneBG.Hide();
                    fader.Play("FadeIn");
                    stage = CutsceneStage.FadeInGame;
                }
                break;
            case CutsceneStage.FadeInGame:
                if (fadeToBlack.Color.a == 0.0) {
                    stage = CutsceneStage.Exit;
                }
                break;
            case CutsceneStage.Exit:
                CutsceneData.cutscenePlayed[CUTSCENE] = true;
                try {
                    ((CutscenePlayer)GetParent()).EndCutscene(this);
                } catch (InvalidCastException) {GD.Print("Stuck in cutscene"); stage = CutsceneStage.None;} // An InvalidCastException  means there is no parent, so it's not being run in the main scene but rather by itself, which is fine
                break;
        }
    }

    private enum CutsceneStage {
        None,
        FadeOutGame,
        FadeIn,
        DisplayText1,
        DisplayText2,
        DisplayText3,
        DisplayText4,
        DisplayText5,
        DisplayText6,
        DisplayText7,
        DisplayText8,
        DisplayText9,
        DisplayText10,
        DisplayText11,
        DisplayText12,
        DisplayText13,
        DisplayText14,
        FadeOut,
        FadeInGame,
        Exit
    }

    private void SetColor(Color color) {
        scrollingText.Modulate = color;
    }
}