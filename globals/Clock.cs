using Godot;
using System;

public partial class Clock : Timer
{
	[Signal]
	public delegate void TickEventHandler(); 
	[Signal]
    public delegate void BPMChangedEventHandler();
	[Signal]
	public delegate void SyncEventHandler();

	private float _nextbpm = -1;
	private float _bpm = 60;
	[Export]
	public float BPM 
	{ 
		get => _bpm; 
		set 
		{
			_nextbpm = value;
		} 
	}
	public double SecondsPerBeat
	{
		get => WaitTime;
	}
	public double ElapsedTime { get; private set; } = 0;

    public override void _Ready()
    {
        base._Ready();
		Timeout += BroadcastTick;
		WaitTime = 1 / (_bpm / 60);
		EmitSignal(SignalName.Sync);
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		ElapsedTime += delta;
    }
    private void BroadcastTick()
	{
		if (_nextbpm > 0)
		{
			_bpm = _nextbpm;
			_nextbpm = -1;
			WaitTime = 1 / (_bpm / 60);
			EmitSignal(SignalName.BPMChanged);
		}

		GD.Print($"Tick @ {ElapsedTime}");
		EmitSignal(SignalName.Tick);
	}

	private void SyncTime()
	{
		EmitSignal(SignalName.Sync);
	}
}
