using Godot;
using System;

public partial class BumpCollider : Area2D, IBumpable
{
	[Export]
	public CharacterBody2D Body; 

	  public void Bump(Vector2 velocity)
    {
        GD.Print($"Bump velocity = {velocity}");
        Body.SetDeferred(CharacterBody2D.PropertyName.Position, Body.Position + velocity.Normalized());
        Body.SetDeferred(CharacterBody2D.PropertyName.Velocity, velocity);
    }

	private void OnAreaEntered(Area2D area)
    {
        GD.Print($"{area} entered area. This position={GlobalPosition}, body={Body.GlobalPosition}, other={area.GlobalPosition}");
        if (area.GlobalPosition.Round() == Body.GlobalPosition.Round() && area is IBumpable obj)
        {
            Body.GlobalPosition = Body.GlobalPosition.Round();
            GD.Print("BUMP");
            obj.Bump(Body.Velocity);
            Body.Velocity = Vector2.Zero;
        }
    }
}
