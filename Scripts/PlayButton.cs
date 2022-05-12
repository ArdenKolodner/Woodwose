using Godot;
using System;

public class PlayButton : Button
{
    [Export]
    float opacityNotHovered;
    bool pressed = false;
    ColorRect fade;
    AudioStreamPlayer2D audio;
    [Export]
    AudioStreamMP3 startup;
    [Export]
    AudioStreamMP3 play;

    public override void _Ready() {
        fade = (ColorRect)GetNode("../FadeToBlack");
        audio = (AudioStreamPlayer2D)GetNode("../AudioStreamPlayer2D");
        audio.Stream = startup;
        audio.Play();
    }

    public override void _Process(float delta)
    {
        if (IsHovered()) {
                SelfModulate = new Color(SelfModulate.r, SelfModulate.g, SelfModulate.b, Math.Min(SelfModulate.a + delta, 1.2f));
        } else {
            if (SelfModulate.a > opacityNotHovered) {
                SelfModulate = new Color(SelfModulate.r, SelfModulate.g, SelfModulate.b, Math.Max(SelfModulate.a - delta, opacityNotHovered));
            }
        }

        if (pressed) {
            fade.Modulate = new Color(fade.Modulate.r, fade.Modulate.g, fade.Modulate.b, Math.Min(fade.Modulate.a + delta/2, 1f));
            if (fade.Modulate.a == 1f) {
                // DEMO CODE!!!!!!!!!!
                //if (Input.IsKeyPressed((int)KeyList.D)) {GetTree().ChangeScene("res://Scenes/Demo.tscn"); return;}

                GetTree().ChangeScene("res://Scenes/Game_New.tscn");
            }
        }
    }

    public override void _Pressed() {
        if (pressed) return;
        pressed = true;
        fade.Show();
        audio.Stream = play;
        audio.Play();
    }
}
