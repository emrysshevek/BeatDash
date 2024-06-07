using Godot;
using System;

public partial class BaseActor : CharacterBody2D
{
  [Signal]
  public delegate void ObjectCollisionEventHandler();
  [Signal]
  public delegate void WallCollisionEventHandler();
  [Signal]
  public delegate void CellIntersectionEventHandler();

  private bool CanSignalObjectCollision = true;
  private bool CanSignalWallCollision = true;
  private bool CanSignalCellIntersection = true;


  public void SignalObjectCollision()
  {
    if (CanSignalObjectCollision)
    {
      EmitSignal(SignalName.ObjectCollision);
      CanSignalObjectCollision = false;
      CallDeferred(BaseActor.MethodName.ResetSignalFlags);
    } 
  }

  public void SignalWallCollision()
  {
    if (CanSignalWallCollision)
    {
      EmitSignal(SignalName.WallCollision);
      CanSignalWallCollision = false;
      CallDeferred(BaseActor.MethodName.ResetSignalFlags);
    } 
  }

  public void SignalCellIntersection()
  {
    if (CanSignalCellIntersection)
    {
      EmitSignal(SignalName.CellIntersection);
      CanSignalCellIntersection = false;
      CallDeferred(BaseActor.MethodName.ResetSignalFlags);
    } 
  }

  private void ResetSignalFlags()
  {
    CanSignalObjectCollision = true;
    CanSignalWallCollision = true;
    CanSignalCellIntersection = true;
  }
  public void Destroy()
  {
    CallDeferred(Node.MethodName.QueueFree);
  }
}
