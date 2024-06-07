using Godot;

public partial class BumpCollider : BaseCollider
{

  public override void _Ready()
  {
    base._Ready();
  }

  public override void OnColliderIntersection(BaseCollider collider)
  {
    base.OnColliderIntersection(collider);
    if (collider is IBumpable obj)
    {
      Body.GlobalPosition = Body.GlobalPosition.Round();
      GD.Print("BUMP");
      obj.Bump(Body.Velocity);
      Body.Velocity = Vector2.Zero;
    }
  }
}
