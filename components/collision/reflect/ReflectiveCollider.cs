using Godot;
using System;

public partial class ReflectiveCollider : BaseCollider
{
	[Export]
	public bool AddVelocity = false;

  public override void OnColliderIntersection(BaseCollider collider)
  {
    base.OnColliderIntersection(collider);
    if (collider is IReflectable obj)
    {
      if (AddVelocity) obj.Reflect(Body.Velocity);
      else obj.Reflect();
    }
  }
}
