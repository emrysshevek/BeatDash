using Godot;
using System;

public partial class TmpLabel : Label
{
	private Metronome Metronome;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Metronome = GetNode<Metronome>("/root/Metronome");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Text = $"{Metronome.TotalElapsedTime}";
	}
}
