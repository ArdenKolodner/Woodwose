using Godot;
using System;
using System.Collections.Generic;

public class Hints : Node2D
{
    Hint currentHint;
    AnimationPlayer anim;
    Queue<Hint> queue;
    CutscenePlayer cutscenePlayer;

    public override void _Ready()
    {
        anim = (AnimationPlayer)GetNode("./AnimationPlayer");
        queue = new Queue<Hint>();
        cutscenePlayer = (CutscenePlayer)GetNode("../CutscenePlayer");

        foreach (Node node in GetChildren()) {
            if (node.Name == "AnimationPlayer") continue;
            Node2D node2D = (Node2D)node;
            node2D.Show();
            node2D.Modulate = new Color(node2D.Modulate.r, node2D.Modulate.g, node2D.Modulate.b, 0f);
        }
        currentHint = Hint.None;
    }

    public override void _Process(float delta) {
        if (queue.Count > 0 && currentHint == Hint.None && !anim.IsPlaying() && !cutscenePlayer.CutscenePlaying) {
            ShowHint(queue.Dequeue());
        }
        if (cutscenePlayer.CutscenePlaying) Hide(); else Show();
    }

    public void ShowHintIfNotAlready(Hint hint) {
        if (currentHint != hint) ShowHint(hint);
    }

    public void ShowHint(Hint hint) {
        if (HintData.learned[hint]) return;

        if (currentHint == Hint.None) {
            anim.Play("Show" + hint.ToString());
            currentHint = hint;
        } else {
            queue.Enqueue(hint);
        }
    }

    public void LearnHint(Hint hint) { // Sets hint to learned, hides hint only if it's the one showing currently
        HintData.learned[hint] = true;
        if (currentHint != hint) return;
        anim.PlayBackwards("Show" + currentHint.ToString());
        currentHint = Hint.None;
    }

    public void QueueHint(Hint hint) {
        if (HintData.learned[hint]) return;
        queue.Enqueue(hint);
    }

    public void HideHint() {
        anim.PlayBackwards("Show" + currentHint.ToString());
        currentHint = Hint.None;
    }
}
