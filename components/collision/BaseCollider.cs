using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BaseCollider : Area2D
{
  [Signal]
  public delegate void ColliderIntersectedEventHandler(BaseCollider area);

	[Export]
	public BaseActor Body;

  protected Metronome Metronome;

  public override void _Ready()
  {
    Metronome = GetNode<Metronome>("/root/Metronome");
    Metronome.Beat += OnBeat;
  }
  public void OnAreaEntered(Area2D area)
  {
    CheckCollision(area);
  }
  
  public void CheckCollision(Area2D area)
  {
    GD.Print($"Checking collision. This position: {Body.GlobalPosition}. Area position: {area.GlobalPosition}");
    if (area is BaseCollider collider)
    {
      if (collider.Body == this.Body) return;
      
      GD.Print("Area entered");
      if(area.GlobalPosition.Round() == Body.GlobalPosition.Round())
      {
        area.GlobalPosition = area.GlobalPosition.Round();
        Body.GlobalPosition = Body.GlobalPosition.Round();
        OnColliderIntersection(collider);
      }
    }
  }

  public void CheckCollisions()
  {
    GD.Print($"[{Metronome.TotalElapsedTime}] {Body.Name} checking for collisions on beat");
    foreach(var area in GetOverlappingAreas())
    {
      CheckCollision(area);
    }
  }

  public void OnBeat()
  {
    CallDeferred(BaseCollider.MethodName.CheckCollisions);    
  }

  public virtual void OnColliderIntersection(BaseCollider collider)
  {
      throw new NotImplementedException();
  }
}
