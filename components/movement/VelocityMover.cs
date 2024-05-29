using Godot;

public partial class VelocityMover: BaseMover
{
    [Export]
    public int Speed = 1;

    public override void _Ready()
    {
        var metronome = GetNode<Metronome>("/root/Metronome");
        var speed = Global.TileSize * Speed * (1 / (float)metronome.SecondsPerBeat);
        var direction = Body.Transform.BasisXform(Vector2.Up).Normalized();
        Body.Velocity = speed * direction;
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
    }
}
