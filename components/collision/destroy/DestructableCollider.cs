using Godot;
using System;
using System.ComponentModel;

public partial class DestructableCollider : BaseCollider, IDestructable
{
  public void Destroy()
  {
    Mover.Destroy();
  }

}
