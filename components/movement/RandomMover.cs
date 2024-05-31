using Godot;
using System;

public partial class RandomMover : BaseMover, IntervalSignalable
{
    private float _speed;
    [Export]
    public int Speed = 1;
    [Export]
    public float Interval = 1;

    public override void _Ready()
    {
        var metronome = GetNode<Metronome>("/root/Metronome");
        _speed = Global.TileSize * Speed * (1 / (float)metronome.GetSecondsPerInterval(Interval));

        metronome.SubscribeToInterval(Interval, this);
        
        Body.Velocity = _speed * ChooseRandomDirection();
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
    }

    private Vector2 ChooseRandomDirection()
    {
        return (GD.Randi() % 5) switch
        {
            0 => Vector2.Up,
            1 => Vector2.Down,
            2 => Vector2.Left,
            3 => Vector2.Right,
            _ => Vector2.Zero
        };
    }

    public void OnIntervalElapsed()
    {
        Body.Velocity = _speed * ChooseRandomDirection();
    }

}
