using Godot;
using System;
using System.Collections.Generic;

public class ScrollingText : Label
{
    #pragma warning disable 0649
    [Export]
    float secPerCharacter;
    [Export]
    float secsPerSound;
    [Export]
    public bool playSound;
    public bool visible;
    public bool scrolling;
    public bool PlayerExited {get; private set;}
    float timeUntilNext;
    float timeUntilNextSound;
    bool eReleasedWhileScrollingFlag = false;
    bool eReleasedAfterDoneScrollingFlag = false;

    Node2D textBox;

    AudioStreamPlayer audio;
    [Export]
    List<AudioStreamMP3> audios;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Randomize();
        timeUntilNext = 0;
        timeUntilNextSound = 0;
        textBox = (Node2D)GetNode("../TextBox");
        audio = (AudioStreamPlayer)GetNode("../AudioStreamPlayer");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (visible && scrolling) {
            timeUntilNext += delta;
            if (playSound) timeUntilNextSound += delta;
            if (timeUntilNext > secPerCharacter) {
                timeUntilNext = 0;
                VisibleCharacters++;

                if (VisibleCharacters == Text.Length) {
                    VisibleCharacters = -1;
                    scrolling = false;
                }
            }

            if (playSound && timeUntilNextSound > secsPerSound) {
                timeUntilNextSound = 0;
                audio.Stream = audios[(int)(Math.Abs(GD.Randi() % audios.Count))];
                audio.Play();
            }

            if (Input.IsKeyPressed((int)KeyList.E)) {
                if (eReleasedWhileScrollingFlag) {
                    VisibleCharacters = -1;
                    scrolling = false;
                }
            } else {
                eReleasedWhileScrollingFlag = true;
            }
        } else if (visible) { // if visible but done scrolling
            if (Input.IsKeyPressed((int)KeyList.E)) {
                if (eReleasedAfterDoneScrollingFlag) {
                    PlayerExited = true;
                }
            } else {
                eReleasedAfterDoneScrollingFlag = true;
            }
        }

        if (!scrolling) {
            audio.Stop();
        }
    }

    public void BeginTextBuffer(string text) {
        PlayerExited = false;
        visible = true;
        eReleasedWhileScrollingFlag = false;
        eReleasedAfterDoneScrollingFlag = false;
        VisibleCharacters = 0;
        Text = text;
        scrolling = true;
        Show();
        textBox.Show();
    }

    public void HideText() {
        visible = false;
        scrolling = false;
        Hide();
        textBox.Hide();
    }

    public void SetAudio(List<AudioStreamMP3> v) {
        audios = v;
    }
}
