using Godot;
using System;

public class CutsceneObject : Area2D
{
    [Export]
    Cutscene CUTSCENE;

    public void Interacted() {
        ((CutscenePlayer)GetNode("/root/Game/Camera2D/CutscenePlayer")).PlaySceneCutscene(CUTSCENE);
        QueueFree();
    }
}
