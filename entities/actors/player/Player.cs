using Godot;
using System;

public partial class Player : BaseActor
{
	[Export]
	public StepComponent Move;
    public override void _Ready()
    {
        Move ??= GetNode<StepComponent>("StepComponent");
    }
    public override void _PhysicsProcess(double delta)
	{
		// Get next direction based on the directional inputs
		var direction = Vector2I.Zero;
		if (Input.IsActionJustPressed("left"))
		{
			direction += Vector2I.Left;
		} else if (Input.IsActionJustPressed("right"))
		{
			direction += Vector2I.Right;
		}
		if (Input.IsActionJustPressed("up"))
		{
			direction += Vector2I.Up;
		} else if (Input.IsActionJustPressed("down"))
		{
			direction += Vector2I.Down;
		}

		if (direction != Vector2I.Zero)
		{
			Move.SetDirection(direction);
		}

	}
}
