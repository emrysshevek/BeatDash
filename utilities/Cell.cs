
using System.ComponentModel.DataAnnotations;
using Godot;

public partial class Cell : Area2D
{
    [Export]
    public float Tolerance = 3;
    public Vector2I Coordinates { get; set; }


    public override void _PhysicsProcess(double delta)
    {
        foreach (var body in GetOverlappingBodies())
        {
            if (body is BaseActor actor)
            {
                CheckIntersection(actor);
                CheckReflection(actor);
            }
        }
    }

    public void CheckIntersection(BaseActor actor)
    {
        if (actor.GlobalPosition.DistanceTo(GlobalPosition) <= Tolerance)
        {
            actor.SignalCellIntersection();
        }
    }

    public void CheckReflection(BaseActor actor)
    {
        ;;
    }
}