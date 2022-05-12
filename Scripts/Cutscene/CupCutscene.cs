using Godot;
using System;

public class CupCutscene : Node2D
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
                    scrollingText.playSound = false;
                    scrollingText.BeginTextBuffer("*crash*");
                    stage = CutsceneStage.DisplayText1;
                }
                break;
            case CutsceneStage.DisplayText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.playSound = true;
                    scrollingText.SetAudio(voices.kayVoice);
                    SetColor(new Color(.8f,0,0));
                    scrollingText.BeginTextBuffer("Oy! You watch where you step!");
                    stage = CutsceneStage.DisplayText2;
                }
                break;
            case CutsceneStage.DisplayText2:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Me watch? By God, you walked straight into me!");
                    stage = CutsceneStage.DisplayText3;
                }
                break;
            case CutsceneStage.DisplayText3:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.kayVoice);
                    SetColor(new Color(.8f,0,0));
                    scrollingText.BeginTextBuffer("Blame me, will you, you cowardly bastard?");
                    stage = CutsceneStage.DisplayText4;
                }
                break;
            case CutsceneStage.DisplayText4:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Why, if you weren’t of my station, I’d strike you dead where you stood!");
                    stage = CutsceneStage.DisplayText5;
                }
                break;
            case CutsceneStage.DisplayText5:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Base villain! You may handle Arthur’s cup, but you have no right to threaten me!");
                    stage = CutsceneStage.DisplayText6;
                }
                break;
            case CutsceneStage.DisplayText6:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.kayVoice);
                    SetColor(new Color(.8f,0,0));
                    scrollingText.BeginTextBuffer("Aye, I’ll question you all you like, you lazy, yellow-bellied, wretch!");
                    stage = CutsceneStage.DisplayText7;
                }
                break;
            case CutsceneStage.DisplayText7:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("I didn’t become seneschal to the king himself just by sitting around torpid like you!"); 
                    stage = CutsceneStage.DisplayText8;
                }
                break;
            case CutsceneStage.DisplayText8:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("While you were stumbling around drunk, walking into people, I was out questing!");
                    stage = CutsceneStage.DisplayText9;
                }
                break;
            case CutsceneStage.DisplayText9:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Shut thy wretched mouth, knave, and stay out of my way until you have some valiant deed to thy name --");
                    stage = CutsceneStage.DisplayText10;
                }
                break;
            case CutsceneStage.DisplayText10:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("or, kingly father or not, you’ll regret crossing Sir Kay of Camelot!");
                    stage = CutsceneStage.DisplayText11;
                }
                break;
            case CutsceneStage.DisplayText11:
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
        FadeOut,
        FadeInGame,
        Exit
    }

    private void SetColor(Color color) {
        scrollingText.Modulate = color;
    }
}