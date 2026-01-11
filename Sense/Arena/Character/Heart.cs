using Godot;
using System;

public partial class Heart : CharacterBody2D
{
	[Export] public ArenaGroup ArenaNode;
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			Velocity = direction * Speed;
		}
		else
		{
			Velocity = Vector2.Zero;
		}

		if (ArenaNode != null)
		{
		}

		MoveAndSlide();
	}
}
