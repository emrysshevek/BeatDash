using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class Cell : Area2D
{
    [Export]
    public float Tolerance = 2;
    public bool IsEdge => Neighbors.Values.Where(cell => cell != null).Any();

    public Vector2I Coordinates { get; set; } = Vector2I.MaxValue;
    public Dictionary<Vector2I, Cell> Neighbors = new Dictionary<Vector2I, Cell>()
    {
        {Vector2I.Up, null},
        {Vector2I.Up + Vector2I.Right, null},
        {Vector2I.Right, null},
        {Vector2I.Right + Vector2I.Down, null},
        {Vector2I.Down, null},
        {Vector2I.Down + Vector2I.Left, null},
        {Vector2I.Left, null},
        {Vector2I.Left + Vector2I.Up, null}
    };
    
    public override void _PhysicsProcess(double delta)
    {
        foreach (var body in GetOverlappingBodies())
        {
            if (body is BaseActor actor)
            {
                TryIntersect(actor);
                if (IsEdge) TryReflect(actor);
            }
        }
    }

    public void TryIntersect(BaseActor actor)
    {
        if (actor.GlobalPosition.DistanceTo(GlobalPosition) <= Tolerance)
        {
            actor.SignalCellIntersection();
            actor.GlobalPosition = GlobalPosition;
            GD.Print();
        }
    }

    public void Tint()
    {
        GetNode<CollisionShape2D>("CollisionShape2D").DebugColor = new Color(1f, 0f, 0f, .5f);
    }

    public void UnTint()
    {
        GetNode<CollisionShape2D>("CollisionShape2D").DebugColor = new Color(0f, .5f, .5f, .5f);
    }

    private void HighlightNeighbors()
    {
        foreach(var cell in Neighbors.Values)
        {
            cell?.CallDeferred(Cell.MethodName.Tint);
        }
    }

    private void UnhighlightNeighbors()
    {
        foreach(var cell in Neighbors.Values)
        {
            cell?.UnTint();
        }
    }   

    public void TryReflect(BaseActor actor)
    {
        
        /*
            Steps:
            - determine if actor is moving into cell or out of cell
            - if moving out of cell, determine which neighbors must exist to move into
            - if neighbor in direction of movement is missing, reflect instead
        */        
        var movementDirection = actor.Velocity.Sign();
        var directionToCenter = actor.GlobalPosition.DirectionTo(GlobalPosition);

        var isLeavingCell = directionToCenter.Normalized().Dot(actor.Velocity.Normalized()) < 0;
        
        var displacement = Vector2.Zero;
        var reflection = Vector2.One;

        if (actor.Velocity.Sign().X != directionToCenter.Sign().X && Neighbors[(Vector2I)movementDirection * Vector2I.Right] == null)
        {
            displacement.X = 2 * directionToCenter.X;
            reflection.X = -1;
        }

        if (actor.Velocity.Sign().Y != directionToCenter.Sign().Y && Neighbors[(Vector2I)movementDirection * Vector2I.Down] == null)
        {
            displacement.Y = 2 * directionToCenter.Y;
            reflection.Y = -1;
        }

        actor.GlobalPosition += displacement;
        actor.Velocity *= reflection;
        // if (isLeavingCell && Neighbors[(Vector2I) movementDirection] == null)
        // {
        //     GD.Print($"Reflecting {actor.Name}. Center={GlobalPosition}, Obj Position={actor.GlobalPosition}");
        //     // Reposition object as if it had reflected, then reflect the velocity


        //     GD.Print($"Displacement={displacement}, reflection={reflection}");
        // }
    }

}