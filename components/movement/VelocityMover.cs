using Godot;

public partial class VelocityMover: BaseMover
{
    [Export]
    public int Speed = 1;

    public override void _Ready()
    {
        base._Ready();
        var speed = Global.TileSize * Speed * (1 / (float)Metronome.SecondsPerBeat);
        var direction = Body.Transform.BasisXform(Vector2.Right).Normalized();
        Body.Velocity = speed * direction;
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
    }
}
