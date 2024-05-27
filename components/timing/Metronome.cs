using Godot;

public partial class Metronome : Timer
{
    [Signal]
	public delegate void BeatEventHandler(); 
	[Signal]
    public delegate void BPMChangedEventHandler();

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

	public virtual double SecondsPerBeat
	{
		get => WaitTime;
	}
	public double ElapsedTime { get; private set; } = 0;
	public double TimeSince { get; private set; } = 0;

    public override void _Ready()
    {
        base._Ready();
		Timeout += BroadcastBeat;
		_bpm = _nextbpm > 0 ? _nextbpm : _bpm;
		WaitTime = 1 / (_bpm / 60);
    }
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
		ElapsedTime += delta;
		TimeSince += delta;
    }
    private void BroadcastBeat()
	{
		if (_nextbpm > 0)
		{
			_bpm = _nextbpm;
			_nextbpm = -1;
			WaitTime = 1 / (_bpm / 60);
			EmitSignal(SignalName.BPMChanged);
		}
		
		// GD.Print($"Tick @ {ElapsedTime}");
		EmitSignal(SignalName.Beat);
		TimeSince = 0;
	}
}
