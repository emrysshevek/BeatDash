using Godot;
using System;

public partial class CircleMover : BaseMover
{
    [Export]
    public float Radius = 1;
    [Export]
    public float RotationsPerBeat = 1;
    [Export]
    public bool Reversed = false;
    [Export]
    public Vector2 StartingDirection = Vector2.Zero;

    private float _turnSpeed;

    public override void _Ready()
    {
        base._Ready();
        if (StartingDirection == Vector2.Zero)
        {
            StartingDirection = Reversed ? Vector2.Left : Vector2.Right;
        }

        var m = GetNode<Metronome>("/root/Metronome");

        _turnSpeed = Mathf.DegToRad(360 * RotationsPerBeat * (1 / (float)m.SecondsPerBeat));

        var circumference = 2 * Mathf.Pi * Radius * Global.TileSize;
        var speed = RotationsPerBeat * (1 / (float)m.SecondsPerBeat) * circumference;
        Body.Velocity = StartingDirection * speed;
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
        Body.Velocity = Body.Velocity.Rotated(_turnSpeed * (float)delta);
    }
}
