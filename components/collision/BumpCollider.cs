using Godot;

public partial class BumpCollider : BaseCollider, IBumpable, IReflectable
{
	public void Bump(Vector2 velocity)
    {
        GD.Print($"Bump velocity = {velocity}");
        Body.SetDeferred(CharacterBody2D.PropertyName.Position, Body.Position + velocity.Normalized());
        Body.SetDeferred(CharacterBody2D.PropertyName.Velocity, velocity);
    }

    public override void OnColliderIntersection(BaseCollider collider)
    {
        if (collider is IBumpable obj)
		{
			Body.GlobalPosition = Body.GlobalPosition.Round();
            GD.Print("BUMP");
            obj.Bump(Body.Velocity);
            Body.Velocity = Vector2.Zero;
		}
    }

    public void Reflect(Vector2 addedVelocity = new Vector2())
    {
        Body.Velocity = addedVelocity - Body.Velocity;
    }

}
