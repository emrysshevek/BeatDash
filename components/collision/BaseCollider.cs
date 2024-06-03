using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BaseCollider : Area2D
{
    [Signal]
    public delegate void ColliderIntersectedEventHandler(BaseCollider area);

	[Export]
	public BaseActor Body;

    protected List<BaseCollider> Intersections;

    public void OnAreaEntered(Area2D area)
    {
      if (area is BaseCollider collider)
      {
        if (collider.Body == this.Body) return;
        
        GD.Print("Area entered");
        if(area.GlobalPosition.Round() == Body.GlobalPosition.Round())
        {
          area.GlobalPosition = area.GlobalPosition.Round();
          Body.GlobalPosition = Body.GlobalPosition.Round();
          OnColliderIntersection(collider);
        }
      }
    }

    public virtual void OnColliderIntersection(BaseCollider collider)
    {
        throw new NotImplementedException();
    }
}
