using Godot;
using System;

public partial class StraightLineComponent : Node
{
    [Export]
    public Vector2I Direction = Vector2I.Up;
    [Export]
    public StepComponent Stepper;

    public override void _Ready()
    {
        Stepper ??= GetNode<StepComponent>("StepComponent");
        Stepper.SetDirection(Direction);
        Stepper.StepFinished += OnStepFinished;
    }

    private void OnStepFinished()
    {
        Stepper.SetDirection(Direction);
    }
}
