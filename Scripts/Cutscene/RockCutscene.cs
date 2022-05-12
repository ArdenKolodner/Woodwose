using Godot;
using System;

public class RockCutscene : Node2D
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
        stage = CutsceneStage.PreText;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch (stage) {
            case CutsceneStage.PreText:
                scrollingText.playSound = false;
                SetColor(new Color(.9f,.9f,.9f));
                scrollingText.BeginTextBuffer("The rock is slick with newly fallen rain. A perfectly normal sight, yet it tickles your memory...");
                stage = CutsceneStage.BeginFadeOut;
                break;
            case CutsceneStage.BeginFadeOut:
                if (scrollingText.PlayerExited) {
                    fadeToBlack.Show();
                    fader.PlayBackwards("FadeIn");
                    scrollingText.Hide();
                    stage = CutsceneStage.FadeOutGame;
                }
                break;
            case CutsceneStage.FadeOutGame:
                if (fadeToBlack.Color.a == 1.0) {
                    cutsceneBG.Show();
                    stage = CutsceneStage.FadeIn;
                    fader.Play("FadeIn");
                }
                break;
            case CutsceneStage.FadeIn:
                if (fadeToBlack.Color.a == 0.0) {
                    scrollingText.playSound = true;
                    scrollingText.Show();
                    scrollingText.SetAudio(voices.escladosVoice);
                    SetColor(new Color(0, 0, 0));
                    scrollingText.BeginTextBuffer("Ho there! You vagrant, what are you doing on my land?");
                    stage = CutsceneStage.DisplayText1;
                }
                break;
            case CutsceneStage.DisplayText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1, 1, 1));
                    scrollingText.BeginTextBuffer("Are you the lord of these lands?");
                    stage = CutsceneStage.DisplayText2;
                }
                break;
            case CutsceneStage.DisplayText2:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.escladosVoice);
                    SetColor(new Color(0, 0, 0));
                    scrollingText.BeginTextBuffer("That’s right, and by our Lord, you’ve much to answer for about their state!");
                    stage = CutsceneStage.DisplayText3;
                }
                break;
            case CutsceneStage.DisplayText3:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("I-");
                    stage = CutsceneStage.DisplayText4;
                }
                break;
            case CutsceneStage.DisplayText4:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.escladosVoice);
                    SetColor(new Color(0, 0, 0));
                    scrollingText.BeginTextBuffer("No -- protest not, for I know that storm was of your doing!");
                    stage = CutsceneStage.DisplayText5;
                }
                break;
            case CutsceneStage.DisplayText5:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("And if it was?");
                    stage = CutsceneStage.DisplayText6;
                }
                break;
            case CutsceneStage.DisplayText6:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.escladosVoice);
                    SetColor(new Color(0, 0, 0));
                    scrollingText.BeginTextBuffer("Then I shall teach you the lesson I taught the last fool I crossed blades with.");
                    stage = CutsceneStage.DisplayText7;
                }
                break;
            case CutsceneStage.DisplayText7:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("I will repay the insult you dealt my cousin.");
                    stage = CutsceneStage.DisplayText8;
                }
                break;
            case CutsceneStage.DisplayText8:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.escladosVoice);
                    SetColor(new Color(0, 0, 0));
                    scrollingText.BeginTextBuffer("Your cousin, was he? And what might your name be, so I may know who it is I defeat?");
                    stage = CutsceneStage.DisplayText9;
                }
                break;
            case CutsceneStage.DisplayText9:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("My name is-");
                    stage = CutsceneStage.DisplayText10;
                }
                break;
            case CutsceneStage.DisplayText10:
                if (scrollingText.PlayerExited) {
                    scrollingText.playSound = false;
                    SetColor(new Color(.9f,.9f,.9f));
                    scrollingText.BeginTextBuffer("The memory fades.");
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
        PreText,
        BeginFadeOut,
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