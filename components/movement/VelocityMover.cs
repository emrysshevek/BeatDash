using Godot;

public partial class VelocityMover: BaseMover, IBumpable, IReversible, IReflectable
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

    public void Bump(Vector2 velocity)
    {
        GD.Print($"Bump velocity = {velocity}");
        Body.SetDeferred(CharacterBody2D.PropertyName.Position, Body.Position + velocity.Normalized());
        Body.SetDeferred(CharacterBody2D.PropertyName.Velocity, velocity);
    }

    public void Reverse(Vector2 addedVelocity = default)
    {
        Body.Velocity = addedVelocity - Body.Velocity;
    }

    public void Reflect(Vector2 normal, Vector2 addedVelocity = default)
    {
        Body.Velocity = Body.Velocity.Bounce(normal) + addedVelocity;
    }
}
