using Godot;
using System;

public partial class BaseActor : CharacterBody2D
{
	
  public void Destroy()
  {
    CallDeferred("queue_free");
  }
}
