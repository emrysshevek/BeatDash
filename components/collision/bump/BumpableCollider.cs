using Godot;
using System;

public partial class BumpableCollider : BaseCollider, IBumpable
{
  public void Bump(Vector2 velocity)
  {
    GD.Print($"Bump velocity = {velocity}");
    Body.SetDeferred(CharacterBody2D.PropertyName.Position, Body.Position + velocity.Normalized());
    Body.SetDeferred(CharacterBody2D.PropertyName.Velocity, velocity);
  }

}
