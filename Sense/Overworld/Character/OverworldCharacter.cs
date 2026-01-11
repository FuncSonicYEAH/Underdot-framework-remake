using Godot;
using System;

public partial class OverworldCharacter : CharacterBody2D
{
	public const float Speed = 100.0f;

	private string State = "";
	public string Direction = "down";

	private AnimatedSprite2D Sprite;
	private RayCast2D RayCast;

	[Export] private Dialog DialogManager;

	public override void _Ready()
	{
		base._Ready();
		Sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		RayCast = GetNode<RayCast2D>("RayCast2D");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (DialogManager != null && DialogManager.Started) return;

		ProcessMove(delta);
		ProcessAnimation();
		MoveAndSlide();
	}

	private void ProcessMove(double delta)
	{
		Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			Velocity = direction * Speed;
			State = "move";
		}
		else
		{
			State = "";
			Velocity = Vector2.Zero;
		}
	}

	private void ProcessAnimation()
	{
		if (Velocity.X != 0)
		{
			if (Velocity.X > 0) Direction = "right";
			if (Velocity.X < 0) Direction = "left";
		}
		else if (Velocity.Y != 0)
		{
			if (Velocity.Y > 0) Direction = "down";
			if (Velocity.Y < 0) Direction = "up";
		}

		string Anim = State + "_" + Direction;
		Sprite.Play(Anim);
	}
}
