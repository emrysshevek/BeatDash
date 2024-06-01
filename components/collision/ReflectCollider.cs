using Godot;
using System;

public partial class ReflectCollider : BaseCollider
{
	[Export]
	public bool AddVelocity = false;

    public override void OnColliderIntersection(BaseCollider collider)
    {
        if (collider is IReflectable obj)
		{
			if (AddVelocity) obj.Reflect(Body.Velocity);
			else obj.Reflect();
		}
    }
}
