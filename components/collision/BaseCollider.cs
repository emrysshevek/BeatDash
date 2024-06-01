using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BaseCollider : Area2D
{
    [Signal]
    public delegate void ColliderIntersectedEventHandler(BaseCollider area);

	[Export]
	public CharacterBody2D Body;
	[Export]
	public float Radius = 64;

    protected List<BaseCollider> Intersections;

    public override void _PhysicsProcess(double delta)
    {
        GD.Print($"Current position: {GlobalPosition.Round()}");
        foreach (var area in GetOverlappingAreas())
        {
            GD.Print($"Intersected area position: {area.GlobalPosition.Round()}");
            if (area.GlobalPosition.Round() == Body.GlobalPosition.Round() && area is BaseCollider collider)
            {
                OnColliderIntersection(collider);
            }
        }
    }

    public virtual void OnColliderIntersection(BaseCollider collider)
    {
        throw new NotImplementedException();
    }
}
