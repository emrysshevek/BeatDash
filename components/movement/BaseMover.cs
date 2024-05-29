using Godot;

public partial class BaseMover: Node
{
    [Export]
    public CharacterBody2D Body;

    public void Move(double delta)
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

    public Vector2 GetNearestCellCenter(Vector2 position)
    {
        var tile = (Vector2I) position / Global.TileSize;
        var result = (Vector2) tile * Global.TileSize;
        result.X += Global.TileSize / 2f;
        result.Y += Global.TileSize / 2f;
        return result.Round();
    }
}
