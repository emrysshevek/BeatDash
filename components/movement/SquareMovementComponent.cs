using Godot;
using System;

public partial class SquareMovementComponent : Node
{
    [Export]
    public Vector2I StartingDirection = Vector2I.Up;
    [Export]
    public Vector2I Diameter = Vector2I.One;
    [Export]
    public bool Reversed = false;
    [Export]
    public StepComponent Stepper;

    private int XCount = 0;
    private int YCount = 0;
    private Vector2I Direction;

    public override void _Ready()
    {
        Direction = StartingDirection;

        Stepper ??= GetNode<StepComponent>("StepComponent");
        Stepper.SetDirection(Direction);
        Stepper.StepFinished += OnStepFinished;
    }

    private void OnStepFinished()
    {
        Direction = (Vector2I) ((Vector2)Direction).Rotated(Mathf.DegToRad(90)).Round();
        Stepper.SetDirection(Direction);
    }
}
