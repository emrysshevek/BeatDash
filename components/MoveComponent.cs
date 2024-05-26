using Godot;
using System;

public partial class MoveComponent : Node2D
{
	[Export]
	public int MoveEvery = 1;
	[Export]
	public int Speed = 1;
	[Export]
	public float ToleranceRatio = 0.1f;
	[Export]
	public Tween.TransitionType Transition = Tween.TransitionType.Linear;
	[Export]
	public CharacterBody2D Body;

	public Vector2I BufferedPositionUpdate = Vector2I.Zero;
	public Vector2I StartingPosition;
	public Vector2I EndingPosition;
	private Tween PositionTween = null;
	private Clock Clock;
	private int TickCount = 0;	
	private bool InStep = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Clock = GetNode<Clock>("/root/Clock");
		Clock.Tick += OnClockTick;

		StartingPosition = (Vector2I) Body.Position;
		EndingPosition = (Vector2I) Body.Position;
	}

	
	public void SetDirection(Vector2I direction)
	{
		if (direction != Vector2I.Zero)
		{
			BufferedPositionUpdate = direction * Global.TileSize;
			// GD.Print($"Updated BufferePositionUpdate={BufferedPositionUpdate}");
		}

		var diff = Math.Abs(GetBeatOffset() / Clock.SecondsPerBeat);
		if (diff < ToleranceRatio)
		{
			GD.Print($"Moving based on tolerance {GetBeatOffset() / Clock.SecondsPerBeat}");
			Move();
		}
	}

	public void Move()
	{
		if (InStep) return;

		if (BufferedPositionUpdate != Vector2I.Zero)
		{
			StartingPosition = EndingPosition;
			EndingPosition = StartingPosition + BufferedPositionUpdate;
			BufferedPositionUpdate = Vector2I.Zero;

			// If we are slightly ahead or behind beat (within tolerance), 
			// factor that time into duration
			var duration = Clock.SecondsPerBeat * MoveEvery + GetBeatOffset();

			GD.Print($"Transitioning to position {EndingPosition}. Duration={duration}");
			ResetPositionTween();
			PositionTween.TweenProperty(Body, "position", (Vector2) EndingPosition, duration);

			InStep = true;
		}
	}

	private float GetBeatOffset()
	{
		return (float)(Clock.TimeLeft > Clock.SecondsPerBeat / 2 ? -(Clock.SecondsPerBeat - Clock.TimeLeft) : Clock.TimeLeft);
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

	private void OnClockTick()
	{
		TickCount = (TickCount + 1) % MoveEvery;
		if (TickCount != 0) return;

		Move();
	}

	private void OnTweenFinished()
	{
		// TODO: Round position to make sure we're centered on tile
		InStep = false;
	}
}
