using Godot;
using System;
using System.ComponentModel;

public partial class DestructableCollider : BaseCollider, IDestructable
{
  public void Destroy()
  {
    GD.Print("Destroy triggered");
    Body.Destroy();

  }

}
