using Godot;
using System;

public partial class Global : Node
{
	[Signal]
    public delegate void BPMChangedEventHandler();

	private float _bpm = 60;
	[Export]
	public float BPM
	{
		get => _bpm;
		set 
		{
			_bpm = value;
			EmitSignal(SignalName.BPMChanged);
		}
	}
	
	public float SecondsPerBeat 
	{
		get => 1 / (_bpm / 60);
	}

	private bool _timeStop = false;
}
