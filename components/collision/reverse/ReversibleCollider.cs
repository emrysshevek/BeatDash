using Godot;

public partial class ReversibleCollider : BaseCollider, IReversible
{
  public void Reverse(Vector2 addedVelocity = default)
  {
    Mover.Reverse();
  }

}
