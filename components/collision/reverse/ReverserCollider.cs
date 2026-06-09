using Godot;

public partial class ReverserCollider : BaseCollider
{
	[Export]
	public bool AddVelocity = false;

  public override void OnColliderIntersection(BaseCollider collider)
  {
    base.OnColliderIntersection(collider);
    if (collider is IReversible obj)
    {
      if (AddVelocity) obj.Reverse(Body.Velocity);
      else obj.Reverse();
    }
  }
}
