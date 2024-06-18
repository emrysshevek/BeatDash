using Godot;

public partial class ReversibleCollider : BaseCollider
{

  public override void _Ready()
  {
    base._Ready();
    if (Mover is not IReversible) GD.PushError("ERROR: Mover is not Reversible");
  }
  
  
}
