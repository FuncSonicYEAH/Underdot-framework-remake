using Godot;
using System;

public partial class LogoScript : MenuState
{
	private AnimationPlayer _animation;

	public override void Enter()
	{
		_animation = GetNode<AnimationPlayer>("Animation");
		_animation.Play("tips");
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
		if (@event.IsActionPressed("ui_accept"))
		{
			RequestChange("MainMenu");
		}
	}

}
