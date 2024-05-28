using Godot;
using System;

public partial class StepComponent : Node, IntervalSignalable
{
	[Signal]
	public delegate void StepFinishedEventHandler(); 

	[Export]
	public int Speed = 1;
	[Export]
	public int StepInterval = 1;
	[Export]
	public float ToleranceRatio = 0.1f;
	[Export]
	public Tween.TransitionType Transition = Tween.TransitionType.Linear;
	[Export]
	public CharacterBody2D Body;

	public Vector2I BufferedPositionUpdate = Vector2I.Zero;
	public Vector2I StartingPosition;
	public Vector2I TargetPosition;
	private Metronome Metronome;
	private Tween PositionTween = null;
	private int TickCount = 0;
	private bool InStep = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Metronome = GetNode<Metronome>("/root/Metronome");
		Metronome.SubscribeToInterval(StepInterval, this);

		StartingPosition = (Vector2I) Body.Position;
		TargetPosition = (Vector2I) Body.Position;
	}

	
	public void SetDirection(Vector2I direction)
	{
		if (direction != Vector2I.Zero)
		{
			BufferedPositionUpdate = direction * Global.TileSize * Speed;
			// GD.Print($"Updated BufferePositionUpdate={BufferedPositionUpdate}");
		}

		var diff = Math.Abs(GetIntervalOffset() / Metronome.SecondsPerBeat);
		if (diff < ToleranceRatio)
		{
			GD.Print($"Moving based on tolerance {GetIntervalOffset() / Metronome.SecondsPerBeat}");
			Move();
		}
	}

	public void OnIntervalElapsed()
	{
		GD.Print($"{StepInterval} interval elapsed @ {Metronome.TotalElapsedTime}");
		Move();
	}

	public void Move()
	{
		if (InStep) return;

		if (BufferedPositionUpdate != Vector2I.Zero)
		{
			StartingPosition = TargetPosition;
			TargetPosition = StartingPosition + BufferedPositionUpdate;
			BufferedPositionUpdate = Vector2I.Zero;

			// If we are slightly ahead or behind beat (within tolerance), 
			// factor that time into duration
			var duration = Metronome.SecondsPerBeat * StepInterval + GetIntervalOffset();

			GD.Print($"Transitioning to position {TargetPosition}. Duration={duration}");
			ResetPositionTween();
			PositionTween.TweenProperty(Body, "position", (Vector2) TargetPosition, duration);

			InStep = true;
		}
	}

	private float GetIntervalOffset()
	{
		float since = (float) Metronome.getSecondsSinceInterval(StepInterval);
		float until = (float) Metronome.GetSecondsUntilInterval(StepInterval);
		return since < until ? -since : until;
	}

	private void ResetPositionTween()
	{
		if (PositionTween != null)
		{
			PositionTween.Kill();
		}

		PositionTween = CreateTween();
		PositionTween.SetEase(Tween.EaseType.Out);
		PositionTween.SetTrans(Transition);
		PositionTween.SetProcessMode(Tween.TweenProcessMode.Physics);
		PositionTween.Finished += OnTweenFinished;
	}

	private void OnTweenFinished()
	{
		// TODO: Round position to make sure we're centered on tile
		InStep = false;
		EmitSignal(SignalName.StepFinished);
	}
}
