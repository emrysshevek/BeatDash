using Godot;
using System;

public partial class ReflectableCollider : BaseCollider, IReflectable
{

    public void Reflect(Vector2 normal, Vector2 addedVelocity = default)
    {
        Mover.Reflect(normal, addedVelocity);
    }
}