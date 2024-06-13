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
  private Timer IntersectionTimer;
  private Timer CollisionTimer;

  public override void _Ready()
  {
    IntersectionTimer = GetNode<Timer>("IntersectionTimer");
    CollisionTimer = GetNode<Timer>("CollisionTimer");
  }

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
      GD.Print("Intersected Cell");
      EmitSignal(SignalName.CellIntersection);
      CanSignalCellIntersection = false;
      // CallDeferred(BaseActor.MethodName.ResetSignalFlags);
      IntersectionTimer.Start();
    } 
  }

  private void ResetCellIntersectionFlag()
  {
    CanSignalCellIntersection = true;
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
