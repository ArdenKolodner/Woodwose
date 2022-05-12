using Godot;
using System;

public class KnifeCutscene : Node2D
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
                    scrollingText.BeginTextBuffer("Here you go, my liege. Sharpened and oiled, as you asked.");
                    stage = CutsceneStage.DisplayText1;
                }
                break;
            case CutsceneStage.DisplayText1:
                if (scrollingText.PlayerExited) {
                    scrollingText.BeginTextBuffer("You'll not often cross swords with a man better equipped than you are now, i'faith.");
                    stage = CutsceneStage.DisplayText2;
                }
                break;
            case CutsceneStage.DisplayText2:
                if (scrollingText.PlayerExited) {
                    scrollingText.SetAudio(voices.yvainVoice);
                    SetColor(new Color(1,1,1));
                    scrollingText.BeginTextBuffer("Thank you, lad -- and yet I fear I shall face foes which no sword can slay...");
                    stage = CutsceneStage.DisplayText3;
                }
                break;
            case CutsceneStage.DisplayText3:
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
        FadeOut,
        FadeInGame,
        Exit
    }

    private void SetColor(Color color) {
        scrollingText.Modulate = color;
    }
}