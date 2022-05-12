using Godot;
using System;

public class Death : Node2D
{
    #pragma warning disable 0649
    ColorRect fadeToBlack;
    AnimationPlayer fader;
    Sprite cutsceneBG;
    Camera2D sceneCamera;
    ScrollingText scrollingText;
    ScrollingText restartText;
    CutsceneStage stage;
    [Export]
    Texture knownTexture;
    float delay;
    public override void _Ready()
    {
        delay = 0;
        scrollingText = (ScrollingText)GetNode("./ScrollingText");
        restartText = (ScrollingText)GetNode("./Restart");
        cutsceneBG = (Sprite)GetNode("./CutsceneBG");
        fadeToBlack = (ColorRect)GetNode("./FadeToBlack");
        fader = (AnimationPlayer)GetNode("./FadeToBlack/Fader");
        sceneCamera = (Camera2D)GetNode("./Camera2D");

        if (CutsceneData.cutscenePlayed[Cutscene.Armor]) cutsceneBG.Texture = knownTexture;

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
                    scrollingText.BeginTextBuffer("You have died.");
                    stage = CutsceneStage.Wait2Sec; // This cutscene is designed to be an infinite loop (because it's the death screen) so it never exits, but it still needs a next mode.
                }
                break;
            case CutsceneStage.Wait2Sec:
                delay += delta;
                if (delay >= 2) stage = CutsceneStage.RestartText;
                break;
            case CutsceneStage.RestartText:
                restartText.BeginTextBuffer("Press R to restart.");
                stage = CutsceneStage.Exit;
                break;
            case CutsceneStage.Exit:
                break;
        }
        if (Input.IsKeyPressed((int)KeyList.R) && (stage == CutsceneStage.RestartText || stage == CutsceneStage.Exit)) GetTree().ChangeScene("res://Scenes/Game_New.tscn");
    }

    private enum CutsceneStage {
        None,
        FadeOutGame,
        FadeIn,
        Wait2Sec,
        RestartText,
        Exit
    }
}