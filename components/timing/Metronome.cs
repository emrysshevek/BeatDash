using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Xml.Schema;
using Godot;

public partial class Metronome : Node
{
    [Signal]
	public delegate void BeatEventHandler();

	private float _bpm = 60;
	private float _nextBpm = -1;
	[Export]
	public float BPM 
	{
		get => _bpm;
		set { _nextBpm = value; }
	}

	public double SecondsPerBeat { get => 1 / (BPM / 60); }
	public double SecondsSinceBeat { get; private set; } = 0;
	public double SecondsUntilBeat { get => SecondsPerBeat - SecondsSinceBeat; }
	public double TotalElapsedTime { get; private set; } = 0;
	public int TotalBeats { get; private set; } = 0;
	private Dictionary<int, List<IntervalSignalable>> SubscriberCallbacks = new();
	private Dictionary<int, double> IntervalCounters = new();

	public double GetSecondsPerInterval(float interval) => SecondsPerBeat * interval;
	public double GetSecondsPerInterval(int interval) => SecondsPerBeat * interval;
	public double getSecondsSinceInterval(float interval) => TotalElapsedTime % GetSecondsPerInterval(interval);
	public double getSecondsSinceInterval(int interval) => TotalElapsedTime % GetSecondsPerInterval(interval);
	public double GetSecondsUntilInterval(float interval) => GetSecondsPerInterval(interval) - getSecondsSinceInterval(interval);
	public double GetSecondsUntilInterval(int interval) => GetSecondsPerInterval(interval) - getSecondsSinceInterval(interval);

    public override void _Ready()
    {
        if (_nextBpm > 0)
		{
			_bpm = _nextBpm;
		}
    }

    public override void _PhysicsProcess(double delta)
    {
		// Check if any frequencies have elapsed and call all subscribers
		foreach (int key in SubscriberCallbacks.Keys)
		{
			IntervalCounters[key] += delta;
			double maxSeconds = GetSecondsPerInterval(key / 100f);
			if (IntervalCounters[key] >= maxSeconds)
			{
				foreach(var sub in SubscriberCallbacks[key]) { sub.OnIntervalElapsed(); }
				IntervalCounters[key] -= maxSeconds;
			}
		}

		// Emit a signal on the beat
        SecondsSinceBeat += delta;
		TotalElapsedTime += delta;
		if (SecondsSinceBeat >= SecondsPerBeat)
		{
			// Subtract instead of setting to zero to make sure the value doesn't drift
			SecondsSinceBeat -= SecondsPerBeat;
			EmitSignal(SignalName.Beat);
			TotalBeats++;
		}
    }

	// Everything concerning subscriptions to frequencies
	private void SaveSubscription(int key, IntervalSignalable val)
	{
		GD.Print($"Saving subscriber {val} for interval {key/100f}");
		if (SubscriberCallbacks.ContainsKey(key))
		{
			SubscriberCallbacks[key].Add(val);
		} else {
			SubscriberCallbacks[key] = new() { val };
			IntervalCounters[key] = 0.0;
		}
	}
	public void SubscribeToInterval(int interval, IntervalSignalable subscriber)
	{
		SaveSubscription(interval * 100, subscriber);
	}
	public void SubscribeToInterval(float interval, IntervalSignalable subscriber)
	{
		SaveSubscription(Mathf.RoundToInt(interval * 100), subscriber);
	}

	private void RemoveSubscription(int key, IntervalSignalable val)
	{
		if (SubscriberCallbacks.ContainsKey(key))
		{
			SubscriberCallbacks[key].Remove(val);
			if (SubscriberCallbacks[key].Count == 0)
			{
				SubscriberCallbacks.Remove(key);
				IntervalCounters.Remove(key);
				GD.Print($"Removed empyty frequency {key / 100f}");
			}
		}
	}
	public void UnsubscribeFromInterval(int interval, IntervalSignalable subscriber)
	{
		RemoveSubscription(interval * 100, subscriber);
	}
	public void UnsubscribeFromInterval(float interval, IntervalSignalable subscriber)
	{
		RemoveSubscription(Mathf.RoundToInt(interval * 100), subscriber);
	}

	
}
