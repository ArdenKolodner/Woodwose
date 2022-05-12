using Godot;
using System;

public class SwordCutscene : Node2D
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
                    scrollingText.SetAudio(voices.sideCharacterVoice);
                    SetColor(new Color(.44f,0,.57f));
                    scrollingText.BeginTextBuffer("My lord! I bear a message from Lady Laudine.");
                    stage = CutsceneStage.DisplayText1;
                }
                break;
            case CutsceneStage.DisplayText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Ah, my love! I hope she can find the Lord’s forgiveness, for I tarried far too long at these tournaments.");
                    stage = CutsceneStage.DisplayText2;
                }
                break;
            case CutsceneStage.DisplayText2:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.sideCharacterVoice);
                    SetColor(new Color(.44f,0,.57f));
                    scrollingText.BeginTextBuffer("I... am sorry to disappoint, sir.");
                    stage = CutsceneStage.DisplayText3;
                }
                break;
            case CutsceneStage.DisplayText3:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("She sends me... she says to tell you that you are no longer welcome at her castle,");
                    stage = CutsceneStage.DisplayText4;
                }
                break;
            case CutsceneStage.DisplayText4:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("and that you have repaid her love and trust with lies.");
                    stage = CutsceneStage.DisplayText5;
                }
                break;
            case CutsceneStage.DisplayText5:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Oh, it wounds my heart to hear those words from my beloved!");
                    stage = CutsceneStage.DisplayText6;
                }
                break;
            case CutsceneStage.DisplayText6:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("And yet, it is perhaps what I deserve.");
                    stage = CutsceneStage.DisplayText7;
                }
                break;
            case CutsceneStage.DisplayText7:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.sideCharacterVoice);
                    SetColor(new Color(.44f,0,.57f));
                    scrollingText.BeginTextBuffer("There is... one more thing.");
                    stage = CutsceneStage.DisplayText8;
                }
                break;
            case CutsceneStage.DisplayText8:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("What is it?");
                    stage = CutsceneStage.DisplayText9;
                }
                break;
            case CutsceneStage.DisplayText9:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.sideCharacterVoice);
                    SetColor(new Color(.44f,0,.57f));
                    scrollingText.BeginTextBuffer("She wants her ring back.");
                    stage = CutsceneStage.DisplayText10;
                }
                break;
            case CutsceneStage.DisplayText10:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Alas, it is hers to take.");
                    stage = CutsceneStage.DisplayText11;
                }
                break;
            case CutsceneStage.DisplayText11:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Here you go.");
                    stage = CutsceneStage.DisplayText12;
                }
                break;
            case CutsceneStage.DisplayText12:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.sideCharacterVoice);
                    SetColor(new Color(.44f,0,.57f));
                    scrollingText.BeginTextBuffer("What will you do now, sir? If I may ask.");
                    stage = CutsceneStage.DisplayText13;
                }
                break;
            case CutsceneStage.DisplayText13:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("I... do not know.");
                    stage = CutsceneStage.DisplayText14;
                }
                break;
            case CutsceneStage.DisplayText14:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("I cannot enter another tournament while my heart lies shattered into countless pieces.");
                    stage = CutsceneStage.DisplayText15;
                }
                break;
            case CutsceneStage.DisplayText15:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer(" I must reflect on what to do, if I may win back her love.");
                    stage = CutsceneStage.DisplayText16;
                }
                break;
            case CutsceneStage.DisplayText16:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Perhaps I shall find a quiet spot in the woods, and just... think.");
                    stage = CutsceneStage.DisplayText17;
                }
                break;
            case CutsceneStage.DisplayText17:
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
        DisplayText15,
        DisplayText16,
        DisplayText17,
        FadeOut,
        FadeInGame,
        Exit
    }

    private void SetColor(Color color) {
        scrollingText.Modulate = color;
    }
}