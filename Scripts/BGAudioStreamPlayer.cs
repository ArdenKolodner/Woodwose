using Godot;
using System;
using System.Collections.Generic;

public class BGAudioStreamPlayer : AudioStreamPlayer
{
    [Export]
    public List<AudioStreamMP3> bgMusics;
    [Export]
    AudioStreamMP3 bgCombat;
    [Export]
    AudioStreamMP3 memory;
    AnimationPlayer anim;
    [Export]
    float timeAfterCombatToFadeOut;
    [Export]
    float timeBetweenBGTracks;
    float timeSinceInCombat;
    float timeSinceBG;
    bool inCutscene = false;
    string lastAnimFinished;
    bool memorySoundScheduled = false;
    public override void _Ready()
    {
        anim = (AnimationPlayer)GetNode("./AnimationPlayer");
        timeSinceInCombat = 0;
        timeSinceBG = timeBetweenBGTracks / 2;
        lastAnimFinished = "";
    }

    public override void _Process(float delta) {
        timeSinceInCombat += delta;
        if (!inCutscene && !Playing) timeSinceBG += delta;

        if (timeSinceInCombat >= timeAfterCombatToFadeOut && !anim.IsPlaying() && Stream == bgCombat) {
            BeginFadeOut(2);
        }

        if (timeSinceBG >= timeBetweenBGTracks && !inCutscene && !Playing) {
            timeSinceBG = 0;
            VolumeDb = 0;
            Stream = bgMusics[(int)(GD.Randi() % 2)];
            Play();
        }
    }

    public void BeginFadeIn(float durationSec) {
        lastAnimFinished = "";
        anim.PlaybackSpeed = 1 / durationSec;
        anim.Play("VolumeFadeIn1Sec");
    }

    public void BeginFadeOut(float durationSec) {
        lastAnimFinished = "";
        anim.PlaybackSpeed = 1 / durationSec;
        anim.Play("VolumeFadeOut1Sec");
    }

    public void Notify_InCombat() {
        if (inCutscene) return;

        timeSinceInCombat = 0;

        if (Stream != bgCombat) {
            if (lastAnimFinished == "VolumeFadeOut1Sec") {
                Stream = bgCombat;
                BeginFadeIn(2);
                Play();
            } else BeginFadeOut(1);

        } else {
            if (!Playing) {
                BeginFadeIn(2);
                Play();
            }
        }
    }

    public void Notify_FadeOutForCutscene(bool scheduleMemorySound) {
        inCutscene = true;
        if (!(anim.IsPlaying() && anim.AssignedAnimation == "VolumeFadeIn1Sec")) {
            anim.PlaybackSpeed = 1;
            anim.Play("VolumeFadeOut1Sec");
        }
        memorySoundScheduled = scheduleMemorySound;
    }

    public void Notify_CutsceneOver() {
        inCutscene = false;
    }

    public void _AnimationFinished(string animName) {
        lastAnimFinished = animName;
        if (animName == "VolumeFadeOut1Sec") {
            Stop();
            if (memorySoundScheduled) {
                memorySoundScheduled = false;
                Stream = memory;
                VolumeDb = 0;
                Play();
            }
        }
    }
}
