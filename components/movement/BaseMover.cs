using System;
using Godot;

public partial class BaseMover: Node
{
    [Export]
    public CharacterBody2D Body;
    protected Metronome Metronome;

    public override void _Ready()
    {
        Metronome = GetNode<Metronome>("/root/Metronome");
        Metronome.Beat += OnBeat;
    }

    public void Move(double delta)
    {
        var collision = Body.MoveAndCollide(Body.Velocity * (float)delta);
        if (collision != null)
        {
            // GD.Print($"[{Metronome.TotalElapsedTime}] Collision with wall");
            Body.Velocity = Body.Velocity.Bounce(collision.GetNormal());
        }

        
    }

    protected virtual void OnBeat()
    {
        GD.Print($"{Body.Name} Position on the beat: {Body.GlobalPosition}. Nearest cell position: {GetNearestCellCenter(Body.GlobalPosition)}");
        // Body.GlobalPosition = GetNearestCellCenter(Body.GlobalPosition);
        if (Mathf.Abs((Body.GlobalPosition - GetNearestCellCenter(Body.GlobalPosition)).Length()) < 5)
        {
            Body.SetDeferred(CharacterBody2D.PropertyName.GlobalPosition, GetNearestCellCenter(Body.GlobalPosition));
            GD.Print($"[{Metronome.TotalElapsedTime}] Snapping {Body.Name} position on beat to cell at {GetNearestCellCenter(Body.GlobalPosition)}");
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
