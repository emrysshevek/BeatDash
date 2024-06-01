using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Background : Node2D, IntervalSignalable
{
	[Export]
	public float PulseInterval = 1;
	public AnimationPlayer AP;
	private Metronome Metronome;

    public override void _Ready()
    {
		Metronome = GetNode<Metronome>("/root/Metronome");
		Metronome.SubscribeToInterval(PulseInterval, this);
		

		AP = GetNode<AnimationPlayer>("AnimationPlayer");
		AP.SpeedScale = (float) Mathf.Max(1 / Metronome.GetSecondsPerInterval(PulseInterval), 1);
    }

	public void OnIntervalElapsed()
	{
		// GD.Print($"{1 / AP.SpeedScale} sec Background Pulse @ {Metronome.TotalElapsedTime}");
		// GD.Print($"SpeedScale={AP.SpeedScale}, Interval={PulseInterval}");

		AP.Stop(keepState: true);

		AP.SpeedScale = (float) Mathf.Max(1 / Metronome.GetSecondsPerInterval(PulseInterval), 1);
		AP.Play("pulse");
	}
}
