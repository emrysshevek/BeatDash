using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class Background : Node2D
{
	[Export]
	public int PulseEvery = 1;

	public int PulseCount = 0;
	public AnimationPlayer AP;
	private Clock Clock = null;
	public float SecondsPerPulse
	{
		get => (float) Clock.SecondsPerBeat * PulseEvery;
	}
    public override void _Ready()
    {
        base._Ready();

		Clock = GetNode<Clock>("/root/Clock");
		Clock.Tick += OnClockTick;
		Clock.BPMChanged += OnBPMChanged;

		AP = GetNode<AnimationPlayer>("AnimationPlayer");
		// AP.SpeedScale = 1 / SecondsPerPulse;;
    }

	public void OnClockTick()
	{
		PulseCount = (PulseCount + 1) % PulseEvery;
		if (PulseCount == 0)
		{
			// GD.Print($"{SecondsPerPulse} sec Background Pulse @ {Clock.ElapsedTime}");
			// GD.Print($"SpeedScale={AP.SpeedScale}, PulseEvery={PulseEvery}");
			AP.Stop(keepState: true);
			AP.Play("pulse");
		}
	}

	public void OnBPMChanged()
	{
		// AP.SpeedScale = 1 / SecondsPerPulse;
	}

	public void OnSync()
	{
		// AP.SpeedScale = 1 / SecondsPerPulse;
		GD.Print($"Clock.SecondsPerBeat={Clock.SecondsPerBeat}, SecondsPerPulse={SecondsPerPulse}, AP.SpeedScale={AP.SpeedScale}");
	}
}
