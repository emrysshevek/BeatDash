using Godot;

public partial class VelocityMover : Area2D, IBumpable
{
    [Export]
    public int Speed = 1;
    [Export]
    public CharacterBody2D Body;

    public override void _Ready()
    {
        Body.Velocity = Body.Transform.BasisXform(Vector2.Up).Normalized() * Speed * Global.TileSize;
    }

    public override void _PhysicsProcess(double delta)
    {
        var collision = Body.MoveAndCollide(Body.Velocity * (float)delta);
        if (collision != null)
        {
            Body.Velocity.Bounce(collision.GetNormal());
        }

        if (Body.GlobalPosition.Round() == GetNearestCellCenter(Body.GlobalPosition))
        {
            Body.GlobalPosition = GetNearestCellCenter(Body.GlobalPosition);
            GD.Print($"Snapping position to cell at {Body.GlobalPosition}");
        }
    }

    public void Bump(Vector2 velocity)
    {
        GD.Print($"Bump velocity = {velocity}");
        Body.SetDeferred(CharacterBody2D.PropertyName.Position, Body.Position + velocity.Normalized());
        Body.SetDeferred(CharacterBody2D.PropertyName.Velocity, velocity);
    }

    private Vector2 GetNearestCellCenter(Vector2 position)
    {
        var tile = (Vector2I) position / Global.TileSize;
        var result = (Vector2) tile * Global.TileSize;
        result.X += Global.TileSize / 2f;
        result.Y += Global.TileSize / 2f;
        return result.Round();
    }

    private void OnAreaEntered(Area2D area)
    {
        GD.Print($"{area} entered area. This position={GlobalPosition}, body={Body.GlobalPosition}, other={area.GlobalPosition}");
        if (area.GlobalPosition.Round() == Body.GlobalPosition.Round() && area is IBumpable obj)
        {
            Body.GlobalPosition = Body.GlobalPosition.Round();
            GD.Print("BUMP");
            obj.Bump(Body.Velocity);
            Body.Velocity = Vector2.Zero;
        }
    }
}
