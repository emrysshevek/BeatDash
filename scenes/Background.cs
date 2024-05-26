using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Background : Node2D
{
	[Export]
	public BeatIntervalComponent Interval;

	public AnimationPlayer AP;
	private Metronome Metronome = null;

    public override void _Ready()
    {
        base._Ready();

		Metronome = GetNode<Metronome>("/root/Metronome");
		Interval ??= GetNode<BeatIntervalComponent>("BeatIntervalComponent");
		Interval.IntervalElapsed += OnIntervalElapsed;

		AP = GetNode<AnimationPlayer>("AnimationPlayer");
		AP.SpeedScale = Mathf.Max(1 / Interval.SecondsPerInterval, 1);
    }

	public void OnIntervalElapsed()
	{
		GD.Print($"{Interval.SecondsPerInterval} sec Background Pulse @ {Metronome.ElapsedTime}");
		GD.Print($"SpeedScale={AP.SpeedScale}, Frequency={Interval.Frequency}");

		AP.Stop(keepState: true);

		AP.SpeedScale = Mathf.Max(1 / Interval.SecondsPerInterval, 1);
		AP.Play("pulse");
	}
}
