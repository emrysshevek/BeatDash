using Godot;
using System;

public partial class TmpLabel : Label
{
	private Clock Clock;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Clock = GetNode<Clock>("/root/Clock");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = $"{Clock.ElapsedTime}";
	}
}
