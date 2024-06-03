using Godot;
using System;

public partial class DestructiveCollider : BaseCollider
{
	public override void OnColliderIntersection(BaseCollider collider)
  {
    GD.Print($"Collided with object at {GlobalPosition}");
    if (collider is IDestructable obj)
		{
			obj.Destroy();
		}
  }
}
