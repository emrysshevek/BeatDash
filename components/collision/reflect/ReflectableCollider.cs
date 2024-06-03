using Godot;
using System;

public partial class ReflectableCollider : BaseCollider, IReflectable
{
  public void Reflect(Vector2 addedVelocity = default)
  {
    Body.Velocity = addedVelocity - Body.Velocity;
  }

}
