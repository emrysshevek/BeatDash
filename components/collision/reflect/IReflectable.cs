
using Godot;

public interface IReflectable
{
    public Vector2 Velocity { get; }
    public void Reflect(Vector2 normal, Vector2 addedVelocity=new Vector2());
}