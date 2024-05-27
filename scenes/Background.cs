using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Background : Node2D
{
	[Export]
	public IntervalCounter Interval;

	public AnimationPlayer AP;
	private Metronome Metronome = null;

    public override void _Ready()
    {
        base._Ready();

		Metronome = GetNode<Metronome>("/root/Metronome");
		Interval ??= GetNode<IntervalCounter>("BeatIntervalComponent");
		Interval.Beat += OnIntervalElapsed;

		AP = GetNode<AnimationPlayer>("AnimationPlayer");
		AP.SpeedScale = (float) Mathf.Max(1 / Interval.SecondsPerBeat, 1);
    }

	public void OnIntervalElapsed()
	{
		GD.Print($"{Interval.SecondsPerBeat} sec Background Pulse @ {Metronome.ElapsedTime}");
		GD.Print($"SpeedScale={AP.SpeedScale}, Frequency={Interval.BeatsPerInterval}");

		AP.Stop(keepState: true);

		AP.SpeedScale = (float) Mathf.Max(1 / Interval.SecondsPerBeat, 1);
		AP.Play("pulse");
	}
}
