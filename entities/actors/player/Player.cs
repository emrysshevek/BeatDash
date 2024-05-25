using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	public const int TileSize = 128;
	public const float Speed = 300.0f;
	public Vector2 BufferedPosition = Vector2.Inf;
	public Vector2 StartingPosition = Vector2.Inf;
	public Vector2 EndingPosition = Vector2.Inf;
	public double ElapsedTime = 0;
	public Clock Clock;
	private Tween PositionTween = null;

    public override void _Ready()
    {
        base._Ready();
		Clock = GetNode<Clock>("/root/Clock");
		Clock.Tick += OnClockTick;

		StartingPosition = Position;
		EndingPosition = Position;
    }
    public override void _PhysicsProcess(double delta)
	{
		// Get next position based on the directional inputs
		// var direction = Input.GetVector("left", "right", "up", "down");
		// if (direction != Vector2.Zero)
		// {
		// 	GD.Print($"Input direction = {direction}");
		// 	BufferedPosition = Vector2.Zero;
		// 	if (direction.X != 0)
		// 	{
		// 		int sign = Mathf.RoundToInt(direction.X / direction.X);
		// 		BufferedPosition.X = Position.X + (TileSize * sign);
		// 		GD.Print($"Updated BufferedPosition {BufferedPosition}, sign = {sign}");
		// 	}
		// 	if (direction.Y != 0)
		// 	{
		// 		var sign = Mathf.RoundToInt(direction.Y / direction.Y);
		// 		BufferedPosition.Y = Position.Y + (TileSize * sign);
		// 		GD.Print($"Updated BufferedPosition {BufferedPosition}");
		// 	}
		// }

		var inputPosition = Vector2.Zero;
		if (Input.IsActionJustPressed("left"))
		{
			inputPosition.X = -TileSize;
			GD.Print($"Left press. inputPosition={inputPosition}");
		} else if (Input.IsActionJustPressed("right"))
		{
			inputPosition.X = TileSize;
			GD.Print($"Right press. inputPosition={inputPosition}");
		}
		if (Input.IsActionJustPressed("up"))
		{
			inputPosition.Y = -TileSize;
			GD.Print($"Up press. inputPosition={inputPosition}");
		} else if (Input.IsActionJustPressed("down"))
		{
			inputPosition.Y = TileSize;
			GD.Print($"Down press. inputPosition={inputPosition}");
		}

		if (inputPosition != Vector2.Zero)
		{
			BufferedPosition += inputPosition;
			GD.Print($"Updated BufferedPosition={BufferedPosition}");
		}

	}

	private void ResetPositionTween()
	{
		if (PositionTween != null)
		{
			PositionTween.Kill();
		}

		PositionTween = CreateTween();
		PositionTween.SetEase(Tween.EaseType.Out);
		PositionTween.SetTrans(Tween.TransitionType.Expo);
		PositionTween.SetProcessMode(Tween.TweenProcessMode.Physics);
	}

	public void OnClockTick()
	{
		if (BufferedPosition == Vector2.Inf)
		{
			BufferedPosition = Position;
		}
		StartingPosition = Position;
		EndingPosition = BufferedPosition;
		ElapsedTime = 0;


		if (EndingPosition != StartingPosition)
		{
			ResetPositionTween();
			PositionTween.TweenProperty(this, "position", EndingPosition, Clock.SecondsPerBeat);
			GD.Print($"Transitioning to position {EndingPosition}");
		}
	}
}
