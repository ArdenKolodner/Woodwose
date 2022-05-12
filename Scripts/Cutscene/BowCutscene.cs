using Godot;
using System;

public class BowCutscene : Node2D
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
                    scrollingText.BeginTextBuffer("I hate these things.");
                    stage = CutsceneStage.DisplayText1;
                }
                break;
            case CutsceneStage.DisplayText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.calogrenantVoice);
                    SetColor(new Color(1,.8f,.3f));
                    scrollingText.BeginTextBuffer("What, bows? How come?");
                    stage = CutsceneStage.DisplayText2;
                }
                break;
            case CutsceneStage.DisplayText2:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("It is a peasant’s weapon. Hard to draw, hard to hold, hard to aim.");
                    stage = CutsceneStage.DisplayText3;
                }
                break;
            case CutsceneStage.DisplayText3:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Not suited for the likes of us... or me, at any rate.");
                    stage = CutsceneStage.DisplayText4;
                }
                break;
            case CutsceneStage.DisplayText4:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.calogrenantVoice);
                    SetColor(new Color(1,.8f,.3f));
                    scrollingText.BeginTextBuffer("I rather like it. And besides, most of the fighting peasants I’ve met tend to use close-range weapons anyway.");
                    stage = CutsceneStage.DisplayText5;
                }
                break;
            case CutsceneStage.DisplayText5:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Like the herder you saw with the cudgel?");
                    stage = CutsceneStage.DisplayText6;
                }
                break;
            case CutsceneStage.DisplayText6:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.calogrenantVoice);
                    SetColor(new Color(1,.8f,.3f));
                    scrollingText.BeginTextBuffer("Exactly. A crude weapon, hardly any artistry to it.");
                    stage = CutsceneStage.DisplayText7;
                }
                break;
            case CutsceneStage.DisplayText7:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Better suited for the beasts he managed than for men of honor to fight with.");
                    stage = CutsceneStage.DisplayText8;
                }
                break;
            case CutsceneStage.DisplayText8:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("A villainous weapon for a villain. But a sword is of a higher class.");
                    stage = CutsceneStage.DisplayText9;
                }
                break;
            case CutsceneStage.DisplayText9:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.calogrenantVoice);
                    SetColor(new Color(1,.8f,.3f));
                    scrollingText.BeginTextBuffer("Indeed, though he was passing helpful. He served his purpose well, as does a bow... and as does a sword.");
                    stage = CutsceneStage.DisplayText10;
                }
                break;
            case CutsceneStage.DisplayText10:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("Many things and many people are acceptable, I suppose, as long as they are true to themselves, and true to their word.");
                    stage = CutsceneStage.DisplayText11;
                }
                break;
            case CutsceneStage.DisplayText11:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Such a storyteller, with your morals! I see well why Queen Guinevere was anxious to hear your tales.");
                    stage = CutsceneStage.DisplayText12;
                }
                break;
            case CutsceneStage.DisplayText12:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.calogrenantVoice);
                    SetColor(new Color(1,.8f,.3f));
                    scrollingText.BeginTextBuffer("I do well enough, I think. Anything to please a lady.");
                    stage = CutsceneStage.DisplayText13;
                }
                break;
            case CutsceneStage.DisplayText13:
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
        FadeOut,
        FadeInGame,
        Exit
    }

    private void SetColor(Color color) {
        scrollingText.Modulate = color;
    }
}